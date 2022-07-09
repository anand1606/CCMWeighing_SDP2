using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CCMDataCapture
{
    public partial class frmModbusTcpConfig : Form
    {
        private string cnstr = Utility.SQLCnStr;
        private string Mode = "NEW";
        public frmModbusTcpConfig()
        {
            InitializeComponent();
        }

        private void frmModbusTcpConfig_Load(object sender, EventArgs e)
        {
            //load modbus config

            string err = string.Empty;
            string modbusip = Utility.GetDescription("select Config_value from ccmParaConfig where Config_Key ='ModbusIP'", cnstr, out err);
            string modbusport = Utility.GetDescription("select Config_value from ccmParaConfig where Config_Key ='ModbusPort'", cnstr, out err);

            txtIP.Text = modbusip;
            txtPort.Text = modbusport;

            ReloadData();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtIP.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Modbus IP Address","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtPort.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Modbus Port Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                try
                {
                    cn.Open();
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string sql = "Update ccmParaConfig Set Config_Value ='" + txtIP.Text.Trim().ToString() + "' where Config_Para = 'ModbusIP'";
                int tres = 0;
                using(SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    tres = cmd.ExecuteNonQuery();  
                }

                if(tres > 0)
                {
                    MessageBox.Show("Record Saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("No Record Affected.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

        }

        private void btnRfAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIP.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Modbus IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtPort.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Modbus Port Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtHoldingReg.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Holding Register Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbMachineName.SelectedIndex == -1 )
            {
                MessageBox.Show("Please Select Machine Name ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = string.Empty;

            if (Mode == "NEW")
                sql = "Insert into ccmModbusRegister (HoldingReg,ccmMachine,Description,AddDt) Values(" +
                    "'" + txtHoldingReg.Text.Trim().ToString() + "','"
                    + cmbMachineName.SelectedItem.ToString() + "','" + txtDesc.Text.Trim().ToString() + "',getDate())";
            else
                sql = "Update ccmModbusRegister set ccmMachine='" + cmbMachineName.SelectedItem.ToString()
                    + "',Description ='" + txtDesc.Text.ToString().Trim() + "' Where HoldingReg ='" + txtHoldingReg.Text.Trim().ToString() + "'";


            int result = 0;
            using(SqlConnection cn = new SqlConnection(cnstr))
            {
                try
                {
                    cn.Open();
                    using(SqlCommand cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Connection = cn;
                        result = cmd.ExecuteNonQuery();

                        if(result == 0)
                        {
                            MessageBox.Show("No record updated...","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return;
                        }

                        MessageBox.Show("Record Saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            ResetCtrl();
            ReloadData();        
        }

        private void ResetCtrl()
        {
            txtHoldingReg.Text = "";
            cmbMachineName.SelectedIndex = -1;
            txtDesc.Text = "";
            Mode = "NEW";
        }

        //reload all available data to dataset
        private void ReloadData()
        {
            string err;
            cmbMachineName.Items.Clear();
 
            DataSet ds = new DataSet();

           //feed ccm machine in combobox - RFID -> CCM Assignment
           cmbMachineName.Items.Clear();
           ds = Utility.GetData("select MachineName,MachineIP from ccmMachineConfig ", cnstr, out err);
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cmbMachineName.Items.Add(dr[0].ToString() );
                }
            }


            //RFID Master and Registration machine ip list
            ds = Utility.GetData("select * from ccmModbusRegister ", cnstr, out err);
            hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                grid.DataSource = ds;
                grid.DataMember = ds.Tables[0].TableName;
                grid.Refresh();
            }

        }

        private void txtHoldingReg_Validated(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHoldingReg.Text.ToString().Trim()))
            {
                Mode = "NEW";
                return;
            }

            string err = string.Empty;
            DataSet ds = Utility.GetData("Select * from ccmModbusRegister Where HoldingReg ='" + txtHoldingReg.Text.ToString().Trim() + "'", cnstr, out err);

            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                Mode = "OLD";
                DataRow dr = ds.Tables[0].Rows[0];
                txtDesc.Text = dr["Description"].ToString();
                cmbMachineName.SelectedIndex = cmbMachineName.Items.IndexOf(dr["ccmMachine"]);
            }
            else
            {
                Mode = "NEW";
            }

            

        }

        private void btnRfCancel_Click(object sender, EventArgs e)
        {
            ResetCtrl();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string sql = "Delete from ccmModbusRegister Where HoldingReg ='" + txtHoldingReg.Text.Trim().ToString() + "'";


            int result = 0;
            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.Connection = cn;
                        result = cmd.ExecuteNonQuery();

                        if (result == 0)
                        {
                            MessageBox.Show("No record Deleted...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            ResetCtrl();
            ReloadData();
        }
    }
}
