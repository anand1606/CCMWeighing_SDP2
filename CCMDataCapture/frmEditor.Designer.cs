namespace CCMDataCapture
{
    partial class frmEditor
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grd_report = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSrNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colShift = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMachine = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSizeSrNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPipeDia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPipeClass = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptClass = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colLength = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptLength = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colBatchNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNomWt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptNomWt = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colActWt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDevPer = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPipeStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptStatus = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colJoint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptJoint = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colMould = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptMould = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colMaterial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptMaterial = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colStandard = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptStandard = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colRemarks = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rptRemarks = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.colMinWt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMaxWt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAlmMinWt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAlmMaxWt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBulkChange = new System.Windows.Forms.Button();
            this.cmbBulkItem = new System.Windows.Forms.ComboBox();
            this.txtBulkValue = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtIncharge = new DevExpress.XtraEditors.TextEdit();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtShift = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lbl_date = new System.Windows.Forms.Label();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.txtMachines = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd_report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptNomWt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptJoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptMould)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptStandard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptRemarks)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBulkValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIncharge.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShift.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grd_report, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1078, 469);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grd_report
            // 
            this.grd_report.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd_report.Location = new System.Drawing.Point(3, 123);
            this.grd_report.MainView = this.gridView1;
            this.grd_report.Name = "grd_report";
            this.grd_report.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rptStatus,
            this.rptMaterial,
            this.rptStandard,
            this.rptRemarks,
            this.rptClass,
            this.rptLength,
            this.rptJoint,
            this.rptMould,
            this.rptNomWt});
            this.grd_report.Size = new System.Drawing.Size(1072, 343);
            this.grd_report.TabIndex = 7;
            this.grd_report.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSrNo,
            this.colDate,
            this.colTime,
            this.colShift,
            this.colMachine,
            this.colSizeSrNo,
            this.colPipeDia,
            this.colPipeClass,
            this.colLength,
            this.colBatchNo,
            this.colNomWt,
            this.colActWt,
            this.colDevPer,
            this.colPipeStatus,
            this.colJoint,
            this.colMould,
            this.colMaterial,
            this.colStandard,
            this.colRemarks,
            this.colMinWt,
            this.colMaxWt,
            this.colAlmMinWt,
            this.colAlmMaxWt});
            this.gridView1.GridControl = this.grd_report;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsFind.AllowFindPanel = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView1.OptionsMenu.ShowSplitItem = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
            // 
            // colSrNo
            // 
            this.colSrNo.Caption = "SrNo";
            this.colSrNo.FieldName = "SrNo";
            this.colSrNo.Name = "colSrNo";
            this.colSrNo.OptionsColumn.AllowEdit = false;
            this.colSrNo.OptionsColumn.AllowMove = false;
            this.colSrNo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colSrNo.OptionsColumn.ReadOnly = true;
            this.colSrNo.Visible = true;
            this.colSrNo.VisibleIndex = 0;
            this.colSrNo.Width = 43;
            // 
            // colDate
            // 
            this.colDate.Caption = "Date";
            this.colDate.FieldName = "LogDate";
            this.colDate.Name = "colDate";
            this.colDate.OptionsColumn.AllowEdit = false;
            this.colDate.OptionsColumn.AllowMove = false;
            this.colDate.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colDate.OptionsColumn.ReadOnly = true;
            this.colDate.Visible = true;
            this.colDate.VisibleIndex = 1;
            this.colDate.Width = 76;
            // 
            // colTime
            // 
            this.colTime.Caption = "Time";
            this.colTime.FieldName = "LogTime";
            this.colTime.Name = "colTime";
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 2;
            this.colTime.Width = 56;
            // 
            // colShift
            // 
            this.colShift.Caption = "Shift";
            this.colShift.FieldName = "tShift";
            this.colShift.Name = "colShift";
            this.colShift.Visible = true;
            this.colShift.VisibleIndex = 3;
            this.colShift.Width = 33;
            // 
            // colMachine
            // 
            this.colMachine.Caption = "Machine";
            this.colMachine.FieldName = "MachineNo";
            this.colMachine.Name = "colMachine";
            this.colMachine.Visible = true;
            this.colMachine.VisibleIndex = 4;
            this.colMachine.Width = 53;
            // 
            // colSizeSrNo
            // 
            this.colSizeSrNo.Caption = "SizeSrNo";
            this.colSizeSrNo.FieldName = "IntSrNo";
            this.colSizeSrNo.Name = "colSizeSrNo";
            this.colSizeSrNo.Visible = true;
            this.colSizeSrNo.VisibleIndex = 5;
            this.colSizeSrNo.Width = 56;
            // 
            // colPipeDia
            // 
            this.colPipeDia.Caption = "Size";
            this.colPipeDia.FieldName = "PipeDia";
            this.colPipeDia.Name = "colPipeDia";
            this.colPipeDia.OptionsColumn.AllowEdit = false;
            this.colPipeDia.OptionsColumn.AllowMove = false;
            this.colPipeDia.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPipeDia.OptionsColumn.ReadOnly = true;
            this.colPipeDia.Visible = true;
            this.colPipeDia.VisibleIndex = 6;
            this.colPipeDia.Width = 48;
            // 
            // colPipeClass
            // 
            this.colPipeClass.Caption = "Class";
            this.colPipeClass.ColumnEdit = this.rptClass;
            this.colPipeClass.FieldName = "PipeClass";
            this.colPipeClass.Name = "colPipeClass";
            this.colPipeClass.OptionsColumn.AllowMove = false;
            this.colPipeClass.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPipeClass.Visible = true;
            this.colPipeClass.VisibleIndex = 7;
            this.colPipeClass.Width = 48;
            // 
            // rptClass
            // 
            this.rptClass.AutoHeight = false;
            this.rptClass.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rptClass.Name = "rptClass";
            // 
            // colLength
            // 
            this.colLength.Caption = "Length";
            this.colLength.ColumnEdit = this.rptLength;
            this.colLength.FieldName = "PipeLength";
            this.colLength.Name = "colLength";
            this.colLength.Visible = true;
            this.colLength.VisibleIndex = 8;
            this.colLength.Width = 48;
            // 
            // rptLength
            // 
            this.rptLength.AutoHeight = false;
            this.rptLength.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rptLength.Name = "rptLength";
            // 
            // colBatchNo
            // 
            this.colBatchNo.Caption = "BatchNo";
            this.colBatchNo.FieldName = "PipeNumber";
            this.colBatchNo.Name = "colBatchNo";
            this.colBatchNo.OptionsColumn.AllowEdit = false;
            this.colBatchNo.OptionsColumn.AllowMove = false;
            this.colBatchNo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colBatchNo.OptionsColumn.ReadOnly = true;
            this.colBatchNo.Visible = true;
            this.colBatchNo.VisibleIndex = 9;
            this.colBatchNo.Width = 81;
            // 
            // colNomWt
            // 
            this.colNomWt.Caption = "NomWt";
            this.colNomWt.ColumnEdit = this.rptNomWt;
            this.colNomWt.FieldName = "NomWt";
            this.colNomWt.Name = "colNomWt";
            this.colNomWt.Visible = true;
            this.colNomWt.VisibleIndex = 11;
            this.colNomWt.Width = 63;
            // 
            // rptNomWt
            // 
            this.rptNomWt.AutoHeight = false;
            this.rptNomWt.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rptNomWt.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.rptNomWt.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.rptNomWt.MaskSettings.Set("mask", "");
            this.rptNomWt.MaskSettings.Set("autoHideDecimalSeparator", false);
            this.rptNomWt.MaskSettings.Set("valueType", typeof(double));
            this.rptNomWt.Name = "rptNomWt";
            this.rptNomWt.UseMaskAsDisplayFormat = true;
            // 
            // colActWt
            // 
            this.colActWt.Caption = "ActWt";
            this.colActWt.FieldName = "ActWt";
            this.colActWt.Name = "colActWt";
            this.colActWt.OptionsColumn.AllowEdit = false;
            this.colActWt.OptionsColumn.AllowMove = false;
            this.colActWt.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colActWt.OptionsColumn.ReadOnly = true;
            this.colActWt.Visible = true;
            this.colActWt.VisibleIndex = 10;
            this.colActWt.Width = 56;
            // 
            // colDevPer
            // 
            this.colDevPer.Caption = "Wt. Gain(%)";
            this.colDevPer.FieldName = "DevPer";
            this.colDevPer.Name = "colDevPer";
            this.colDevPer.OptionsColumn.AllowEdit = false;
            this.colDevPer.OptionsColumn.ReadOnly = true;
            this.colDevPer.Visible = true;
            this.colDevPer.VisibleIndex = 12;
            // 
            // colPipeStatus
            // 
            this.colPipeStatus.Caption = "PipeStatus";
            this.colPipeStatus.ColumnEdit = this.rptStatus;
            this.colPipeStatus.FieldName = "PipeStatus";
            this.colPipeStatus.Name = "colPipeStatus";
            this.colPipeStatus.OptionsColumn.AllowMove = false;
            this.colPipeStatus.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPipeStatus.Visible = true;
            this.colPipeStatus.VisibleIndex = 13;
            this.colPipeStatus.Width = 90;
            // 
            // rptStatus
            // 
            this.rptStatus.AutoHeight = false;
            this.rptStatus.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rptStatus.Name = "rptStatus";
            // 
            // colJoint
            // 
            this.colJoint.Caption = "Joint";
            this.colJoint.ColumnEdit = this.rptJoint;
            this.colJoint.FieldName = "JointType";
            this.colJoint.Name = "colJoint";
            this.colJoint.Visible = true;
            this.colJoint.VisibleIndex = 16;
            this.colJoint.Width = 43;
            // 
            // rptJoint
            // 
            this.rptJoint.AutoHeight = false;
            this.rptJoint.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegularMaskManager));
            this.rptJoint.MaskSettings.Set("MaskManagerSignature", "ignoreMaskBlank=True");
            this.rptJoint.MaskSettings.Set("mask", "[0-9 a-zA-Z \\-\\[\\]+=.*(){},$:;!@#?><]+");
            this.rptJoint.MaskSettings.Set("ignoreMaskBlank", false);
            this.rptJoint.MaxLength = 10;
            this.rptJoint.Name = "rptJoint";
            // 
            // colMould
            // 
            this.colMould.Caption = "Mould";
            this.colMould.ColumnEdit = this.rptMould;
            this.colMould.FieldName = "MouldNo";
            this.colMould.Name = "colMould";
            this.colMould.Visible = true;
            this.colMould.VisibleIndex = 15;
            this.colMould.Width = 87;
            // 
            // rptMould
            // 
            this.rptMould.AutoHeight = false;
            this.rptMould.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegularMaskManager));
            this.rptMould.MaskSettings.Set("MaskManagerSignature", "ignoreMaskBlank=True");
            this.rptMould.MaskSettings.Set("mask", "[0-9 a-zA-Z \\-\\[\\]+=.*(){},$:;!@#?><]+");
            this.rptMould.MaxLength = 10;
            this.rptMould.Name = "rptMould";
            // 
            // colMaterial
            // 
            this.colMaterial.Caption = "Material";
            this.colMaterial.ColumnEdit = this.rptMaterial;
            this.colMaterial.FieldName = "Material";
            this.colMaterial.Name = "colMaterial";
            this.colMaterial.Visible = true;
            this.colMaterial.VisibleIndex = 14;
            this.colMaterial.Width = 91;
            // 
            // rptMaterial
            // 
            this.rptMaterial.AutoHeight = false;
            this.rptMaterial.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rptMaterial.Name = "rptMaterial";
            this.rptMaterial.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colStandard
            // 
            this.colStandard.Caption = "Standard";
            this.colStandard.ColumnEdit = this.rptStandard;
            this.colStandard.FieldName = "Standard";
            this.colStandard.Name = "colStandard";
            this.colStandard.Visible = true;
            this.colStandard.VisibleIndex = 17;
            this.colStandard.Width = 123;
            // 
            // rptStandard
            // 
            this.rptStandard.AutoHeight = false;
            this.rptStandard.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rptStandard.Name = "rptStandard";
            this.rptStandard.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colRemarks
            // 
            this.colRemarks.Caption = "Remarks";
            this.colRemarks.ColumnEdit = this.rptRemarks;
            this.colRemarks.FieldName = "Remarks";
            this.colRemarks.Name = "colRemarks";
            this.colRemarks.Visible = true;
            this.colRemarks.VisibleIndex = 18;
            // 
            // rptRemarks
            // 
            this.rptRemarks.AutoHeight = false;
            this.rptRemarks.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegularMaskManager));
            this.rptRemarks.MaskSettings.Set("MaskManagerSignature", "ignoreMaskBlank=True");
            this.rptRemarks.MaskSettings.Set("mask", "[0-9 a-zA-Z \\-\\[\\]+=.*(){},$:;!@#?><]+");
            this.rptRemarks.MaxLength = 50;
            this.rptRemarks.Name = "rptRemarks";
            // 
            // colMinWt
            // 
            this.colMinWt.Caption = "MinWt";
            this.colMinWt.FieldName = "MinWt";
            this.colMinWt.Name = "colMinWt";
            // 
            // colMaxWt
            // 
            this.colMaxWt.Caption = "MaxWt";
            this.colMaxWt.FieldName = "MaxWt";
            this.colMaxWt.Name = "colMaxWt";
            // 
            // colAlmMinWt
            // 
            this.colAlmMinWt.Caption = "AlmMinWt";
            this.colAlmMinWt.FieldName = "AlmMinWt";
            this.colAlmMinWt.Name = "colAlmMinWt";
            // 
            // colAlmMaxWt
            // 
            this.colAlmMaxWt.Caption = "AlmMaxWt";
            this.colAlmMaxWt.FieldName = "AlmMaxWt";
            this.colAlmMaxWt.Name = "colAlmMaxWt";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.txtIncharge);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.txtShift);
            this.groupBox1.Controls.Add(this.lbl_date);
            this.groupBox1.Controls.Add(this.txtDate);
            this.groupBox1.Controls.Add(this.txtMachines);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1072, 114);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBulkChange);
            this.groupBox2.Controls.Add(this.cmbBulkItem);
            this.groupBox2.Controls.Add(this.txtBulkValue);
            this.groupBox2.Location = new System.Drawing.Point(602, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 88);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "On Multiple Selected Rows";
            // 
            // btnBulkChange
            // 
            this.btnBulkChange.Location = new System.Drawing.Point(122, 49);
            this.btnBulkChange.Name = "btnBulkChange";
            this.btnBulkChange.Size = new System.Drawing.Size(95, 27);
            this.btnBulkChange.TabIndex = 32;
            this.btnBulkChange.Text = "Bulk Change";
            this.btnBulkChange.UseVisualStyleBackColor = true;
            this.btnBulkChange.Click += new System.EventHandler(this.btnBulkChange_Click);
            // 
            // cmbBulkItem
            // 
            this.cmbBulkItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBulkItem.FormattingEnabled = true;
            this.cmbBulkItem.Items.AddRange(new object[] {
            "",
            "PipeClass",
            "PipeLength",
            "JointType",
            "MouldNo",
            "NomWt",
            "Material",
            "Standard",
            "PipeStatus",
            "Remarks"});
            this.cmbBulkItem.Location = new System.Drawing.Point(6, 19);
            this.cmbBulkItem.Name = "cmbBulkItem";
            this.cmbBulkItem.Size = new System.Drawing.Size(110, 21);
            this.cmbBulkItem.TabIndex = 10;
            this.cmbBulkItem.SelectedIndexChanged += new System.EventHandler(this.cmbBulkItem_SelectedIndexChanged);
            // 
            // txtBulkValue
            // 
            this.txtBulkValue.Location = new System.Drawing.Point(122, 20);
            this.txtBulkValue.Name = "txtBulkValue";
            this.txtBulkValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtBulkValue.Size = new System.Drawing.Size(197, 20);
            this.txtBulkValue.TabIndex = 11;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(501, 68);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(95, 27);
            this.btnExport.TabIndex = 30;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(501, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 27);
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(400, 68);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 27);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(400, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(95, 27);
            this.btnRefresh.TabIndex = 27;
            this.btnRefresh.Text = "Go";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtIncharge
            // 
            this.txtIncharge.Location = new System.Drawing.Point(96, 72);
            this.txtIncharge.Name = "txtIncharge";
            this.txtIncharge.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegularMaskManager));
            this.txtIncharge.Properties.MaskSettings.Set("MaskManagerSignature", "ignoreMaskBlank=True");
            this.txtIncharge.Properties.MaskSettings.Set("mask", "[0-9 a-zA-Z \\-\\[\\]+=.*(){},$:;!@#?><]+");
            this.txtIncharge.Properties.MaxLength = 50;
            this.txtIncharge.Size = new System.Drawing.Size(298, 20);
            this.txtIncharge.TabIndex = 26;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 75);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(79, 13);
            this.label27.TabIndex = 25;
            this.label27.Text = "Shift Incharge :";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(51, 49);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(34, 13);
            this.label26.TabIndex = 24;
            this.label26.Text = "Shift :";
            // 
            // txtShift
            // 
            this.txtShift.Location = new System.Drawing.Point(96, 46);
            this.txtShift.Name = "txtShift";
            this.txtShift.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtShift.Properties.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.txtShift.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.txtShift.Size = new System.Drawing.Size(110, 20);
            this.txtShift.TabIndex = 23;
            // 
            // lbl_date
            // 
            this.lbl_date.AutoSize = true;
            this.lbl_date.Location = new System.Drawing.Point(214, 23);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Size = new System.Drawing.Size(39, 13);
            this.lbl_date.TabIndex = 11;
            this.lbl_date.Text = "Date  :";
            // 
            // txtDate
            // 
            this.txtDate.EditValue = null;
            this.txtDate.Location = new System.Drawing.Point(269, 18);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtDate.Properties.Appearance.Options.UseFont = true;
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDate.Size = new System.Drawing.Size(125, 22);
            this.txtDate.TabIndex = 10;
            // 
            // txtMachines
            // 
            this.txtMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtMachines.FormattingEnabled = true;
            this.txtMachines.Items.AddRange(new object[] {
            "P",
            "Q",
            "R"});
            this.txtMachines.Location = new System.Drawing.Point(96, 19);
            this.txtMachines.Name = "txtMachines";
            this.txtMachines.Size = new System.Drawing.Size(110, 21);
            this.txtMachines.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Machine :";
            // 
            // frmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 469);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmEditor";
            this.Text = "Quality Editor";
            this.Load += new System.EventHandler(this.frmEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd_report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptNomWt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptJoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptMould)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptStandard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptRemarks)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBulkValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIncharge.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtShift.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl grd_report;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_date;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private System.Windows.Forms.ComboBox txtMachines;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label26;
        private DevExpress.XtraEditors.ComboBoxEdit txtShift;
        private DevExpress.XtraEditors.TextEdit txtIncharge;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCancel;
        private DevExpress.XtraGrid.Columns.GridColumn colSrNo;
        private DevExpress.XtraGrid.Columns.GridColumn colDate;
        private DevExpress.XtraGrid.Columns.GridColumn colBatchNo;
        private DevExpress.XtraGrid.Columns.GridColumn colPipeDia;
        private DevExpress.XtraGrid.Columns.GridColumn colPipeClass;
        private DevExpress.XtraGrid.Columns.GridColumn colActWt;
        private DevExpress.XtraGrid.Columns.GridColumn colPipeStatus;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rptStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colTime;
        private DevExpress.XtraGrid.Columns.GridColumn colSizeSrNo;
        private DevExpress.XtraGrid.Columns.GridColumn colLength;
        private DevExpress.XtraGrid.Columns.GridColumn colJoint;
        private DevExpress.XtraGrid.Columns.GridColumn colMould;
        private DevExpress.XtraGrid.Columns.GridColumn colShift;
        private DevExpress.XtraGrid.Columns.GridColumn colMachine;
        private DevExpress.XtraGrid.Columns.GridColumn colNomWt;
        private DevExpress.XtraGrid.Columns.GridColumn colMaterial;
        private DevExpress.XtraGrid.Columns.GridColumn colStandard;
        private System.Windows.Forms.Button btnExport;
        private DevExpress.XtraGrid.Columns.GridColumn colDevPer;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rptMaterial;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rptStandard;
        private DevExpress.XtraGrid.Columns.GridColumn colRemarks;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rptRemarks;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rptClass;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rptLength;
        private DevExpress.XtraGrid.Columns.GridColumn colMinWt;
        private DevExpress.XtraGrid.Columns.GridColumn colMaxWt;
        private DevExpress.XtraGrid.Columns.GridColumn colAlmMinWt;
        private DevExpress.XtraGrid.Columns.GridColumn colAlmMaxWt;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rptNomWt;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rptJoint;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rptMould;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBulkChange;
        private System.Windows.Forms.ComboBox cmbBulkItem;
        private DevExpress.XtraEditors.ComboBoxEdit txtBulkValue;
    }
}