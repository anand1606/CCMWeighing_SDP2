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
using System.IO;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;

namespace CCMDataCapture
{
    public partial class frmMasters : Form
    {
        private string SQLConStr;
        private string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();
        private DataSet dsMaster = new DataSet();

        private string oldCode = "";
        private string TableName = string.Empty;
        public frmMasters()
        {
            InitializeComponent();
        }


        private bool AddMaster(string tablename, string id, string desc)
        {
            if (id == "0" && !string.IsNullOrEmpty(desc.Trim()))
            {
                using (SqlConnection cn = new SqlConnection(SQLConStr))
                {
                    try
                    {
                        cn.Open();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error-" + TableName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {

                        string err = string.Empty;

                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;

                        int newid = Convert.ToInt32(Utility.GetDescription("Select isnull(Max(ID),0) + 1  From [" + tablename + "]", SQLConStr, out err));

                        if (string.IsNullOrEmpty(err) && newid > 0)
                        {
                            cmd.CommandText = "Insert into [" + tablename + "] (ID,Description,AddDt) values ('" + newid.ToString() + "','" + desc.ToString() + "',GetDate())";
                            cmd.ExecuteNonQuery();
                            return true;
                        }
                        else
                        {
                            return false;
                        }


                    }
                }
            }
            else
            {
                if(string.IsNullOrEmpty(desc.Trim()))
                {
                    MessageBox.Show("Please Enter Description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (SqlConnection cn = new SqlConnection(SQLConStr))
                {
                    try
                    {
                        cn.Open();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cn;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "Update [" + tablename + "] Set Description ='" + desc.Trim().ToString() + "',UpdDt = GetDate() Where ID = '" + id.ToString() + "'";
                            cmd.ExecuteNonQuery();
                            return true;
                        }
                    }catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    
                }
            }


        }

        private bool DeleteMaster(string tablename, string id, string desc)
        {
            if (id != "0" && !string.IsNullOrEmpty(desc))
            {
                using (SqlConnection cn = new SqlConnection(SQLConStr))
                {
                    try
                    {
                        cn.Open();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Delete From [" + tablename + "] Where ID = '" + id.ToString() + "'";
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }

        }

        private void ResetControl()
        {
            txtID.Text = "0";
            txtDesc.Text = "";
           
            oldCode = "";
        }

        private void frmMasters_Load(object sender, EventArgs e)
        {

            SQLConStr = Utility.SQLCnStr;

            //set default selected item of Master Table
            grpMaster.EditValue = "ccmSize";
            ResetControl();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool t = AddMaster(TableName, txtID.Text.ToString(), txtDesc.Text.ToString());
            ResetControl();
            LoadGrid();
            DataTable dt = GetDataTable(gv);
            string details = TableName + "-";
            foreach (DataRow dr in dt.Rows)
            {
                details +=   dr["Description"].ToString() + ",";
            }

            string cutlen = details.Substring(0, details.Length - 1);

            Utility.Publish_RBMQ_MSG(cutlen, "MasterReload");

            //Utility.Publish_RBMQ_MSG(TableName, "MasterReload");


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool t = DeleteMaster(TableName, txtID.Text.ToString(), txtDesc.Text.ToString());
            ResetControl();
            LoadGrid();
            DataTable dt = GetDataTable(gv);
            string details = TableName + "-";
            foreach(DataRow dr in dt.Rows)
            {
                details +=  dr["Description"].ToString() + ",";
            }
            string cutlen = details.Substring(0, details.Length - 1);
            Utility.Publish_RBMQ_MSG(cutlen, "MasterReload");
        }


        DataTable GetDataTable(GridView view)
        {
            DataTable dt = new DataTable();
            foreach (GridColumn c in view.Columns)
                dt.Columns.Add(c.FieldName, c.ColumnType);
            for (int r = 0; r < view.RowCount; r++)
            {
                object[] rowValues = new object[dt.Columns.Count];
                for (int c = 0; c < dt.Columns.Count; c++)
                    rowValues[c] = view.GetRowCellValue(r, dt.Columns[c].ColumnName);
                dt.Rows.Add(rowValues);
            }
            return dt;
        }

        private void grpMaster_EditValueChanged(object sender, EventArgs e)
        {
            TableName = grpMaster.EditValue.ToString();
            ResetControl();
            LoadGrid();
        }

        private void LoadGrid()
        {
            string sql = "Select * from [" + TableName + "]  Order by Description";
            string err = string.Empty;
            dsMaster = Utility.GetData(sql, SQLConStr, out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, TableName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dsMaster = new DataSet();
                grid.DataSource = null;
                grid.Refresh();
                return;
            }
            grid.DataSource = dsMaster;
            grid.DataMember = dsMaster.Tables[0].TableName;
            grid.RefreshDataSource();

            
        }

        private void btnShiftConfig_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmShiftConfig"];

            if (t == null)
            {
                frmShiftConfig m = new frmShiftConfig();
                m.Show();
            }
        }

        private void gv_DoubleClick(object sender, EventArgs e)
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
                txtID.Text = gv.GetRowCellValue(info.RowHandle, "ID").ToString();
            }
            object o = new object();
            EventArgs e = new EventArgs();
           
            oldCode = txtID.Text.ToString();
            txtID_Validated(o, e);
        }

        private void txtID_Validated(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtID.Text.Trim()))
            {
               
                ResetControl();
                return;
            }

            string sql = "Select Description from [" + TableName + "]  Where ID = '" + txtID.Text.Trim() + "'";
            string err;
            txtDesc.Text = Utility.GetDescription(sql,SQLConStr, out err);
            
            oldCode = txtID.Text.Trim();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetControl();
        }

    }//end mainformclass

}//end namespace
