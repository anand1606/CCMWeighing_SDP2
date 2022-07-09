using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
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


namespace CCMDataCapture
{
    public partial class frmEmailConfig : Form
    {

        public string mode = "NEW";
        
        public string oldCode = "";
        private static string cnstr;
        private string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();


        
        public frmEmailConfig()
        {
            InitializeComponent();
        }

        private void frmEmailConfig_Load(object sender, EventArgs e)
        {
            string sqlconfig = "sql_connection.txt";
            string fullpath = Path.Combine(strpath, sqlconfig);
            cnstr = File.ReadLines(fullpath).First();

            ResetCtrl();
            ResetCtrl2();
            ResetCtrl3();
            loadEmailReportType();
            loadMachinePara();
            LoadSchLogGrid();
        }



        private void loadMachinePara()
        {
            string sql = "select MachineName from ccmMachineConfig where 1 = 1  ";

            string err = string.Empty;

            DataSet ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            txtMachinePara.Items.Clear();

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtMachinePara.Items.Add(dr["MachineName"].ToString());

                }
            }

            txtMachinePara.Items.Add("ALL");
        }


        private string selectReportType(string reportid)
        {
            string sql = "select ReportID ,ReportName from EmailReports where ReportID ='" + reportid + "'";
            string retstr = string.Empty;
            string err = string.Empty;

            DataSet ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    retstr = dr["ReportID"].ToString() + "," + dr["ReportName"].ToString();
                    
                }
            }

            return retstr;
        }
        
        private void loadEmailReportType()
        {
            string sql = "select ReportID ,ReportName from EmailReports where 1 = 1 Order by ReportID ";

            string err = string.Empty;

            DataSet ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            txtEmailReportID.Items.Clear();

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string t = dr["ReportID"].ToString() + "," + dr["ReportName"].ToString();
                    txtEmailReportID.Items.Add(t);

                }
            }
        }

        private bool isContainSpace(string x)
        {
            bool result = false;

            if (x.Contains(' '))
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        #region EmailAccount
        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtEmailAccount.Text))
            {
                err = err + "Please Enter Sender's Email" + Environment.NewLine;
            }
            else
            {
                if(isContainSpace(txtEmailAccount.Text.Trim().ToString()))
                    err = err + "Invalid Sender's Email, contains space" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtDisplayName.Text))
            {
                err = err + "Please Enter Display Name..." + Environment.NewLine;
            }
            


            if (string.IsNullOrEmpty(txtAccountUserID.Text))
            {
                err = err + "Please Enter Sender's UserID" + Environment.NewLine;
            }
            else
            {
                if (isContainSpace(txtAccountUserID.Text.Trim().ToString()))
                    err = err + "Invalid Sender's UserID, contains space" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtAccountPassword.Text))
            {
                err = err + "Please Enter Password" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtSmtpServer.Text))
            {
                err = err + "Please Enter SMTP Server Address" + Environment.NewLine;
            }
            else
            {
                if (isContainSpace(txtSmtpServer.Text.Trim().ToString()))
                    err = err + "Invalid SMTP Server Address, contains space" + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtOutGoingPort.Text))
            {
                err = err + "Please Enter Outgoing Port" + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(txtOutGoingPort.Text.Trim().ToString()))
            {
                int tmpport = 0;
                int.TryParse(txtOutGoingPort.Text.Trim().ToString(), out tmpport);

                if (tmpport == 0)
                {
                    err = err + "Invalid Outgoing Port, enter in number format" + Environment.NewLine;
                }
            }
            

            return err;
        }

        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;


            object s = new object();
            EventArgs e = new EventArgs();
            txtEmailAccount.Text = "";
            txtDisplayName.Text = "";
            txtEmailAccount_Validated(s, e);

            txtAccountUserID.Text = "";
            txtAccountPassword.Text = "";
            txtSmtpServer.Text = "";
            txtOutGoingPort.Text = "";

            chkIsDefault.Checked = false;
            chkIsTLS.Checked = false;
            
            oldCode = "";
            mode = "NEW";
            LoadGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Insert into EmailConfig (EmailAccount,DisplayName,AccountUserID,AccountPassword,SmtpServer,OutGoingPort,IsDefault,isTLS,AddDt) Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',GetDate())";
                        sql = string.Format(sql, txtEmailAccount.Text.Trim().ToString(),
                             txtDisplayName.Text.Trim().ToString(),
                             txtAccountUserID.Text.Trim().ToString(),
                             txtAccountPassword.Text.Trim().ToString(),
                             txtSmtpServer.Text.Trim().ToString(),
                             txtOutGoingPort.Text.Trim().ToString(),
                             (chkIsDefault.Checked?"1":"0"),
                             (chkIsTLS.Checked?"1":"0")                            
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string err = DataValidate();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Update EmailConfig Set DisplayName='{0}',AccountUserID='{1}',AccountPassword='{2}', UpdDt = GetDate(), " +
                            " SmtpServer = '{3}', OutGoingPort = '{4}', IsDefault = '{5}' , isTLS = '{6}' " +
                            " Where EmailAccount = '{7}' ";

                        //txtEmailAccount.Text.ToString().trim(),
                        sql = string.Format(sql, 
                             txtDisplayName.Text.Trim().ToString(),
                             txtAccountUserID.Text.Trim().ToString(),
                             txtAccountPassword.Text.Trim().ToString(),
                             txtSmtpServer.Text.Trim().ToString(),
                             txtOutGoingPort.Text.Trim().ToString(),
                             (chkIsDefault.Checked?"1":"0"),
                             (chkIsTLS.Checked?"1":"0"),
                             txtEmailAccount.Text.Trim().ToString()
                           );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(txtEmailAccount.Text.Trim().ToString()))
            {
                MessageBox.Show("Please Enter Sender's Email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Delete From EmailConfig Where EmailAccount = '{0}'";
                        sql = string.Format(sql, txtEmailAccount.Text.Trim().ToString()
                             
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetCtrl();
           
        }

        private void txtEmailAccount_Validated(object sender, EventArgs e)
        {
            if (txtEmailAccount.Text.Trim() == "" )
            {

                return;
            }
            
            
            DataSet ds = new DataSet();
            string sql = "select * From EmailConfig where EmailAccount ='" + txtEmailAccount.Text.Trim() + "' ";

            string err;
            ds = Utility.GetData(sql, cnstr,out err);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                   
                    txtDisplayName.Text = dr["DisplayName"].ToString();
                   
                    txtAccountUserID.Text = dr["AccountUserID"].ToString();
                    txtAccountPassword.Text = dr["AccountPassword"].ToString();
                    txtSmtpServer.Text = dr["SmtpServer"].ToString();
                    txtOutGoingPort.Text = dr["OutGoingPort"].ToString();

                    chkIsDefault.Checked = Convert.ToBoolean(dr["IsDefault"]);
                    chkIsTLS.Checked = Convert.ToBoolean(dr["isTLS"]); ;


                    mode = "OLD";
                    oldCode = dr["EmailAccount"].ToString();
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                    btnAdd.Enabled = false;

                }
            }
            else
            {
                mode = "NEW";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnAdd.Enabled = true;
            }

            
        }


        private void LoadGrid()
        {
            

            DataSet ds = new DataSet();
            string sql = "select EmailAccount from EmailConfig where 1 = 1 " ;

            string err = string.Empty;

            ds = Utility.GetData(sql, cnstr,out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);


            if (hasRows)
            {
                gridAccount.DataSource = ds;
                gridAccount.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                gridAccount.DataSource = null;
            }
        }

       

        private void gv_Account_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClick(view, pt);
        }

        private void DoRowDoubleClick(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                txtEmailAccount.Text = gv_Account.GetRowCellValue(info.RowHandle, "EmailAccount").ToString();
               

                object o = new object();
                EventArgs e = new EventArgs();
                mode = "OLD";
                oldCode = txtEmailAccount.Text.Trim();
                txtEmailAccount_Validated(o, e);
                
            }


        }
        #endregion


        #region EmailReceipint

        private void LoadRecGrid()
        {


            DataSet ds = new DataSet();
            string sql = "select ID,PersonName,EmailId,EmailType from EmailRecipients where 1 = 1 ";

            string err = string.Empty;

            ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);


            if (hasRows)
            {
                gridControl1.DataSource = ds;
                gridControl1.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                gridControl1.DataSource = null;
            }
        }
        private void ResetCtrl2()
        {
            btnAddRec.Enabled = false;
            btnUpdateRec.Enabled = false;
            btnDeleteRec.Enabled = false;

            txtID.Text = "";
            txtPersonName.Text = "";
            txtEmailID.Text = "";
            txtEmailType.Text = "";
            
            LoadRecGrid();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClickRec(view, pt);
        }

        private void DoRowDoubleClickRec(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                txtID.Text = gridView1.GetRowCellValue(info.RowHandle, "ID").ToString();
                txtPersonName.Text = gridView1.GetRowCellValue(info.RowHandle, "PersonName").ToString();
                
                //txtEmailID.Text = gridView1.GetRowCellValue(info.RowHandle, "EmailID").ToString();
                //txtEmailType.Text = gridView1.GetRowCellValue(info.RowHandle, "EmailType").ToString();

                object o = new object();
                EventArgs e = new EventArgs();
                txtID_Validated(o, e);

            }


        }
        
        private void txtID_Validated(object sender, EventArgs e)
        {
            if (txtID.Text.Trim() == "")
            {

                return;
            }


            DataSet ds = new DataSet();
            string sql = "select * From EmailRecipients where ID ='" + txtID.Text.Trim() + "' ";

            string err;
            ds = Utility.GetData(sql, cnstr, out err);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtPersonName.Text = dr["PersonName"].ToString();

                    txtEmailID.Text = dr["EmailID"].ToString();
                    txtEmailType.Text = dr["EmailType"].ToString();
                    
                    btnUpdateRec.Enabled = true;
                    btnDeleteRec.Enabled = true;
                    btnAddRec.Enabled = false;

                }
            }
            else
            {
                
                btnUpdateRec.Enabled = false;
                btnDeleteRec.Enabled = false;
                btnAddRec.Enabled = true;
            }


        }

        private string DataValidate2()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtID.Text))
            {
                err = err + "Please Enter Sr.No." + Environment.NewLine;
            }
            

            if (string.IsNullOrEmpty(txtPersonName.Text))
            {
                err = err + "Please Enter Person Name..." + Environment.NewLine;
            }



            if (string.IsNullOrEmpty(txtEmailID.Text))
            {
                err = err + "Please Enter recipients's email id.." + Environment.NewLine;
            }
            

            if (string.IsNullOrEmpty(txtEmailType.Text))
            {
                err = err + "Please Enter Email Type" + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(txtID.Text.Trim().ToString()))
            {
                int tmpport = 0;
                int.TryParse(txtID.Text.Trim().ToString(), out tmpport);

                if (tmpport == 0)
                {
                    err = err + "Invalid Sr.No, enter in number format" + Environment.NewLine;
                }
            }
            

            return err;
        }

        private void btnAddRec_Click(object sender, EventArgs e)
        {
            string err = DataValidate2();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Insert into EmailRecipients (ID,PersonName,EmailID,EmailType,AddDt) Values ('{0}','{1}','{2}','{3}',GetDate())";
                        sql = string.Format(sql, txtID.Text.Trim().ToString(),
                             txtPersonName.Text.Trim().ToString(),
                             txtEmailID.Text.Trim().ToString(),
                             txtEmailType.Text.Trim().ToString()
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl2();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnUpdateRec_Click(object sender, EventArgs e)
        {
            string err = DataValidate2();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Update EmailRecipients Set PersonName='{0}',EmailID='{1}',EmailType='{2}', UpdDt = GetDate() " +
                            " Where ID = '{3}' ";

                        //txtEmailAccount.Text.ToString().trim(),
                        sql = string.Format(sql,
                             txtPersonName.Text.Trim().ToString(),
                             txtEmailID.Text.Trim().ToString(),
                             txtEmailType.Text.Trim().ToString(),
                             txtID.Text.Trim().ToString()
                           );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl2();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnDeleteRec_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtID.Text.Trim().ToString()))
            {
                MessageBox.Show("Please Enter Sr.No ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Delete From EmailRecipients Where ID = '{0}'";
                        sql = string.Format(sql, txtID.Text.Trim().ToString()
                             
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl2();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

        private void btnCancelRec_Click(object sender, EventArgs e)
        {
            ResetCtrl2();

        }

        #endregion

        #region EmailScheduling

        private void LoadSchGrid()
        {


            DataSet ds = new DataSet();
            string sql = "select ID,SchTime,DatePara,ShiftPara,MachinePara from EmailSchedule where 1 = 1 ";

            string err = string.Empty;

            ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);


            if (hasRows)
            {
                gridSchedule.DataSource = ds;
                gridSchedule.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                gridSchedule.DataSource = null;
            }
        }

        private void ResetCtrl3()
        {
            btnAddSch.Enabled = false;
            btnUpdateSch.Enabled = false;
            btnDeleteSch.Enabled = false;

            txtSchID.Text = "";
            txtSchTime.EditValue = null;
            txtSubject.Text = "";
            txtDateParaType.SelectedIndex = -1;
            txtShiftPara.SelectedIndex = -1;
            txtMachinePara.SelectedIndex = -1;
            txtEmailReportID.SelectedIndex = -1;
            txtExportPath.Text = "";
            LoadSchGrid();
        }

        private void gv_Schedule_DoubleClick(object sender, EventArgs e)
        {
            GridView view = (GridView)sender;
            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            DoRowDoubleClickSch(view, pt);
        }

        private void DoRowDoubleClickSch(GridView view, Point pt)
        {
            GridHitInfo info = view.CalcHitInfo(pt);
            if (info.InRow || info.InRowCell)
            {
                txtSchID.Text = gv_Schedule.GetRowCellValue(info.RowHandle, "ID").ToString();
              
                object o = new object();
                EventArgs e = new EventArgs();
                txtSchID_Validated(o, e);

            }


        }

        private void txtSchID_Validated(object sender, EventArgs e)
        {
            if (txtSchID.Text.Trim() == "")
            {

                return;
            }


            DataSet ds = new DataSet();
            string sql = "select * From EmailSchedule where ID ='" + txtSchID.Text.Trim() + "' ";

            string err;
            ds = Utility.GetData(sql, cnstr, out err);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtSchTime.EditValue = dr["SchTime"].ToString();
                    txtSubject.Text = dr["Subject"].ToString();
                    txtDateParaType.Text = dr["DatePara"].ToString();
                    txtShiftPara.Text = dr["ShiftPara"].ToString();
                    txtMachinePara.Text = dr["MachinePara"].ToString();

                    string reportid = dr["EmailReportID"].ToString();
                    txtEmailReportID.Text = selectReportType(reportid);
                    txtExportPath.Text = dr["ExportPath"].ToString();
                    //set report id


                    btnUpdateSch.Enabled = true;
                    btnDeleteSch.Enabled = true;
                    btnAddSch.Enabled = false;

                }
            }
            else
            {

                btnUpdateSch.Enabled = false;
                btnDeleteSch.Enabled = false;
                btnAddSch.Enabled = true;
            }


        }

        private string DataValidate3()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtSchID.Text))
            {
                err = err + "Please Enter Sr.No." + Environment.NewLine;
            }


            if (txtSchTime.EditValue == null)
            {
                err = err + "Please Enter Schedule Time..." + Environment.NewLine;
            }



            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                err = err + "Please Enter Subject.." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtEmailReportID.Text))
            {
                err = err + "Please Select Report.." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtDateParaType.Text))
            {
                err = err + "Please Select Date Parameter Type.." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtShiftPara.Text))
            {
                err = err + "Please Select Shift Parameter.." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtMachinePara.Text))
            {
                err = err + "Please Select Machine Parameter.." + Environment.NewLine;
            }

            
            if (!string.IsNullOrEmpty(txtSchID.Text.Trim().ToString()))
            {
                int tmpport = 0;
                int.TryParse(txtSchID.Text.Trim().ToString(), out tmpport);

                if (tmpport == 0)
                {
                    err = err + "Invalid Sr.No, enter in number format" + Environment.NewLine;
                }
            }

            if (!string.IsNullOrEmpty(txtExportPath.Text.Trim().ToString()))
            {
                //check if path exist
                if (!Directory.Exists(txtExportPath.Text.Trim().ToString()))
                {
                    err = err + "Export Path does not exist.." + Environment.NewLine;
                }
            }

            return err;
        }

        private void btnAddSch_Click(object sender, EventArgs e)
        {
            string err = DataValidate3();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Insert into EmailSchedule (ID,SchTime,Subject,EmailReportID,DatePara,ShiftPara,MachinePara,ExportPath,LastExecutedOn,AddDt) Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',GetDate())";
                        sql = string.Format(sql, txtSchID.Text.Trim().ToString(),
                             Convert.ToDateTime(txtSchTime.EditValue).ToString("HH:mm"),
                             txtSubject.Text.Trim().ToString(),
                             txtEmailReportID.Text.Split(',')[0],
                             txtDateParaType.Text.Trim().ToString(),
                             txtShiftPara.Text.Trim().ToString(),
                             txtMachinePara.Text.Trim().ToString(),                             
                             txtExportPath.Text.Trim().ToString(),
                             "2000-01-01"
                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl3();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnUpdateSch_Click(object sender, EventArgs e)
        {
            string err = DataValidate3();
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Update EmailSchedule Set SchTime='{0}',Subject='{1}', EmailReportID = '{2}', DatePara = '{3}' , ShiftPara = '{4}', MachinePara = '{5}' , ExportPath = '{6}', UpdDt = GetDate()  " +
                            " Where ID = '{7}' ";

                        //txtEmailAccount.Text.ToString().trim(),
                        sql = string.Format(sql,
                             Convert.ToDateTime(txtSchTime.EditValue).ToString("HH:mm"),
                             txtSubject.Text.Trim().ToString(),
                             txtEmailReportID.Text.Split(',')[0],
                             txtDateParaType.Text.Trim().ToString(),
                             txtShiftPara.Text.Trim().ToString(),
                             txtMachinePara.Text.Trim().ToString(),
                             txtExportPath.Text.Trim().ToString(),
                             txtSchID.Text.Trim().ToString()
                           );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Updated...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl3();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void btnDeleteSch_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtSchID.Text.Trim().ToString()))
            {
                MessageBox.Show("Please Enter Sr.No ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        cn.Open();
                        cmd.Connection = cn;
                        string sql = "Delete From EmailSchedule Where ID = '{0}'";
                        sql = string.Format(sql, txtSchID.Text.Trim().ToString()

                            );

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetCtrl3();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

        private void btnCancelSch_Click(object sender, EventArgs e)
        {
            ResetCtrl3();

        }
        #endregion

        #region EmailLog

        private void LoadSchLogGrid()
        {


            DataSet ds = new DataSet();
            string sql = "SELECT TOP (50) [ID],Convert(varchar(19),[ExecutionDt],121) as ExecutedOn,[ScheduleID],[EmailStatus],[Error] FROM [EmailScheduleLog] Order by ID Desc";

            string err = string.Empty;

            ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);


            if (hasRows)
            {
                gridEmailLog.DataSource = ds;
                gridEmailLog.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                gridEmailLog.DataSource = null;
            }
        }

        private void btnRefLog_Click(object sender, EventArgs e)
        {
            LoadSchLogGrid();
        }

        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtExportPath.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }  
        }




    }
}
