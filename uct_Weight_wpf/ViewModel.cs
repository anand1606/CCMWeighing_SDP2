using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using Newtonsoft.Json;

namespace uct_Weight_wpf
{

    

    public class ViewModel : IDisposable, INotifyPropertyChanged 
    {

        private string curValue = "0";
        private bool curStatus = false;
        private bool shutdown = false;
        private System.Timers.Timer curTimer = new System.Timers.Timer();


        private List<string>  _sizes = new List<string>();        
        private List<string> _lengths = new List<string>();
        private List<string> _classes = new List<string>();
        private List<string> _materials = new List<string>();
        private List<string> _standards = new List<string>();

        private bool isDataSaveRequired = false;
        private Queue<StreamReceiver> _queue = new Queue<StreamReceiver>();

        private string curSize  = string.Empty;
        private string curLength = string.Empty;
        private string curClass = string.Empty;
        private string indcolor = "White";

        private string curMould = string.Empty;
        private string curJoint = string.Empty;
        private string curMachine = string.Empty;
        private string curEmpCode = string.Empty;
        private string curEmpName = string.Empty;
        private string curPunch = string.Empty;
        private string curOperatorDetails = string.Empty;

        private double curMinWt = 0;
        private double curMaxWt = 0;
        private double curNomWt = 0;
        private double curAlmMinWt = 0;
        private double curAlmMaxWt = 0;
        private string _AlarmRangeText = string.Empty;

        private string curMaterial = string.Empty;
        private string curStandard = string.Empty;

        private string lastwt = string.Empty;
        private string lastwttime = string.Empty;
        private string lastpipeno = string.Empty;

        private string RBMQServer = string.Empty;
             

        private string curWeightClient = string.Empty;

        private bool isstarted = false;
        

        private CancellationTokenSource logtokensrc;
        private CancellationToken logtoken;
        private Task DataSaveTask;

        

        private static ConnectionFactory factory = new ConnectionFactory();
        private static IConnection conn;
        private EventingBasicConsumer consumer;
        private IModel channel;
        public event PropertyChangedEventHandler PropertyChanged;


        private string curTableNm = string.Empty;
        private string curSQLConStr = string.Empty;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string GetCurrentSQLConn
        {
            get
            {
                return curSQLConStr;
            }
        }

