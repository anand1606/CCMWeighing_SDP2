namespace CCMDataCapture
{
    partial class frmMasters
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShiftConfig = new System.Windows.Forms.Button();
            this.grpMaster = new DevExpress.XtraEditors.RadioGroup();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDesc = new DevExpress.XtraEditors.TextEdit();
            this.txtID = new DevExpress.XtraEditors.TextEdit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpMaster.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1015, 627);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Master Data";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grid, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox8, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1009, 608);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 153);
            this.grid.MainView = this.gv;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(1003, 452);
            this.grid.TabIndex = 7;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv});
            // 
            // gv
            // 
            this.gv.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gv.Appearance.EvenRow.Options.UseBackColor = true;
            this.gv.GridControl = this.grid;
            this.gv.Name = "gv";
            this.gv.OptionsBehavior.Editable = false;
            this.gv.OptionsCustomization.AllowColumnMoving = false;
            this.gv.OptionsCustomization.AllowFilter = false;
            this.gv.OptionsCustomization.AllowGroup = false;
            this.gv.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv.OptionsCustomization.AllowSort = false;
            this.gv.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gv.OptionsFilter.AllowFilterEditor = false;
            this.gv.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gv.OptionsFilter.AllowMRUFilterList = false;
            this.gv.OptionsFind.AllowFindPanel = false;
            this.gv.OptionsMenu.EnableColumnMenu = false;
            this.gv.OptionsMenu.EnableFooterMenu = false;
            this.gv.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gv.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gv.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gv.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gv.OptionsMenu.ShowSplitItem = false;
            this.gv.OptionsView.ColumnAutoWidth = false;
            this.gv.OptionsView.ShowDetailButtons = false;
            this.gv.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gv.OptionsView.ShowGroupPanel = false;
            this.gv.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gv.DoubleClick += new System.EventHandler(this.gv_DoubleClick);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnCancel);
            this.groupBox8.Controls.Add(this.btnShiftConfig);
            this.groupBox8.Controls.Add(this.grpMaster);
            this.groupBox8.Controls.Add(this.btnDelete);
            this.groupBox8.Controls.Add(this.btnAdd);
            this.groupBox8.Controls.Add(this.label6);
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.Controls.Add(this.txtDesc);
            this.groupBox8.Controls.Add(this.txtID);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(3, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(1003, 144);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(574, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnShiftConfig
            // 
            this.btnShiftConfig.Location = new System.Drawing.Point(707, 19);
            this.btnShiftConfig.Name = "btnShiftConfig";
            this.btnShiftConfig.Size = new System.Drawing.Size(133, 23);
            this.btnShiftConfig.TabIndex = 30;
            this.btnShiftConfig.Text = "Shift Config";
            this.btnShiftConfig.UseVisualStyleBackColor = true;
            this.btnShiftConfig.Visible = false;
            this.btnShiftConfig.Click += new System.EventHandler(this.btnShiftConfig_Click);
            // 
            // grpMaster
            // 
            this.grpMaster.Location = new System.Drawing.Point(14, 20);
            this.grpMaster.Name = "grpMaster";
            this.grpMaster.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ccmSize", "Size", true, "SIZE", "selSize"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ccmClass", "Class", true, "CLASS", "selClass"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ccmLength", "Length", true, "LENGTH", "selLength"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ccmMaterial", "Material", true, "MATERIAL", "selMaterial"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ccmStandard", "Standard", true, "STANDARD", "selStandard"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("ccmDefect", "Defect", true, "DEFECT", "selDefect")});
            this.grpMaster.Size = new System.Drawing.Size(219, 99);
            this.grpMaster.TabIndex = 12;
            this.grpMaster.EditValueChanged += new System.EventHandler(this.grpMaster_EditValueChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(574, 48);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(574, 22);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(324, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Desc. :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(324, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "ID :";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(381, 47);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtDesc.Properties.MaskSettings.Set("allowBlankInput", null);
            this.txtDesc.Properties.MaskSettings.Set("mask", ".+");
            this.txtDesc.Properties.MaskSettings.Set("placeholder", '_');
            this.txtDesc.Properties.MaskSettings.Set("isAutoComplete", false);
            this.txtDesc.Properties.MaskSettings.Set("showPlaceholders", false);
            this.txtDesc.Properties.MaxLength = 50;
            this.txtDesc.Size = new System.Drawing.Size(171, 20);
            this.txtDesc.TabIndex = 1;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(381, 21);
            this.txtID.Name = "txtID";
            this.txtID.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtID.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtID.Properties.MaskSettings.Set("mask", "[0-9]+");
            this.txtID.Properties.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(51, 20);
            this.txtID.TabIndex = 0;
            this.txtID.Validated += new System.EventHandler(this.txtID_Validated);
            // 
            // frmMasters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 627);
            this.Controls.Add(this.groupBox3);
            this.Name = "frmMasters";
            this.Text = "Master Data";
            this.Load += new System.EventHandler(this.frmMasters_Load);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpMaster.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private System.Windows.Forms.GroupBox groupBox8;
        private DevExpress.XtraEditors.RadioGroup grpMaster;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txtDesc;
        private DevExpress.XtraEditors.TextEdit txtID;
        private System.Windows.Forms.Button btnShiftConfig;
        private System.Windows.Forms.Button btnCancel;
    }
}