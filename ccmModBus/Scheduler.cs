using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using FluentModbus;
using System.Data.SqlClient;
namespace ccmModBus
{
    public partial class Scheduler : ServiceBase
    {
        private static string sqlconfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "sql_connection.txt");

        private static string sqlcnstr = string.Empty;
        private static Boolean ShutdownSignal = false;
        private static System.Timers.Timer timer = new System.Timers.Timer();
        private static ModbusTcpServer modserver = new ModbusTcpServer();

        private static Dictionary<string, int> map = new Dictionary<string, int>();
        //private static Random random = new Random();
        //private static ModbusTcpClient modclient = new ModbusTcpClient();

        public Scheduler()
        {
            InitializeComponent();
        }


        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwCheckPoint;
            public long dwControlsAccepted;
            public ServiceState dwCurrentState;
            public long dwServiceSpecificExitCode;
            public long dwServiceType;
            public long dwWaitHint;
            public long dwWin32ExitCode;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);


        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // service state update done.

            try
            {
                sqlcnstr = File.ReadLines(sqlconfig).First();

            }
            catch (Exception ex)
            {
                Library.WriteInfoLog("Modbus Service->Did not get SQL Connection Details");
                Library.WriteInfoLog("Modbus Service->Service Stopped with Error");
                OnStop();
            }
            
            timer.AutoReset = true;
            timer.Interval = 4000;
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
           
            timer.Start();
            ShutdownSignal = false;

            Library.WriteInfoLog("Modbus Service->Started");
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            modserver.EnableRaisingEvents = true;
            modserver.RegistersChanged += Modserver_RegistersChanged;
            //modserver.ConnectionTimeout = new TimeSpan(0, 0, 30);
            modserver.MaxConnections = 10;
            modserver.Start();
        }

        private void Modserver_RegistersChanged(object sender, List<int> e)
        {
            int t = 0;
            foreach(int i in e)
            {
                Library.WriteInfoLog("Modbus Service->Info->Register Change->Index(" + t.ToString() +")->Value->" + i.ToString());
                t++;
            }
                
        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ShutdownSignal)
            {
                Library.WriteInfoLog("Modbus Service->Error->Shutdown Signal Received");
                timer.Stop();
                return;
            }
            timer.Stop();

            //get the latest parameters
            string err = string.Empty;
            Library.WriteInfoLog("Modbus Service->info->Heartbeat");


            DataSet ds = Library.GetData("Select * From  ccmModbusRegister ", sqlcnstr, out err);
            if (!string.IsNullOrEmpty(err))
            {
                Library.WriteInfoLog("Modbus Service->Error->Getting Holding Register->" + err);
                timer.Start();
                return;
            }

           
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (!hasRows)
            {
                Library.WriteInfoLog("Modbus Service->Error->Holding Register not Configured->" + err);
                timer.Start();
                return;
            }
            else
            {
                map.Clear();
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    map.Add(dr["ccmMachine"].ToString(),Convert.ToInt32(dr["HoldingReg"].ToString()));
                    //Library.WriteInfoLog("Modbus Service->Register Mapping->" + dr["ccmMachine"].ToString() + "->" + dr["HoldingReg"].ToString());
                }
            }

            err = string.Empty;
            string sql = "Select * From  ccmAlarm where tDate = Convert(date,GetDate()) " +
                " and AlmSent = 0 And ABS(DateDiff(second,getdate(),AddDt)) between 0 and 50 ";
            ds = Library.GetData(sql, sqlcnstr, out err);

            if (!string.IsNullOrEmpty(err))
            {
                Library.WriteInfoLog("Modbus Service->Error-> while Getting pending records.." + err);
                timer.Start();
                return;
            }
            

            //act like modbus salve
            hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                //Library.WriteInfoLog("Modbus Service->Record Founds");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                   
                    int regadd = 0;
                    map.TryGetValue(row["MachineNo"].ToString(), out regadd);
                    if (regadd != 0)
                    {
                        //Library.WriteInfoLog("Modbus Service->writing register->" + regadd.ToString() + "->" + row["ID"].ToString());
                        int regval = Convert.ToInt16(row["ID"].ToString());

                        Library.WriteInfoLog("Modbus Service->writing register->" + regadd.ToString() + "-Value->" + regval.ToString() );
                        //----- auto normal performance version, more flexibility
                        //modserver.ClearBuffers();
                        //ushort	0 to 65,535	Unsigned 16-bit integer
                        var registers = modserver.GetHoldingRegisters();
                        registers.SetBigEndian<ushort>(regadd, Convert.ToUInt16(regval));
                        modserver.Update();


                        sql = "Update ccmAlarm Set AlmSent = 1, AlmSentTime = GetDate() where " +
                                        " tDate ='" + Convert.ToDateTime(row["tDate"]).ToString("yyyy-MM-dd") + "' And " +
                                        " tShift='" + row["tShift"].ToString() + "' And " +
                                        " MachineNo ='" + row["MachineNo"].ToString() + "' And " +
                                        " SrNo ='" + row["SrNo"].ToString() + "'";
                        try
                        {
                            //update db status to true;
                            using (SqlConnection cn = new SqlConnection(sqlcnstr))
                            {
                                cn.Open();
                                using (SqlCommand cmd = cn.CreateCommand())
                                {

                                    cmd.CommandText = sql;
                                    cmd.ExecuteNonQuery();

                                    //reset id to 1 using trancate due to 16-bit restriction
                                    if (regval >= 65500)
                                    {
                                        sql = "truncate table ccmAlarm ";
                                        cmd.CommandText = sql;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Library.WriteInfoLog("Modbus Service->Update DB Error->" + sql);
                        }
                    }
                    //else
                    //{
                    //    Library.WriteInfoLog("Modbus Service->Map not found->");
                    //    foreach (KeyValuePair<string, int> entry in map)
                    //    {
                    //        Library.WriteInfoLog("Modbus Service->Key->" + entry.Key + "-Value-" + entry.Value.ToString());
                    //    }
                    //}
                }
            }
            //else
            //{
            //    Library.WriteInfoLog("Modbus Service->Error->No Records found for Alarm");
            //}

            timer.Start();

        }

        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            ShutdownSignal = true;
            timer.Stop();
            timer.Close();

            modserver.Stop();
            modserver.Dispose();
            
            Library.WriteInfoLog("Modbus Service->Closing Modbus Server");
            

            // Update the service state to stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            Library.WriteInfoLog("Modbus Service->Service Stopped");
        }
    }
}
