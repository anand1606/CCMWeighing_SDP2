using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Threading.Tasks;
using System.Management;
using System.Collections;
using System.Windows.Forms.Integration;
using uct_Weight_wpf;
using System.Collections.ObjectModel;



namespace CCMDataCapture
{



    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {

        private frmPass _PasswordForm = new frmPass();
        Sysinfo sys = new Sysinfo();
       
        private string strpath = AppDomain.CurrentDomain.BaseDirectory.ToString();
        
        public List<string> strlstdia = new List<string>();
        public List<string> strlstClass = new List<string>();
        public List<string> strlstLength = new List<string>();
        public List<string> strlstMaterial = new List<string>();
        public List<string> strlstStandard = new List<string>();

        private string ErrTable = "ccmErrLog";
        private DataSet dsSize;
        private DataSet dsClass;
        private DataSet dsLen;

        private DataSet dsSummaryRpt;
        private DataSet dsMaterial;
        private DataSet dsStandard;

        private string RBMQServer;
        private string SQLConStr;
#pragma warning disable CS0414 // The field 'frmMain.key' is assigned but its value is never used
        static string key = "b14ca5898a4e4133bbce2ea2315a1916";
#pragma warning restore CS0414 // The field 'frmMain.key' is assigned but its value is never used
        // create a timer which will fire the poller
        static string install_lic = string.Empty;
       

        public frmMain()
        {
            InitializeComponent();
           
            timer1.Enabled = false;

        }

        

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            e.Cancel = (e.CloseReason == CloseReason.UserClosing);

            if (e.CloseReason == CloseReason.TaskManagerClosing || e.CloseReason == CloseReason.WindowsShutDown)
            {
                timer1.Enabled = false;

                this.weightCanwas1.model.Disconnect();
                this.weightCanwas2.model.Disconnect();
                this.weightCanwas3.model.Disconnect();
                this.weightCanwas4.model.Disconnect();
                this.weightCanwas5.model.Disconnect();
                this.weightCanwas6.model.Disconnect();

                this.mainCanwas1.model.Disconnect();
                this.mainCanwas2.model.Disconnect();
                this.mainCanwas3.model.Disconnect();
                this.mainCanwas4.model.Disconnect();
                this.mainCanwas5.model.Disconnect();
                this.mainCanwas6.model.Disconnect();

                
                this.elm_wt_1.Dispose();
                this.elm_wt_2.Dispose();
                this.elm_wt_3.Dispose();
                this.elm_wt_4.Dispose();
                this.elm_wt_5.Dispose();
                this.elm_wt_6.Dispose();
                this.elm_main_1.Dispose();
                this.elm_main_2.Dispose();
                this.elm_main_3.Dispose();
                this.elm_main_4.Dispose();
                this.elm_main_5.Dispose();
                this.elm_main_6.Dispose();

                this.Cursor = Cursors.Default;
                this.Close();
                Environment.Exit(Environment.ExitCode);

            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.CloseBox = false;

           

            txtFromDt.DateTime = DateTime.Now;
            txtToDt.DateTime = DateTime.Now;
            txtDate.DateTime = DateTime.Now;
           

            //read the server.config for host server brodcast and port            
            string serverconfig = "server_config.txt";
            string cnstr = string.Empty;
            string fullpath = Path.Combine(strpath, serverconfig);
            cnstr = File.ReadLines(fullpath).First();
            RBMQServer = cnstr;
            
            //string t1_tbname = "CCM1";
            //string t2_tbname = "CCM2";
            //string t3_tbname = "CCM3";
            //string t4_tbname = "CCM4";
            //string t5_tbname = "CCM5";

           
            
            //read the sql_connection for datalogging 
            string sqlconfig = "sql_connection.txt";
            fullpath = Path.Combine(strpath, sqlconfig);
            SQLConStr = File.ReadLines(fullpath).First();

            Utility.SQLCnStr = SQLConStr;

            string err = string.Empty;
            sys = new Sysinfo();



            string connectionerror;
            ReloadMasterData(out connectionerror);

            //added 01/05/22 sql server connection error 
            if (!string.IsNullOrEmpty(connectionerror))
            {
                MessageBox.Show(connectionerror,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnClose_Click(sender, e,"FORCE");
            }

            cmbWeighLog.Visible = false;
            cmbWeighLog.Items.Clear();
            cmb_Report_Machines.Items.Clear();
            cmb_10.Items.Clear();
            DataSet dsMachine = Utility.GetData("Select * from ccmMachineConfig Where 1 = 1 Order by MachineName", SQLConStr, out err);
            bool hasrows = dsMachine.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in dsMachine.Tables[0].Rows)
                {
                    cmbWeighLog.Items.Add(dr["MachineName"].ToString() + ","+ dr["MachineIP"].ToString());
                    cmb_10.Items.Add(dr["MachineName"].ToString());
                    cmb_Report_Machines.Items.Add(dr["MachineName"].ToString());
                }
            }


            //this is used for intercommuntion - between front end and wpf dell 
            //used for reload - size,class,length,material,standard - etc
            //exchange = "COMMUNICATION"

           

            Utility.Start_RBMQ_Client(RBMQServer);

            
            bool t1,t2,t3,t4,t5,t6;


            xtraTabCCM1.Text = "CCM-U";
            t1 = weightCanwas1.model.Connect(RBMQServer, SQLConStr, strlstLength, strlstClass, strlstdia,strlstMaterial,strlstStandard, "U", "", "",true);
            Thread.Sleep(100);

            xtraTabCCM2.Text = "CCM-V";
            t2 = weightCanwas2.model.Connect(RBMQServer, SQLConStr, strlstLength, strlstClass, strlstdia, strlstMaterial, strlstStandard, "V", "", "", true);
            Thread.Sleep(100);

            xtraTabCCM3.Text = "CCM-W";
            t3 = weightCanwas3.model.Connect(RBMQServer, SQLConStr, strlstLength, strlstClass, strlstdia, strlstMaterial, strlstStandard, "W", "", "", true);
            Thread.Sleep(100);

            xtraTabCCM4.Text = "CCM-X";
            t4 = weightCanwas4.model.Connect(RBMQServer, SQLConStr, strlstLength, strlstClass, strlstdia, strlstMaterial, strlstStandard, "X", "", "", true);
            Thread.Sleep(100);

            xtraTabCCM5.Text = "CCM-Y";
            t5 = weightCanwas5.model.Connect(RBMQServer, SQLConStr, strlstLength, strlstClass, strlstdia, strlstMaterial, strlstStandard, "Y", "", "", true);
            Thread.Sleep(100);

            xtraTabCCM6.Text = "CCM-Z";
            t6 = weightCanwas6.model.Connect(RBMQServer, SQLConStr, strlstLength, strlstClass, strlstdia, strlstMaterial, strlstStandard, "Z", "", "", true);
            Thread.Sleep(100);


            t1 = mainCanwas1.model.Connect(RBMQServer, SQLConStr, "U", "", "", false);
            Thread.Sleep(100);

            t2 = mainCanwas2.model.Connect(RBMQServer, SQLConStr, "V", "", "", false);
            Thread.Sleep(100);

            t3 = mainCanwas3.model.Connect(RBMQServer, SQLConStr, "W", "", "", false);
            Thread.Sleep(100);

            t4 = mainCanwas4.model.Connect(RBMQServer, SQLConStr, "X", "", "", false);
            Thread.Sleep(100);

            t5 = mainCanwas5.model.Connect(RBMQServer, SQLConStr, "Y", "", "", false);
            Thread.Sleep(100);

            t6 = mainCanwas6.model.Connect(RBMQServer, SQLConStr, "Z", "", "", false);
            Thread.Sleep(100);



            //bool t5 = weightCanwas5.model.Connect(RBMQServer, SQLConStr, strlstLength, strlstClass, strlstdia, "TT", "192.168.11.16:1702", t5_tbname, true);


            cmb_10.SelectedIndex = 0;
            cmb_Report_Machines.SelectedIndex = 0;

            //button1_Click(sender,e);
            timer1.Enabled = true;
            timer1.Start();
            lblRecCount.Text = "0";

            //ResetClassEntry();
            //ResetSizeEntry();
            //ResetLengthEntry();
            //ResetDefectEntry();
            
            cmb_Options.SelectedIndex = 0;

        }

