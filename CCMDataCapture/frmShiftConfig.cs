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
    public partial class frmShiftConfig : Form
    {
        public string mode = "NEW";
        private string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();
        public string oldCode = "";
        private static string cnstr;

        public frmShiftConfig()
        {
            InitializeComponent();
        }

        private void frmShiftConfig_Load(object sender, EventArgs e)
        {
            string sqlconfig = "sql_connection.txt";
            string fullpath = Path.Combine(strpath, sqlconfig);
            cnstr = File.ReadLines(fullpath).First();

            ResetCtrl();
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

        private string DataValidate()
        {
            string err = string.Empty;

            if (string.IsNullOrEmpty(txtShift.Text))
            {
                err = err + "Please Enter Shift Code" + Environment.NewLine;
            }
            else
            {
                if (isContainSpace(txtShift.Text.Trim().ToString()))
                    err = err + "Invalid Shift Code, contains space" + Environment.NewLine;
            }

            if (txtStartTime.EditValue == null)
            {
                err = err + "Please Enter Shift Start Time..." + Environment.NewLine;
                return err;
            }
            
            if (string.IsNullOrEmpty(txtEndTime.Text))
            {
                err = err + "Please Enter Shift End Time" + Environment.NewLine;
                return err;
            }
            
            if (isNight.Checked)
            {
                DateTime st = Convert.ToDateTime(txtStartTime.EditValue);
                DateTime et = Convert.ToDateTime(txtStartTime.EditValue);

                if (st > et)
                {

                }
                
            }
            else
            {

            }
            return err;
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
                        string sql = "Insert into ShiftConfig (ShiftCode,StartTime,EndTime,IsNight,AddDt) Values ('{0}','{1}','{2}','{3}',GetDate())";
                        sql = string.Format(sql, txtShift.Text.Trim().ToString().ToUpper(),
                             Convert.ToDateTime(txtStartTime.EditValue).ToString("HH:mm:ss"),
                              Convert.ToDateTime(txtEndTime.EditValue).ToString("HH:mm:ss"), 
                             (isNight.Checked ? "1" : "0")                            
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
                        string sql = "Update ShiftConfig Set StarTime='{0}',EndTime='{1}',IsNight='{2}', UpdDt = GetDate(), " +                           
                            " Where ShiftCode = '{3}' ";

                        //txtEmailAccount.Text.ToString().trim(),
                        sql = string.Format(sql,
                             txtShift.Text.Trim().ToString(),
                             Convert.ToDateTime(txtStartTime.EditValue).ToString("HH:mm:ss"),
                              Convert.ToDateTime(txtEndTime.EditValue).ToString("HH:mm:ss"),                             
                             (isNight.Checked ? "1" : "0")                            
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

            if (string.IsNullOrEmpty(txtShift.Text.Trim().ToString()))
            {
                MessageBox.Show("Please Enter ShiftCode", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("for application security this function is disabled, please contact system admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //using (SqlConnection cn = new SqlConnection(cnstr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        try
            //        {
            //            cn.Open();
            //            cmd.Connection = cn;
            //            string sql = "Delete From ShiftConfig Where EmailAccount = '{0}'";
            //            sql = string.Format(sql, txtShift.Text.Trim().ToString()

            //                );

            //            cmd.CommandText = sql;
            //            cmd.ExecuteNonQuery();
            //            MessageBox.Show("Record Deleted...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            ResetCtrl();

            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetCtrl();

        }

        private void ResetCtrl()
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;


            object s = new object();
            EventArgs e = new EventArgs();
            txtShift.Text = "";
            txtStartTime.EditValue = null;
            txtEndTime.EditValue = null;
            isNight.Checked = false;
            txtShift_Validated(s, e);

            oldCode = "";
            mode = "NEW";
            LoadGrid();
        }

        private void LoadGrid()
        {


            DataSet ds = new DataSet();
            string sql = "select ShiftCode,StarTime,EndTime,IsNight from ShiftConfig where 1 = 1 ";

            string err = string.Empty;

            ds = Utility.GetData(sql, cnstr, out err);

            Boolean hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);


            if (hasRows)
            {
                gridShift.DataSource = ds;
                gridShift.DataMember = ds.Tables[0].TableName;
            }
            else
            {
                gridShift.DataSource = null;
            }
        }

        private void txtShift_Validated(object sender, EventArgs e)
        {
            if (txtShift.Text.Trim() == "")
            {

                return;
            }


            DataSet ds = new DataSet();
            string sql = "select * From ShiftConfig where ShiftCode ='" + txtShift.Text.Trim() + "' ";

            string err;
            ds = Utility.GetData(sql, cnstr, out err);
            bool hasRows = ds.Tables.Cast<DataTable>()
                           .Any(table => table.Rows.Count != 0);

            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    txtShift.Text = dr["ShiftCode"].ToString();

                    txtStartTime.EditValue = dr["StartTime"].ToString();
                    txtEndTime.EditValue = dr["EndTime"].ToString();
                  
                    isNight.Checked = Convert.ToBoolean(dr["isNight"]); ;


                    mode = "OLD";
                    oldCode = dr["ShiftCode"].ToString();
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

        private void gv_Shift_DoubleClick(object sender, EventArgs e)
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
                txtShift.Text = gv_Shift.GetRowCellValue(info.RowHandle, "ShiftCode").ToString();


                object o = new object();
                EventArgs e = new EventArgs();
                mode = "OLD";
                oldCode = txtShift.Text.Trim();
                txtShift_Validated(o, e);

            }


        }
    }
}
