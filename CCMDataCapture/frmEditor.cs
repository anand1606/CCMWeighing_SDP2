using DevExpress.XtraEditors.Controls;
using System;
using System.Collections;
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
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors;

namespace CCMDataCapture
{
    public partial class frmEditor : Form
    {

        public static DataSet DS;

        private static string cnstr;
        private string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();
        

        public frmEditor()
        {
            InitializeComponent();
            DS = new DataSet();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV File|*.csv|Excel (2010)|*.xlsx|Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".csv":
                            gridView1.ExportToCsv(exportFilePath);
                            break;
                        case ".xls":
                            gridView1.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridView1.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gridView1.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gridView1.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gridView1.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            gridView1.ExportToMht(exportFilePath);
                            break;
                        default:
                            break;
                    }

                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            //Try to open the file and let windows decide how to open it.
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            String msg = "The file could not be opened." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                            MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        String msg = "The file could not be saved." + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        MessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (txtDate.EditValue == null || txtMachines.Text.Trim() == "" || txtShift.Text.Trim() == "")
            {
                MessageBox.Show("Please Select required parameters..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LockCtrl();

            string err = string.Empty;
            string sql = "select InchargeName from ccmShiftWiseInfo where tDate='" + txtDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim().ToString() + "'";

            txtIncharge.Text = Utility.GetDescription(sql, cnstr, out err);

            //load grid..
            string tablename = GetTableName(txtMachines.Text.Trim());
            sql =
                "Select SrNo, Convert(varchar(10),LogDateTime,121) as LogDate,Convert(varchar(5),Convert(time(5),LogDateTime)) as LogTime, " +
                " MachineNo,tShift,IntSrNo,PipeNumber,PipeDia,PipeClass,PipeLength,ActWt,NomWt,Material,Standard,JointType,MouldNo,PipeStatus,MinWt,MaxWt,AlmMinWt,AlmMaxWt, " +
                " ABS((Case When(NomWt <= 0) then 0 else Round(((NomWt - ActWt) / NomWt * 100), 3) end)) as DevPer,Remarks, OperatorCode,OperatorName " +
                " From [" + tablename + "] " +
                " where tDate='" + txtDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim().ToString() + "'";

                       
            grd_report.DataSource = null;

            DataTable dt;
            DataSet ds = Utility.GetData(sql, cnstr,out err);
            dt = null;
           
            try
            {
                dt = ds.Tables[0].Copy();
                dt.TableName = GetTableName(txtMachines.Text.Trim());
                DS = new DataSet();
                DS.Tables.Add(dt);

                grd_report.DataSource = DS;
                grd_report.DataMember = tablename;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetCtrl();
                return ;
            }
           

        }

        private void frmEditor_Load(object sender, EventArgs e)
        {
            txtDate.EditValue = DateTime.Now.Date;
            txtShift.SelectedText = "A";
            txtMachines.SelectedText = "P";


            cnstr = Utility.SQLCnStr;

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                string str = "select Description from ccmDefect Order by Description ";                             
                SqlCommand cmd = new SqlCommand(str, cn);

                //fill defect codes
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ComboBoxItemCollection items_DT = this.rptStatus.Items;
                items_DT = this.rptStatus.Items;
                while (dr.Read())
                {
                    items_DT.Add(dr["Description"].ToString());
                }
                dr.Close();
                this.rptStatus.TextEditStyle = TextEditStyles.DisableTextEditor;
                this.gridView1.Columns["PipeStatus"].ColumnEdit = this.rptStatus;


                txtMachines.Items.Clear();
                str = "select MachineName from ccmMachineConfig  ";
                cmd = new SqlCommand(str, cn);
                dr = cmd.ExecuteReader();                
               
                while (dr.Read())
                {
                    txtMachines.Items.Add(dr["MachineName"].ToString());
                }
                dr.Close();

                //fill standard
                items_DT = this.rptStandard.Items;
                str = "Select Description from ccmStandard order by Description";
                cmd = new SqlCommand(str,cn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items_DT.Add(dr["Description"].ToString());
                }
                dr.Close();
                this.rptStandard.TextEditStyle = TextEditStyles.DisableTextEditor;
                this.gridView1.Columns["Standard"].ColumnEdit = this.rptStandard;



                //class
                items_DT = this.rptClass.Items;
                str = "Select Description from ccmClass order by Description";
                cmd = new SqlCommand(str, cn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items_DT.Add(dr["Description"].ToString());
                }
                dr.Close();
                this.rptClass.TextEditStyle = TextEditStyles.DisableTextEditor;
                this.gridView1.Columns["PipeClass"].ColumnEdit = this.rptClass;

                //length
                items_DT = this.rptLength.Items;
                str = "Select Description from ccmLength order by Description";
                cmd = new SqlCommand(str, cn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items_DT.Add(dr["Description"].ToString());
                }
                dr.Close();
                this.rptLength.TextEditStyle = TextEditStyles.DisableTextEditor;
                this.gridView1.Columns["PipeLength"].ColumnEdit = this.rptLength;




                //fill material
                items_DT = this.rptMaterial.Items;
                str = "Select Description from ccmMaterial order by Description";
                cmd = new SqlCommand(str, cn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    items_DT.Add(dr["Description"].ToString());
                }
                dr.Close();
                this.rptMaterial.TextEditStyle = TextEditStyles.DisableTextEditor;
                this.gridView1.Columns["Material"].ColumnEdit = this.rptMaterial;




            }//using sql connection


            

        }


