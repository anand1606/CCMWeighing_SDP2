using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zkemkeeper;

namespace CCMDataCapture
{
    public partial class frmRFID : Form
    {
        private string sqlcn = string.Empty;
        private DataSet Ds = new DataSet();
        private string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();
        public frmRFID()
        {
            InitializeComponent();
        }

        private void frmRFID_Load(object sender, EventArgs e)
        {

            sqlcn = Utility.SQLCnStr;
            gridPunching.DataSource = null;

            ReloadData();
        }

        private void ResetCtrl(string module)
        {

        }

        //reload all available data to dataset
        private void ReloadData()
        {
            string err;
            cmbMachineName.Items.Clear();
            chkListRFID.Items.Clear();
            cmbEmp.Items.Clear(); 
            DataSet ds = new DataSet();
            gridOperator.DataSource = null;
            gridPunching.DataSource = null; 
            gridRegister.DataSource = null; 
            gridRFID.DataSource = null; 

            //feed operator details in grid and combo selection
            ds = Utility.GetData("select OperatorCode,OperatorName from ccmRFIDOperator Where Active = 1", sqlcn , out err);

            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                gridOperator.DataSource = ds;
                gridOperator.DataMember = ds.Tables[0].TableName;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cmbEmp.Items.Add(dr[0].ToString() + "," + dr[1].ToString());
                }
            }

