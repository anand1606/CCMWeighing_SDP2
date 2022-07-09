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
    public partial class frmWeightMaster : Form
    {

        public string mode = "NEW";

        public string oldCode = "";
        private static string cnstr;
        private string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();

        
        
        public frmWeightMaster()
        {
            InitializeComponent();
        }

        private void frmWeightMaster_Load(object sender, EventArgs e)
        {
            string sqlconfig = "sql_connection.txt";
            string fullpath = Path.Combine(strpath, sqlconfig);
            cnstr = File.ReadLines(fullpath).First();

            loadSize();
            loadClass();
            loadLength();
            ResetCtrl();
        }

        private void loadSize()
        {
            txtSize.Items.Clear();
            string sql = "select Description from ccmSize where 1 = 1 Order by ID ";

            string err = string.Empty;

            DataSet ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string t = dr["Description"].ToString();
                    txtSize.Items.Add(t);

                }
            }
        }

        private void loadClass()
        {
            txtClass.Items.Clear();
            string sql = "select Description from ccmClass where 1 = 1 Order by ID ";

            string err = string.Empty;

            DataSet ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string t = dr["Description"].ToString();
                    txtClass.Items.Add(t);

                }
            }
        }

        private void loadLength()
        {
            txtLength.Items.Clear();
            string sql = "select Description from ccmLength where 1 = 1 Order by ID ";

            string err = string.Empty;

            DataSet ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string t = dr["Description"].ToString();
                    txtLength.Items.Add(t);

                }
            }
        }

        private void LoadGrid()
        {
            DataSet ds = new DataSet();
            string sql = "select Size,Class,Len,MinWt,MaxWt,NomWt,AlmMinWt,AlmMaxWt from ccmWeightMaster where 1 = 1 ";
            string err = string.Empty;

            ds = Utility.GetData(sql, cnstr, out err);
            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                gridWt.DataSource = ds;
                gridWt.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                gridWt.DataSource = null;
            }
        }

        private void SetMode()
        {
            string tSize = string.Empty, tClass = string.Empty, tLen = string.Empty, err = string.Empty;

            tSize = txtSize.Text;
            tClass = txtClass.Text;
            tLen = txtLength.Text;

            if (string.IsNullOrEmpty(tSize) || string.IsNullOrEmpty(tClass) || string.IsNullOrEmpty(tLen))
            {
                mode = "NEW";
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }
            string sql = "Select * from ccmWeightMaster Where Size ='" + tSize + "' and Class = '" + tClass + "' and Len ='" + tLen + "'";
            DataSet ds = Utility.GetData(sql, cnstr, out err);
            Boolean hasRows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtMinWt.EditValue = dr["MinWt"];
                    txtMaxWt.EditValue = dr["MaxWt"];
                    txtNomWt.EditValue = dr["NomWt"];
                    txtAlmMinWt.EditValue = dr["AlmMinWt"];
                    txtAlmMaxWt.EditValue = dr["AlmMaxWt"];
                }

                mode = "OLD";
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                return;
            }
            else
            {
                mode = "NEW";
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }
        }

        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtSize.Text))
            {
                err = err + "Please Enter Size" + Environment.NewLine;
            }
           
            if (string.IsNullOrEmpty(txtClass.Text))
            {
                err = err + "Please Enter Class..." + Environment.NewLine;
            }

            if (string.IsNullOrEmpty(txtLength.Text))
            {
                err = err + "Please Enter Length" + Environment.NewLine;
            }

            if (txtMinWt.EditValue == null )
            {
                err = err + "Please Enter Minimum Weight" + Environment.NewLine;
            }
            
            if (txtMaxWt.EditValue == null)
            {
                err = err + "Please Enter Max Weight" + Environment.NewLine;
            }

            if (txtNomWt.EditValue == null)
            {
                err = err + "Please Enter Nominal Weight" + Environment.NewLine;
            }

            if (txtAlmMinWt.EditValue == null)
            {
                err = err + "Please Enter Alarm Min Weight" + Environment.NewLine;
            }

            if (txtAlmMaxWt.EditValue == null)
            {
                err = err + "Please Enter Alarm Max Weight" + Environment.NewLine;
            }

            double tMin = 0, tMax = 0, tNom = 0, tAlmMin = 0, tAlmMax = 0;
            double.TryParse(txtMinWt.EditValue.ToString(), out tMin);
            double.TryParse(txtMaxWt.EditValue.ToString(), out tMax);
            double.TryParse(txtNomWt.EditValue.ToString(), out tNom);
            //double.TryParse(txtAlmMinWt.EditValue.ToString(),out tAlmMin);
            //double.TryParse(txtAlmMaxWt.EditValue.ToString(),out tAlmMax);
 
            if (tMin == 0 || tMax == 0 || tMax == 0)
            {
                err = err + "Please Enter Valid Min/Max/Nom Weight" + Environment.NewLine;
                return err;
            }

            if (tMin > tMax )
            {
                err = err + "Minimum Weight must be less than Max Weight" + Environment.NewLine;
            }

            if (tMin > tNom)
            {
                err = err + "Minimum Weight must be less than Nominal Weight" + Environment.NewLine;
            }

            if (tMax < tMin)
            {
                err = err + "Maximum Weight must be grator than Minimum Weight" + Environment.NewLine;
            }

            if (tMax < tNom)
            {
                err = err + "Maximum Weight must be grator than Nominal Weight" + Environment.NewLine;
            }


            if (tNom < tMin)
            {
                err = err + "Nominal Weight must be grator than Minimum Weight" + Environment.NewLine;
            }

            if (tNom > tMax)
            {
                err = err + "Nominal Weight must be less than Maximum Weight" + Environment.NewLine;
            }

            //if(tAlmMin > tMin)
            //{
            //    err = err + "Alarm Min Weight must be less than Minimum Weight" + Environment.NewLine;
            //}

            //if (tAlmMax < tMax)
            //{
            //    err = err + "Alarm Max Weight must be gretor than Maximum Weight" + Environment.NewLine;
            //}

            return err;
        }

        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            
            txtMinWt.EditValue = 0;
            txtMaxWt.EditValue = 0;
            txtNomWt.EditValue = 0;

            txtAlmMaxWt.EditValue = 0;
            txtAlmMinWt.EditValue = 0;

            txtSize.SelectedIndex = -1;
            txtClass.SelectedIndex = -1;
            txtLength.SelectedIndex = -1;
            
            
            LoadGrid();
            SetMode();
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
                        string sql = "Insert into ccmWeightMaster (Size,Class,Len,MinWt,MaxWt,NomWt,AlmMinWt,AlmMaxWt,AddDt) Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',GetDate())";
                        sql = string.Format(sql, 
                            txtSize.Text.ToString(),
                            txtClass.Text.ToString(),
                            txtLength.Text.ToString(),
                            txtMinWt.EditValue.ToString(),
                            txtMaxWt.EditValue.ToString(),
                            txtNomWt.EditValue.ToString(),
                            txtAlmMinWt.EditValue.ToString(), 
                            txtAlmMaxWt.EditValue.ToString()
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
                        string sql = "Update ccmWeightMaster Set MinWt='{0}',MaxWt='{1}',NomWt='{2}',AlmMinWt = '{3}', AlmMaxWt = '{4}', UpdDt = GetDate() " +                           
                            " Where Size = '{5}' And Class = '{6}' And Len = '{7}' ";

                        //txtEmailAccount.Text.ToString().trim(),
                        sql = string.Format(sql,
                             txtMinWt.EditValue.ToString(),
                             txtMaxWt.EditValue.ToString(),
                             txtNomWt.EditValue.ToString(),
                             txtAlmMinWt.EditValue.ToString(),
                             txtAlmMaxWt.EditValue.ToString(),
                             txtSize.Text.ToString(),
                             txtClass.Text.ToString(),
                             txtLength.Text.ToString()
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

            if (string.IsNullOrEmpty(txtSize.Text.ToString()) || 
                string.IsNullOrEmpty(txtClass.Text.ToString()) || 
                string.IsNullOrEmpty(txtLength.Text.ToString()))
            {
                MessageBox.Show("Please Select Size/Class/Length..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        string sql = "Delete From ccmWeightMaster Where Size = '{0}' And Class = '{1}' And Len = '{2}' ";
                        sql = string.Format(sql,
                                txtSize.Text.ToString(),
                                txtClass.Text.ToString(),
                                txtLength.Text.ToString()
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

        private void gv_Wt_DoubleClick(object sender, EventArgs e)
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
                
                int idx1 = txtSize.FindString(gv_Wt.GetRowCellValue(info.RowHandle, "Size").ToString());
                if(idx1 >= 0){
                    txtSize.SelectedIndex = idx1;
                }

                int idx2 = txtClass.FindString(gv_Wt.GetRowCellValue(info.RowHandle, "Class").ToString());
                if (idx2 >= 0)
                {
                    txtClass.SelectedIndex = idx2;
                }

                int idx3 = txtLength.FindString(gv_Wt.GetRowCellValue(info.RowHandle, "Len").ToString());
                if (idx3 >= 0)
                {
                    txtLength.SelectedIndex = idx3;
                }
                
                txtMinWt.EditValue = gv_Wt.GetRowCellValue(info.RowHandle, "MinWt").ToString();
                txtMaxWt.EditValue = gv_Wt.GetRowCellValue(info.RowHandle, "MaxWt").ToString();
                txtNomWt.EditValue = gv_Wt.GetRowCellValue(info.RowHandle, "NomWt").ToString();
                txtAlmMinWt.EditValue = gv_Wt.GetRowCellValue(info.RowHandle, "AlmMinWt").ToString();
                txtAlmMaxWt.EditValue = gv_Wt.GetRowCellValue(info.RowHandle, "AlmMaxWt").ToString();
                SetMode();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gv_Wt.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gv_Wt.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gv_Wt.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gv_Wt.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gv_Wt.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            gv_Wt.ExportToMht(exportFilePath);
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

        private void txtSize_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetMode();
        }
        
        private void txtClass_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetMode();
        }

        private void txtLength_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetMode();
        }

       

    }
}