        private void cmb_Options_Validated(object sender, EventArgs e)
        {
            set_report_option();

        }
        
        private void set_report_option()
        {
            lblRecCount.Text = "0";

            if (cmb_Options.Text.ToString() == "Timing")
            {
                txtFromDt.Visible = true;
                txtToDt.Visible = true;
                
                lbl_fromdt.Visible = true;
                lbl_todt.Visible = true;

                txtDate.Visible = false;
                lbl_date.Visible = false;
            }
            else
            {
                txtFromDt.Visible = false;
                txtToDt.Visible = false;
                
                lbl_fromdt.Visible = false;
                lbl_todt.Visible = false;
                
                lbl_date.Visible = true;
                txtDate.Visible = true;
            }
        }

        private void cmb_Options_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_report_option();

        }

        private void cmb_10_SelectedIndexChanged(object sender, EventArgs e)
        {
            //last 10 pipes

            string tb = GetTableName(cmb_10.Text.Trim().ToString());
            loadGrid_Last10(tb);
               
            
        }

        private string GetTableName(string tmachine)
        {
            string tablename = string.Empty;

            if (!string.IsNullOrEmpty(tmachine))
            {

                string err = string.Empty;
                tablename = Utility.GetDescription("Select TableName from ccmMachineConfig Where MachineName = '" + tmachine + "'", SQLConStr, out err);
                

                
            }
            return tablename;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            //var sm = GetSystemMenu(Handle, false);
            //EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);
        }

        private void btnClose_Click(object sender, EventArgs e,string extraflg="")
        {
            _PasswordForm.Password = "";
            
            //default used with extraflg - if not blank - usefull when force exit require..
            if (extraflg == "FORCE")
            {
                exitcode();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _PasswordForm.Password = "";

            DialogResult dr = _PasswordForm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {

                //get user/password values from dialog
                //ask password for closing the same
                if (_PasswordForm.Password.Trim().ToUpper() == "CLOSE")
                {
                    exitcode();
                }
            }
            

        }

        private void exitcode()
        {
            timer1.Enabled = false;
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);

            this.Cursor = Cursors.WaitCursor;


            this.weightCanwas1.model.Disconnect();
            this.weightCanwas2.model.Disconnect();
            this.weightCanwas3.model.Disconnect();
            this.weightCanwas4.model.Disconnect();
            this.weightCanwas5.model.Disconnect();
            this.weightCanwas6.model.Disconnect();

            this.mainCanwas1.model.Disconnect();
            this.mainCanwas2.model.Disconnect();
            this.mainCanwas3.model.Disconnect();
            this.mainCanwas4.model.Disconnect();
            this.mainCanwas5.model.Disconnect();
            this.mainCanwas6.model.Disconnect();

            this.elm_wt_1.Dispose();
            this.elm_wt_2.Dispose();
            this.elm_wt_3.Dispose();
            this.elm_wt_4.Dispose();
            this.elm_wt_5.Dispose();
            this.elm_wt_6.Dispose();
            this.elm_main_1.Dispose();
            this.elm_main_2.Dispose();
            this.elm_main_3.Dispose();
            this.elm_main_4.Dispose();
            this.elm_main_5.Dispose();
            this.elm_main_6.Dispose();

            Utility.STOP_RBMQ_Client();


            this.Cursor = Cursors.Default;
            this.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void loadGrid_Last10(string tablename)
        {

            if (!string.IsNullOrEmpty(tablename))
            {
                string sql =
                    "Select top 15 Convert(varchar(10),C.tDate,121) as [Date],C.tShift as [Shift],SrNo,IntSrNo as SizeSrNo, Convert(varchar(8),Convert(time(5),LogDateTime)) as [Time]," +
                    " MachineNo,PipeDia,PipeClass,PipeLength,PipeNumber,MinWt,MaxWt,ActWt,NomWt, " +
                    " (case when (ActWt <= NomWt) then (ActWt-NomWt) else (NomWt-ActWt) end) as DevKG " +
                    ",(case when (NomWt > 0) then Round(((ActWt-NomWt)/NomWt*100),3) else 100 end) as DevPer, C.PipeStatus " +
                    " From [" + tablename + "] C left outer join ccmShiftWiseInfo info on c.tDate = info.tDate and c.tShift = info.tShift" +
                    " where C.tdate = Convert(date,Getdate())  " +
                    " Order By C.LogDateTime Desc ";

                string err;
                DataSet ds = Utility.GetData(sql, SQLConStr, out err);

                bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrows)
                {
                    grd_last10.DataSource = ds;
                    grd_last10.DataMember = ds.Tables[0].TableName;
                }
                else
                {
                    grd_last10.DataSource = null;
                }


                //Alarmed Pipes
                string MachineNo = cmb_10.Text.Trim().ToString();
                sql = "Select top 50 Convert(varchar(10),tDate,121) as [Date], tShift as [Shift],SrNo,PipeNumber,PipeDia,PipeClass,PipeWt,PipeStatus,OperatorCode,OperatorName " +
                    " From ccmAlarm Where tDate = Convert(date,getdate()) and MachineNo ='" + MachineNo + "' Order by AddDt Desc";

                ds = Utility.GetData(sql, SQLConStr, out err);

                hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
                if (hasrows)
                {
                    gridOverWt.DataSource = ds;
                    gridOverWt.DataMember = ds.Tables[0].TableName;
                }
                else
                {
                    gridOverWt.DataSource = null;
                }

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string tb = GetTableName(cmb_10.Text.Trim().ToString());
            loadGrid_Last10(tb);
            //validate_lic();
            
            setCurShiftInchargeGroup();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(cmb_Report_Machines.Text.ToString()))
            {
                MessageBox.Show("Please Select Machine...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(cmb_Options.Text.ToString()))
            {
                MessageBox.Show("Please Select Report Options...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tMachine = cmb_Report_Machines.Text.ToString().Trim();
            string tOption = cmb_Options.Text.ToString().Trim();
            string tTableName = GetTableName(tMachine);
            string sql =
                "Select Convert(varchar(10),C.tDate,121) as [Date],C.tShift as [Shift],SrNo, IntSrNo as [SizeSrNo], Convert(varchar(8),Convert(time(5),LogDateTime)) as [Time]," +
                " MachineNo,PipeDia,PipeClass,PipeLength,PipeNumber,JointType,MouldNo,MinWt,MaxWt,ActWt,NomWt, " +
                " (case when (ActWt <= NomWt) then (ActWt-NomWt) else (NomWt-ActWt) end) as DevKG , " +
                " ABS((Case When (NomWt <= 0 ) then 0 else Round(((NomWt-ActWt)/NomWt*100),3) end)) as DevPer,Remarks,info.InchargeName, PipeStatus,Material,Standard, OperatorCode,OperatorName  " +
                " From [" + tTableName + "] C left outer join ccmShiftWiseInfo info on c.tDate = info.tDate and c.tShift = info.tShift ";
            
                    

            if (!tOption.Contains("Timing") && !string.IsNullOrEmpty(tOption))
            {
                if (txtDate.EditValue == DBNull.Value)
                {
                    MessageBox.Show("Please Select Date...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (tOption == "Shift-A")
            {
                sql += " Where C.tDate ='" + txtDate.DateTime.ToString("yyyy-MM-dd") + "' and C.tShift = 'A' Order By LogDateTime";
            }
            else if (tOption == "Shift-B")
            {
                sql += " Where C.tDate ='" + txtDate.DateTime.ToString("yyyy-MM-dd") + "' and C.tShift = 'B' Order By LogDateTime";
            }
            else if (tOption == "Shift-C")
            {
                sql += " Where C.tDate ='" + txtDate.DateTime.ToString("yyyy-MM-dd") + "' and C.tShift = 'C' Order By LogDateTime";
            }
            else if (tOption == "ALL") 
            {
                sql += " Where C.tDate ='" + txtDate.DateTime.ToString("yyyy-MM-dd") + "' Order by LogDateTime ";
            }
            else if (tOption == "Timing")
            {
                if (txtFromDt.EditValue == DBNull.Value)
                {
                    MessageBox.Show("Please Select From DateTime...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtToDt.EditValue == DBNull.Value)
                {
                    MessageBox.Show("Please Select To DateTime...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                sql += " Where C.LogDateTime between '" + txtFromDt.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + txtToDt.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' Order by LogDateTime ";

            }
            
            lblRecCount.Text = "0";
            string err;
            DataSet ds = Utility.GetData(sql, SQLConStr,out err);
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                grd_report.DataSource = ds;
                grd_report.DataMember = ds.Tables[0].TableName;
                lblRecCount.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                grd_report.DataSource = null;
                lblRecCount.Text = "0";
            }

        }

        private void setCurShiftInchargeGroup()
        {
            DateTime cur = DateTime.Now;
            if (cur.Hour >= 0 && cur.Hour <= 5 && cur.Minute <= 59 && cur.Second <= 59)
            {
                txtCurDate.DateTime = cur.AddDays(-1).Date;
                txtShift.Text = "C";
            }
            if (cur.Hour >= 6 && cur.Hour <= 13 && cur.Minute <= 59 && cur.Second <= 59)
            {
                txtCurDate.DateTime = cur.Date;
                txtShift.Text = "A";
            }
            if (cur.Hour >= 14 && cur.Hour <= 21 && cur.Minute <= 59 && cur.Second <= 59)
            {
                txtCurDate.DateTime = cur.Date;
                txtShift.Text = "B";
            }
            if (cur.Hour >= 22 && cur.Hour <= 23 && cur.Minute <= 59 && cur.Second <= 59)
            {
                txtCurDate.DateTime = cur.Date;
                txtShift.Text = "C";
            }

            string err = string.Empty;
            string sql = "select InchargeName from ccmShiftWiseInfo where tDate='" + txtCurDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim().ToString() + "'";
            
            string tempincharge =  Utility.GetDescription(sql, SQLConStr, out err);
            
            if(string.IsNullOrEmpty(txtIncharge.Text.Trim().ToString()) && !string.IsNullOrEmpty(tempincharge))
                txtIncharge.Text = tempincharge;


        }


        //private void validate_lic()
        //{
        //    string err;
        //    bool x = Utility.RetriveLic(SQLConStr, key, out sys, out err);
        //    //txtLicID.Text = sys.Hostkey;
        //    //lblInstallDt.Text = sys.InstallDt.ToString("yyyy-MM-dd");
        //    //lblLicType.Text = sys.LicType + "-" + sys.Limitdays.ToString() + "Days";

        //    if (x && sys.LicType != "TRIAL")
        //    {
        //        x = Utility.MatchLic(sys, SQLConStr, key);
        //        if (!x)
        //        {
        //            licenseExpExit();
                    
        //        }
        //        else
        //        {
        //            lblLicType.Text = sys.LicType + "-FULL";
        //        }
        //    }
        //    else
        //    {
        //        if (!Utility.MatchLic(sys, SQLConStr, key))
        //        {
        //            licenseExpExit();
                    
        //        }
        //    }
            
        //}

        #region Master_Data_Handling

        private void licenseExpExit()
        {
            timer1.Enabled = false;
            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);

            this.Cursor = Cursors.WaitCursor;


            this.weightCanwas1.model.Disconnect();
            this.weightCanwas2.model.Disconnect();
            this.weightCanwas3.model.Disconnect();
            //this.weightCanwas4.model.Disconnect();
            //this.weightCanwas5.model.Disconnect();

           
            this.Cursor = Cursors.Default;
            this.elm_wt_1.Dispose();
            this.elm_wt_2.Dispose();
            this.elm_wt_3.Dispose();
            //this.elementHost4.Dispose();
            //this.elementHost5.Dispose();

            this.Close();
            Environment.Exit(Environment.ExitCode);
        }

        //private void ResetSizeEntry()
        //{
        //    txtID_Size.Text = "0";
        //    txtDesc_Size.Text = "";
        //}

        //private void ResetLengthEntry()
        //{
        //    txtID_Len.Text = "0";
        //    txtDesc_Len.Text = "";
        //}

        //private void ResetClassEntry()
        //{
        //    txtID_Class.Text = "0";
        //    txtDesc_Class.Text = "";
        //}

        //private void ResetDefectEntry()
        //{
        //    txtID_Defect.Text = "0";
        //    txtDesc_Defect.Text = "";
        //}
        
       

        //private void btnAdd_Class_Click(object sender, EventArgs e)
        //{
        //    bool t = AddMaster("ccmClass", txtID_Class.Text.ToString(), txtDesc_Class.Text.ToString());
        //    if (t)
        //    {
        //        ResetClassEntry();
        //        ReloadMasterData();
        //    }
        //    else
        //    {
        //        ResetClassEntry();
        //    }
        //}
        
        //private void btnAdd_Size_Click(object sender, EventArgs e)
        //{
        //    bool t = AddMaster("ccmSize", txtID_Size.Text.ToString(), txtDesc_Size.Text.ToString());
        //    if (t)
        //    {
        //        ResetSizeEntry();
        //        ReloadMasterData();
        //    }
        //    else
        //    {
        //        ResetSizeEntry();
        //    }
        //}

        //private void btnAdd_Len_Click(object sender, EventArgs e)
        //{
        //    bool t = AddMaster("ccmLength", txtID_Len.Text.ToString(), txtDesc_Len.Text.ToString());
        //    if (t)
        //    {
        //        ResetLengthEntry();
        //        ReloadMasterData();
        //    }
        //    else
        //    {
        //        ResetLengthEntry();
        //    }
        //}

        //private void btnAdd_Defect_Click(object sender, EventArgs e)
        //{
        //    bool t = AddMaster("ccmDefect", txtID_Defect.Text.ToString(), txtDesc_Defect.Text.ToString());
        //    if (t)
        //    {
        //        ResetDefectEntry();

        //        string err = string.Empty;
        //        dsDefect = Utility.GetData("Select * from ccmDefect Order by Description", SQLConStr, out err);
        //        if (!string.IsNullOrEmpty(err))
        //        {
        //            MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        bool hasrows = dsDefect.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
        //        if (hasrows)
        //        {
        //            gridDefect.DataSource = dsDefect;
        //            gridDefect.DataMember = dsDefect.Tables[0].TableName;
        //        }
        //    }
        //    else
        //    {
        //        ResetDefectEntry();
        //    }
        //}

        //private void btnDelete_Class_Click(object sender, EventArgs e)
        //{
        //    bool t = DeleteMaster("ccmClass", txtID_Class.Text.ToString(), txtDesc_Class.Text.ToString());
        //    if (t)
        //    {
        //        ResetClassEntry();
        //        ReloadMasterData();
        //    }
        //}

        //private void btnDelete_Size_Click(object sender, EventArgs e)
        //{
        //    bool t = DeleteMaster("ccmSize", txtID_Size.Text.ToString(), txtDesc_Size.Text.ToString());
        //    if (t)
        //    {
        //        ResetSizeEntry();
        //        ReloadMasterData();
        //    }
        //}

        //private void btnDelete_Len_Click(object sender, EventArgs e)
        //{
        //    bool t = DeleteMaster("ccmLength", txtID_Len.Text.ToString(), txtDesc_Len.Text.ToString());
        //    if (t)
        //    {
        //        ResetLengthEntry();
        //        ReloadMasterData();
        //    }
        //}

        //private void btnDelete_Defect_Click(object sender, EventArgs e)
        //{
        //    bool t = DeleteMaster("ccmDefect", txtID_Defect.Text.ToString(), txtDesc_Defect.Text.ToString());
        //    if (t)
        //    {
        //        ResetDefectEntry();
        //        string err = string.Empty;
        //        dsDefect = Utility.GetData("Select * from ccmDefect Order by Description", SQLConStr, out err);
        //        if (!string.IsNullOrEmpty(err))
        //        {
        //            MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        bool hasrows = dsDefect.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
        //        if (hasrows)
        //        {
        //            gridDefect.DataSource = dsDefect;
        //            gridDefect.DataMember = dsDefect.Tables[0].TableName;
        //        }
        //    }
        //}


        public void ReloadMasterData(out string err1)
        {
            err1 = string.Empty;
            strlstdia.Clear();
            strlstClass.Clear();
            strlstLength.Clear();
            strlstMaterial.Clear();
            strlstStandard.Clear();

            string err;
            dsSize = Utility.GetData("Select * from ccmSize Order by Description", SQLConStr,out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Master-Size", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err1 += err + Environment.NewLine;
                return;
            }
            dsClass = Utility.GetData("Select * from ccmClass Order by Description", SQLConStr,out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Master-Class", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err1 += err + Environment.NewLine;
            }
            dsLen = Utility.GetData("Select * from ccmLength Order by Description", SQLConStr,out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Master-Length", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err1 += err + Environment.NewLine;
            }

            //dsDefect = Utility.GetData("Select * from ccmDefect Order by Description", SQLConStr, out err);
            //if (!string.IsNullOrEmpty(err))
            //{
            //    MessageBox.Show(err, "Master-Defect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            dsMaterial = Utility.GetData("Select * from ccmMaterial Order By Description", SQLConStr, out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Master-Material", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err1 += err + Environment.NewLine;
            }

            dsStandard = Utility.GetData("Select * from ccmStandard Order By Description", SQLConStr, out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Master-Standard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err1 += err + Environment.NewLine;
            }

            //dsMachine = Utility.GetData("Select * from ccmMachineConfig Order by MachineName", SQLConStr, out err);
            //if (!string.IsNullOrEmpty(err))
            //{
            //    MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            dsSummaryRpt = Utility.GetData("Select * from ccmReportMaster Order by ReportID", SQLConStr,out err);
            if (!string.IsNullOrEmpty(err))
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                err1 += err + Environment.NewLine;
            }

            cmbSumReport.DataSource = null;
            cmbSumReport.Items.Clear();
            bool hasrows = dsSummaryRpt.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                cmbSumReport.DataSource = dsSummaryRpt.Tables[0];
                cmbSumReport.DisplayMember = "ReportName";
                cmbSumReport.ValueMember = "ReportID";
            }

           
            hasrows = dsSize.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in dsSize.Tables[0].Rows)
                {
                    strlstdia.Add(dr["Description"].ToString());
                }                
            }

            hasrows = dsClass.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in dsClass.Tables[0].Rows)
                {
                    strlstClass.Add(dr["Description"].ToString());
                }
                
            }
            
            hasrows = dsLen.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in dsLen.Tables[0].Rows)
                {
                    strlstLength.Add(dr["Description"].ToString());
                }                
            }

            hasrows = dsMaterial.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in dsMaterial.Tables[0].Rows)
                {
                    strlstMaterial.Add(dr["Description"].ToString());
                }
            }

            hasrows = dsStandard.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                foreach (DataRow dr in dsStandard.Tables[0].Rows)
                {
                    strlstStandard.Add(dr["Description"].ToString());
                }
            }


        }

        #endregion


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

        private void btnLogRefresh_Click(object sender, EventArgs e)
        {
            if (errdt1.DateTime == DateTime.MinValue || errdt2.DateTime == DateTime.MinValue)
            {
                MessageBox.Show("Please Enter From/To DateTime", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(cmbLogType.Text.Trim()))
            {
                MessageBox.Show("Please Select Log/Error Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (errdt1.DateTime > errdt2.DateTime)
            {
                MessageBox.Show("Invalid DateTime Range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sql = string.Empty;
            if (cmbLogType.Text.Trim() == "Weighing Log")
            {

                if (cmbWeighLog.Text.Trim() == "")
                {
                    sql =
                      "Select Convert(varchar(23),LogDateTime,121) as LogDateTime,MachineIP,SignalMsg,Signal,Weight,Processed,Saved from ccmSignalDetection where  LogDateTime between '" + errdt1.DateTime.ToString("yyyy-MM-dd HH:mm:ss") +
                      "' And '" + errdt2.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' Order By LogDateTime Asc ";
                }
                else
                {
                    sql =
                        "Select Convert(varchar(23),LogDateTime,121) as LogDateTime,MachineIP,SignalMsg,Signal,Weight,Processed,Saved from ccmSignalDetection where  LogDateTime between '" + errdt1.DateTime.ToString("yyyy-MM-dd HH:mm:ss") +
                        "' And '" + errdt2.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and MachineIP = '" + cmbWeighLog.Text.Trim().Split(',')[1].ToString() + "' Order By LogDateTime Asc ";
                }

                
              


            }
            else
            {
                sql =
                "Select ID, Convert(varchar(23),LogDateTime,121) as LogDateTime, LogTopic,LogDesc " +
                " From [" + ErrTable + "] where LogDateTime between '" + errdt1.DateTime.ToString("yyyy-MM-dd HH:mm:ss") +
                "' And '" + errdt2.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and LogTopic = '" + cmbLogType.Text.Trim().ToString() + "' Order By LogDateTime Asc ";
            }

            grdLog.DataSource = null;
            gvLog.Columns.Clear();
            string err;
            DataSet ds = Utility.GetData(sql, SQLConStr,out err);       

            if (ds.Tables.Count > 0)
            {
                
                grdLog.DataSource = ds;
                grdLog.DataMember = ds.Tables[0].TableName;
                lblLogCount.Text = ds.Tables[0].Rows.Count.ToString();
            }

        }

        private void btnLogExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV File|*.csv|Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx|Html File (.html)|*.html";
                
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;

                    try
                    {
                        switch (fileExtenstion)
                        {
                            case ".csv":
                                gvLog.ExportToCsv(exportFilePath);
                                break;
                            case ".xls":
                                gvLog.ExportToXls(exportFilePath);
                                break;
                            case ".xlsx":
                                gvLog.ExportToXlsx(exportFilePath);
                                break;
                            case ".rtf":
                                gvLog.ExportToRtf(exportFilePath);
                                break;
                            case ".pdf":
                                gvLog.ExportToPdf(exportFilePath);
                                break;
                            case ".html":
                                gvLog.ExportToHtml(exportFilePath);
                                break;
                            case ".mht":
                                gvLog.ExportToMht(exportFilePath);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnSumRefresh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbSumReport.Text.ToString()))
            {
                MessageBox.Show("Please Select Report Type...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            if (txtSumFromDt.EditValue == null || txtSumFromDt.DateTime == DateTime.MinValue)
            {
                MessageBox.Show("Please Select From Date...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtSumToDt.EditValue == null || txtSumToDt.DateTime == DateTime.MinValue)
            {
                MessageBox.Show("Please Select To Date...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (txtSumToDt.DateTime < txtSumFromDt.DateTime)
            {
                MessageBox.Show("Invalid Date Range...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string ReportID = cmbSumReport.SelectedValue.ToString();
            if(string.IsNullOrEmpty(ReportID))
            {
                MessageBox.Show("Invalid Report...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblRecCount.Text = "0";
            string err = string.Empty;
            string sql = Utility.GetDescription("select ReportSQL from ccmReportMaster Where ReportID ='" + ReportID + "'",SQLConStr,out err);

            if (string.IsNullOrEmpty(err))
            {
                if(string.IsNullOrEmpty(sql))
                {
                    MessageBox.Show("Unable to determine report query..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sql = sql.Replace("@Para1", "'" + txtSumFromDt.DateTime.Date.ToString("yyyy-MM-dd") + "'");
            sql = sql.Replace("@Para2", "'" + txtSumToDt.DateTime.Date.ToString("yyyy-MM-dd") + "'");

            sql = sql.Replace("@para1", "'" + txtSumFromDt.DateTime.Date.ToString("yyyy-MM-dd") + "'");
            sql = sql.Replace("@para2", "'" + txtSumToDt.DateTime.Date.ToString("yyyy-MM-dd") + "'");

            sql = sql.Replace("@PARA1", "'" + txtSumFromDt.DateTime.Date.ToString("yyyy-MM-dd") + "'");
            sql = sql.Replace("@PARA2", "'" + txtSumToDt.DateTime.Date.ToString("yyyy-MM-dd") + "'");

            gridView3.Columns.Clear();
            gridSum_Report.DataSource = null;
            gridSum_Report.Refresh();
            
            DataSet ds = Utility.GetData(sql, SQLConStr,out err);
            bool hasrows = ds.Tables.Cast<DataTable>().Any(table => table.Rows.Count != 0);
            if (hasrows)
            {
                gridSum_Report.DataSource = ds;
                gridSum_Report.DataMember = ds.Tables[0].TableName;
                lblSumCount.Text = ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                //MessageBox.Show(sql, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSumCount.Text = "0";
            }
            gridSum_Report.Refresh();

        }

        private void btnSumExport_Click(object sender, EventArgs e)
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
                            gridView3.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridView3.ExportToXlsx(exportFilePath);
                            break;
                        case ".rtf":
                            gridView3.ExportToRtf(exportFilePath);
                            break;
                        case ".pdf":
                            gridView3.ExportToPdf(exportFilePath);
                            break;
                        case ".html":
                            gridView3.ExportToHtml(exportFilePath);
                            break;
                        case ".mht":
                            gridView3.ExportToMht(exportFilePath);
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

        private void cmbLogType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbLogType.Text.Trim() == "Weighing Log")
            {
                cmbWeighLog.Visible = true;

            }
            else
            {
                cmbWeighLog.Visible = false;
            }

        }

        private void btnSaveIncharge_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtIncharge.Text.Trim().ToString()) || txtCurDate.DateTime == DateTime.MinValue || string.IsNullOrEmpty(txtShift.Text.Trim()))
            {
                MessageBox.Show("Please Enter Required Information..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string sql = "select count(*) from ccmShiftWiseInfo where tDate='" + txtCurDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim().ToString() + "'";
            string err = string.Empty;
            string rec = Utility.GetDescription(sql, SQLConStr, out err);
            
            int t = 0;

            int.TryParse(rec,out t);

            if(t == 0)
            {
                
                sql = "Insert into ccmShiftWiseInfo (tDate,tShift,InchargeName,AddDt) Values ('" + txtCurDate.DateTime.Date.ToString("yyyy-MM-dd") + "','" + txtShift.Text.Trim() + "','" + txtIncharge.Text.Trim().ToString() + "',getdate())";
            }
            else
            {
                sql = "Update ccmShiftWiseInfo Set InchargeName ='" + txtIncharge.Text.Trim().ToString() + "', UpdDt = GetDate() where tDate='" + txtCurDate.DateTime.Date.ToString("yyyy-MM-dd") + "' and tShift ='" + txtShift.Text.Trim().ToString() + "'";
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
                    return;
                }

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Information Saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            
                
        }

        private void btnDefect_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmEditor"];

            if (t == null)
            {
                frmEditor m = new frmEditor();
                m.Show();
            }
        }

        private static DialogResult ShowInputDialogBox(ref string input, string prompt, string title = "Title", int width = 300, int height = 200)
        {
            //This function creates the custom input dialog box by individually creating the different window elements and adding them to the dialog box

            //Specify the size of the window using the parameters passed
            Size size = new Size(width, height);
            //Create a new form using a System.Windows Form
            Form inputBox = new Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            //Set the window title using the parameter passed
            inputBox.Text = title;

            //Create a new label to hold the prompt
            Label label = new Label();
            label.Text = prompt;
            label.Location = new Point(5, 5);
            label.Width = size.Width - 10;
            inputBox.Controls.Add(label);
            
            //Create a textbox to accept the user's input
            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, 25);
            textBox.Location = new Point(5, label.Location.Y + 25);
            textBox.Text = input;
            textBox.PasswordChar = '*';
            inputBox.Controls.Add(textBox);

            //Create an OK Button 
            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 80 - 80, size.Height - 30);
            inputBox.Controls.Add(okButton);

            //Create a Cancel Button
            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new Point(size.Width - 80, size.Height - 30);
            inputBox.Controls.Add(cancelButton);

            //Set the input box's buttons to the created OK and Cancel Buttons respectively so the window appropriately behaves with the button clicks
            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            //Show the window dialog box 
            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;

            //After input has been submitted, return the input value
            return result;
        }



        private void btnMasterData_Click(object sender, EventArgs e)
        {

            //Initialize the input variable which will be referenced by the custom input dialog box
            string input = "";
            //Display the custom input dialog box with the following prompt, window title, and dimensions
            ShowInputDialogBox(ref input, "Password for Master data Management?", "Password", 300, 200);

            if (input != "master")
                return;

            Form t = Application.OpenForms["frmMasters"];

            if (t == null)
            {
                frmMasters m = new frmMasters();
                m.Show();
            }
        }


        private void btnWeightSetting_Click(object sender, EventArgs e)
        {
            Form t = Application.OpenForms["frmWeightMaster"];

            if (t == null)
            {
                frmWeightMaster m = new frmWeightMaster();
                m.Show();
            }
        }


        private void btnEmailSetting_Click(object sender, EventArgs e)
        {

            //Initialize the input variable which will be referenced by the custom input dialog box
            string input = "";
            //Display the custom input dialog box with the following prompt, window title, and dimensions
            ShowInputDialogBox(ref input, "Password for Master data Management?", "Password", 300, 200);

            if (input != "master")
                return;

            Form t = Application.OpenForms["frmEmailConfig"];

            if (t == null)
            {
                frmEmailConfig m = new frmEmailConfig();
                m.Show();
            }
        }


        private void btnRFIDSetting_Click(object sender, EventArgs e)
        {
            //Initialize the input variable which will be referenced by the custom input dialog box
            string input = "";
            //Display the custom input dialog box with the following prompt, window title, and dimensions
            ShowInputDialogBox(ref input, "Password for Master data Management?", "Password", 300, 200);

            if (input != "master")
                return;

            Form t = Application.OpenForms["frmRFID"];

            if (t == null)
            {
                frmRFID m = new frmRFID();
                m.Show();
            }
        }

        private void btnModbusSetting_Click(object sender, EventArgs e)
        {
            //Initialize the input variable which will be referenced by the custom input dialog box
            string input = "";
            //Display the custom input dialog box with the following prompt, window title, and dimensions
            ShowInputDialogBox(ref input, "Password for Master data Management?", "Password", 300, 200);

            if (input != "master")
                return;

            Form t = Application.OpenForms["frmModbusTcpConfig"];

            if (t == null)
            {
                frmModbusTcpConfig m = new frmModbusTcpConfig();
                m.Show();
            }
        }
    }
}