            //feed ccm machine in combobox - RFID -> CCM Assignment
            ds = Utility.GetData("select MachineName,MachineIP from ccmMachineConfig",sqlcn, out err);
            hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cmbMachineName.Items.Add(dr[0].ToString() + "," + dr[1].ToString() );
                }
            }


            //RFID Master and Registration machine ip list
            ds = Utility.GetData("select MachineIP,ccmMachine,ccmMachineIP from ccmRFIDMaster", sqlcn, out err);
            hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                gridRFID.DataSource = ds;
                gridRFID.DataMember = ds.Tables[0].TableName;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    chkListRFID.Items.Add(dr[1].ToString(), dr[0].ToString() + "," + dr[1].ToString());
                }
            }

            //RFID Registratino Histroy show
            ds = Utility.GetData("select top 50 Convert(varchar(21), Adddt) as [DateTime],MachineIP as RfidMachine,OperatorCode,Operation from ccmRFIDHistory Order By ID Desc", sqlcn, out err);
            hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                gridRegister.DataSource = ds;
                gridRegister.DataMember = ds.Tables[0].TableName;
                
            }


        }

        private void btnPunchQry_Click(object sender, EventArgs e)
        {
            if(txtdate.EditValue == null)
            {
                MessageBox.Show("Please Select Date..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //RFID Punching History
            string sql = string.Empty;
            sql= "select Convert(varchar(21),a.PunchTime,121) as PunchTime, a.OperatorCode, a.MachineIP, rm.ccmMachine, b.OperatorName,a.AddDt " +
            " from ccmRFIDTransaction a Left outer join ccmRFIDOperator b on a.OperatorCode = b.OperatorCode " +
            " Left outer join ccmRFIDMaster rm on a.MachineIP = rm.MachineIP " +
            " where Convert(Date,PunchTime) = '" + txtdate.DateTime.Date.ToString("yyyy-MM-dd") + "'";
            string err = string.Empty;
            DataSet ds = Utility.GetData(sql, sqlcn, out err);
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                gridPunching.DataSource = ds;
                gridPunching.DataMember = ds.Tables[0].TableName;

            }
            else
            {
                gridPunching.DataSource = null;
                gridPunching.Refresh();
            }
        }

        #region RFID Master

        private void btnRfAdd_Click(object sender, EventArgs e)
        {
            string err = String.Empty;

            //validation
            if (string.IsNullOrEmpty(txtIP.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter IP Address of RFID Machine","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if(cmbMachineName.SelectedIndex == -1)
            {
                MessageBox.Show("Please Select ccm Machine..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string mode = "";
            string strcnt = Utility.GetDescription("Select Count(*) from ccmRFIDMaster Where MachineIP ='" + txtIP.Text.ToString().Trim() + "'", sqlcn, out err);

            if(Convert.ToInt32(strcnt) == 0)
            {
                mode = "NEW";
            }
            else
            {
                mode = "OLD";
            }

            using(SqlConnection cn = new SqlConnection(sqlcn))
            {
                try
                {
                    cn.Open();
                }catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string sql;
                int selidx = cmbMachineName.SelectedIndex;
                 

                string[] selccm = cmbMachineName.GetItemText(cmbMachineName.SelectedItem).Split(',') ;

                if(selccm.Length <= 0)
                {
                    MessageBox.Show("Something went wrong please contact to suppler", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (mode == "OLD")
                    sql = "Update ccmRFIDMaster set ccmMachine ='" + selccm[0].ToString() + "'," +
                        " ccmMachineIP ='" + selccm[1].ToString() + "', UpdDt = GetDate() " +
                        " Where MachineIP ='" + txtIP.Text.ToString() + "'";

                else
                    sql = "Insert into ccmRFIDMaster (MachineIP,ccmMachine,ccmMachineIP,AddDt) values " +
                        " ('" + txtIP.Text.ToString().Trim() + "','" + selccm[0].ToString().Trim() + "'," +
                        " '" + selccm[1].ToString().Trim() + "',GetDate() ) ";


                int rescnt = 0;
                using (SqlCommand cmd = cn.CreateCommand())
                {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    try
                    {
                        cmd.CommandText = sql;
                        rescnt = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) { }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used



                    if (rescnt > 0)
                    {
                        if (mode == "NEW")
                        {
                            MessageBox.Show("New Record inserted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           
                        }
                        else
                        {
                            MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                        }

                    }
                    else
                    {
                        MessageBox.Show("No Records were affected...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }//using command

            }//using connection
            txtIP.Text = "";
            cmbMachineName.SelectedIndex = -1;
            ReloadData();
        }

        private void btnRfDelete_Click(object sender, EventArgs e)
        {
            string err = String.Empty;

            //validation
            if (string.IsNullOrEmpty(txtIP.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter IP Address of RFID Machine", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(sqlcn))
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string sql;               

                sql = "Delete From ccmRFIDMaster Where MachineIP ='" + txtIP.Text.ToString() + "'";

                int rescnt = 0;
                using (SqlCommand cmd = cn.CreateCommand())
                {
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    try
                    {
                        cmd.CommandText = sql;
                        rescnt = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) { }
#pragma warning restore CS0168 // The variable 'ex' is declared but never used



                    if (rescnt > 0)
                    {
                        MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("No Records were affected...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }//using command

            }//using connection

            txtIP.Text = "";
            cmbMachineName.SelectedIndex = -1;
            ReloadData();
        }

        private void btnRfCancel_Click(object sender, EventArgs e)
        {
            txtIP.Text = "";
            cmbMachineName.SelectedIndex = -1;
        }

        #endregion

        #region RFID Operator

        private void btnOpAdd_Click(object sender, EventArgs e)
        {
            string err = String.Empty;
            string sql = string.Empty;

            //validation
            if (string.IsNullOrEmpty(txtEmpCode.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Employee Code of Operator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtEmpName.Text.ToString().Trim()))
            {
                MessageBox.Show("Invalid Operator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string rfidno = string.Empty;
            string ActiveSts = string.Empty;
           
            string remoteconn = Utility.GetDescription("Select config_value from ccmParaConfig Where config_key = 'HRMasterData'", sqlcn, out err);

            if (string.IsNullOrEmpty(remoteconn))
            {
                MessageBox.Show("HRMasterData is not configured in ccmParaConfig..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //get rfidcode of employee
            sql = "Select RFIDNO from EmpBioData Where EmpUnqID ='" + txtEmpCode.Text.Trim().ToString() + "' " +
                " And Type = 'RFID'";
            rfidno = Utility.GetDescription(sql, remoteconn, out err);

            sql = "Select Active from MastEmp Where EmpUnqID = '" + txtEmpCode.Text.Trim().ToString() + "'";
            ActiveSts = Utility.GetDescription(sql, remoteconn, out err);

            if(string.IsNullOrEmpty(rfidno))
            {
                MessageBox.Show("Error ->HRMasterData->RFID No not found..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(ActiveSts))
            {
                MessageBox.Show("Error ->HRMasterData->Active Status No not found..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string mode = "";
            string strcnt = Utility.GetDescription("Select Count(*) from ccmRFIDOperator Where OperatorCode ='" + txtEmpCode.Text.ToString().Trim() + "'", sqlcn, out err);

            if (Convert.ToInt32(strcnt) == 0)
            {
                mode = "NEW";
            }
            else
            {
                mode = "OLD";
            }

            using (SqlConnection cn = new SqlConnection(sqlcn))
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                if (mode == "OLD")
                    sql = "Update ccmRFIDOperator set RFIDNo ='" + rfidno + "'," +
                        " OperatorName ='" + txtEmpName.Text.Trim().ToString() + "', UpdDt = GetDate(), Active = '" + ActiveSts + "' " +
                        " Where OperatorCode ='" + txtEmpCode.Text.ToString() + "' ";

                else
                    sql = "Insert into ccmRFIDOperator (OperatorCode,OperatorName,RFIDNo,AddDt,active) values " +
                        " ('" + txtEmpCode.Text.ToString().Trim() + "','" + txtEmpName.Text.ToString().Trim() + "'," +
                        " '" + rfidno.Trim() + "',GetDate(),1 ) ";


                int rescnt = 0;
                using (SqlCommand cmd = cn.CreateCommand())
                {

                    try
                    {
                        cmd.CommandText = sql;
                        rescnt = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) {

                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                    if (rescnt > 0)
                    {
                        if (mode == "NEW")
                        {
                            MessageBox.Show("New Record inserted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                    }
                    else
                    {
                        MessageBox.Show("No Records were affected...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }//using command

            }//using connection
            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            ReloadData();

        }

        private void txtEmpCode_Validated(object sender, EventArgs e)
        {
            string err = string.Empty;
            string remoteconn = Utility.GetDescription("Select config_value from ccmParaConfig Where config_key = 'HRMasterData'", sqlcn, out err);

            if (string.IsNullOrEmpty(remoteconn))
            {
                MessageBox.Show("HRMasterData is not configured in ccmParaConfig..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                txtEmpName.Text = "";
                return;
            }

            txtEmpName.Text = Utility.GetDescription("select EmpName from MastEmp Where  EmpUnqID ='" + txtEmpCode.Text.Trim().ToString() + "'", remoteconn, out err);

            if (!string.IsNullOrEmpty(err))
            {
               
                txtEmpName.Text = "";
            }
            


        }

        private void btnOpDelete_Click(object sender, EventArgs e)
        {
            string err = String.Empty;

            //validation
            if (string.IsNullOrEmpty(txtEmpCode.Text.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Employee Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(sqlcn))
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string sql;

                sql = "Delete From ccmRFIDOperator Where OperatorCode ='" + txtEmpCode.Text.ToString() + "'";

                int rescnt = 0;
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = sql;
                        rescnt = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }



                    if (rescnt > 0)
                    {
                        MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("No Records were affected...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }//using command

            }//using connection

            txtEmpCode.Text = "";
            txtEmpName.Text = "";
            ReloadData();
        }
       
        private void btnOpCancel_Click(object sender, EventArgs e)
        {
            txtEmpCode.Text = "";
            txtEmpName.Text = "";
        }

        #endregion



        #region Registration


        private void btnRegALL_Click(object sender, EventArgs e)
        {

            DataSet ds = Utility.GetData("Select * from ccmRFIDOperator where active = 1", Utility.SQLCnStr, out string err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem selitem in chkListRFID.CheckedItems)
                {
                   
                    string MachineIP = selitem.Description.ToString().Split(',')[0];
                    clsMachine machine = new clsMachine(MachineIP, "I");
                    machine.Connect(out err);
                    if (!string.IsNullOrEmpty(err))
                    {
                        MessageBox.Show(err, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        machine.DisConnect(out err);
                        continue;
                    }
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        string empcode = dr["OperatorCode"].ToString().Trim();
                        string rfid = dr["RFIDNO"].ToString().Trim();
                        machine.Register(empcode, rfid, out err);
                        MessageBox.Show(err, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    machine.DisConnect(out err);
                }

               
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show("Registraton of All operator has been complete in selected machine.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRegAdd_Click(object sender, EventArgs e)
        {
            string[] selemp = cmbEmp.GetItemText(cmbEmp.SelectedItem).ToString().Split(',');
            if (selemp.Length <= 0)
            {
                MessageBox.Show("Please Select Employee", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string empcode = selemp[0];
            //get rfid from db
            string err = "";
            string rfid = Utility.GetDescription("Select RFIDNo from ccmRFIDOperator where OperatorCode ='" + empcode + "'", sqlcn, out err);
            if (string.IsNullOrEmpty(rfid))
            {
                MessageBox.Show("RFCard No not found in database,please add/update operator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem selitem in chkListRFID.CheckedItems)
            {
                this.Cursor = Cursors.WaitCursor;
                string MachineIP = selitem.Description.ToString().Split(',')[0];
                clsMachine machine = new clsMachine(MachineIP, "I");
                machine.Connect(out err);
                if (!string.IsNullOrEmpty(err)) 
                {
                    MessageBox.Show(err, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    machine.DisConnect(out err);
                    continue;
                }
                machine.Register(empcode, rfid, out err);
                this.Cursor=Cursors.Default;
                MessageBox.Show(err,"Result",MessageBoxButtons.OK,MessageBoxIcon.Information);
                machine.DisConnect(out err);
            }

            ReloadData();
        }       

        private void btnRegDel_Click(object sender, EventArgs e)
        {
            string[] selemp = cmbEmp.GetItemText(cmbEmp.SelectedItem).ToString().Split(',');
            if (selemp.Length <= 0)
            {
                MessageBox.Show("Please Select Employee", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string empcode = selemp[0];
            //get rfid from db
            string err = "";
            string rfid = Utility.GetDescription("Select RFIDNo from ccmRFIDOperator where OperatorCode ='" + empcode + "'", sqlcn, out err);
            if (string.IsNullOrEmpty(rfid))
            {
                MessageBox.Show("RFCard No not found in database,please add/update operator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem selitem in chkListRFID.CheckedItems)
            {
                this.Cursor = Cursors.WaitCursor;
                string MachineIP = selitem.Description.ToString().Split(',')[0];
                clsMachine machine = new clsMachine(MachineIP, "I");
                
                machine.Connect(out err);
                if (!string.IsNullOrEmpty(err))
                {
                    MessageBox.Show(err, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    machine.DisConnect(out err);
                    continue;
                }

                machine.DeleteUser(empcode, out err);
                machine.DisConnect(out err);
                this.Cursor = Cursors.Default;
                MessageBox.Show(err, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ReloadData();
        }

        private void btnRegTime_Click(object sender, EventArgs e)
        {
            
            string err = "";
            foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem selitem in chkListRFID.CheckedItems)
            {
                this.Cursor = Cursors.WaitCursor;
                string MachineIP = selitem.Description.ToString().Split(',')[0];
                clsMachine machine = new clsMachine(MachineIP, "I");
                machine.Connect(out err);
                if (!string.IsNullOrEmpty(err))
                {
                    MessageBox.Show(err, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    machine.DisConnect(out err);
                    continue;
                }
                machine.SetTime(out err);
                if(string.IsNullOrEmpty(err))
                    machine.StoreHistory("","Time Set",out err);
                
                machine.DisConnect(out err);
                this.Cursor = Cursors.Default;
                
                MessageBox.Show("Time Set Completed->" + MachineIP, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
       
        private void btnRegDownload_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string err = string.Empty;
            foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem selitem in chkListRFID.CheckedItems)
            {
                string MachineIP = selitem.Description.ToString().Split(',')[0];
                clsMachine machine = new clsMachine(MachineIP, "I");
                machine.Connect(out err);
                if (!string.IsNullOrEmpty(err))
                {
                    MessageBox.Show(err, "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    machine.DisConnect(out err);
                    continue;
                }
                List<AttdLog> punches = new List<AttdLog>();
                
                machine.GetAttdRec(out punches, out err);
                machine.StoreHistory("", "Download Data", out err);
                machine.DisConnect(out err);
            }
            
            this.Cursor = Cursors.Default;
            MessageBox.Show("Complted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ReloadData();
        }
       
        
        
        #endregion

       
    }
}
