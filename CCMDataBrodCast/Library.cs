using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using MSWinsockLib;

using RabbitMQ.Client;
using System.Threading;

namespace CCMDataBrodCast
{
    internal class Library
    {
        
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string basepath = AppDomain.CurrentDomain.BaseDirectory;
                string fullpath = Path.Combine(basepath, dt.ToString("yyyyMMdd") + ".txt");
                sw = new StreamWriter(fullpath, true);
                sw.WriteLine((DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + ex.ToString().Trim()));
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }

        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                DateTime dt = new DateTime() ;
                dt = DateTime.Now;
                string basepath = AppDomain.CurrentDomain.BaseDirectory;
                string fullpath = Path.Combine(basepath, dt.ToString("yyyyMMdd") + ".txt");
                sw = new StreamWriter(fullpath, true);

                sw.WriteLine((DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + Message));
                sw.Flush();
                sw.Close();
            }
            catch
            {
                
            }
        }

        public static string GetUserDataPath()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, "CCMDataLogger");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }


        public static string GetDescription(string sql, string ConnectionString, out string err)
        {
            object result;
            err = string.Empty;

            string returndesc = string.Empty;
            if (string.IsNullOrEmpty(sql))
            {
                return returndesc;
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                return returndesc;
            }

            if (sql.Contains("insert"))
            {
                return returndesc;
            }
            if (sql.Contains("update"))
            {
                return returndesc;
            }
            if (sql.Contains("delete"))
            {
                return returndesc;
            }

            if (!sql.ToUpper().Trim().Contains("TOP 1"))
            {
                sql = sql.ToUpper().Replace("SELECT", "SELECT TOP 1 ");
            }

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(sql, conn) { CommandType = CommandType.Text };


            try
            {
                conn.Open();
                command.CommandTimeout = 1500;
                result = command.ExecuteScalar();

                if (result != null)
                    returndesc = Convert.ToString(result);

                conn.Close();
            }
            catch (SqlException ex) { err = ex.Message.ToString(); }
            catch (Exception ex) { err = ex.Message.ToString(); }
            finally
            {
                conn.Close();
            }

            return returndesc;
        }

        public static DataSet GetData(string sql, string ConnectionString, out string err)
        {
            err = string.Empty;
            DataSet Result = new DataSet();
            if (string.IsNullOrEmpty(sql))
            {
                return Result;
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
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
                conn.Close();
            }

            return Result;
        }
        //public static string ConStr 
        //{ get; set; }

    }

    public class ClientPara
    {
        
        public DateTime LogDateTime { get; set; }
        public string DataString { get; set; }
        public string MachineIP { get; set; }
        public int MachinePort { get; set; }
        public int MachineStatus { get; set; }
        public DateTime LastLogDateTime { get; set; }
        public ClientPara()
        {
            this.LogDateTime = DateTime.Now;
            this.LastLogDateTime = this.LogDateTime;
            this.DataString = "";
            this.MachineID = "";
        }
        public string MachineID { get; set; }
        public override string ToString()
        {
            string retstr = string.Empty;

            if (this.DataString.Contains("#1#"))
            {
                 retstr = "LOG_{0}${1:yyyy-MM-dd HH:mm:ss}${2}${3}${4}${5}" + Environment.NewLine;
            }
            else
            {
                 retstr = "GEN_{0}${1:yyyy-MM-dd HH:mm:ss}${2}${3}${4}${5}" + Environment.NewLine;
                
            }
            
            retstr = string.Format(retstr, this.MachineID, this.LogDateTime, this.MachineIP, this.MachinePort, string.IsNullOrEmpty(this.DataString) ? "       0.0#0#" : this.DataString, this.MachineID);
            return retstr;
        }

        

    }

    public class wsckclient
    {
        public string RemoteIP {get; set;}
        public int RemotePort { get; set; }
        public short State { get; set; }
        
        public string RBMQServerUri { get; set; }
        public string MachineID { get; set; }

        private bool _manualdisconnect = false;

        private System.Timers.Timer _chktimer = new System.Timers.Timer();

       
        private ClientPara para = new ClientPara();

        private ConnectionFactory factory = new ConnectionFactory();
        private IConnection conn;
        private IModel channel;

        private bool _init = false;
        private MSWinsockLib.Winsock _tClient = new Winsock();

        public string SQLConnection { get; set; }

        public wsckclient()
        {
            this.RemoteIP = "";
            this.RemotePort = 0;
            this.RBMQServerUri = "";
            this.SQLConnection = "";
            para.DataString = "";
            para.LastLogDateTime = DateTime.Now;
            para.LogDateTime = DateTime.Now;
            para.MachineID = MachineID;
            para.MachineIP = RemoteIP;
            para.MachinePort = RemotePort;
            para.MachineStatus = this.State;
            this._tClient = new Winsock();
        }

        public bool init()
        {
            if (this.MachineID == string.Empty || this.RemoteIP == string.Empty || this.RemotePort == 0 || this.RBMQServerUri == string.Empty )
            {
                _init = false;
                Library.WriteErrorLog("Client Parameters is mising... : " + this._tClient.ToString());
                return false;
            }
            else
            {

                try
                {
                    _init = true;
                    this._chktimer.Interval = 10000;
                    this._chktimer.AutoReset = true;
                    this._chktimer.Elapsed += new System.Timers.ElapsedEventHandler(_chktimer_Elapsed);
                    this._chktimer.Start();
                    
                    Library.WriteErrorLog("Heartbeat Started... for : " + this.RemoteIP);
                    
                    this._tClient.RemoteHost = this.RemoteIP;
                    this._tClient.RemotePort = this.RemotePort;
                    this._tClient.Protocol = ProtocolConstants.sckTCPProtocol;


                   
                }
                catch (Exception ex)
                {
                    Library.WriteErrorLog("Client Init Error : " + ex.StackTrace);
                    
                }
                Library.WriteErrorLog("Connecting : " + this.RBMQServerUri);
                Start_RBMQ_Client(this.RBMQServerUri);
                return true;

            }
        }

        public bool Connect()
        {
            this._chktimer.Enabled = false;

            this._manualdisconnect = false;
            para.MachineID = this.MachineID;
            para.MachineIP = this.RemoteIP;
            para.MachinePort = this.RemotePort;
            para.LastLogDateTime = DateTime.Now;
            para.MachineStatus = 0;
            para.DataString = "";
  
            try
            {

                try
                {
                    this._tClient.Error -= _tClient_Error;
                    this._tClient.DataArrival -= _tClient_DataArrival;
                    this._tClient.Close();
                
                }catch{}

                
                this._tClient.RemoteHost = this.RemoteIP;
                this._tClient.RemotePort = this.RemotePort;
                this._tClient.Connect();
                this._tClient.Error +=   _tClient_Error;                    
                this._tClient.DataArrival += _tClient_DataArrival;
                    
                this.State = _tClient.State;
                Library.WriteErrorLog("Client Connected to : " + this.RemoteIP + ":" + this.RemotePort.ToString());
                       
            }
            catch (Exception ex)
            {
                Library.WriteErrorLog("Client Connection Error : " + ex.Message.ToString());
            }

            this._chktimer.Enabled = true;

            
            return true;
        }

        public bool Disconnect()
        {
            this._chktimer.Stop();
            
            this._tClient.Error -= _tClient_Error;
            this._tClient.DataArrival -= _tClient_DataArrival;
            this._tClient.Close();
            
            this.State = 0;
            

            Library.WriteErrorLog("Client Disconnected from : " + this.RemoteIP + ":" + this.RemotePort.ToString());

            this._manualdisconnect = true;
           


            STOP_RBMQ_Client();

            return true;

        }

        private void _tClient_DataArrival(int bytesTotal)
        {
            this.State = 1;

            para.MachineID = MachineID;
            para.MachineIP = RemoteIP;
            para.MachinePort = RemotePort;

            

            //handle the data..... hear
            string inputstr = "";
            object data = (object)inputstr;
            try
            {
                _tClient.GetData(ref data);
            }
            catch (Exception ex)
            {
                Library.WriteErrorLog("Client Data Reading Error : " + this.RemoteIP + ":" + this.RemotePort.ToString());

            }
            
            string str1 = (string)data;
            //Library.WriteErrorLog("Client Data received : " + this.RemoteIP + ":" + this.RemotePort.ToString() + "->" + str1);
            
            if(!string.IsNullOrEmpty(str1))
            {
                Publish_RBMQ_MSG("1", "STS");

                //@     1679 #0#


                string[] msg = str1.Split('@');
                foreach(string t in msg)
                {
                   
                    para.LogDateTime = DateTime.Now;
                    para.LastLogDateTime = para.LogDateTime;
                    para.MachineStatus = 1;
                    para.DataString = t;

                    
                    
                    SaveSignalData(para.DataString);
                        
                    Publish_RBMQ_MSG(para.ToString(), "GEN");
                    if (para.DataString.Contains("#1#"))
                    {
                        Publish_RBMQ_MSG(para.ToString(), "LOG");
                       
                    }
                }
            }           
            
        }

        private void SaveSignalData(string tMsg)
        {
            if (string.IsNullOrEmpty(tMsg))
            {
                return;
            }

            using (SqlConnection cn = new SqlConnection(this.SQLConnection))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {

                        string[] tmp = tMsg.Split('#');
                        double wt = 0;
                        double.TryParse(tmp[0].ToString(), out wt);

                        if (wt < 0)
                            wt = 0;

                        if (wt >= 0 )
                        {
                            string sql = string.Empty;

                            //do not store 0 wt with no signal
                            //if (wt == 0 && tmp[1].ToString().Trim() == "0" )
                            //    sql = "Insert into ccmSignalDetection (LogDateTime,MachineIP,SignalMsg,Signal,Weight,Processed) values (getdate(),'" + this.RemoteIP + "','" + tMsg + "','" + tmp[1].ToString() + "','" + wt.ToString() + "',1)";
                            //else 
                            if (wt == 0 && tmp[1].ToString().Trim() == "1")
                                sql = "Insert into ccmSignalDetection (LogDateTime,MachineIP,SignalMsg,Signal,Weight,Processed) values (getdate(),'" + this.RemoteIP + "','" + tMsg + "','" + tmp[1].ToString() + "','" + wt.ToString() + "',1)";
                            else if (wt > 0 && tmp[1].ToString().Trim() == "1")
                                sql = "Insert into ccmSignalDetection (LogDateTime,MachineIP,SignalMsg,Signal,Weight,Processed) values (getdate(),'" + this.RemoteIP + "','" + tMsg + "','" + tmp[1].ToString() + "','" + wt.ToString() + "',0)";
                            
                            cmd.CommandText = sql;
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }
                                                
                    }
                }
                catch (Exception ex)
                {
                    //Library.WriteErrorLog("Signal Saving Error : " + this.RemoteIP + ":" + this.RemotePort.ToString() + "->" + ex.Message);
                }
            }
        }
        
        private void _tClient_Error(short Number, ref string Description, int Scode, string Source, string HelpFile, int HelpContext, ref bool CancelDisplay)
        {
            this.State = 0;
           
            Publish_RBMQ_MSG("0", "STS");
            Publish_RBMQ_MSG("Client Error : " + this.RemoteIP + ":" + this.RemotePort.ToString() + "->" + Description, "ERR");
            
            Library.WriteErrorLog("Client Error : " + this.RemoteIP + ":" + this.RemotePort.ToString() + "->" + Description);
            para.MachineStatus = 0;
            //this.Connect();
        }

        private void _chktimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
                       
            Publish_RBMQ_MSG("1", "HB");
           
            if ((DateTime.Now - this.para.LastLogDateTime).TotalMinutes > 1)
            {
                this.State = 0;
                Publish_RBMQ_MSG("0", "STS");
                this.Connect();
            }
        }

        public override string ToString()
        {
            string retstr = string.Empty;           
            retstr = "Client_Para->MachineID:{0},Client IP:{1},Client Port:{2},RBMQServerURI:{3}"; 
            retstr = string.Format(retstr, this.MachineID, this.RemoteIP, this.RemotePort,this.RBMQServerUri);
            return retstr;
        }


        public bool Start_RBMQ_Client(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                uri = "amqp://anand:anand123@172.16.12.44:5672/CCM";
            }
            
            try
            {
                //uri = "amqp://anand:anand123@172.16.12.44:5672/CCM";
                
                //"amqp://user:pass@hostName:port/vhost";
                //Property 	Default Value
                //Username 	"guest"
                //Password 	"guest"
                //Virtual host 	"/"
                //Hostname 	"localhost"
                //port 	5672 for regular connections, 5671 for connections that use TLS

                Uri t = new Uri(uri);
                factory.Uri = t;
                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);

               
                try
                {
                    conn = factory.CreateConnection();
                    channel = conn.CreateModel();
                    channel.ExchangeDeclare(exchange: this.MachineID, type: "direct",durable:true,autoDelete:false);
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ect)
                {
                    
                    Library.WriteErrorLog("Rabbit MQ Connection Error : " + t.AbsoluteUri + "->" + ect.Message);
                    Thread.Sleep(5000);
                    // apply retry logic
                    Start_RBMQ_Client(uri);
                }

            }
            catch (Exception ex)
            {
               
                return false;
            }
            return true;
        }

        public bool Publish_RBMQ_MSG(string message,string routingkey)
        {
            if (!string.IsNullOrEmpty(message))
            {

                try
                {

                    
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: this.MachineID,
                                         routingKey: routingkey,
                                         basicProperties: null,
                                         body: body);


                    //Library.WriteErrorLog("Pulibshed : " + message);
                    return true;
                }
                catch (Exception ex)
                {
                    Library.WriteErrorLog("Error From Brodcaster");
                    Library.WriteErrorLog(ex);
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        public bool STOP_RBMQ_Client()
        {
            try
            {
                if (conn != null && channel != null)
                {

                    channel.Close();
                    conn.Close();

                }

            }
            catch (Exception ex) { }
            finally
            {
                channel.Close();
                conn.Close();
            }

            return true;
        }
    }

}
