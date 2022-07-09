using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace uct_main_wpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class mainCanwas : UserControl
    {
        public ViewModel model = new ViewModel();

        public mainCanwas()
        {
            InitializeComponent();

            this.DataContext = model;
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
             
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

        public  string GetDescription(string sql, string ConnectionString, out string err)
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

        private void sel_Size_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetWeightInfo();
        }

        private void GetWeightInfo()
        {
            string sqlcn = model.GetCurrentSQLConn;
            string tSize = model.CurrentSize;
            string tClass = model.CurrentClass;
            string tLen = model.CurrentLength;

            string tminwt = string.Empty,tmaxwt = string.Empty,tnomwt = string.Empty, err = string.Empty;
            string sql = "Select top 1 * from ccmWeightMaster where Size = '" + tSize + "' And Class ='" + tClass + "' and Len = '" + tLen + "'";
            DataSet tds = GetData(sql, sqlcn, out err);
            Boolean hasRows = tds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach (DataRow dr in tds.Tables[0].Rows)
                {
                    
                    model.CurrentAlmMinWt = (double)dr["AlmMinWt"];
                    model.CurrentAlmMaxWt = (double)dr["AlmMaxWt"];

                    model.CurAlramWtRangeDesc = "Min/Max : " + dr["AlmMinWt"].ToString() + " / " + dr["AlmMaxWt"].ToString(); 
                } 
            }
        }

        private void sel_Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetWeightInfo();
        }

        private void sel_Length_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetWeightInfo();
        }

        private void sel_Material_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void sel_Standard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
