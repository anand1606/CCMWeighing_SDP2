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

namespace uct_Weight_wpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WeightCanwas : UserControl
    {
        public ViewModel model = new ViewModel();
        private bool lockstatus = true;
        private string tmp_pw = String.Empty;
        public WeightCanwas()
        {
            InitializeComponent();

            this.DataContext = model;
            
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //timer.Stop();
            //timer.Start();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            sel_Size.IsEnabled = false;
            
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
             Config subWindow = new Config();
             string err = string.Empty;
             subWindow.txtIP.Text = model.CurrentIP;
             subWindow.txtPort.Text = model.CurrentPort;
             subWindow.lbl_CurrentMachine.Text = GetDescription("Select MachineName From ccmMachineConfig Where MachineIP='" + model.CurrentIP + "'", model.GetCurrentSQLConn, out err);

             subWindow.sqlcn = model.GetCurrentSQLConn;
             subWindow.ShowDialog();
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
            string err = string.Empty;
            model.CurrentMinWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + tSize + "','" + tClass + "','" + tLen + "','MINWT')", sqlcn, out err));
            model.CurrentMaxWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + tSize + "','" + tClass + "','" + tLen + "','MAXWT')", sqlcn, out err));
            model.CurrentNomWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + tSize + "','" + tClass + "','" + tLen + "','NOMWT')", sqlcn, out err));
            model.CurrentAlmMinWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + tSize + "','" + tClass + "','" + tLen + "','ALMMINWT')", sqlcn, out err));
            model.CurrentAlmMaxWt = Convert.ToDouble(GetDescription("select dbo.udf_get_Wt_minmax('" + tSize + "','" + tClass + "','" + tLen + "','ALMMAXWT')", sqlcn, out err));

            model.CurAlramWtRangeDesc = "Min/Max : " + model.CurrentAlmMinWt.ToString() + " / " + model.CurrentAlmMaxWt.ToString(); 
           
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

        private void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            string sqlcn = model.GetCurrentSQLConn;
            string err = string.Empty;
            tmp_pw = GetDescription("Select Config_Value from ccmParaConfig where Config_Key ='QualitySizePW'", sqlcn, out err);

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (txtSizePass.Password == tmp_pw)
            {
                sel_Size.IsEnabled = true;
                lockstatus = false;
                btnUnlock.Content = "Lock";
            }
            else
            {
                sel_Size.IsEnabled = false;
                lockstatus = true;
                txtSizePass.Password = "";
                btnUnlock.Content = "Unlock";
            }
        
        }

        private void sel_Size_DropDownClosed(object sender, EventArgs e)
        {
            txtSizePass.Password = "";
            lockstatus = true ;
            sel_Size.IsEnabled = false;
            btnUnlock.Content = "Unlock";
        }
    }
}
