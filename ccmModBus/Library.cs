using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace ccmModBus
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
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                string basepath = AppDomain.CurrentDomain.BaseDirectory;
                string fullpath = Path.Combine(basepath, "ModeBus_Error_" + dt.ToString("yyyyMMdd") + ".txt");
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
                string fullpath = Path.Combine(basepath, "ModeBus_Info_" + dt.ToString("yyyyMMdd") + ".txt");
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
}