        private void LockCtrl()
        {
            txtDate.Enabled = false;
            txtShift.Enabled = false;
            txtMachines.Enabled = false;
            btnRefresh.Enabled = false;
            btnSave.Enabled = true;
        }

        private void UnlockCtrl()
        {
            txtDate.Enabled = true;
            txtShift.Enabled = true;
            txtMachines.Enabled = true;
            btnSave.Enabled = false;
            btnRefresh.Enabled = true;
        }

        private void ResetCtrl()
        {
            //txtDate.EditValue = null;
            txtIncharge.Text = "";
            //txtShift.Text = "";
            //txtMachines.Text = "";
            //txtMachines.SelectedIndex = -1;
            //txtShift.SelectedIndex = -1;
            btnSave.Enabled = false;
            btnRefresh.Enabled = true;
            cmbBulkItem.SelectedIndex = 0;
            txtBulkValue.EditValue = "";
            txtBulkValue.Text = "";
        }


        private string GetTableName(string tmachine)
        {
            string tablename = string.Empty;

            if (!string.IsNullOrEmpty(tmachine))
            {

                string err = string.Empty;
                tablename = Utility.GetDescription("Select TableName from ccmMachineConfig Where MachineName = '" + tmachine + "'", cnstr, out err);



            }
            return tablename;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            grd_report.DataSource = null;
            ResetCtrl();
            UnlockCtrl();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDate.EditValue == null || txtMachines.Text.Trim() == "" || txtShift.Text.Trim() == "")
            {
                MessageBox.Show("Please Select required parameters..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            string sql = "select count(*) from ccmShiftWiseInfo where tDate='" + txtDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim().ToString() + "'";
            string err = string.Empty;
            string rec = Utility.GetDescription(sql, cnstr, out err);

            int t = 0;

            int.TryParse(rec, out t);

            if (t == 0)
            {
                sql = "Insert into ccmShiftWiseInfo (tDate,tShift,InchargeName,AddDt) Values ('" + txtDate.DateTime.Date.ToString("yyyy-MM-dd") + "','" + txtShift.Text.Trim() + "','" + txtIncharge.Text.Trim().ToString() + "',getdate())";
            }
            else
            {
                sql = "Update ccmShiftWiseInfo Set InchargeName ='" + txtIncharge.Text.Trim().ToString() + "', UpdDt = GetDate() where tDate='" + txtDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim().ToString() + "'";
            }

            using (SqlConnection cn = new SqlConnection(cnstr))
            {
                try
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        string tablename = GetTableName(txtMachines.Text.Trim());

                        //foreach datarow and save pipe status
                        DataTable HrlyView = DS.Tables[GetTableName(txtMachines.Text.Trim())].Copy();
                        foreach (DataRow Nhr in HrlyView.Rows)
                        {
                            sql = "update [" + tablename + "] Set PipeStatus ='" + Nhr["PipeStatus"].ToString() + "'," +
                                " Material='" + Nhr["Material"].ToString() + "'," +
                                " Standard='" + Nhr["Standard"].ToString() + "'," +
                                " JointType='" + Nhr["JointType"].ToString() + "'," +
                                " MouldNo='" + Nhr["MouldNo"].ToString() + "'," +
                                " PipeClass='" + Nhr["PipeClass"].ToString() + "'," +
                                " PipeLength='" + Nhr["PipeLength"].ToString() + "'," +
                                " NomWt='" + Nhr["NomWt"].ToString() + "'," +
                                " MinWt='" + Nhr["MinWt"].ToString() + "'," +
                                " MaxWt='" + Nhr["MaxWt"].ToString() + "'," +
                                " AlmMinWt='" + Nhr["AlmMinWt"].ToString() + "'," +
                                " AlmMaxWt='" + Nhr["AlmMaxWt"].ToString() + "'," +
                                " Remarks='" + Nhr["Remarks"].ToString() + "'," +
                                " UpdDt = GetDate() Where " +
                                " tDate ='" + txtDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim() + "' " +
                                " And SrNo ='" + Nhr["SrNo"].ToString() + "'";
                            cmd.CommandText = sql;
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = cn;
                            cmd.ExecuteNonQuery();
                        }
                        
                        MessageBox.Show("Information Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Cursor = Cursors.Default;
                        btnRefresh_Click(sender, e);
                        return;
                    }

                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }                
            }

        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            string pstatus = Convert.ToString(gridView1.GetRowCellValue(e.RowHandle, "PipeStatus"));
            if (string.IsNullOrEmpty(pstatus))
            {
                e.Valid = false;
                e.ErrorText = string.Format("Pipe Status is Requred.\n");
                return;
            }

            double nomwt = Convert.ToDouble(gridView1.GetRowCellValue(e.RowHandle, "NomWt"));

            if(nomwt <= 0)
            {
                e.Valid = false;
                e.ErrorText = string.Format("Nominam Weight is Requred.\n");
                return;
            }

            string err = string.Empty;
            string sql = "select * from ccmWeightMaster where Size='" + gridView1.GetRowCellValue(e.RowHandle, "PipeDia") + "' " +
                " And Class='" + gridView1.GetRowCellValue(e.RowHandle, "PipeClass") + "' " +
                " And Len='" + gridView1.GetRowCellValue(e.RowHandle, "PipeLength") + "'";
            //minwt,maxwt,nomwt,alminwt,almmaxwt - change to be need if incase any change in size,class,length
            DataSet ds = Utility.GetData(sql, Utility.SQLCnStr, out err);
            if (!string.IsNullOrEmpty(err))
            {
                e.Valid = false;
                e.ErrorText = string.Format("Error from ->Get Auto Weight informaiton->" + err);
                return;
            }

            bool hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasRows)
            {                
                DataRow dr = ds.Tables[0].Rows[0];
                gridView1.SetRowCellValue(e.RowHandle, "MinWt", dr["MinWt"].ToString());
                gridView1.SetRowCellValue(e.RowHandle, "MaxWt", dr["MaxWt"].ToString());
               // gridView1.SetRowCellValue(e.RowHandle, "NomWt", dr["NomWt"].ToString());
                gridView1.SetRowCellValue(e.RowHandle, "AlmMinWt", dr["AlmMinWt"].ToString());
                gridView1.SetRowCellValue(e.RowHandle, "AlmMaxWt", dr["AlmMaxWt"].ToString());
            }
            

        }