        public string CurrentWeight
        {
            get
            {
                return this.curValue;
            }

            set
            {
                if (value != this.curValue)
                {
                    this.curValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool CurrentStatus
        {
            get
            {
                return this.curStatus;
            }

            set
            {
                if (value != this.curStatus)
                {
                    this.curStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public List<string> Sizes
        {
            get
            {
                
                return _sizes;
            }

            set
            {
                _sizes = value;
                NotifyPropertyChanged();

            }
        }

        public List<string> Classes
        {
            get
            {
                return this._classes;
            }

            set
            {
                this._classes = value;
                NotifyPropertyChanged();

            }
        }

        public List<string> Lengths
        {
            get
            {
                return this._lengths;
            }

            set
            {
                this._lengths = value;
                NotifyPropertyChanged();

            }
        }

        public List<string> Materials
        {
            get { return this._materials;}
            set { this._materials = value; NotifyPropertyChanged(); }
        }

        public List<string> Standards
        {
            get { return this._standards;}
            set { this._standards = value; NotifyPropertyChanged(); }
        }

        public double CurrentMinWt
        {
            get
            {
                return this.curMinWt;
            }

            set
            {
                if (value != this.curMinWt)
                {
                    this.curMinWt = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public double CurrentMaxWt
        {
            get
            {
                return this.curMaxWt;
            }

            set
            {
                if (value != this.curMaxWt)
                {
                    this.curMaxWt = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public double CurrentNomWt
        {
            get
            {
                return this.curNomWt;
            }

            set
            {
                if (value != this.curNomWt)
                {
                    this.curNomWt = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public double CurrentAlmMinWt
        {
            get
            {
                return this.curAlmMinWt;
            }

            set
            {
                if (value != this.curAlmMinWt)
                {
                    this.curAlmMinWt = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public double CurrentAlmMaxWt
        {
            get
            {
                return this.curAlmMaxWt;
            }

            set
            {
                if (value != this.curAlmMaxWt)
                {
                    this.curAlmMaxWt = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public string CurAlramWtRangeDesc
        {
            get { return this._AlarmRangeText; }

            set
            {
                if (value != this._AlarmRangeText)
                {
                    this._AlarmRangeText = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string CurrentJoint
        {
            get
            {
                return this.curJoint;
            }

            set
            {
                if (value != this.curJoint)
                {
                    this.curJoint = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public string CurrentMould
        {
            get
            {
                return this.curMould;
            }

            set
            {
                if (value != this.curMould)
                {
                    this.curMould = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public string Status
        {
            get
            {
                return this.indcolor;
            }

            set
            {
                if (value != this.indcolor)
                {
                    this.indcolor = value;
                    NotifyPropertyChanged();
                    
                }
            }
        }

        public string CurrentIP
        {
            get
            {
                var t = CurrentWeightClient.Split(':');
                if (t.Length > 0)
                    return t[0].ToString();
                else
                    return string.Empty;
            }
        }

        public string CurrentPort
        {
            get
            {
                var t = CurrentWeightClient.Split(':');
                if (t.Length > 0)
                    return t[1].ToString();
                else
                    return string.Empty;
            }
        }

        public string LastPipeNo
        {
            get
            {
                return this.lastpipeno;
            }

            set
            {
                this.lastpipeno = value;
                NotifyPropertyChanged();

            }
        }

        public string LastWeight
        {
            get
            {
                return this.lastwt;
            }

            set
            {
                this.lastwt = value;
                NotifyPropertyChanged();
                
            }
        }

        public string LastWeightTime
        {
            get
            {
                return this.lastwttime;
            }

            set
            {
                this.lastwttime = value;
                NotifyPropertyChanged();

            }
        }

        public string CurrentSize
        {
            get
            {
                return curSize;
            }
            set
            {
                if (value != this.curSize)
                {
                    this.curSize = value;
                    NotifyPropertyChanged();
                    if(this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public string CurrentLength
        {
            get
            {
                return curLength;
            }
            set
            {
                if (value != this.curLength)
                {
                    this.curLength = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }
        
        public string CurrentClass
        {
            get
            {
                return curClass;
            }
            set
            {
                if (value != this.curClass)
                {
                    this.curClass = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public string CurrentMaterial
        {
            get
            {
                return curMaterial;
            }
            set
            {
                if (value != this.curMaterial)
                {
                    this.curMaterial = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public string CurrentStandard
        {
            get
            {
                return curStandard;
            }
            set
            {
                if (value != this.curStandard)
                {
                    this.curStandard = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }
        public string CurrentMachine
        {
            get
            {
                return this.curMachine;
            }

            set
            {
                if (value != this.curMachine)
                {
                    this.curMachine = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public string CurrentOperator
        {
            get
            {
                return curEmpCode;
            }
            set
            {
                if (value != this.curEmpCode)
                {
                    this.curEmpCode = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }
        public string CurrentOperatorName
        {
            get
            {
                return this.curEmpName;
            }

            set
            {
                if (value != this.curEmpName)
                {
                    this.curEmpName = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }

        public string CurrentOperatorDetails
        {
            get 
            {
                return curOperatorDetails;                    
            }

            set {
                if(value != this.curOperatorDetails)
                {
                    this.curOperatorDetails = value;
                    NotifyPropertyChanged();
                    //if (this.isDataSaveRequired)
                    //    SaveLastParameters();
                }
                
            }
            
        }


        public string CurrentPunch
        {
            get { return this.curPunch; }
            set 
            { 
                if (value != this.curPunch)
                {
                    this.curPunch = value;
                    NotifyPropertyChanged();
                    if (this.isDataSaveRequired)
                        SaveLastParameters();
                }
            }
        }


        public string CurrentWeightClient
        {
            get
            {
                return this.curWeightClient;
            }

            set
            {
                if (value != this.curWeightClient)
                {
                    this.curWeightClient = value;
                    NotifyPropertyChanged();
                }
            }
        }

                
        public bool Connect(string tRBMQServer,string tSQLConnection,
            List<string> tLengths, List<string> tClasses, List<string> tSizes , List<string> tMaterials, List<string> tStandards,
            string tMachineID, string tTcpWeightClient, string tCurrentTable, bool tisDataSaveRequired = false
            )
        {

            
            
            this.RBMQServer = tRBMQServer;
            this.curSQLConStr = tSQLConnection;
            this.CurrentMachine = tMachineID;

            if(string.IsNullOrEmpty(tTcpWeightClient.ToString().Trim()))
            {
                //get the machine ip from ccmMachineConfig table
                string err = string.Empty;
                DataSet ds = GetData("Select * from ccmMachineConfig Where MachineName ='" + tMachineID + "'", tSQLConnection, out err);
                bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrows)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    tTcpWeightClient = dr["MachineIP"].ToString() + ":" + dr["MachinePort"].ToString();
                    tCurrentTable = dr["TableName"].ToString();
                }
                else
                {
                    LogErrors("Controller Configuraiton Error->Please Config in ccmMachineConfig : " + this.CurrentMachine, "ERROR");
                }
            }

            this.CurrentWeightClient = tTcpWeightClient;
            this.CurrentWeight = "0.0";
            this.curTableNm = tCurrentTable;
            this.isDataSaveRequired = tisDataSaveRequired;
            this.CurrentStatus = true;
            Lengths = tLengths;
            Classes = tClasses;
            Sizes = tSizes;
            Materials = tMaterials;
            Standards = tStandards;
            LoadLastParameters();
            Start_RBMQ_Client();
            shutdown = false;

            logtokensrc = new CancellationTokenSource();
            logtoken = logtokensrc.Token;

            LogErrors("Data Saving Server->Application Start Command Received for :" + this.CurrentMachine, "ERROR");


            DataSaveTask = Task.Factory.StartNew(() =>
                    {
                        //Library.WriteErrorLog("Starting Status Brodcasting");
                        while (true)
                        {
                            // Poll on this property if you have to do 
                            // other cleanup before throwing. 
                            if (logtoken.IsCancellationRequested)
                            {
                                // Clean up here, then...

                                Thread.Sleep(100);

                                break;
                            }

                            string sql = "Select top 1 * from " + this.curTableNm + " Order by LogDateTime Desc";
                            string err = string.Empty;
                            DataSet ds = GetData(sql, curSQLConStr, out err);
                            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                            if (hasRows)
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    this.LastPipeNo = dr["PipeNumber"].ToString();
                                    this.LastWeightTime = Convert.ToDateTime(dr["LogDateTime"]).ToString("HH:mm:ss");
                                    this.LastWeight = dr["ActWt"].ToString();
                                }
                            }

                            Thread.Sleep(1000);

                        }//while true;
                    },
                    logtoken, TaskCreationOptions.LongRunning, TaskScheduler.Default
                    
                    );
            this.isstarted = true;
            return true;
        }

        private void LoadLastParameters()
        {

            if (string.IsNullOrEmpty(this.curSQLConStr) || string.IsNullOrEmpty(this.curTableNm) || string.IsNullOrEmpty(CurrentMachine))
                return;

           
            string sql = "Select Top 1 * from ccmLastPara where MachineID ='" + CurrentMachine + "' Order by ID Desc";
            string err;
            DataSet ds = GetData(sql, curSQLConStr, out err);
            if (!string.IsNullOrEmpty(err))
                return;

            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                
                CurrentClass = dr["LastClass"].ToString();
                CurrentLength = dr["LastLength"].ToString();
                CurrentSize = dr["LastSize"].ToString();
                CurrentMinWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + CurrentSize + "','" + CurrentClass + "','" + CurrentLength + "','MINWT')", curSQLConStr, out err));
                curAlmMinWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + CurrentSize + "','" + CurrentClass + "','" + CurrentLength +"','AlmMinWt')", curSQLConStr, out err));
                curAlmMaxWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + CurrentSize + "','" + CurrentClass + "','" + CurrentLength + "','AlmMaxWt')", curSQLConStr, out err));

                CurAlramWtRangeDesc = "Min/Max : " + curAlmMinWt.ToString() + " / " + curAlmMaxWt.ToString();
                CurrentMaxWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + CurrentSize + "','" + CurrentClass + "','" + CurrentLength + "','MAXWT')", curSQLConStr, out err));
                CurrentNomWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + CurrentSize + "','" + CurrentClass + "','" + CurrentLength + "','NOMWT')", curSQLConStr, out err));
                
                CurrentMould = dr["LastMould"].ToString();
                CurrentJoint = dr["LastJoint"].ToString();
                CurrentMaterial = dr["LastMaterial"].ToString();
                CurrentStandard = dr["LastStandard"].ToString();
                CurrentOperator = dr["LastOperatorCode"].ToString();
                CurrentOperatorName = dr["LastOperatorName"].ToString();
                CurrentOperatorDetails = CurrentOperator + ":" + CurrentOperatorName + "," + dr["LastPunch"].ToString();
            }

            ds.Dispose();
            
            this.isstarted = true;

            return ;
        }

        private void SaveLastParameters()
        {
             if (string.IsNullOrEmpty(this.curSQLConStr)  || string.IsNullOrEmpty(curMachine))
                return;

             if (!this.isDataSaveRequired)
                 return;


             if (!this.isstarted)
                 return;

             using (SqlConnection cn = new SqlConnection(curSQLConStr))
             {
                 try
                 {
                    //LastSize	varchar(10)	Checked
                    //LastLength	varchar(10)	Checked
                    //LastClass	varchar(10)	Checked
                    //LastMinWt	float	Checked
                    //LastMaxWt	float	Checked
                    //LastNomWt	float	Checked
                    //LastJoint	varchar(50)	Checked
                    //LastMould	varchar(50)	Checked
                    //LastMaterial varchar(50)
                    //LastStandard varchar(50)
                    //UpdDt datetime

                    string err;
                    string tmpname = GetDescription("Select OperatorName from ccmRFIDOperator where [OperatorCode] = '" + CurrentOperator + "'", curSQLConStr, out err);

                    cn.Open();
                     using (SqlCommand cmd = new SqlCommand())
                     {
                         string sql = "Insert into ccmLastPara (MachineID,LastSize,LastLength,LastClass,LastMinWt,LastMaxWt,LastNomWt," + 
                            " LastAlmMinWt,LastAlmMaxWt,LastJoint,LastMould,LastMaterial,LastStandard,LastOperatorCode,LastOperatorName,LastPunch,UpdDt)" +
                             " Values ('" + curMachine.ToString() + "'," +
                             "'" + CurrentSize.ToString() + "'," +
                             "'" + CurrentLength.ToString() + "'," +
                             "'" + CurrentClass.ToString() + "'," +
                             "'" + CurrentMinWt.ToString() + "'," +
                             "'" + CurrentMaxWt.ToString() + "'," +
                             "'" + CurrentNomWt.ToString() + "'," +
                             "'" + CurrentAlmMinWt.ToString() + "'," +
                             "'" + CurrentAlmMaxWt.ToString() + "'," +
                             "'" + CurrentJoint.ToString() + "'," +
                             "'" + CurrentMould.ToString() + "'," +
                             "'" + CurrentMaterial.ToString() + "'," +
                             "'" + CurrentStandard.ToString() + "'," +
                             "'" + CurrentOperator.ToString() + "'," +
                          //   "'" + CurrentOperatorName.ToString() + "'," +
                             "'" + tmpname + "'," +
                             "'" + CurrentPunch.ToString() + "',GetDate())";
                         cmd.Connection = cn;
                         cmd.CommandType = CommandType.Text;
                         cmd.CommandText = sql;
                         cmd.ExecuteNonQuery();
                     }
                 }catch(Exception ex)
                 {
                 }

                 return;
             }
        }

        private  DataSet GetData(string sql, string ConnectionString,out string err)
        {
            DataSet Result = new DataSet();
            err = string.Empty;

            if (string.IsNullOrEmpty(sql))
            {
                err = "Query can not be empty";
                return Result;
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                err = "Connection string can not be empty";
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
                
            }
            catch (SqlException ex) { err = ex.Message.ToString(); Debug.Print(ex.Message); }
            catch (Exception ex) { err = ex.Message.ToString(); Debug.Print(ex.Message); }
            finally
            {
                conn.Close();
            }

            command.Dispose();
            da.Dispose();
            return Result;
        }

        private  string GetDescription(string sql, string ConnectionString, out string err)
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


        private bool Start_RBMQ_Client()
        {
            curTimer.Interval = 5000;
            curTimer.Elapsed += CurTimer_Elapsed;
            curTimer.Start();

            if (string.IsNullOrEmpty(this.RBMQServer))
            {
                this.RBMQServer = "amqp://anand:anand123@172.16.12.44:5672/CCM";
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

                Uri t = new Uri(this.RBMQServer);
                factory.Uri = t;
                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);


                try
                {
                    conn = factory.CreateConnection();
                    channel = conn.CreateModel();
                    channel.ExchangeDeclare(exchange: this.CurrentMachine, type: "direct",durable:true,autoDelete:false);

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                exchange: this.CurrentMachine,
                                routingKey: "LOG");

                    channel.QueueBind(queue: queueName,
                            exchange: this.CurrentMachine,
                            routingKey: "GEN");

                    channel.QueueBind(queue: queueName,
                            exchange: this.CurrentMachine,
                            routingKey: "ERR");

                    channel.QueueBind(queue: queueName,
                            exchange: this.CurrentMachine,
                            routingKey: "STS");

                    channel.QueueBind(queue: queueName,
                            exchange: "COMMUNICATION",
                            routingKey: "MasterReload");

                    consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray(); 
                        string message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;

                        if (routingKey == "ERR")
                        {
                            LogErrors(message,"ERROR");
                            return;
                        }

                        if (routingKey == "STS")
                        {
                            if (message == "1")
                            {
                                Status = "#FFC9F37F";
                            }
                            else if (message == "0")
                            {
                                Status = "Red";
                            }
                            return;
                        }


                        if(routingKey == "MasterReload")
                        {
                            //if new master data added need to load on-the fly
                            //Reload_MSG_MasterData(message);

                            string testvar = "ccmSize,ccmLength,ccmClass,ccmMaterial,ccmStandard";
                            string[] msgtable = message.Split('-');
                            if (msgtable.Length <= 0)
                                return;

                            if (testvar.Contains(msgtable[0]))
                            {
                                string[] msgdata = msgtable[1].Split(',');

                                if (msgdata.Length > 0)
                                {
                                    
                                    var strlist = new List<String>();

                                    foreach (string row in msgdata)
                                    {
                                        switch (msgtable[0])
                                        {
                                            case "ccmSize":
                                                strlist.Add(row);
                                                break;
                                            case "ccmClass":
                                                strlist.Add(row);
                                                break;
                                            case "ccmLength":
                                                strlist.Add(row);
                                                break;
                                            case "ccmMaterial":
                                                strlist.Add(row);
                                                break;
                                            case "ccmStandard":
                                                strlist.Add(row);
                                                break;
                                            default:
                                                break;
                                        } //switch
                                    } //foreach row 

                                    switch (msgtable[0])
                                    {
                                        case "ccmSize":
                                            Sizes = strlist;
                                            break;
                                        case "ccmClass":
                                            Classes = strlist;
                                            break;
                                        case "ccmLength":
                                            Lengths = strlist;
                                            break;
                                        case "ccmMaterial":
                                            Materials = strlist;
                                            break;
                                        case "ccmStandard":
                                            Standards = strlist;
                                            break;

                                        default:
                                            break;
                                    }

                                }//if data found

                            }


                            return;
                        }

                        //GEN_PP$2018-11-22 17:44:17$192.168.11.14$1702$       0.0#0#$PP
                        string[] msg = message.Split('$');
                        string lasttime = msg[1];
                        string actmsg = msg[4];
                        string[] wt = actmsg.Split('#');
                        string actwt = wt[0].Split('.')[0];

                        double tmpwt = 0;
                        double.TryParse(actwt, out tmpwt);
                        this.CurrentWeight = (tmpwt < 0 ? "0.0" : string.Format("{0:0.0}", tmpwt));
                        
                      

                        
                        

                    };

                    channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);




                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ect)
                {
                    //logerror
                    LogErrors("AMQP Basic Consumer Error in uct DLL->" + ect.Message, "ERROR");
                    
                    Thread.Sleep(3000);
                    // apply retry logic
                    Start_RBMQ_Client();
                }

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        private void CurTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.curTimer.Stop();
            //operator information
            string err = string.Empty;
            string sql = "select RFIP from v_ccm_rf_link where ccmTable ='" + this.curTableNm + "'";
            string rfip = GetDescription(sql, curSQLConStr, out err);

            if (!string.IsNullOrEmpty(rfip))
            {
                sql = "Select Top 1 * from ccmRFIDTransaction where MachineIP ='" + rfip + "' Order by PunchTime Desc";
                DataSet ds = GetData(sql, curSQLConStr, out err);
                bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasRows)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        this.CurrentOperator = dr["OperatorCode"].ToString();
                        this.CurrentPunch = Convert.ToDateTime(dr["PunchTime"]).ToString("dd/MM/yy HH:mm:ss");
                        this.CurrentOperatorName = GetDescription("Select OperatorName from ccmRFIDOperator where OperatorCode ='" + CurrentOperator + "'", curSQLConStr, out err);
                        //this.curOperatorDetails = this.CurrentOperator + ":" + this.CurrentOperatorName + "," + this.CurrentPunch;
                        this.CurrentOperatorDetails = this.CurrentOperator + " : " + this.CurrentOperatorName + " , " + this.CurrentPunch;
                    }
                }
            }
            this.curTimer.Start();
        }

        private bool Publish_RBMQ_MSG(string message, string routingkey)
        {
            if (channel.IsOpen && !string.IsNullOrEmpty(message))
            {

                try
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: this.CurrentMachine,
                                         routingKey: routingkey,
                                         basicProperties: null,
                                         body: body);


                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }


        public bool Disconnect()
        {

            STOP_RBMQ_Client();
            shutdown = true;
            Status = "Red";
            
            try  
            {  
                if (logtokensrc != null)
                {
                    logtokensrc.Cancel();
                    
                    DataSaveTask.Wait();
                }
            }  
            catch (AggregateException ex)  
            {  
               
            }

            LogErrors("Data Saving Server->Application Close Command Received for :" + this.CurrentMachine, "ERROR");

            this.CurrentStatus = false;
            return true;
        }
        
        private bool STOP_RBMQ_Client()
        {
            this.curTimer.Stop();
            this.curTimer.Elapsed -= CurTimer_Elapsed;
            this.curTimer.Close();

            try
            {
                if (conn != null && channel != null)
                {
                    
                    channel.Close();
                    conn.Close();
                    
                }

            }
            catch (Exception ex) { }
           

            
            return true;
        }

        private void LogErrors(string error, string topic)
        {
            DateTime dt = DateTime.Now;
            //ccmErrLog
            //LogDateTime
            //LogTopic
            //LogDesc
            if(string.IsNullOrEmpty(error))
                return;

            if(String.IsNullOrEmpty(topic))
                topic = "ERROR";

            using (SqlConnection cn = new SqlConnection(curSQLConStr))
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
        
        private void Queue_Loop(CancellationToken token)
        {
            //Library.WriteErrorLog("Starting Status Brodcasting");
            while (true)
            {
                // Poll on this property if you have to do 
                // other cleanup before throwing. 
                if (token.IsCancellationRequested )
                {
                    // Clean up here, then...

                    Thread.Sleep(100);
                    
                    break;
                }

                string sql = "Select top 1 * from " + this.curTableNm + " Order by LogDateTime Desc";
                string err = string.Empty;
                DataSet ds = GetData(sql, curSQLConStr, out err);
                bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasRows)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        this.LastPipeNo = dr["PipeNo"].ToString();
                        this.LastWeightTime = Convert.ToDateTime(dr["LogDateTime"]).ToString("HH:mm:ss");
                        this.LastWeight = dr["ActWt"].ToString();
                    }
                }

                
                Thread.Sleep(1000);

            }//while true;
        }


        public ViewModel()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
