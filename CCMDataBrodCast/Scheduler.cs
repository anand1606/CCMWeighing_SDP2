using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.ServiceModel.Channels;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using MSWinsockLib;

namespace CCMDataBrodCast
{
    

    public partial class Scheduler : ServiceBase
    {
        #region global_Vars

       
        private static string ErrTable = "ccmErrLog";
       
        private static List<wsckclient> clients = new List<wsckclient>();
        private static List<ClientPara> ClientList = new List<ClientPara>();

        private static string RBMQServerUri = "";
        private static bool shutdown = false;
        private static string sqlconfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "sql_connection.txt");
        private static string sqlcnstr = string.Empty;


        
        private static bool waitforserver = false;
        
        #endregion

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

        public Scheduler()
        {
            InitializeComponent();
        }

        static void updatesysstatus(bool t)
        {
            //update sysstatus
            using (SqlConnection cn = new SqlConnection(sqlcnstr))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        string sql = string.Empty;

                        //get lastdatetime from db
                        sql = "Update SysStatus set BrodCastSts = '" + (t ? 1 : 0) + "',BrodCastUpdDt = getdate()";
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // service state update done.

           

            // create a timer which will fire the poller
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            updatesysstatus(true);

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }


        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                try
                {
                    sqlcnstr = File.ReadLines(sqlconfig).First();
                }catch(Exception ex)
                {

                }

                //read the server.config for host server brodcast and port
                string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();
                string serverconfig = "server_config.txt";                
                string fullpath = Path.Combine(strpath, serverconfig);
                RBMQServerUri = File.ReadLines(fullpath).First();
                
                shutdown = false;
                
                enqueue("Service is Started",sqlcnstr);
                enqueue("Logging Thread Started", sqlcnstr);
                enqueue("Starting Client Init..", sqlcnstr);
                clients = new List<wsckclient>();

                Restart();

            }
            catch (Exception ex)
            {
                enqueue("Service Start Error :" + ex.Message.ToString(), sqlcnstr);
            }
        }

        protected override void OnStop()
        {
           
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

           
            
            
            foreach (wsckclient t in clients)
            {
                t.Disconnect();
                WaitWhile(1);
                enqueue(string.Format("Closing Client : {0}", t.RemoteIP), sqlcnstr);
            }

            shutdown = true;


           
            enqueue("Service is Stoped", sqlcnstr);
            
            clients.Clear();

            updatesysstatus(false);
            
            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
                        
        }

      
        
        static void Restart()
        {

           
            
            ClientList = new List<ClientPara>();
            
            /** previous logic depend on client_config.txt 
            string clientconfig = "client_config.txt";
            string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            string fullpath = Path.Combine(strpath, clientconfig);
            var lines = File.ReadLines(fullpath);

            //read the client.config for weightmachine-ip and port
            //192.168.11.23;1702;P
           
            **/
            enqueue("List of Configured Clients", sqlcnstr);
            enqueue("--------------------------", sqlcnstr);

            /** New Logic get from ccmMachineConfig Table **/
            string err = string.Empty;
            string sql = "Select * from ccmMachineConfig where 1 = 1";
            DataSet ds = Library.GetData(sql,sqlcnstr,out err);

            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (shutdown)
                    {
                        return;
                    }

                    ClientPara p = new ClientPara();
                    p.MachineIP = dr["MachineIP"].ToString();
                    p.MachinePort = Convert.ToInt32(dr["MachinePort"].ToString());
                    p.MachineStatus = 0;
                    p.LastLogDateTime = DateTime.Now;
                    p.LogDateTime = DateTime.Now;
                    p.MachineID = dr["MachineName"].ToString();
                    ClientList.Add(p);
                    enqueue("Client : " + p.MachineID + " => " + p.MachineIP + ":" + p.MachinePort.ToString(), sqlcnstr);
                }
            }
            else
            {
                enqueue("Machine Configuration is missing in ccmMachineConfig", sqlcnstr);
                enqueue("--------------------------", sqlcnstr);
            }
            enqueue("--------------------------", sqlcnstr);
            shutdown = false;

            ///**** Old Logic 
            //string[] tclientlist;
            //string clientip = string.Empty;
            //int clientport = 0;
            //string clientid = string.Empty;
           
            

            //foreach (var line in lines)
            //{
            //    if (shutdown)
            //    {
            //        return;
            //    }

            //    tclientlist = line.Split(';');
            //    clientip = tclientlist[0];
            //    clientport = Convert.ToInt32(tclientlist[1]);
            //    clientid = tclientlist[2];

            //    ClientPara p = new ClientPara();
            //    p.MachineIP = clientip;
            //    p.MachinePort = clientport;
            //    p.MachineStatus = 0;
            //    p.LastLogDateTime = DateTime.Now;
            //    p.LogDateTime = DateTime.Now;
            //    p.MachineID = clientid;
            //    ClientList.Add(p);
            //    enqueue("Client : " + p.MachineID + " => " + p.MachineIP + ":" + p.MachinePort.ToString(), sqlcnstr);
            //}
            

            
            clients = new List<wsckclient>();

            //finaly create tcp client
            foreach (ClientPara p in ClientList)
            {
                wsckclient c = new wsckclient();
                c.MachineID = p.MachineID;
                c.RemoteIP = p.MachineIP;
                c.RemotePort = p.MachinePort;
                c.State = 0;
                c.RBMQServerUri = RBMQServerUri;
                c.SQLConnection = sqlcnstr;

                enqueue("Try to init : " + c.RemoteIP, sqlcnstr);
                c.init();
                clients.Add(c);
                
            }

            //finally connect each client
            foreach (wsckclient p in clients)
            {
                if (p.Connect())
                    enqueue(p.MachineID + ":" + "Connected", sqlcnstr);
                else
                    enqueue("Error " + p.MachineID + ":" + "could not connected", sqlcnstr);
            }
        }


        static void WaitWhile(int seconds)
        {
            //wait a while
            int i = 0;
            Stopwatch sw = new Stopwatch(); // sw cotructor
            sw.Start(); // starts the stopwatch
            for (int t = 0; ; i++)
            {
                if (t % 100000 == 0) // if in 100000th iteration (could be any other large number
                // depending on how often you want the time to be checked) 
                {
                    sw.Stop(); // stop the time measurement
                    if (sw.ElapsedMilliseconds > seconds * 1000) // check if desired period of time has elapsed
                    {
                        break; // if more than 5000 milliseconds have passed, stop looping and return
                        // to the existing code
                    }
                    else
                    {
                        sw.Start(); // if less than 5000 milliseconds have elapsed, continue looping
                        // and resume time measurement
                    }
                }
            }
        }

        

        static void enqueue(string msg,string cnstr)
        {
            string t = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + msg ;            
            DateTime dt = DateTime.Now;
            string logpath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory.ToString(), dt.ToString("yyyyMMdd") + ".txt");
            

            using (StreamWriter sw2 = new StreamWriter(logpath, true))
            {
                sw2.WriteLine(t);
                
                sw2.Close();
            }

            if(!string.IsNullOrEmpty(cnstr))
            {
                
                using (SqlConnection cn = new SqlConnection(sqlcnstr))
                {
                    try
                    {
                        cn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            string sContent = msg.Replace("'", "''");
                            string sql = "Insert into ccmErrLog (LogDateTime,LogTopic,LogDesc) values (GetDate(),'Service','" + t.ToString() + "')";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Library.WriteErrorLog(ex.Message.ToString());
                    }

                }
            }

        }

       

    }//inner class
}//namespace