        private void UpdateSelectedRows(string columnname, string columnvalue)
        {
            ArrayList rows = new ArrayList();

            // Add the selected rows to the list.
            Int32[] selectedRowHandles = gridView1.GetSelectedRows();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                int selectedRowHandle = selectedRowHandles[i];
                if (selectedRowHandle >= 0)
                    rows.Add(gridView1.GetDataRow(selectedRowHandle));
            }
            try
            {
                gridView1.BeginUpdate();
                for (int i = 0; i < rows.Count; i++)
                {
                    DataRow row = rows[i] as DataRow;
                    // Change the field value.
                    row[columnname] = columnvalue;
                }
            }
            finally
            {
                gridView1.EndUpdate();
            }
        }

        private void btnBulkChange_Click(object sender, EventArgs e)
        {
            
            //return if no item is selected
            if(cmbBulkItem.SelectedIndex <= 0)
            {
                
                return;
            }

            if(cmbBulkItem.SelectedItem.ToString().Trim() == "")
            {
                return;
            }

            //return if not selected any rows
            Int32[] selectedRowHandles = gridView1.GetSelectedRows();
            if (selectedRowHandles.Length <= 1)
            {
                MessageBox.Show("Please Select multiple rows..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                return;
            }

            if(txtBulkValue.EditValue == null)
            {
                MessageBox.Show("Please Enter Value..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtBulkValue.EditValue.ToString().Trim()))
            {
                MessageBox.Show("Please Enter Value..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //validate nomwt


            if (cmbBulkItem.SelectedItem.ToString() == "NomWt")
            {
                double tnomwt = 0;
                double.TryParse(txtBulkValue.EditValue.ToString().Trim(), out tnomwt);
                if (tnomwt <= 0)
                {
                    MessageBox.Show("Please Enter Nominal Weight..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string changedvalue = txtBulkValue.EditValue.ToString().Trim();

            switch (cmbBulkItem.Text.Trim().ToString())
            {
                case "PipeClass":
                    UpdateSelectedRows("PipeClass", changedvalue);
                    break;
                case "PipeLength":
                    UpdateSelectedRows("PipeLength", changedvalue);
                    break;
                case "JointType":
                    UpdateSelectedRows("JointType", changedvalue);
                    break;
                case "MouldNo":
                    UpdateSelectedRows("MouldNo", changedvalue);
                    break;
                case "NomWt":
                    UpdateSelectedRows("NomWt", changedvalue);
                    break;
                case "Standard":
                    UpdateSelectedRows("Standard", changedvalue);
                    break;
                case "Material":
                    UpdateSelectedRows("Material", changedvalue);
                    break;
                case "PipeStatus":
                    UpdateSelectedRows("PipeStatus", changedvalue);
                    break;
                case "Remarks":
                    UpdateSelectedRows("Remarks", changedvalue);
                    break;

                default:
                    break;
            }

            cmbBulkItem.SelectedIndex = 0;
            txtBulkValue.EditValue = null;
        }

        private void cmbBulkItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBulkItem.SelectedIndex <= 0)
            {
                return;
            }


            if (string.IsNullOrEmpty(cmbBulkItem.SelectedItem.ToString()))
            {
                return;
            }

            txtBulkValue.EditValue = "";
            txtBulkValue.Properties.Items.Clear();

            switch (cmbBulkItem.SelectedItem.ToString())
            {
                case "PipeClass":                    
                    txtBulkValue.Properties.Items.AddRange(this.rptClass.Items);
                    break;
                case "PipeLength":                    
                    txtBulkValue.Properties.Items.AddRange(this.rptLength.Items);
                    break; 
                case "Standard":                   
                    txtBulkValue.Properties.Items.AddRange(this.rptStandard.Items);
                    break;
                case "Material":                    
                    txtBulkValue.Properties.Items.AddRange(this.rptMaterial.Items);
                    break;
                case "PipeStatus":
                    txtBulkValue.Properties.Items.AddRange(this.rptStatus.Items);
                    break;
               

                default:
                    break;
            }

        }
    }
}
