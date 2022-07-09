namespace CCMDataCapture
{
    partial class frmRFID
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabMachine = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutRFIDMaster = new System.Windows.Forms.TableLayoutPanel();
            this.gridRFID = new DevExpress.XtraGrid.GridControl();
            this.gvRFID = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbMachineName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRfCancel = new System.Windows.Forms.Button();
            this.btnRfDelete = new System.Windows.Forms.Button();
            this.btnRfAdd = new System.Windows.Forms.Button();
            this.txtIP = new DevExpress.XtraEditors.TextEdit();
            this.xtraTabOperator = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutOperator = new System.Windows.Forms.TableLayoutPanel();
            this.gridOperator = new DevExpress.XtraGrid.GridControl();
            this.gvOperator = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEmpName = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOpCancel = new System.Windows.Forms.Button();
            this.btnOpDelete = new System.Windows.Forms.Button();
            this.btnOpAdd = new System.Windows.Forms.Button();
            this.txtEmpCode = new DevExpress.XtraEditors.TextEdit();
            this.xtraTabOperation = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gridRegister = new DevExpress.XtraGrid.GridControl();
            this.gvRegister = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRegDownload = new System.Windows.Forms.Button();
            this.btnRegTime = new System.Windows.Forms.Button();
            this.chkListRFID = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbEmp = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRegDel = new System.Windows.Forms.Button();
            this.btnRegAdd = new System.Windows.Forms.Button();
            this.xtraTabPunching = new DevExpress.XtraTab.XtraTabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gridPunching = new DevExpress.XtraGrid.GridControl();
            this.gvPunching = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPunchQry = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtdate = new DevExpress.XtraEditors.DateEdit();
            this.btnRegALL = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabMachine.SuspendLayout();
            this.tableLayoutRFIDMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRFID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRFID)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).BeginInit();
            this.xtraTabOperator.SuspendLayout();
            this.tableLayoutOperator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOperator)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpCode.Properties)).BeginInit();
            this.xtraTabOperation.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRegister)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRegister)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkListRFID)).BeginInit();
            this.xtraTabPunching.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPunching)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPunching)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtdate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtdate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabMachine;
            this.xtraTabControl1.Size = new System.Drawing.Size(876, 450);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabMachine,
            this.xtraTabOperator,
            this.xtraTabOperation,
            this.xtraTabPunching});
            // 
            // xtraTabMachine
            // 
            this.xtraTabMachine.Controls.Add(this.tableLayoutRFIDMaster);
            this.xtraTabMachine.Name = "xtraTabMachine";
            this.xtraTabMachine.Size = new System.Drawing.Size(874, 425);
            this.xtraTabMachine.Text = "RFID Machine Master";
            // 
            // tableLayoutRFIDMaster
            // 
            this.tableLayoutRFIDMaster.ColumnCount = 1;
            this.tableLayoutRFIDMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutRFIDMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutRFIDMaster.Controls.Add(this.gridRFID, 0, 1);
            this.tableLayoutRFIDMaster.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutRFIDMaster.Location = new System.Drawing.Point(11, 3);
            this.tableLayoutRFIDMaster.Name = "tableLayoutRFIDMaster";
            this.tableLayoutRFIDMaster.RowCount = 2;
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.36643F));
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.63357F));
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutRFIDMaster.Size = new System.Drawing.Size(860, 423);
            this.tableLayoutRFIDMaster.TabIndex = 0;
            // 
            // gridRFID
            // 
            this.gridRFID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRFID.Location = new System.Drawing.Point(3, 68);
            this.gridRFID.MainView = this.gvRFID;
            this.gridRFID.Name = "gridRFID";
            this.gridRFID.Size = new System.Drawing.Size(854, 352);
            this.gridRFID.TabIndex = 8;
            this.gridRFID.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRFID});
            // 
            // gvRFID
            // 
            this.gvRFID.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gvRFID.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvRFID.GridControl = this.gridRFID;
            this.gvRFID.Name = "gvRFID";
            this.gvRFID.OptionsBehavior.Editable = false;
            this.gvRFID.OptionsCustomization.AllowColumnMoving = false;
            this.gvRFID.OptionsCustomization.AllowFilter = false;
            this.gvRFID.OptionsCustomization.AllowGroup = false;
            this.gvRFID.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvRFID.OptionsCustomization.AllowSort = false;
            this.gvRFID.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvRFID.OptionsFilter.AllowFilterEditor = false;
            this.gvRFID.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gvRFID.OptionsFilter.AllowMRUFilterList = false;
            this.gvRFID.OptionsFind.AllowFindPanel = false;
            this.gvRFID.OptionsMenu.EnableColumnMenu = false;
            this.gvRFID.OptionsMenu.EnableFooterMenu = false;
            this.gvRFID.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvRFID.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gvRFID.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvRFID.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvRFID.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvRFID.OptionsMenu.ShowSplitItem = false;
            this.gvRFID.OptionsView.ColumnAutoWidth = false;
            this.gvRFID.OptionsView.ShowDetailButtons = false;
            this.gvRFID.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvRFID.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbMachineName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnRfCancel);
            this.groupBox1.Controls.Add(this.btnRfDelete);
            this.groupBox1.Controls.Add(this.btnRfAdd);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 59);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(253, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "CCM Machine";
            // 
            // cmbMachineName
            // 
            this.cmbMachineName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMachineName.FormattingEnabled = true;
            this.cmbMachineName.Location = new System.Drawing.Point(330, 19);
            this.cmbMachineName.Name = "cmbMachineName";
            this.cmbMachineName.Size = new System.Drawing.Size(191, 21);
            this.cmbMachineName.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "RFID IP :";
            // 
            // btnRfCancel
            // 
            this.btnRfCancel.Location = new System.Drawing.Point(689, 18);
            this.btnRfCancel.Name = "btnRfCancel";
            this.btnRfCancel.Size = new System.Drawing.Size(75, 23);
            this.btnRfCancel.TabIndex = 37;
            this.btnRfCancel.Text = "Cancel";
            this.btnRfCancel.UseVisualStyleBackColor = true;
            this.btnRfCancel.Click += new System.EventHandler(this.btnRfCancel_Click);
            // 
            // btnRfDelete
            // 
            this.btnRfDelete.Location = new System.Drawing.Point(608, 18);
            this.btnRfDelete.Name = "btnRfDelete";
            this.btnRfDelete.Size = new System.Drawing.Size(75, 23);
            this.btnRfDelete.TabIndex = 36;
            this.btnRfDelete.Text = "Delete";
            this.btnRfDelete.UseVisualStyleBackColor = true;
            this.btnRfDelete.Click += new System.EventHandler(this.btnRfDelete_Click);
            // 
            // btnRfAdd
            // 
            this.btnRfAdd.Location = new System.Drawing.Point(527, 18);
            this.btnRfAdd.Name = "btnRfAdd";
            this.btnRfAdd.Size = new System.Drawing.Size(75, 23);
            this.btnRfAdd.TabIndex = 35;
            this.btnRfAdd.Text = "Add/Update";
            this.btnRfAdd.UseVisualStyleBackColor = true;
            this.btnRfAdd.Click += new System.EventHandler(this.btnRfAdd_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(76, 20);
            this.txtIP.Name = "txtIP";
            this.txtIP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtIP.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtIP.Properties.MaskSettings.Set("mask", "(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(" +
        "25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[" +
        "0-4][0-9])|(25[0-5]))");
            this.txtIP.Properties.MaxLength = 50;
            this.txtIP.Size = new System.Drawing.Size(171, 20);
            this.txtIP.TabIndex = 34;
            // 
            // xtraTabOperator
            // 
            this.xtraTabOperator.Controls.Add(this.tableLayoutOperator);
            this.xtraTabOperator.Name = "xtraTabOperator";
            this.xtraTabOperator.Size = new System.Drawing.Size(874, 425);
            this.xtraTabOperator.Text = "Operator Master";
            // 
            // tableLayoutOperator
            // 
            this.tableLayoutOperator.ColumnCount = 1;
            this.tableLayoutOperator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutOperator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutOperator.Controls.Add(this.gridOperator, 0, 1);
            this.tableLayoutOperator.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutOperator.Location = new System.Drawing.Point(11, 1);
            this.tableLayoutOperator.Name = "tableLayoutOperator";
            this.tableLayoutOperator.RowCount = 2;
            this.tableLayoutOperator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.09456F));
            this.tableLayoutOperator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.90543F));
            this.tableLayoutOperator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutOperator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutOperator.Size = new System.Drawing.Size(776, 423);
            this.tableLayoutOperator.TabIndex = 1;
            // 
            // gridOperator
            // 
            this.gridOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOperator.Location = new System.Drawing.Point(3, 88);
            this.gridOperator.MainView = this.gvOperator;
            this.gridOperator.Name = "gridOperator";
            this.gridOperator.Size = new System.Drawing.Size(770, 332);
            this.gridOperator.TabIndex = 8;
            this.gridOperator.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOperator});
            // 
            // gvOperator
            // 
            this.gvOperator.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gvOperator.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvOperator.GridControl = this.gridOperator;
            this.gvOperator.Name = "gvOperator";
            this.gvOperator.OptionsBehavior.Editable = false;
            this.gvOperator.OptionsCustomization.AllowColumnMoving = false;
            this.gvOperator.OptionsCustomization.AllowFilter = false;
            this.gvOperator.OptionsCustomization.AllowGroup = false;
            this.gvOperator.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvOperator.OptionsCustomization.AllowSort = false;
            this.gvOperator.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvOperator.OptionsFilter.AllowFilterEditor = false;
            this.gvOperator.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gvOperator.OptionsFilter.AllowMRUFilterList = false;
            this.gvOperator.OptionsFind.AllowFindPanel = false;
            this.gvOperator.OptionsMenu.EnableColumnMenu = false;
            this.gvOperator.OptionsMenu.EnableFooterMenu = false;
            this.gvOperator.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvOperator.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gvOperator.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvOperator.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvOperator.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvOperator.OptionsMenu.ShowSplitItem = false;
            this.gvOperator.OptionsView.ColumnAutoWidth = false;
            this.gvOperator.OptionsView.ShowDetailButtons = false;
            this.gvOperator.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvOperator.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtEmpName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnOpCancel);
            this.groupBox2.Controls.Add(this.btnOpDelete);
            this.groupBox2.Controls.Add(this.btnOpAdd);
            this.groupBox2.Controls.Add(this.txtEmpCode);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(770, 79);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(249, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "* Please Enter According To HR Employee Records";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Name :";
            // 
            // txtEmpName
            // 
            this.txtEmpName.Location = new System.Drawing.Point(230, 20);
            this.txtEmpName.Name = "txtEmpName";
            this.txtEmpName.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtEmpName.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtEmpName.Properties.MaskSettings.Set("mask", "[0-9A-Z]+");
            this.txtEmpName.Properties.MaxLength = 50;
            this.txtEmpName.Properties.ReadOnly = true;
            this.txtEmpName.Size = new System.Drawing.Size(291, 20);
            this.txtEmpName.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "EmpCode :";
            // 
            // btnOpCancel
            // 
            this.btnOpCancel.Location = new System.Drawing.Point(689, 18);
            this.btnOpCancel.Name = "btnOpCancel";
            this.btnOpCancel.Size = new System.Drawing.Size(75, 23);
            this.btnOpCancel.TabIndex = 37;
            this.btnOpCancel.Text = "Cancel";
            this.btnOpCancel.UseVisualStyleBackColor = true;
            this.btnOpCancel.Click += new System.EventHandler(this.btnOpCancel_Click);
            // 
            // btnOpDelete
            // 
            this.btnOpDelete.Location = new System.Drawing.Point(608, 18);
            this.btnOpDelete.Name = "btnOpDelete";
            this.btnOpDelete.Size = new System.Drawing.Size(75, 23);
            this.btnOpDelete.TabIndex = 36;
            this.btnOpDelete.Text = "Delete";
            this.btnOpDelete.UseVisualStyleBackColor = true;
            this.btnOpDelete.Click += new System.EventHandler(this.btnOpDelete_Click);
            // 
            // btnOpAdd
            // 
            this.btnOpAdd.Location = new System.Drawing.Point(527, 18);
            this.btnOpAdd.Name = "btnOpAdd";
            this.btnOpAdd.Size = new System.Drawing.Size(75, 23);
            this.btnOpAdd.TabIndex = 35;
            this.btnOpAdd.Text = "Add";
            this.btnOpAdd.UseVisualStyleBackColor = true;
            this.btnOpAdd.Click += new System.EventHandler(this.btnOpAdd_Click);
            // 
            // txtEmpCode
            // 
            this.txtEmpCode.Location = new System.Drawing.Point(76, 20);
            this.txtEmpCode.Name = "txtEmpCode";
            this.txtEmpCode.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtEmpCode.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtEmpCode.Properties.MaskSettings.Set("mask", "\\d+");
            this.txtEmpCode.Properties.MaxLength = 50;
            this.txtEmpCode.Size = new System.Drawing.Size(101, 20);
            this.txtEmpCode.TabIndex = 34;
            this.txtEmpCode.Validated += new System.EventHandler(this.txtEmpCode_Validated);
            // 
            // xtraTabOperation
            // 
            this.xtraTabOperation.Controls.Add(this.tableLayoutPanel1);
            this.xtraTabOperation.Name = "xtraTabOperation";
            this.xtraTabOperation.Size = new System.Drawing.Size(874, 425);
            this.xtraTabOperation.Text = "Register Operator to Punch Machine";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 534F));
            this.tableLayoutPanel1.Controls.Add(this.gridRegister, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 423F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(861, 423);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // gridRegister
            // 
            this.gridRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRegister.Location = new System.Drawing.Point(330, 3);
            this.gridRegister.MainView = this.gvRegister;
            this.gridRegister.Name = "gridRegister";
            this.gridRegister.Size = new System.Drawing.Size(528, 417);
            this.gridRegister.TabIndex = 8;
            this.gridRegister.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRegister});
            // 
            // gvRegister
            // 
            this.gvRegister.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gvRegister.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvRegister.GridControl = this.gridRegister;
            this.gvRegister.Name = "gvRegister";
            this.gvRegister.OptionsBehavior.Editable = false;
            this.gvRegister.OptionsCustomization.AllowColumnMoving = false;
            this.gvRegister.OptionsCustomization.AllowFilter = false;
            this.gvRegister.OptionsCustomization.AllowGroup = false;
            this.gvRegister.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvRegister.OptionsCustomization.AllowSort = false;
            this.gvRegister.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvRegister.OptionsFilter.AllowFilterEditor = false;
            this.gvRegister.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gvRegister.OptionsFilter.AllowMRUFilterList = false;
            this.gvRegister.OptionsFind.AllowFindPanel = false;
            this.gvRegister.OptionsMenu.EnableColumnMenu = false;
            this.gvRegister.OptionsMenu.EnableFooterMenu = false;
            this.gvRegister.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvRegister.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gvRegister.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvRegister.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvRegister.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvRegister.OptionsMenu.ShowSplitItem = false;
            this.gvRegister.OptionsView.ColumnAutoWidth = false;
            this.gvRegister.OptionsView.ShowDetailButtons = false;
            this.gvRegister.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvRegister.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRegALL);
            this.groupBox3.Controls.Add(this.btnRegDownload);
            this.groupBox3.Controls.Add(this.btnRegTime);
            this.groupBox3.Controls.Add(this.chkListRFID);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.cmbEmp);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.btnRegDel);
            this.groupBox3.Controls.Add(this.btnRegAdd);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(319, 417);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // btnRegDownload
            // 
            this.btnRegDownload.Location = new System.Drawing.Point(196, 387);
            this.btnRegDownload.Name = "btnRegDownload";
            this.btnRegDownload.Size = new System.Drawing.Size(112, 23);
            this.btnRegDownload.TabIndex = 45;
            this.btnRegDownload.Text = "Download Punch";
            this.btnRegDownload.UseVisualStyleBackColor = true;
            this.btnRegDownload.Visible = false;
            this.btnRegDownload.Click += new System.EventHandler(this.btnRegDownload_Click);
            // 
            // btnRegTime
            // 
            this.btnRegTime.Location = new System.Drawing.Point(233, 329);
            this.btnRegTime.Name = "btnRegTime";
            this.btnRegTime.Size = new System.Drawing.Size(75, 23);
            this.btnRegTime.TabIndex = 44;
            this.btnRegTime.Text = "Set Time";
            this.btnRegTime.UseVisualStyleBackColor = true;
            this.btnRegTime.Click += new System.EventHandler(this.btnRegTime_Click);
            // 
            // chkListRFID
            // 
            this.chkListRFID.Location = new System.Drawing.Point(71, 53);
            this.chkListRFID.Name = "chkListRFID";
            this.chkListRFID.Size = new System.Drawing.Size(242, 270);
            this.chkListRFID.TabIndex = 43;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 42;
            this.label4.Text = "Machine :";
            // 
            // cmbEmp
            // 
            this.cmbEmp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmp.FormattingEnabled = true;
            this.cmbEmp.Location = new System.Drawing.Point(71, 20);
            this.cmbEmp.Name = "cmbEmp";
            this.cmbEmp.Size = new System.Drawing.Size(242, 21);
            this.cmbEmp.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "EmpCode :";
            // 
            // btnRegDel
            // 
            this.btnRegDel.Location = new System.Drawing.Point(152, 329);
            this.btnRegDel.Name = "btnRegDel";
            this.btnRegDel.Size = new System.Drawing.Size(75, 23);
            this.btnRegDel.TabIndex = 36;
            this.btnRegDel.Text = "Delete";
            this.btnRegDel.UseVisualStyleBackColor = true;
            this.btnRegDel.Click += new System.EventHandler(this.btnRegDel_Click);
            // 
            // btnRegAdd
            // 
            this.btnRegAdd.Location = new System.Drawing.Point(71, 329);
            this.btnRegAdd.Name = "btnRegAdd";
            this.btnRegAdd.Size = new System.Drawing.Size(75, 23);
            this.btnRegAdd.TabIndex = 35;
            this.btnRegAdd.Text = "Register";
            this.btnRegAdd.UseVisualStyleBackColor = true;
            this.btnRegAdd.Click += new System.EventHandler(this.btnRegAdd_Click);
            // 
            // xtraTabPunching
            // 
            this.xtraTabPunching.Controls.Add(this.tableLayoutPanel2);
            this.xtraTabPunching.Name = "xtraTabPunching";
            this.xtraTabPunching.Size = new System.Drawing.Size(874, 425);
            this.xtraTabPunching.Text = "Punching Records";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.gridPunching, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(11, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.36643F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.63357F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(776, 423);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // gridPunching
            // 
            this.gridPunching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPunching.Location = new System.Drawing.Point(3, 68);
            this.gridPunching.MainView = this.gvPunching;
            this.gridPunching.Name = "gridPunching";
            this.gridPunching.Size = new System.Drawing.Size(770, 352);
            this.gridPunching.TabIndex = 8;
            this.gridPunching.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPunching});
            // 
            // gvPunching
            // 
            this.gvPunching.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gvPunching.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvPunching.GridControl = this.gridPunching;
            this.gvPunching.Name = "gvPunching";
            this.gvPunching.OptionsBehavior.Editable = false;
            this.gvPunching.OptionsCustomization.AllowColumnMoving = false;
            this.gvPunching.OptionsCustomization.AllowFilter = false;
            this.gvPunching.OptionsCustomization.AllowGroup = false;
            this.gvPunching.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvPunching.OptionsCustomization.AllowSort = false;
            this.gvPunching.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvPunching.OptionsFilter.AllowFilterEditor = false;
            this.gvPunching.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gvPunching.OptionsFilter.AllowMRUFilterList = false;
            this.gvPunching.OptionsFind.AllowFindPanel = false;
            this.gvPunching.OptionsMenu.EnableColumnMenu = false;
            this.gvPunching.OptionsMenu.EnableFooterMenu = false;
            this.gvPunching.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPunching.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gvPunching.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvPunching.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvPunching.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvPunching.OptionsMenu.ShowSplitItem = false;
            this.gvPunching.OptionsView.ColumnAutoWidth = false;
            this.gvPunching.OptionsView.ShowDetailButtons = false;
            this.gvPunching.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvPunching.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnPunchQry);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.txtdate);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(770, 59);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // btnPunchQry
            // 
            this.btnPunchQry.Location = new System.Drawing.Point(218, 18);
            this.btnPunchQry.Name = "btnPunchQry";
            this.btnPunchQry.Size = new System.Drawing.Size(75, 23);
            this.btnPunchQry.TabIndex = 44;
            this.btnPunchQry.Text = "Query";
            this.btnPunchQry.UseVisualStyleBackColor = true;
            this.btnPunchQry.Click += new System.EventHandler(this.btnPunchQry_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Date :";
            // 
            // txtdate
            // 
            this.txtdate.EditValue = null;
            this.txtdate.Location = new System.Drawing.Point(97, 20);
            this.txtdate.Name = "txtdate";
            this.txtdate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtdate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtdate.Size = new System.Drawing.Size(100, 20);
            this.txtdate.TabIndex = 1;
            // 
            // btnRegALL
            // 
            this.btnRegALL.Location = new System.Drawing.Point(71, 358);
            this.btnRegALL.Name = "btnRegALL";
            this.btnRegALL.Size = new System.Drawing.Size(237, 23);
            this.btnRegALL.TabIndex = 46;
            this.btnRegALL.Text = "Register ALL Operator";
            this.btnRegALL.UseVisualStyleBackColor = true;
            this.btnRegALL.Click += new System.EventHandler(this.btnRegALL_Click);
            // 
            // frmRFID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 450);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "frmRFID";
            this.Text = "RFID Maintainance";
            this.Load += new System.EventHandler(this.frmRFID_Load);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabMachine.ResumeLayout(false);
            this.tableLayoutRFIDMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRFID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRFID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).EndInit();
            this.xtraTabOperator.ResumeLayout(false);
            this.tableLayoutOperator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOperator)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpCode.Properties)).EndInit();
            this.xtraTabOperation.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRegister)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRegister)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkListRFID)).EndInit();
            this.xtraTabPunching.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPunching)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPunching)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtdate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtdate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabMachine;
        private DevExpress.XtraTab.XtraTabPage xtraTabOperator;
        private DevExpress.XtraTab.XtraTabPage xtraTabOperation;
        private DevExpress.XtraTab.XtraTabPage xtraTabPunching;
        private System.Windows.Forms.TableLayoutPanel tableLayoutRFIDMaster;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRfCancel;
        private System.Windows.Forms.Button btnRfDelete;
        private System.Windows.Forms.Button btnRfAdd;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMachineName;
        private DevExpress.XtraGrid.GridControl gridRFID;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRFID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutOperator;
        private DevExpress.XtraGrid.GridControl gridOperator;
        private DevExpress.XtraGrid.Views.Grid.GridView gvOperator;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtEmpName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOpCancel;
        private System.Windows.Forms.Button btnOpDelete;
        private System.Windows.Forms.Button btnOpAdd;
        private DevExpress.XtraEditors.TextEdit txtEmpCode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl gridRegister;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRegister;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbEmp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRegDel;
        private System.Windows.Forms.Button btnRegAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraGrid.GridControl gridPunching;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPunching;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnPunchQry;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.DateEdit txtdate;
        private DevExpress.XtraEditors.CheckedListBoxControl chkListRFID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnRegTime;
        private System.Windows.Forms.Button btnRegDownload;
        private System.Windows.Forms.Button btnRegALL;
    }
}