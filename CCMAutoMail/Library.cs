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
using System.Net.Mime;
using System.Net.Mail;

namespace CCMAutoMail
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
                string fullpath = Path.Combine(basepath, "Auto_Mail_" + dt.ToString("yyyyMMdd") + ".txt");
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
                string fullpath = Path.Combine(basepath, "Auto_Mail_" + dt.ToString("yyyyMMdd") + ".txt");
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
                string fullpath = Path.Combine(basepath, "Auto_Mail_" + dt.ToString("yyyyMMdd") + ".txt");
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

        public static string GetUserDataPath()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, "CCMDataLogger");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }
        
    }

    public class MailAttachment
    {
        #region Fields
        private MemoryStream stream;
        private string filename;
        private string mediaType;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the data stream for this attachment
        /// </summary>
        public Stream Data { get { return stream; } }
        /// <summary>
        /// Gets the original filename for this attachment
        /// </summary>
        public string Filename { get { return filename; } }
        /// <summary>
        /// Gets the attachment type: Bytes or String
        /// </summary>
        public string MediaType { get { return mediaType; } }
        /// <summary>
        /// Gets the file for this attachment (as a new attachment)
        /// </summary>
        public Attachment File { get { return new Attachment(Data, Filename, MediaType); } }
        #endregion
        #region Constructors
        /// <summary>
        /// Construct a mail attachment form a byte array
        /// </summary>
        /// <param name="data">Bytes to attach as a file</param>
        /// <param name="filename">Logical filename for attachment</param>
        public MailAttachment(byte[] data, string filename)
        {
            this.stream = new MemoryStream(data);
            this.filename = filename;
            this.mediaType = MediaTypeNames.Application.Octet;
        }
        /// <summary>
        /// Construct a mail attachment from a string
        /// </summary>
        /// <param name="data">String to attach as a file</param>
        /// <param name="filename">Logical filename for attachment</param>
        public MailAttachment(string data, string filename)
        {
            this.stream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(data));
            this.filename = filename;
            this.mediaType = MediaTypeNames.Text.Html;
        }
        #endregion
    }
    

}
