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
using System.Windows.Shapes;

namespace uct_Weight_wpf
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Config : Window
    {

        public string sqlcn;
        
        public Config()
        {
            InitializeComponent();
            
        }


        public string Validate()
        {
            string err = string.Empty;
            if (string.IsNullOrEmpty(this.txtIP.Text.Trim()))
            {
                err = "Please Enter IP Address";
                return err;
            }

            if (string.IsNullOrEmpty(this.txtPort.Text.Trim()))
            {
                err = "Please Enter Port Number";
                return err;
            }
            else
            {
                if (txtPort.Text.Trim() == "0")
                {
                    err = "Please Enter Port Number";
                    return err;
                }

                int t;
                bool k = int.TryParse(txtPort.Text.Trim(), out t);

                if (t <= 0)
                {
                    err = "Please Enter Port Number";
                    return err;
                }
            }


            DataSet ds = GetData("Select * from ccmMachineConfig Where MachineName ='" + this.lbl_CurrentMachine.Text.Trim() + "'", sqlcn, out err);
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (!hasrows)
            {
                err += "Database Configuration not found, please configure in ccmMachineConfig..";
            }

            return err;
        }


        private DataSet GetData(string sql, string ConnectionString, out string err)
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
            catch (SqlException ex) { err = ex.Message.ToString(); }
            catch (Exception ex) { err = ex.Message.ToString(); }
            finally
            {
                conn.Close();
            }

            command.Dispose();
            da.Dispose();
            return Result;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //update in ccmMachineConfig table according to input parameter.
            //validate ip and port

            string err = Validate();

            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(sqlcn))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = "Update ccmMachineConfig Set MachineIP ='" + txtIP.Text.Trim() + "', MachinePort ='" + txtPort.Text.Trim().ToString() + "' Where MachineName ='" + this.lbl_CurrentMachine.Text.Trim() + "'";
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();

                        //service restart required...
                        //try
                        //{
                        //    System.Diagnostics.Process.Start("net", "stop CCMDataBrodCast");
                        //    System.Diagnostics.Process.Start("net", "start CCMDataBrodCast");
                        //    System.Diagnostics.Process.Start("net", "stop CCMDataWriter");
                        //    System.Diagnostics.Process.Start("net", "start CCMDataWriter");
                        //}
                        //catch(Exception ex)
                        //{
                        //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                        //    return;
                        //}                       

                        
                        
                        MessageBox.Show("Configuration Saved..,System Restart required..", "Information", MessageBoxButton.OK, MessageBoxImage.Information);


                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                    return;
                }
            }
        }
    }
}
