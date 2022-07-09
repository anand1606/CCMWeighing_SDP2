using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Management;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CCMDataCapture
{
    class Utility
    {

        private static ConnectionFactory factory = new ConnectionFactory();
        private static IConnection conn;
        private static IModel channel;

        public static bool Start_RBMQ_Client(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                uri = "amqp://anand:anand123@172.16.12.44:5672/CCM";
            }

#pragma warning disable CS0168 // The variable 'ex' is declared but never used
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


#pragma warning disable CS0168 // The variable 'ect' is declared but never used
                try
                {
                    conn = factory.CreateConnection();
                    channel = conn.CreateModel();
                    channel.ExchangeDeclare(exchange: "COMMUNICATION", type: "direct", durable: true, autoDelete: false);
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ect)
                {


                    // apply retry logic
                    Start_RBMQ_Client(uri);
                }
#pragma warning restore CS0168 // The variable 'ect' is declared but never used

            }
            catch (Exception ex)
            {

                return false;
            }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            return true;
        }

        public static bool Publish_RBMQ_MSG(string message, string routingkey)
        {
            if (!string.IsNullOrEmpty(message))
            {

#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                try
                {


                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "COMMUNICATION",
                                         routingKey: routingkey,
                                         basicProperties: null,
                                         body: body);


                    //Library.WriteErrorLog("Pulibshed : " + message);
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used

            }
            else
            {
                return false;
            }
        }

        public static bool STOP_RBMQ_Client()
        {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            try
            {
                if (conn != null && channel != null)
                {

                    channel.Close();
                    conn.Close();

                }

            }
            catch (Exception ex) { }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            finally
            {
               
            }

            return true;
        }


        public static DataSet GetData(string sql, string ConnectionString,out string err)
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
            catch (SqlException ex) {err = ex.Message.ToString(); }
            catch (Exception ex) { err = ex.Message.ToString(); }
            finally
            {
                conn.Close();
            }

            return Result;
        }


        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }


        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
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

        public static List<string> GetHostKey()
        {

            List<string> DiskKey = new List<string>();
            ManagementObjectSearcher moSearcher = new
                    ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in moSearcher.Get())
            {
                //HardDrive hd = new HardDrive();  // User Defined Class
                //hd.Model = wmi_HD["Model"].ToString();  //Model Number
                //hd.Type = wmi_HD["InterfaceType"].ToString();  //Interface Type
                DiskKey.Add(wmi_HD["SerialNumber"].ToString());  //Serial Number
                //hardDriveDetails.Add(hd);  
                //label1.Text ="Model : "+ hd.Model ;
                //label2.Text = " Type : " + hd.Type;
                //label3.Text = " Serial Number : " + hd.SerialNo;
            }

            return DiskKey;
        }

        public static bool RetriveLic(string cnstr,string key , out Sysinfo t,out string err)
        {
            //decrypt text 
            err = string.Empty;
            t = new Sysinfo();

            string sql = "Select top 1 * from Sysinfo where active = 1";
            DataSet ds = Utility.GetData(sql,cnstr,out err);
         
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            bool result = false;
            if (hasrows)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string x = dr["sysinfo"].ToString();
                if (string.IsNullOrEmpty(x))
                {
                    err = "Invalid Licence";                   
                    result = false;
                }
                else
                {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    try
                    {
                        string tempstr = DecryptString(key, x);
                        t = JsonConvert.DeserializeObject<Sysinfo>(tempstr);
                        result = true;
                    }
                    
                    catch (JsonSerializationException ex)
                    {
                        err = "Invalid Licence";
                        result = false;
                    }
                    catch (Exception ex)
                    {
                        err = "Invalid Licence";
                        result = false;
                    }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                }
                
            }
            else
            {
                err = "Invalid Licence";
                result = false;
            }

            if (!result)
            {
                string x1 = String.Join(",", GetHostKey());
                t = GenerateTrial(cnstr,x1,key );
                SaveLic(cnstr, t, key, "TRIAL", out err);               
            }

            return result;
        }

        public static bool MatchLic(Sysinfo t,string cnstr,string key)
        {
            DateTime tmp = t.InstallDt.AddDays(t.Limitdays).Date ;
            DateTime cur = DateTime.Now.Date;
           
            string err;
            string dbreg = GetDescription("select top 1 RegKey from SysInfo where Active  = 1", cnstr, out err);
            string dbkey = DecryptString(key, dbreg);

            if (t.LicType.ToUpper().Contains("TRIAL") && dbkey == "TRIAL")
            {
                if ((cur <= tmp))
                {
                    //valid trial period
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            else if(t.LicType.ToUpper().Contains("TRIAL") && dbkey != "TRIAL" )
            {
                if(dbkey == DecryptString(key,GenerateRegKey(t.Hostkey, key)))
                {
                    //valid license
                    t.LicType = "Single";
                    t.Limitdays = 0;
                    t.Regkey = t.Hostkey + key;
                    SaveLic(cnstr, t, key, t.Regkey, out err);
                    return true;
                }
                else
                {
                    return false;
                }            
            }
            else if (t.LicType.ToUpper().Contains("SINGLE") && dbkey != "TRIAL")
            {
                if (dbkey == DecryptString(key, GenerateRegKey(t.Hostkey, key)))
                {
                    //valid license
                    t.LicType = "Single";
                    t.Limitdays = 0;
                    t.Regkey = t.Hostkey + key; 
                    SaveLic(cnstr, t, key, t.Regkey, out err);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            
        }

        public static string getDBName(string cnstr)
        {
            //Server=172.16.13.224;Database=CCM;User Id=sa;Password=testomonials;

            string dbname = string.Empty;
            string[] x = cnstr.Split(';');
            foreach (string tmp in x)
            {
                if (tmp.Contains("Database") && tmp.Contains("="))
                {
                    string[] t = tmp.Split('=');
                    dbname = t[1];
                }
            }

            if (string.IsNullOrEmpty(dbname))
            {
                dbname = "CCM";
            }

            return dbname;
        }

        public static Sysinfo GenerateTrial(string cnstr, string hostkey, string key)
        {
            Sysinfo t = new Sysinfo();
            
            string dbname = getDBName(cnstr);
            
            string sql = "SELECT create_date FROM  sys.databases WHERE name = '" + dbname + "'";
            string err;
            DataSet ds = Utility.GetData(sql,cnstr,out err);
            //pending
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            
            if (hasrows)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                t.InstallDt = Convert.ToDateTime(dr["create_date"]);
            }

            t.Limitdays = 30;           
            t.Hostkey = hostkey;
            t.LicType = "TRIAL";
            t.Regkey = "TRIAL";

            return t;

        }

        public static bool SaveLic(string cnstr,Sysinfo s,string key, string regkey ,out string err)
        {
            //encrypted json save
            string s1 = JsonConvert.SerializeObject(s).ToString();
            string encstr = EncryptString(key,s1);
            err = string.Empty;

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                try
                {
                    cn.Open();

                }
                catch (Exception ex)
                {
                    err = ex.Message.ToString();
                    return false;
                }

                using (SqlCommand cmd = new SqlCommand())
                {

                    err = string.Empty;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.CommandText = "delete from sysinfo";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "Insert into SysInfo (Sysinfo,Adddt,Active,regkey) " +
                             "Values ('" + encstr + "',GetDate(),1,'" + EncryptString(key,regkey) + "')";
                        cmd.ExecuteNonQuery();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        err = ex.Message.ToString();
                        return false;
                    }
                }//using cmd
            } //using cn
        }

        public static string GenerateRegKey(string hostkey, string key)
        {
            return EncryptString(key,hostkey + key );
        }

        public static string SQLCnStr { get; set; }


        public static void WriteInfoLog(string Message,string tFolder,string prifix)
        {
            StreamWriter sw = null;
            try
            {
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string basepath = AppDomain.CurrentDomain.BaseDirectory;
                string fullpath = Path.Combine(basepath,tFolder,prifix + dt.ToString("yyyyMMdd") + ".txt");
                sw = new StreamWriter(fullpath, true);

                sw.WriteLine((DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + Message));
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }

    class Sysinfo
    {
        public string LicType { get; set; }

        //hard disk key
        public string Hostkey { get; set; }

        public int Limitdays { get; set; }

        public string Regkey { get; set; }

        public DateTime InstallDt {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        
    }
}
