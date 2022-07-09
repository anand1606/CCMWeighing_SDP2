using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Threading;

namespace CCMDataWriter
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

        public static void WriteInfoLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string basepath = AppDomain.CurrentDomain.BaseDirectory;
                string fullpath = Path.Combine(basepath,"Info_" + dt.ToString("yyyyMMdd") + ".txt");
                sw = new StreamWriter(fullpath, true);

                sw.WriteLine((DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " : " + Message));
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
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

            if (!sql.Contains("TOP 1"))
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
        
    }

    class CurrentSetting
    {
        public decimal MinWt { get; set; }
        public decimal MaxWt { get; set; }
        public decimal NomWt { get; set; }
        public decimal Length { get; set; }
        public string Size { get; set; }
        public string Class { get; set; }
        public string Joint { get; set; }
        public string MouldNo { get; set; }

        public string Material { get; set; }
        public string Standard { get; set; }
        public string OperatorCode { get; set; }
        public string OperatorName { get; set; }

        public string LastPunch { get; set; }

        public decimal AlmMinWt { get; set; }
        public decimal AlmMaxWt { get; set; }



        public CurrentSetting()
        {
            this.MinWt = 0;
            this.MaxWt = 0;
            this.NomWt = 0;
            this.Length = 0;
            this.Size = "";
            this.Class = "";
            this.Joint = "";
            this.MouldNo = "";

            this.AlmMaxWt = 0;
            this.AlmMinWt = 0;
            this.Material = "";
            this.Standard = "";
            this.OperatorCode = "";
            this.OperatorName = "";
            this.LastPunch = "";
        }

        public override string ToString()
        {
            string tmpstr = "{Size = {0}, Length = {1}, Class = {2} , Joint = {3} , MouldNo = {4} , MinWt = {5} , MaxWt = {6} , NomWt = {7}," +
                " AlmMaxWt = {8}, AlmMinWt = {9}, Material = {10}, Standard = {11}, OperatorCode = {12}, OperatorName = {13} " +
                "" +
                " }";
            string tmpstr1 = string.Format(tmpstr, this.Size, this.Length.ToString(), 
                this.Class, this.Joint, this.MouldNo, this.MinWt.ToString(),
                this.MaxWt.ToString(), this.NomWt.ToString(),
                this.AlmMaxWt.ToString(), this.AlmMinWt.ToString(),
                this.Material.ToString(), this.Standard.ToString(), this.OperatorCode,this.OperatorName);
               
            return tmpstr1;
        }
    }

    class WeightClient
    {
        public string MachineIP { get; set; }
        public string MachineID { get; set; }
        public string TableName { get; set; }
    }

    class StreamReceiver
    {
        public string MachineID { get; set; }
        public double ActWt { get; set; }
        public int SrNo { get; set; }
        public string PipeNumber { get; set; }
        public DateTime LogDateTime { get; set; }
        public CurrentSetting Parameters { get; set; }

        public StreamReceiver(CurrentSetting t)
        {
            this.MachineID = "";
            this.ActWt = 0;
            this.Parameters = t;
            this.LogDateTime = DateTime.MinValue;
        }

        public StreamReceiver()
        {
            this.MachineID = "";
            this.ActWt = 0;
            this.LogDateTime = DateTime.MinValue;
        }

    }


}
