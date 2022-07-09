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
//using System.Net.Mail;
using Spire.Xls;
using Spire.License;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace CCMAutoMail
{
    public partial class Scheduler : ServiceBase
    {

        private static string sqlconfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "sql_connection.txt");
        
        private static string sqlcnstr = string.Empty;
        private static Boolean ShutdownSignal = false;
        private static System.Timers.Timer timer = new System.Timers.Timer();
        private static System.Timers.Timer timer_logCleaner = new System.Timers.Timer();
        

        static IDictionary<string, string> Shift = new Dictionary<string, string>();
        static IDictionary<string, string> Machine = new Dictionary<string, string>();
        static int LastexecutedReportID = 0; 

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
                        sql = "Update SysStatus set AutoMailSts = '" + (t ? 1 : 0) + "',AutoMailUpdDt = getdate()";
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

            try
            {
                sqlcnstr = File.ReadLines(sqlconfig).First();
               
            }
            catch (Exception ex)
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->Did not get SQL Connection Details");
                Library.WriteInfoLog("Auto Mail Scheduler Service->Service Stopped with Error");
                OnStop();
            }
            
            try
            {

                Library.WriteInfoLog("Auto Mail Scheduler Service->Started");
                string err = string.Empty;
                string sql = "Select * from ccmMachineConfig where 1 = 1";
                DataSet ds = Library.GetData(sql, sqlcnstr, out err);

                bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrows)
                {
                    string tmp = string.Empty;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tmp = "'" + dr["MachineName"].ToString() + "',";
                        Machine.Add(dr["MachineName"].ToString(), "'" + dr["MachineName"].ToString() + "'");
                    }

                    if (tmp.Length > 0)
                    {
                        string t = tmp.Substring(0, tmp.Length - 1);
                        Machine.Add("ALL", t);
                    }

                    Shift.Add("A", "'A'");
                    Shift.Add("B", "'B'");
                    Shift.Add("C", "'C'");
                    Shift.Add("ALL", "'A','B','C'");


                    Library.WriteInfoLog("Auto Mail Scheduler Service->Machine Para and Shift Para Set..");

                }
                else
                {
                    Library.WriteInfoLog("Auto Mail Scheduler Service->Did not get Machine Details from ccmMachineConfig");
                    OnStop();
                }



              

            }
            catch (Exception ex)
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->error->" + ex.Message.ToString());
                Library.WriteInfoLog("Auto Mail Scheduler Service->Service Stopped with Error");
                OnStop();
            }

            timer_logCleaner.AutoReset = true;
            timer_logCleaner.Interval = 60 * 1000;
            timer_logCleaner.Elapsed += timer_logCleaner_Elapsed;
            timer_logCleaner.Start();


            timer.AutoReset = true;
            timer.Interval = 5000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            ShutdownSignal = false;

            

            updatesysstatus(true);


            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
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

            updatesysstatus(false);

            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            Library.WriteInfoLog("Auto Mail Scheduler Service->Service Stopped");
        }

        static void timer_logCleaner_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ShutdownSignal)
            {
                return;
            }


            updatesysstatus(true);

            DateTime t1 = DateTime.Now;
            string err = string.Empty;

            string sql = "select id,LastExecutedOn from EmailSchedule where SchTime between '" + t1.ToString("HH:mm") + "' and '" + t1.AddMinutes(1).ToString("HH:mm") + "'";
            DataSet ds = Library.GetData(sql, sqlcnstr, out err);
            if (!string.IsNullOrEmpty(err))
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->Error While getting mail schedule.." + err);
                return;
            }

            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                timer_logCleaner.Stop();
                Library.WriteInfoLog("Auto Mail Scheduler Service->Executing reports");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    string err2 = string.Empty;

                    if (dr["LastExecutedOn"] != DBNull.Value)
                    {
                        DateTime lastexe = Convert.ToDateTime(dr["LastExecutedOn"]);
                        DateTime curtime = DateTime.Now;
                        TimeSpan ts = curtime - lastexe;
                        if (ts.TotalSeconds > 1 && ts.TotalSeconds <= 120)
                            continue;
                    }

                    emailreport(dr["id"].ToString(), out err2);

                    if (!string.IsNullOrEmpty(err2))
                    {
                        if (err2 == "send")
                            LogEmail(true, "", dr["id"].ToString());
                        else
                            LogEmail(false, err2, dr["id"].ToString());
                    }
                }
                
                timer_logCleaner.Start();
            }            
            
        }

        static void LogEmail(bool isSent, string err,string schid)
        {
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

                    string sql = "Insert into EmailScheduleLog (ExecutionDt,ScheduleID,EmailStatus,Error,AddDt) values (getdate(),'" + schid+ "','" + (isSent?"Sent":"Failed") + "','" + err.ToString() + "',GetDate())";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    sql = "update EmailSchedule set LastExecutedOn =getdate() where ID='" + schid + "'";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                }
            }
        }

        static void emailreport(string schid,out string err2)
        {
            err2 = string.Empty;
            #region PrimaryCheck

            //Email Config
            string tmpSmtpHost = string.Empty;
            int tmpSmtpPort = 0;

            bool IsTLS = false;

            string SenderEmail = string.Empty;
            string DisplayName = string.Empty;

            string AccountUserid = string.Empty;
            string AccountPass = string.Empty;

            string err = string.Empty;
            string sql = "Select Top 1 * from EmailConfig where isDefault = 1";
            DataSet ds = Library.GetData(sql, sqlcnstr, out err);
            if (!string.IsNullOrEmpty(err))
            {

                Library.WriteInfoLog("Auto Mail Scheduler Service->No Default Account Found.." + err);
                return;
            }
            else
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->Default Account Found.." + err);
            }

            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {

                Library.WriteInfoLog("Auto Mail Scheduler Service->Generating Email Account setting..");
                try
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tmpSmtpHost = dr["SmtpServer"].ToString();
                        tmpSmtpPort = Convert.ToInt32(dr["OutGoingPort"]);
                        IsTLS = Convert.ToBoolean(dr["IsTLS"]);
                        AccountUserid = dr["AccountUserID"].ToString();
                        AccountPass = dr["AccountPassword"].ToString();
                        DisplayName = dr["DisplayName"].ToString();
                        SenderEmail = dr["EmailAccount"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Library.WriteInfoLog("Auto Mail Scheduler Service->Error Email Account setting.." + ex.Message);
                }
                

            }
            else
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->No Default Account Found..");
                return;
            }


            //create email receipents
            string EmailTo = string.Empty;
            string EmailCC = string.Empty;
            string EmailBCC = string.Empty;

            sql = "Select  * from EmailRecipients where 1 = 1";
            ds = Library.GetData(sql, sqlcnstr, out err);
            if (!string.IsNullOrEmpty(err))
            {

                Library.WriteInfoLog("Auto Mail Scheduler Service->No Email Recipients Found.." + err);
                return;
            }
            else
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->configuring Email Recepient setting..");
            }

            hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["EmailType"].ToString().ToUpper())
                    {
                        case "TO":
                            EmailTo += dr["EmailID"].ToString() + ";";
                            break;
                        case "CC":
                            EmailCC += dr["EmailID"].ToString() + ";";
                            break;
                        case "BCC":
                            EmailBCC += dr["EmailID"].ToString() + ";";
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->No Email Recipients Found..");
                return;
            }

            //report parameters
            string ScheduleID = schid;
            string ReportID = string.Empty;
            string DatePara = string.Empty;
            string ShiftPara = string.Empty;
            string dbShiftPara = string.Empty;
            string dbMachinePara = string.Empty;
            string MachinePara = string.Empty;
            string EmailSubject = string.Empty;
            string ExportPara = string.Empty;
            DateTime tmpDate = new DateTime();

            sql = "Select  * from EmailSchedule where ID = '" + ScheduleID + "'";
            ds = Library.GetData(sql, sqlcnstr, out err);

            if (!string.IsNullOrEmpty(err))
            {

                Library.WriteInfoLog("Auto Mail Scheduler Service->No Email Schedule Found.." + err);
                return;
            }
            else
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->Email Schedule exists..");
            }


            hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                try
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ReportID = dr["EmailReportID"].ToString();
                        DatePara = (dr["DatePara"].ToString().ToUpper() == "CURRENT" ? DateTime.Now.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd"));
                        dbShiftPara = dr["ShiftPara"].ToString().ToUpper();
                        ShiftPara = Shift[dr["ShiftPara"].ToString().ToUpper()];
                        dbMachinePara = dr["MachinePara"].ToString().ToUpper();
                        MachinePara = Machine[dr["MachinePara"].ToString().ToUpper()];
                        tmpDate = (dr["DatePara"].ToString().ToUpper() == "CURRENT" ? DateTime.Now.Date : DateTime.Now.Date.AddDays(-1));
                        EmailSubject = dr["Subject"].ToString();
                        LastexecutedReportID = Convert.ToInt32(dr["ID"]);
                        ExportPara = dr["ExportPath"].ToString();
                    }
                    Library.WriteInfoLog("Auto Mail Scheduler Service->Setting Report's Parameters completed." );
                }
                catch (Exception ex)
                {
                    Library.WriteInfoLog("Auto Mail Scheduler Service->Setting Parameter Error->" + ex.Message);
                }
            }
            else
            {
                tmpDate = DateTime.Now;
                Library.WriteInfoLog("Auto Mail Scheduler Service->No Email current Schedule Found..");
                return;
            }

            //check report sql
            string ReportSQL = Library.GetDescription("Select ReportSQL from EmailReports Where ReportID = '" + ReportID + "'", sqlcnstr, out err);

            if (!string.IsNullOrEmpty(err))
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->getting reports sql->Error:" + err);
                return;
            }
            
            if (string.IsNullOrEmpty(ReportSQL))
            {
                Library.WriteInfoLog("Auto Mail Scheduler Service->No Report Found..");
                return;
            }

            #endregion
            

            if (dbMachinePara != "ALL")
            {
                try
                {
                    ReportSQL = ReportSQL.Replace("{date}", "'" + DatePara + "'");
                    ReportSQL = ReportSQL.Replace("{Machine}", MachinePara);
                    ReportSQL = ReportSQL.Replace("{Shift}", ShiftPara);
                }
                catch (Exception ex)
                {
                    Library.WriteInfoLog(ex.ToString());
                    return;
                }
                ds = Library.GetData(ReportSQL, sqlcnstr, out err);
                hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                if (!string.IsNullOrEmpty(err))
                {

                    Library.WriteInfoLog("Auto Mail Scheduler Service->No Data Found.." + err);


                    //Library.WriteInfoLog(ReportSQL);
                    return;
                }
                if (hasrows)
                {
                    Spire.Xls.Workbook wrkbook = new Workbook();
                    Worksheet sheet = wrkbook.Worksheets[0];
                    sheet.InsertDataTable(ds.Tables[0], true, 1, 1);

                    string filepath = Library.GetUserDataPath();
                    string filename = DatePara + "-Shift-" + dbShiftPara + "-Machine-" + dbMachinePara + ".xls";
                    string filefullpath = Path.Combine(filepath, filename );

                    if (File.Exists(filefullpath))
                        File.Delete(filefullpath);

                    wrkbook.SaveToFile(filefullpath, ExcelVersion.Version97to2003);
                    byte[] byteary = File.ReadAllBytes(filefullpath);
                    //MailAttachment m1 = new MailAttachment(readText,filename);

                    string sb = "<p>Dear Sir,</p>";
                    sb += "<p>Please find the attached file for your reference.</p>";
                    sb += "<p>*This is auto generated mail, please do not reply.</p>";
                    sb += "<p>Thanks</p>";

                    Email(EmailTo, EmailCC, EmailBCC, sb.ToString(), EmailSubject, SenderEmail, DisplayName, AccountUserid, AccountPass, tmpSmtpHost, tmpSmtpPort, filename,out err2, byteary);

                    if (!string.IsNullOrEmpty(ExportPara))
                    {
                        try
                        {
                            if (Directory.Exists(ExportPara))
                            {
                                string destfullpath = Path.Combine(ExportPara,filename);
                                File.Move(filefullpath,destfullpath);
                            }
                            else
                            {
                                Library.WriteInfoLog("Auto Mail Scheduler Service->Export Path does not exists.." );
                            }
                        }
                        catch (Exception ex)
                        {
                             Library.WriteInfoLog("Auto Mail Scheduler Service->File Save Error" + err );
                        }
                    }
                    
                }
                else
                {
                    string sb = "<p>Dear Sir,</p>";
                    sb += "<p>No data found.</p>";
                    sb += "<p>*This is auto generated mail, please do not reply.</p>";
                    sb += "<p>Thanks</p>";
                    Library.WriteInfoLog("Auto Mail Scheduler Service->No Data Found..Mail send without attachment");
                    Email(EmailTo, EmailCC, EmailBCC, sb.ToString(), EmailSubject, SenderEmail, DisplayName, AccountUserid, AccountPass, tmpSmtpHost, tmpSmtpPort, "",out err2);
                }
            }
            else
            {
                //ALL Machine make machine wise sheet wise report
                sql = "Select * from ccmMachineConfig where 1 = 1";
                ds = Library.GetData(sql, sqlcnstr, out err);
                if (!string.IsNullOrEmpty(err))
                {
                    Library.WriteInfoLog("Auto Mail Scheduler Service->ALL Machine's Report Process.." + err);
                    return;
                }

                hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                Spire.Xls.Workbook wrkbook = new Workbook();

                int sheetidx = -1;
                //insert new sheets - with number of machines
                if (hasrows)
                {
                    int mcnt = ds.Tables[0].Rows.Count ;
                    wrkbook.CreateEmptySheets(mcnt);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sheetidx += 1;
                        Worksheet sheet = wrkbook.Worksheets[sheetidx];
                        sheet.Name = "Machine_" + dr["MachineName"].ToString();                            
                    }
                }

                sheetidx = -1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        sheetidx += 1;
                        string tmpReportSQL = ReportSQL.Replace("{date}", "'" + DatePara + "'");
                        tmpReportSQL = tmpReportSQL.Replace("{Machine}", "'" + dr["MachineName"].ToString() + "'");
                        tmpReportSQL = tmpReportSQL.Replace("{Shift}", ShiftPara);

                        DataSet ds2 = Library.GetData(tmpReportSQL, sqlcnstr, out err);
                        hasrows = ds2.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

                        if (hasrows)
                        {
                            Worksheet sheet = wrkbook.Worksheets[sheetidx];
                            sheet.Name = "Machine_" + dr["MachineName"].ToString();
                            sheet.InsertDataTable(ds2.Tables[0], true, 1, 1);                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Library.WriteInfoLog(ex.ToString());
                        continue;
                    }
                    
                } //for each

                if (sheetidx > -1)
                {
                    string filepath = Library.GetUserDataPath();
                    string filename = DatePara + "-Shift-" + dbShiftPara + "-Machine-" + dbMachinePara + ".xls";
                    string filefullpath = Path.Combine(filepath,filename);
                    if (File.Exists(filefullpath))
                        File.Delete(filefullpath);

                    wrkbook.SaveToFile(filefullpath, ExcelVersion.Version97to2003);
                    byte[] byteary = File.ReadAllBytes(filefullpath);
                    //MailAttachment m1 = new MailAttachment(readText, filename);
                    //MimeEntity m2 = new MimeEntity();
                   
                    string sb = "<p>Dear Sir,</p>";
                    sb += "<p>Please find the attached file for your reference.</p>";
                    sb += "<p>*This is auto generated mail, please do not reply.</p>";
                    sb += "<p>Thanks</p>";

                    Email(EmailTo, EmailCC, EmailBCC, sb.ToString(), EmailSubject, SenderEmail, DisplayName, AccountUserid, AccountPass, tmpSmtpHost, tmpSmtpPort, filename,out err2, byteary);

                    if (!string.IsNullOrEmpty(ExportPara))
                    {
                        try
                        {
                            if (Directory.Exists(ExportPara))
                            {
                                string destfullpath = Path.Combine(ExportPara,filename);
                                if (File.Exists(destfullpath))
                                    File.Delete(destfullpath);

                                File.Move(filefullpath,destfullpath);
                            }
                            else
                            {
                                Library.WriteInfoLog("Auto Mail Scheduler Service->Export Path does not exists.." );
                            }
                        }
                        catch (Exception ex)
                        {
                             Library.WriteInfoLog("Auto Mail Scheduler Service->File Move Error" + err );
                        }
                    }
                }
                else
                {
                    string sb = "<p>Dear Sir,</p>";
                    sb += "<p>No data found.</p>";
                    sb += "<p>*This is auto generated mail, please do not reply.</p>";
                    sb += "<p>Thanks</p>";
                    Library.WriteInfoLog("Auto Mail Scheduler Service->No Data Found..Mail send without attachment");
                    Email(EmailTo, EmailCC, EmailBCC, sb.ToString(), EmailSubject, SenderEmail, DisplayName, AccountUserid, AccountPass, tmpSmtpHost, tmpSmtpPort,"", out err2);
                }
                

            }

            
            
            
            
            

        }

        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ShutdownSignal)
            {
                return;
            }

            string err = string.Empty;

            updatesysstatus(true);
                        
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


        public static void Email(string to,
                                 string cc,
                                 string bcc,
                                 string body,
                                 string subject,
                                 string fromAddress,
                                 string fromDisplay,
                
                                 string credentialUser,
                                 string credentialPassword,
                                 string smtphost,                                 
                                 int smtpport,
                                 string attachedfilename,
                                 out string err,
                                 params byte[] attachment
                                 
            )
        {
            err = string.Empty;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", fromAddress));

                message.Subject = subject;

                string[] mailto = to.Split(';');
                string[] mailcc = cc.Split(';');
                string[] mailbcc = bcc.Split(';');


                foreach (string tto in mailto)
                {
                    if (!string.IsNullOrWhiteSpace(tto))
                    {
                        message.To.Add(new MailboxAddress("", tto.ToString()));
                    }

                }

                foreach (string tcc in mailcc)
                {
                    if (!string.IsNullOrWhiteSpace(tcc))
                    {
                        message.Cc.Add(new MailboxAddress("", tcc.ToString()));
                    }

                }

                foreach (string tbcc in mailbcc)
                {
                    if (!string.IsNullOrWhiteSpace(tbcc))
                    {
                        message.Bcc.Add(new MailboxAddress("", tbcc.ToString()));
                    }

                }

                if (mailto.Count() <= 0 && to.Trim().Length > 0)
                {
                    message.To.Add(new MailboxAddress("", to.ToString()));
                    
                }

                if (mailcc.Count() <= 0 && cc.Trim().Length > 0)
                {
                    
                    message.Cc.Add(new MailboxAddress("", cc.ToString()));
                }

                if (mailbcc.Count() <= 0 && bcc.Trim().Length > 0)
                {
                    message.Bcc.Add(new MailboxAddress("", bcc.ToString()));
                    
                }

                var builder = new BodyBuilder();
                builder.HtmlBody = body;

                if (!string.IsNullOrEmpty(attachedfilename))
                {
                    //builder.Attachments.Add(attachedfilename);
                    builder.Attachments.Add(attachedfilename, attachment);
                }
                   

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(smtphost, smtpport, MailKit.Security.SecureSocketOptions.StartTls);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(credentialUser, credentialPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

               
                err = "send";
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(1024);
                
                sb.Append("\nTo:" + to);
                sb.Append("\nCC:" + cc);
                sb.Append("\nBCC:" + bcc);
                sb.Append("\nbody:" + body);
                sb.Append("\nsubject:" + subject);
                sb.Append("\nfromAddress:" + fromAddress);
                sb.Append("\nfromDisplay:" + fromDisplay);

                Library.WriteInfoLog("Auto Mail Scheduler Service->Error.." + ex.ToString());
                Library.WriteInfoLog(sb.ToString());
                err = ex.Message.ToString();
            }
        }
        

       
    }
}
