using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.IO;
using System.Data.SqlClient;

namespace CCMNetworkMon
{
    public partial class CCMNetworkMon : ServiceBase
    {

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

        #region globalvars
           public System.Timers.Timer timer = new System.Timers.Timer();
           public List<string> ccmip = new List<string>();
           public string dbconnection;
           public string basepath;
        #endregion

        public CCMNetworkMon()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // service state update done.

            ccmip.Clear();

            //read the server.config for host server brodcast and port
            basepath = AppDomain.CurrentDomain.BaseDirectory.ToString();

            try
            {
                string sqlpath = Path.Combine(basepath, "sql_connection.txt");
                dbconnection = File.ReadLines(sqlpath).First();

                
            }
            catch (Exception ex)
            {

            }
            

            // create a timer which will fire the poller
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Elapsed += timer_Elapsed;
            timer.Interval = 15000;
            timer.Start();


            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            timer.Elapsed -= new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = false;


            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }


        public static DataSet GetData(string sql, string ConnectionString, out string err)
        {
            err = string.Empty;
            DataSet Result = new DataSet();
            if (string.IsNullOrEmpty(sql))
            {
                err = "Query is not defined";
                return Result;
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                err = "Connection String is not defined";
                return Result;
            }


            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(sql, conn) { CommandType = CommandType.Text };
            SqlDataAdapter da = new SqlDataAdapter();


            try
            {
                conn.Open();
                command.ExecuteNonQuery();
                da.SelectCommand = command;
                da.Fill(Result, "RESULT");
                conn.Close();
            }
            catch (SqlException ex) { err = ex.Message.ToString(); }
            catch (Exception ex) { err = ex.Message.ToString(); }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return Result;
        }


        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            try
            {
                string err1 = string.Empty;

                DataSet ds = GetData("Select MachineIP from ccmMachineConfig where 1=1", dbconnection, out err1);
                
                if(!string.IsNullOrEmpty(err1))
                {
                    timer.Enabled = true;
                    return;
                }

                bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (!hasrows)
                {
                    timer.Enabled = true;
                    return;
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string ip = dr["MachineIP"].ToString();
                    string err = string.Empty;
                    string result = PingMachine(out err, ip);

                    if (result != "Success")
                    {
                        if (!string.IsNullOrEmpty(dbconnection))
                        {
                            try
                            {

                                using (SqlConnection cn = new SqlConnection(dbconnection))
                                {
                                    cn.Open();
                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.Connection = cn;
                                        cmd.CommandText = "Insert into CCMErrLog (LogDatetime,LogTopic,LogDesc) values (GetDate(),'Network','" + ip + "->" + result + "')";
                                        cmd.ExecuteNonQuery();

                                    }
                                }
                            }catch (Exception ex){}
                        }//endif dbconnection
                    }//endif ping result
                }
                
            }
            catch (Exception ex)
            {
                
            }

            timer.Enabled = true;
        }

        public string PingMachine(out string err,string serverip)
        {
            string status = string.Empty;
            err = string.Empty;

            if (string.IsNullOrEmpty(serverip))
            {
                err = "IP Address is required..";
                status = "Bad Request";

                return status;
            }

            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(serverip, 2000);

                if (reply.Status == IPStatus.Success)
                {
                    status = "Success";
                }
                else
                {
                    status = reply.Status.ToString();
                }

            }
            catch (Exception ex)
            {
                status = "Request timeout";
                err = ex.Message.ToString();
            }

            return status;
        }

    }
}
