using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using RabbitMQ.Client;

namespace CCMDataWriter
{
    public partial class Scheduler : ServiceBase
    {

        private static string sqlconfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "sql_connection.txt");
        
        private static string sqlcnstr = string.Empty;
        private static Boolean ShutdownSignal = false;
        private static System.Timers.Timer timer = new System.Timers.Timer();
        private static System.Timers.Timer timer_logCleaner = new System.Timers.Timer();
        private static DateTime lastlogclean = new DateTime();

        private static List<WeightClient> clientlist = new List<WeightClient>();

        private ConnectionFactory factory = new ConnectionFactory();
        private IConnection conn;
        private IModel channel;

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
                Library.WriteInfoLog("Data Writer Service->Did not get SQL Connection Details");
                Library.WriteInfoLog("Data Writer Service->Service Stopped with Error");
                OnStop();
            }
            
            try
            {
                //old logic
                //var lines = File.ReadLines(clientconfig);
                //int i = 1;
                //foreach (var line in lines)
                //{
                //    string[] tclientlist;
                //    string clientip = string.Empty;
                //    string clientid = string.Empty;

                //    tclientlist = line.Split(';');
                //    WeightClient t = new WeightClient();
                //    t.MachineIP = tclientlist[0];
                //    t.MachineID = tclientlist[2];
                //    t.TableName = "ccm" + i.ToString();
                //    clientlist.Add(t);
                //    i += 1;
                //}

                string err = string.Empty;
                string sql = "Select * from ccmMachineConfig where 1 = 1";
                DataSet ds = Library.GetData(sql, sqlcnstr, out err);

                bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrows)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {


                        WeightClient t = new WeightClient();
                        t.MachineIP = dr["MachineIP"].ToString();
                        t.MachineID = dr["MachineName"].ToString();
                        t.TableName = dr["TableName"].ToString();
                        clientlist.Add(t);
                    }
                }
                else
                {
                    Library.WriteInfoLog("Data Writer Service->Did not get client Details from ccmMachineConfig");
                    OnStop();
                }

              

            }
            catch (Exception ex)
            {
                Library.WriteInfoLog("Data Writer Service->error->" + ex.Message.ToString());
                Library.WriteInfoLog("Data Writer Service->Service Stopped with Error");
                OnStop();
            }

            //weight log cleaner
            timer_logCleaner.AutoReset = true;
            timer_logCleaner.Interval = 3600000;
            timer_logCleaner.Elapsed += timer_logCleaner_Elapsed;
            timer_logCleaner.Start();

            //data writer, pipe save from log
            timer.AutoReset = true;
            timer.Interval = 2000;
            timer.Elapsed += timer_Elapsed;
            //timer.Elapsed += timer_Elapsed_LowHighLow;
            timer.Start();
            ShutdownSignal = false;

            Library.WriteInfoLog("Data Writer Service->Started");

            updatesysstatus(true);
            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
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
                        sql = "Update SysStatus set DataWriterSts = '" + (t ? 1 : 0) + "',DataWriteUpdDt = getdate()";
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


        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
           

            ShutdownSignal = true;
            timer.Stop();
            timer.Close();

            timer_logCleaner.Stop();
            timer_logCleaner.Close();

            
            //update sysstatus

            updatesysstatus(false);

            // Update the service state to stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            Library.WriteInfoLog("Data Writer Service->Service Stopped");
        }

        static void timer_logCleaner_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ShutdownSignal)
            {
                return;
            }
            DateTime tDate = DateTime.Now;
           if((tDate - lastlogclean).TotalMinutes < 180)
           {
               return;
           }

           using (SqlConnection cn = new SqlConnection(sqlcnstr))
           {
               try
               {
                   cn.Open();
                   Library.WriteInfoLog("Log_Cleaner->Cleaning Old WeightLogData before 3 days");
                   using (SqlCommand cmd = new SqlCommand())
                   {
                       cmd.Connection = cn;
                       cmd.CommandTimeout = 0;
                       cmd.CommandType = CommandType.Text;
                       cmd.CommandText = "Delete from ccmSignalDetection Where LogDateTime <= DateAdd(day,-3,'" + tDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "') and Processed = 1 ";
                       cmd.ExecuteNonQuery();
                       lastlogclean = tDate;
                   }
               }
               catch (Exception ex)
               {
                   Library.WriteInfoLog("Log_Cleaner_Error->" + ex.Message);
               }
           }

        }

        //Low high Low based pipe record ->second method
        /****
        static void timer_Elapsed_LowHighLow(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ShutdownSignal)
            {
                return;
            }
            
            //get the last time from db where stopped..
            string err = string.Empty;
            DataSet ds = Library.GetData("Select * from ccmMachineConfig ", sqlcnstr, out err);

            if (!string.IsNullOrEmpty(err))
            {
                Library.WriteInfoLog("Data Writer Service->" + err);
                return;
            }

            timer.Stop();

            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //Library.WriteInfoLog("processing LowHiLow-> 0 -> " + dr["MachineIP"].ToString());
                    
                    if (dr["LastDataProcessed"] == null)
                    {
                        #region nullinlastprocess
                        //set minimum date avaialbe from ccmSignalDetection
                        string st = Library.GetDescription("Select Min(LogDateTime) from ccmSignalDetection where MachineIP ='" + dr["MachineIP"].ToString().Trim() + "'", sqlcnstr, out err);
                        if (!string.IsNullOrEmpty(err))
                        {
                            Library.WriteInfoLog("Error LowHiLow-> 1 -> " + err);
                            continue;
                        }

                        if (!string.IsNullOrEmpty(st))
                        {
                            using (SqlConnection cn = new SqlConnection(sqlcnstr))
                            {
                                try
                                {
                                    cn.Open();
                                    string sql = "Update ccmMachineConfig set LastDataProcessed = '" + st + "' where MachineIP ='" + dr["MachineIP"].ToString() + "' ";

                                    using (SqlCommand cmd = new SqlCommand())
                                    {
                                        cmd.Connection = cn;
                                        cmd.CommandText = sql;
                                        cmd.CommandType = CommandType.Text;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Library.WriteInfoLog("Error LowHiLow-> 2 ->" + ex.Message);
                                }
                            }
                            continue;
                        }

                    }
                    #endregion
                    else
                    {
                        DateTime LastLogTime = Convert.ToDateTime(dr["LastDataProcessed"]);
                        string MachineIP = dr["MachineIP"].ToString();
                        //Library.WriteInfoLog("sp call started->" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        DataSet Logds = Library.GetData("Exec sp_Pipe_Log_LowHighLow '" + MachineIP + "'", sqlcnstr, out err);
                        //Library.WriteInfoLog("sp call complated->" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF"));
                        if (!string.IsNullOrEmpty(err))
                        {
                            Library.WriteInfoLog("Error->" + err);
                            continue;
                        }
                        hasRows = false;
                        hasRows = Logds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                        if (hasRows)
                        {
                            
                            DataRow LogRow = Logds.Tables[0].Rows[0];
                            //Library.WriteInfoLog("info->" + LogRow["StartTime"].ToString() + "-" + LogRow["EndTime"].ToString());
                            if (LogRow["MaxTime"] == DBNull.Value && LogRow["MaxWT"] == DBNull.Value)
                            {

                                //Library.WriteInfoLog("next LowHiLow-> 0 ");
                                continue;
                            }
                            else
                            {
                                //Library.WriteInfoLog("step-> 1");
                                var result = (from a in clientlist
                                              where a.MachineIP.ToString().Trim() == dr["MachineIP"].ToString().Trim()
                                              select a).SingleOrDefault();

                                if (result != null)
                                {
                                    //Library.WriteInfoLog("step-> 2");

                                    WeightClient t = (WeightClient)result;

                                    string err1 = string.Empty;
                                    //get lastparameters
                                    CurrentSetting tmpset = GetLastPara(t.MachineID.ToString(), Convert.ToDateTime(LogRow["EndTime"]), out err1);
                                    //Library.WriteInfoLog("Try->GetLastPara");

                                    StreamReceiver tmprec = new StreamReceiver(tmpset);
                                    //Library.WriteInfoLog("Try->GetStreamReceiver");
                                    tmprec.MachineID = t.MachineID;
                                    tmprec.LogDateTime = Convert.ToDateTime(LogRow["MaxTime"]);

                                    //string[] wt = dr["SignalMsg"].ToString().Split('#');
                                    //string strwt = wt[0];

                                    double actwt = 0;
                                    double.TryParse(LogRow["MaxWT"].ToString().Trim(), out actwt);
                                    tmprec.ActWt = actwt;

                                    //Library.WriteInfoLog("Try->Signal->" + t.TableName + "->" + t.MachineIP + "->" + tmprec.LogDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "->" + tmprec.ActWt.ToString());

                                    //try to save in db
                                    string err2 = string.Empty;
                                    string retjson = string.Empty;

                                    bool issaved = false;
                                    bool isupdated = false;
                                    try
                                    {
                                        issaved = SaveRecordToDB(tmprec, out err2, t.TableName, out retjson);
                                        if (!string.IsNullOrEmpty(err2))
                                        {
                                            //Library.WriteInfoLog("step-> 3-> Error while save record ->" + err2);

                                            //duplicate less then 5 second
                                            using (SqlConnection cn = new SqlConnection(sqlcnstr))
                                            {
                                                try
                                                {
                                                    cn.Open();
                                                    string sql = "Update ccmMachineConfig set LastDataProcessed = '" + Convert.ToDateTime(LogRow["EndTime"]).AddSeconds(5).ToString("yyyy-MM-dd HH:mm:ss.FFF") + "' where MachineIP ='" + dr["MachineIP"].ToString() + "' ";
                                                    //string sql = string.Empty;
                                                    using (SqlCommand cmd = new SqlCommand())
                                                    {
                                                        cmd.Connection = cn;
                                                        cmd.CommandText = sql;
                                                        cmd.CommandType = CommandType.Text;
                                                        cmd.ExecuteNonQuery();                                                        
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Library.WriteInfoLog("Error LowHiLow-> 3 ->" + ex.Message);
                                                }
                                            }//end Connection

                                            continue;
                                        }
                                        isupdated = UpdateSignalRecords(tmprec.LogDateTime, t.MachineIP, issaved, err2);

                                        if (issaved)
                                        {
                                            Library.WriteInfoLog("info->" + retjson.ToString());
                                        }
                                        
                                    }
                                    catch (Exception ex)
                                    {
                                        Library.WriteInfoLog("Error->" + ex.Message);
                                    }

                                    //update LastDataProcessed and ProcessedFLG in signal detaction

                                    using (SqlConnection cn = new SqlConnection(sqlcnstr))
                                    {
                                        try
                                        {
                                            cn.Open();
                                            string sql = "Update ccmMachineConfig set LastDataProcessed = '" + Convert.ToDateTime(LogRow["EndTime"]).ToString("yyyy-MM-dd HH:mm:ss.FFF") + "' where MachineIP ='" + dr["MachineIP"].ToString() + "' ";
                                            //string sql = string.Empty;
                                            using (SqlCommand cmd = new SqlCommand())
                                            {
                                                cmd.Connection = cn;
                                                cmd.CommandText = sql;
                                                cmd.CommandType = CommandType.Text;
                                                cmd.ExecuteNonQuery();

                                                sql = "Update ccmSignalDetection set Processed = 1 where MachineIP = '" + dr["MachineIP"].ToString() + "' and LogDateTime between '" +  Convert.ToDateTime(LogRow["StartTime"]).AddMinutes(-5).ToString("yyyy-MM-dd HH:mm:ss.FFF") + "' and '" + Convert.ToDateTime(LogRow["EndTime"]).ToString("yyyy-MM-dd HH:mm:ss.FFF") + "'";
                                                cmd.CommandType = CommandType.Text;
                                                cmd.CommandText = sql;
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //Library.WriteInfoLog("Error LowHiLow-> 3 ->" + ex.Message);
                                        }
                                    }//end Connection

                                }
                                else
                                {
                                    //Library.WriteInfoLog("Error LowHiLow-> 4 -> No matching machine found in client list");
                                }
                            }//if pipe found
                        }
                        else
                        {
                            //Library.WriteInfoLog("no records in sp call-> 5 ");
                            continue;
                        }
                        
                    }


                } //foreach

            }//if has rows machine config
            else
            {
                Library.WriteInfoLog("no records in machines config-> 5 ");
            }
            

            timer.Start();
           
        }
        ***/
        //pure signal based pipe record
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ShutdownSignal)
            {
                return;
            }

            //updatesysstatus(true);

            timer.Stop();
            string err = string.Empty;
            DataSet ds = Library.GetData("Select Top 500 * From  ccmSignalDetection where Processed = 0 Order By LogDateTime,MachineIP Asc", sqlcnstr, out err);

            if (!string.IsNullOrEmpty(err))
            {
                Library.WriteInfoLog("Data Writer Service->Step-1->" + err);
                ShutdownSignal = false;
                timer.Start();
                return;
            }

            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {

                    if (dr["Signal"].ToString() == "0")
                    {
                         bool tmp = UpdateSignalRecords(Convert.ToDateTime(dr["LogDateTime"]), dr["MachineIP"].ToString(), false, "No Signal");
                         continue;
                    }
                    
                    string machineip = dr["MachineIP"].ToString();
                    
                    //get machineid from machineip
                    var result = (from a in clientlist
                                  where a.MachineIP.ToString() == machineip
                                  select a).SingleOrDefault();


                    //Library.WriteInfoLog("Try->" + result.MachineIP);

                    if (result != null)
                    {
                        WeightClient t = (WeightClient)result;
                        
                        string err1 = string.Empty;
                        //get lastparameters
                        CurrentSetting tmpset = GetLastPara(t.MachineID.ToString(), Convert.ToDateTime(dr["LogDateTime"]), out err1);
                        //Library.WriteInfoLog("Try->GetLastPara");
                       
                        StreamReceiver tmprec = new StreamReceiver(tmpset);
                        //Library.WriteInfoLog("Try->GetStreamReceiver");
                        tmprec.MachineID = t.MachineID;
                        tmprec.LogDateTime = Convert.ToDateTime(dr["LogDateTime"]);
                        
                        //string[] wt = dr["SignalMsg"].ToString().Split('#');
                        //string strwt = wt[0];
                        
                        double actwt = 0;
                        double.TryParse(dr["Weight"].ToString().Trim(),out actwt);
                        tmprec.ActWt = actwt;

                        //Library.WriteInfoLog("Try->Signal->" + t.TableName + "->" + t.MachineIP + "->" + tmprec.LogDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "->" + tmprec.ActWt.ToString());
                       
                        //try to save in db
                        string err2 = string.Empty;
                        string retjson = string.Empty;
                       
                        bool issaved = false;
                        bool isupdated = false;
                        try
                        {
                             issaved = SaveRecordToDB(tmprec, out err2, t.TableName, out retjson);
                            if (!string.IsNullOrEmpty(err2))
                            {
                                Library.WriteInfoLog("Step 3->Error->" + err2);
                               
                            }



                            isupdated = UpdateSignalRecords(tmprec.LogDateTime, t.MachineIP, issaved,"");

                             if (issaved)
                             {
                                 Library.WriteInfoLog("info->" + retjson.ToString());
                             }
                        }
                        catch (Exception ex)
                        {
                            Library.WriteInfoLog("Error->" + ex.Message);
                        }
                        
                    }

                }
            }
            timer.Start();
                        
        }

        #region NotUsed
        private static void WeighVsSign(string MachineIP, string cnstr)
        {
            
            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Process_WeightVsSignal", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@tMachineIP", SqlDbType.VarChar, 15).Value = MachineIP;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { }
            }
        }


        private static async Task WeighVsSignAsync(string MachineIP, string cnstr)
        {
            await Task.Delay(4000);
            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Process_WeightVsSignal", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@tMachineIP", SqlDbType.VarChar, 15).Value = MachineIP;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { }
            }
        }
        #endregion

        private static bool UpdateSignalRecords(DateTime tLogDateTime, string tMachineIP,bool tSaved,string remarks)
        {
            using (SqlConnection cn = new SqlConnection(sqlcnstr))
            {
                string sql = string.Empty;
                try
                {
                    cn.Open();
                    sql = "Update ccmSignalDetection set Remarks = '" + remarks.Trim().ToString() + "' " +
                        " ,Saved = '" + ((tSaved)?"1":"0") + "' " +
                        " ,Processed = 1 " +
                        " where " +
                        " LogDateTime ='" + tLogDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' " +
                        " and MachineIP ='" + tMachineIP.Trim() + "' ";
    
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Library.WriteInfoLog("Signal_Update_Error->" + ex.Message);
                    Library.WriteInfoLog("Signal_Update_Error->" + sql);
                }
            }

            return true;
        }


        private static void LogErrors(string error, string topic)
        {
            DateTime dt = DateTime.Now;
            //ccmErrLog
            //LogDateTime
            //LogTopic
            //LogDesc
            if (string.IsNullOrEmpty(error))
                return;

            if (String.IsNullOrEmpty(topic))
                topic = "ERROR";

            using (SqlConnection cn = new SqlConnection(sqlcnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                    }
                    catch
                    {
                        return;
                    }

                    cmd.Connection = cn;

                    string sql = "Insert into ccmErrLog (LogDateTime,LogTopic,LogDesc) values ('" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "','" + topic + "','" + error + "')";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static bool SaveRecordToDB(StreamReceiver t, out string err, string curTableNm,out string retjson)
        {
            err = string.Empty;
            retjson = string.Empty;
            

            if (t.ActWt <= 40)
            {
                err = "Low Weight (" + t.ActWt.ToString() + ") Signal Detected in " + t.MachineID;
                LogErrors(err, "ERROR");
                return false;
            }

            if (t.LogDateTime == DateTime.MinValue)
            {
                err = "DateTime Min Value Found...";
                LogErrors(err, "ERROR");
                return false;
            }

           
                        
            using (SqlConnection cn = new SqlConnection(sqlcnstr))
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    err = ex.Message.ToString();
                    LogErrors("Error While Opnining SQL Connection", "ERROR");
                    return false;
                }

                //pending
                //set logic to store in db with srno and pipeno.
                //MonthName : A,B,C,D,E,F,G,H,I,J,L,M
                //Day in two digit : 01,02,03,04,
                //MachineNo : X,XX
                //Dash : -
                //PipeNo : 1,22,333,
                //PipeStatus : 'OK' Default

                using (SqlCommand cmd = new SqlCommand())
                {

                    string sql = string.Empty;

                    //get lastdatetime from db
                    sql = "Select isnull(Max(LogDateTime), cast('1753-1-1' as datetime)) from [" + curTableNm + "];";
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    DateTime dt = (DateTime)cmd.ExecuteScalar();

                    if ((t.LogDateTime - dt).TotalSeconds < 5)
                    {

                        //err = "Duplicate Weight in less than 5 Sec (" + t.ActWt.ToString() + ") Signal Detected in " + t.MachineID;
                        //LogErrors(err, "ERROR");
                        err = "DUPLICATE";
                        return false;
                    }

                    if ((t.LogDateTime - dt).TotalSeconds < 15)
                    {
                        err = "Duplicate Weight in less than 15 Sec (" + t.ActWt.ToString() + ") Signal Detected in " + t.MachineID;
                        LogErrors(err, "ERROR");
                        return false;
                    }


                    //get the maxpipeno. of the day if (logdatetime.hour between 0 to 5 and minutes betwen 0 to 59:59 ) keep minus 1 day and search.
                    DateTime tDate = new DateTime();
                    string tShift = string.Empty;
                    int tSrNo = 0, tintSrno =0 ;
                    string PipeNumber = string.Empty;
                    string MachineNo = t.MachineID;
                    string MonthName = string.Empty;
                    string CurYear = string.Empty;

                    #region SetShift as per -Samaghogha
                    if (t.LogDateTime.Hour >= 0 && t.LogDateTime.Hour <= 5 && t.LogDateTime.Minute <= 59 && t.LogDateTime.Second <= 59)
                    {
                        tDate = t.LogDateTime.AddDays(-1).Date;
                        CurYear = tDate.Year.ToString("0000").Substring(3);
                        tShift = "C";
                    }
                    if (t.LogDateTime.Hour >= 6 && t.LogDateTime.Hour <= 13 && t.LogDateTime.Minute <= 59 && t.LogDateTime.Second <= 59)
                    {
                        tDate = t.LogDateTime.Date;
                        CurYear = tDate.Year.ToString("0000").Substring(3);
                        tShift = "A";
                    }
                    if (t.LogDateTime.Hour >= 14 && t.LogDateTime.Hour <= 21 && t.LogDateTime.Minute <= 59 && t.LogDateTime.Second <= 59)
                    {
                        tDate = t.LogDateTime.Date;
                        CurYear = tDate.Year.ToString("0000").Substring(3);
                        tShift = "B";
                    }
                    if (t.LogDateTime.Hour >= 22 && t.LogDateTime.Hour <= 23 && t.LogDateTime.Minute <= 59 && t.LogDateTime.Second <= 59)
                    {
                        tDate = t.LogDateTime.Date;
                        tShift = "C";
                    }
                    #endregion


                    
                    //srno-daily sequence no
                    sql = "Select isnull(Max(SrNo),0) + 1 from [" + curTableNm + "] Where tDate ='" + tDate.ToString("yyyy-MM-dd") + "'";
                                        
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    tSrNo = (int)cmd.ExecuteScalar();
                  
                    //internal serial no - customized , dia wise sequence
                    sql = "Select isnull(Max(IntSrNo),0) + 1 from [" + curTableNm + "] Where tDate ='" + tDate.ToString("yyyy-MM-dd") + "' and PipeDia ='" + t.Parameters.Size + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    tintSrno = (int)cmd.ExecuteScalar();

                    #region SetMonth
                    switch (tDate.Month)
                    {
                        case 1:
                            MonthName = "A";
                            break;
                        case 2:
                            MonthName = "B";
                            break;
                        case 3:
                            MonthName = "C";
                            break;
                        case 4:
                            MonthName = "D";
                            break;
                        case 5:
                            MonthName = "E";
                            break;
                        case 6:
                            MonthName = "F";
                            break;
                        case 7:
                            MonthName = "G";
                            break;
                        case 8:
                            MonthName = "H";
                            break;
                        case 9:
                            MonthName = "I";
                            break;
                        case 10:
                            MonthName = "J";
                            break;
                        case 11:
                            MonthName = "L";
                            break;
                        case 12:
                            MonthName = "M";
                            break;
                    }
                    #endregion

                    //Batch No - as per old 
                    //PipeNumber = CurYear + MonthName + tDate.ToString("dd") + MachineNo  + tintSrno.ToString("0000");

                    //curYear not required
                    PipeNumber = MonthName + tDate.ToString("dd") + MachineNo + tintSrno.ToString("0000");

                    t.Parameters.MouldNo = t.Parameters.MouldNo.Replace("'", "");
                    t.Parameters.MouldNo = t.Parameters.MouldNo.Replace('"', ' ');
                    t.Parameters.MouldNo = t.Parameters.MouldNo.Replace("&", "");
                    t.Parameters.MouldNo = t.Parameters.MouldNo.Replace("%", "");
                    t.Parameters.MouldNo = t.Parameters.MouldNo.Replace("`", "");
                    t.Parameters.MouldNo = t.Parameters.MouldNo.Replace(" ", "");
                    t.Parameters.MouldNo = t.Parameters.MouldNo.ToUpper();
                    t.Parameters.MouldNo = t.Parameters.MouldNo.Trim();

                    t.Parameters.Joint = t.Parameters.Joint.Replace("'", "");
                    t.Parameters.Joint = t.Parameters.Joint.Replace('"', ' ');
                    t.Parameters.Joint = t.Parameters.Joint.Replace("&", "");
                    t.Parameters.Joint = t.Parameters.Joint.Replace("%", "");
                    t.Parameters.Joint = t.Parameters.Joint.Replace("`", "");
                    t.Parameters.Joint = t.Parameters.Joint.Replace(" ", "");
                    t.Parameters.Joint = t.Parameters.Joint.ToUpper();
                    t.Parameters.Joint = t.Parameters.Joint.Trim();

                    if (t.Parameters.MouldNo.Length > 10)
                        t.Parameters.MouldNo = t.Parameters.MouldNo.Substring(0, 9);

                    if (t.Parameters.Joint.Length > 10)
                        t.Parameters.Joint = t.Parameters.Joint.Substring(0, 9);

                    string tPipeStatus = "OK";

                    if(t.Parameters.AlmMinWt > 0 && t.ActWt <= Convert.ToDouble(t.Parameters.AlmMinWt))
                            tPipeStatus = "LOW_WT_ALM";

                    if (t.Parameters.AlmMaxWt > 0 && t.ActWt >= Convert.ToDouble(t.Parameters.AlmMaxWt))
                        tPipeStatus = "OVER_WT_ALM";

                   

                    // LogDateTime,SrNo,PipeNumber,PipeClass,PipeLength,PipeDia,JointType,MouldNo,ActWt,MinWt,MaxWt,NomWt
                    string sql1 = "Insert into [" + curTableNm + "]  (tDate,tShift,SrNo,LogDateTime,IntSrNo,MachineNo,PipeNumber,PipeClass,PipeLength,PipeDia," +
                        " JointType,MouldNo,ActWt,MinWt,MaxWt,NomWt,AddDt,PipeStatus," +
                        " Material,Standard,OperatorCode,OperatorName,AlmMinWt,AlmMaxWt ) " +
                        " values ('" + tDate.ToString("yyyy-MM-dd") + "','" + tShift + "','" + tSrNo.ToString() + "','" + t.LogDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "','" + tintSrno.ToString() + "'," +
                        " '" + t.MachineID + "','" + PipeNumber + "','" + t.Parameters.Class + "','" + t.Parameters.Length.ToString() + "','" + t.Parameters.Size.ToString() + "','" + t.Parameters.Joint + "','" + t.Parameters.MouldNo + "'," +
                        " '" + t.ActWt.ToString() + "','" + t.Parameters.MinWt.ToString() + "','" + t.Parameters.MaxWt.ToString() + "','" + t.Parameters.NomWt.ToString() + "',GetDate(),'OK'," +
                        " '" + t.Parameters.Material + "','" + t.Parameters.Standard + "','" + t.Parameters.OperatorCode + "','" + t.Parameters.OperatorName + "','" + t.Parameters.AlmMinWt.ToString() + "','" + t.Parameters.AlmMaxWt.ToString() + "');";

                    try
                    {
                        cmd.CommandText = sql1;
                        cmd.ExecuteNonQuery();
                        t.SrNo = tSrNo;
                        t.PipeNumber = PipeNumber;
                        //this.LastPipeNo = tSrNo.ToString();
                        //this.LastWeight = t.ActWt.ToString();
                        //this.LastWeightTime = t.LogDateTime.ToString("HH:mm:ss");

                        if(tPipeStatus != "OK")
                        {
                            string sql2 = "Insert into ccmAlarm (tDate,tShift,MachineNo,SrNo,PipeNumber,PipeWt,AlmSent,AddDt,PipeDia,PipeClass,OperatorCode,OperatorName,PipeStatus) " +
                                " Values ('" + tDate.ToString("yyyy-MM-dd") + "','" + tShift + "','" + t.MachineID.ToString() + "'," +
                                " '" + t.SrNo.ToString() + "','" + t.PipeNumber + "','" + t.ActWt.ToString() + "',0,GetDate()," +
                                "'" + t.Parameters.Size + "','" + t.Parameters.Class + "','" + t.Parameters.OperatorCode + "','" + t.Parameters.OperatorName + "','" + tPipeStatus + "')";

                            try
                            {
                                cmd.CommandText = sql2;
                                cmd.ExecuteNonQuery();
                            }
                            catch(Exception ex)
                            {
                                err += "Alarm Error ->" + sql2 + "->" + ex.Message;
                                
                            }
                            
                            

                        }

                        retjson = JsonConvert.SerializeObject(t);

                        

                        return true;
                    }
                    catch (SqlException duex)
                    {

                        if (duex.Number == 2627)
                        {
                            //Violation of primary key. Handle Exception
                            err = "DUPLICATE";
                            return false;
                        }

                        err = duex.ToString();
                        err = err.Replace("'", "");
                        err = err.Replace('"', ' ');
                        err = err.Replace("&", "");
                        err = err.Replace("%", "");
                        err = err.Replace("`", "");
                        err = err.Trim();
                        return false;
                    }
                    catch (Exception ex)
                    {

                        err = ex.ToString();
                        err = err.Replace("'", "");
                        err = err.Replace('"', ' ');
                        err = err.Replace("&", "");
                        err = err.Replace("%", "");
                        err = err.Replace("`", "");
                        err = err.Trim();
                        return false;
                    }

                }//using command
            }//using connection

        }

        private static CurrentSetting GetLastPara(string MachineID,DateTime LastSaveTime, out string err)
        {
            string sql = "SELECT top 1 * from  ccmLastPara where   " +
                         "   Len(Rtrim([LastSize])) > 0 and " +
                         "   Len(Rtrim([LastLength])) > 0  and " +
                         "   Len(Rtrim([LastClass])) > 0 and  " +
                         "   LastMinWt > 0 and LastMaxWt > 0 and LastNomWt > 0 " +
                         "   and MachineID = '" + MachineID + "' and UpdDt <= '" + LastSaveTime.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                         "   order by [ID] desc ";

            
            err = string.Empty;
            DataSet ds  = Library.GetData(sql,sqlcnstr,out err);
            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            
            CurrentSetting tCurSetting = new CurrentSetting();

            if(!string.IsNullOrEmpty(err))
                Library.WriteInfoLog(err);
            
            if (hasRows)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    //Library.WriteInfoLog("Machine->" + dr["MachineID"].ToString());
                    //Library.WriteInfoLog("Size->" + dr["LastSize"].ToString());
                    //Library.WriteInfoLog("MaxWt->" + dr["LastMaxWt"].ToString());
                    //Library.WriteInfoLog("MinWt->" + dr["LastMinWt"].ToString());
                    //Library.WriteInfoLog("Class->" + dr["LastClass"].ToString());

                   

                    tCurSetting.Size = dr["LastSize"].ToString();
                    tCurSetting.Class = dr["LastClass"].ToString();
                    tCurSetting.Joint = dr["LastJoint"].ToString();
                    tCurSetting.MouldNo = dr["LastMould"].ToString();
                    tCurSetting.Material = (dr["LastMaterial"] == null ? "": dr["LastMaterial"].ToString());
                    tCurSetting.Standard = (dr["LastStandard"] == null ? "" : dr["LastStandard"].ToString());
                    tCurSetting.OperatorCode = ( dr["LastOperatorCode"] == null ? "" : dr["LastOperatorCode"].ToString());
                    tCurSetting.OperatorName = ( dr["LastOperatorName"] == null ? "" : dr["LastOperatorName"].ToString());
                    try
                    {
                        decimal tlength = 0;
                        decimal.TryParse(dr["LastLength"].ToString(), out tlength);
                        tCurSetting.Length = tlength;
                    }
                    catch (Exception ex)
                    {
                        Library.WriteInfoLog("Class->" + ex.Message.ToString());
                    }

                    try
                    {
                        decimal maxwt = 0;
                        decimal.TryParse(dr["LastMaxWt"].ToString(), out maxwt);
                        tCurSetting.MaxWt = maxwt;
                    }
                    catch (Exception ex)
                    {
                        Library.WriteInfoLog("MaxWt->" + ex.Message.ToString());
                    }

                    try
                    {
                        decimal minwt = 0;
                        decimal.TryParse(dr["LastMinWt"].ToString(), out minwt);
                        tCurSetting.MinWt = minwt;
                    }
                    catch (Exception ex)
                    {
                        Library.WriteInfoLog("MinWt->" + ex.Message.ToString());
                    }

                    try
                    {
                        decimal nomwt = 0;
                        decimal.TryParse(dr["LastNomWt"].ToString(), out nomwt);
                        tCurSetting.NomWt = nomwt;
                    }
                    catch (Exception ex)
                    {
                        Library.WriteInfoLog("NomWt->" + ex.Message.ToString());
                    }

                    try
                    {
                        decimal almminwt = 0;
                        decimal.TryParse(dr["LastAlmMinWt"].ToString(), out almminwt);
                        tCurSetting.AlmMinWt = almminwt;
                    }
                    catch (Exception ex)
                    {
                        Library.WriteInfoLog("AlmMinWt->" + ex.Message.ToString());
                    }

                    try
                    {
                        decimal almmaxwt = 0;
                        decimal.TryParse(dr["LastAlmMaxWt"].ToString(), out almmaxwt);
                        tCurSetting.AlmMinWt = almmaxwt;
                    }
                    catch (Exception ex)
                    {
                        Library.WriteInfoLog("AlmMaxWt->" + ex.Message.ToString());
                    }

                    //Library.WriteInfoLog("Last Parameters : " + tCurSetting.ToString());
                }
            }
            


            return tCurSetting;
        
        }

       
    }
}
