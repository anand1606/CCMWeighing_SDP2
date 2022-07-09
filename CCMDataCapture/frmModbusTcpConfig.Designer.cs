namespace CCMDataCapture
{
    partial class frmModbusTcpConfig
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
            this.tableLayoutRFIDMaster = new System.Windows.Forms.TableLayoutPanel();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gv = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDesc = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHoldingReg = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbMachineName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRfCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRfAdd = new System.Windows.Forms.Button();
            this.txtIP = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutRFIDMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoldingReg.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutRFIDMaster
            // 
            this.tableLayoutRFIDMaster.ColumnCount = 1;
            this.tableLayoutRFIDMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutRFIDMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutRFIDMaster.Controls.Add(this.grid, 0, 1);
            this.tableLayoutRFIDMaster.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutRFIDMaster.Location = new System.Drawing.Point(3, 12);
            this.tableLayoutRFIDMaster.Name = "tableLayoutRFIDMaster";
            this.tableLayoutRFIDMaster.RowCount = 2;
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.78723F));
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.21277F));
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutRFIDMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutRFIDMaster.Size = new System.Drawing.Size(860, 423);
            this.tableLayoutRFIDMaster.TabIndex = 1;
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 128);
            this.grid.MainView = this.gv;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(854, 292);
            this.grid.TabIndex = 8;
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
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtDesc);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtHoldingReg);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbMachineName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnRfCancel);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnRfAdd);
            this.groupBox1.Controls.Add(this.txtIP);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(854, 119);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(234, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 47;
            this.label4.Text = "Description";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(233, 88);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtDesc.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtDesc.Properties.MaskSettings.Set("mask", "[a-zA-Z]+");
            this.txtDesc.Properties.MaskSettings.Set("MaskManagerSignature", "isOptimistic=False");
            this.txtDesc.Properties.MaxLength = 50;
            this.txtDesc.Size = new System.Drawing.Size(139, 20);
            this.txtDesc.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Holding Register No";
            // 
            // txtHoldingReg
            // 
            this.txtHoldingReg.Location = new System.Drawing.Point(6, 88);
            this.txtHoldingReg.Name = "txtHoldingReg";
            this.txtHoldingReg.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtHoldingReg.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtHoldingReg.Properties.MaskSettings.Set("mask", "\\d\\d\\d\\d\\d");
            this.txtHoldingReg.Properties.MaxLength = 50;
            this.txtHoldingReg.Size = new System.Drawing.Size(139, 20);
            this.txtHoldingReg.TabIndex = 44;
            this.txtHoldingReg.Validated += new System.EventHandler(this.txtHoldingReg_Validated);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(770, 27);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 43;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(685, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Port :";
            this.label2.Visible = false;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(688, 30);
            this.txtPort.Name = "txtPort";
            this.txtPort.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtPort.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtPort.Properties.MaskSettings.Set("mask", "\\d+");
            this.txtPort.Properties.MaxLength = 50;
            this.txtPort.Size = new System.Drawing.Size(76, 20);
            this.txtPort.TabIndex = 41;
            this.txtPort.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "CCM Machine";
            // 
            // cmbMachineName
            // 
            this.cmbMachineName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMachineName.FormattingEnabled = true;
            this.cmbMachineName.Location = new System.Drawing.Point(151, 87);
            this.cmbMachineName.Name = "cmbMachineName";
            this.cmbMachineName.Size = new System.Drawing.Size(76, 21);
            this.cmbMachineName.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(543, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 38;
            this.label6.Text = "Modbus IP :";
            this.label6.Visible = false;
            // 
            // btnRfCancel
            // 
            this.btnRfCancel.Location = new System.Drawing.Point(591, 85);
            this.btnRfCancel.Name = "btnRfCancel";
            this.btnRfCancel.Size = new System.Drawing.Size(75, 23);
            this.btnRfCancel.TabIndex = 37;
            this.btnRfCancel.Text = "Cancel";
            this.btnRfCancel.UseVisualStyleBackColor = true;
            this.btnRfCancel.Click += new System.EventHandler(this.btnRfCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(510, 85);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 36;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRfAdd
            // 
            this.btnRfAdd.Location = new System.Drawing.Point(429, 85);
            this.btnRfAdd.Name = "btnRfAdd";
            this.btnRfAdd.Size = new System.Drawing.Size(75, 23);
            this.btnRfAdd.TabIndex = 35;
            this.btnRfAdd.Text = "Add/Update";
            this.btnRfAdd.UseVisualStyleBackColor = true;
            this.btnRfAdd.Click += new System.EventHandler(this.btnRfAdd_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(543, 30);
            this.txtIP.Name = "txtIP";
            this.txtIP.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.RegExpMaskManager));
            this.txtIP.Properties.MaskSettings.Set("allowBlankInput", true);
            this.txtIP.Properties.MaskSettings.Set("mask", "(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(" +
        "25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[0-4][0-9])|(25[0-5]))\\.(([01]?[0-9]?[0-9])|(2[" +
        "0-4][0-9])|(25[0-5]))");
            this.txtIP.Properties.MaxLength = 50;
            this.txtIP.Size = new System.Drawing.Size(139, 20);
            this.txtIP.TabIndex = 34;
            this.txtIP.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(459, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "* This Software is using self contained Modbus TCP  Server, Listening Any IP Addr" +
    "ess of this PC";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(553, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "* Holding Register Values are Unsigned 16-bit integer , incresing to +1 whenever " +
    "Low / Over Weight Pipe Recorded";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(494, 13);
            this.label8.TabIndex = 50;
            this.label8.Text = "* Do not Assign Duplicate CCM Machine , also Please keep minimum 4 nos of gap bet" +
    "ween two register";
            // 
            // frmModbusTcpConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 450);
            this.Controls.Add(this.tableLayoutRFIDMaster);
            this.Name = "frmModbusTcpConfig";
            this.Text = "ModbusTcp :  Modbus Master, Station ID : 1 , Port : 502, Base Address : 0, Endian" +
    " - Big ";
            this.Load += new System.EventHandler(this.frmModbusTcpConfig_Load);
            this.tableLayoutRFIDMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoldingReg.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutRFIDMaster;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMachineName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRfCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRfAdd;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtPort;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtHoldingReg;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtDesc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
    }
}