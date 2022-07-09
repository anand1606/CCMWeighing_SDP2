namespace CCMDataCapture
{
    partial class frmShiftConfig
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
            this.isNight = new System.Windows.Forms.CheckBox();
            this.txtEndTime = new DevExpress.XtraEditors.TimeEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartTime = new DevExpress.XtraEditors.TimeEdit();
            this.txtShift = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gridShift = new DevExpress.XtraGrid.GridControl();
            this.gv_Shift = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Shift)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.isNight);
            this.groupBox3.Controls.Add(this.txtEndTime);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtStartTime);
            this.groupBox3.Controls.Add(this.txtShift);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.btnCancel);
            this.groupBox3.Controls.Add(this.gridShift);
            this.groupBox3.Controls.Add(this.btnDelete);
            this.groupBox3.Controls.Add(this.btnUpdate);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Location = new System.Drawing.Point(20, 23);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(875, 252);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Shift Configuration";
            // 
            // isNight
            // 
            this.isNight.AutoSize = true;
            this.isNight.Location = new System.Drawing.Point(151, 103);
            this.isNight.Name = "isNight";
            this.isNight.Size = new System.Drawing.Size(235, 17);
            this.isNight.TabIndex = 19;
            this.isNight.Text = "is Night Shift ( if shift timing cover night time )";
            this.isNight.UseVisualStyleBackColor = true;
            // 
            // txtEndTime
            // 
            this.txtEndTime.EditValue = new System.DateTime(2021, 6, 16, 0, 0, 0, 0);
            this.txtEndTime.Location = new System.Drawing.Point(323, 65);
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEndTime.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtEndTime.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtEndTime.Size = new System.Drawing.Size(92, 20);
            this.txtEndTime.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(252, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "End Time";
            // 
            // txtStartTime
            // 
            this.txtStartTime.EditValue = new System.DateTime(2021, 6, 16, 0, 0, 0, 0);
            this.txtStartTime.Location = new System.Drawing.Point(151, 65);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtStartTime.Properties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.txtStartTime.Properties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.txtStartTime.Size = new System.Drawing.Size(92, 20);
            this.txtStartTime.TabIndex = 16;
            // 
            // txtShift
            // 
            this.txtShift.Location = new System.Drawing.Point(151, 33);
            this.txtShift.Margin = new System.Windows.Forms.Padding(4);
            this.txtShift.MaxLength = 2;
            this.txtShift.Name = "txtShift";
            this.txtShift.Size = new System.Drawing.Size(92, 20);
            this.txtShift.TabIndex = 0;
            this.txtShift.Validated += new System.EventHandler(this.txtShift_Validated);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(520, 16);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(79, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Available Shifts";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(391, 178);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gridShift
            // 
            this.gridShift.Location = new System.Drawing.Point(523, 35);
            this.gridShift.MainView = this.gv_Shift;
            this.gridShift.Name = "gridShift";
            this.gridShift.Size = new System.Drawing.Size(346, 196);
            this.gridShift.TabIndex = 14;
            this.gridShift.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gv_Shift});
            // 
            // gv_Shift
            // 
            this.gv_Shift.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gv_Shift.Appearance.EvenRow.Options.UseBackColor = true;
            this.gv_Shift.GridControl = this.gridShift;
            this.gv_Shift.Name = "gv_Shift";
            this.gv_Shift.OptionsBehavior.Editable = false;
            this.gv_Shift.OptionsCustomization.AllowColumnMoving = false;
            this.gv_Shift.OptionsCustomization.AllowFilter = false;
            this.gv_Shift.OptionsCustomization.AllowGroup = false;
            this.gv_Shift.OptionsCustomization.AllowQuickHideColumns = false;
            this.gv_Shift.OptionsCustomization.AllowSort = false;
            this.gv_Shift.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gv_Shift.OptionsFilter.AllowFilterEditor = false;
            this.gv_Shift.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gv_Shift.OptionsFilter.AllowMRUFilterList = false;
            this.gv_Shift.OptionsFilter.FilterEditorUseMenuForOperandsAndOperators = false;
            this.gv_Shift.OptionsFind.AllowFindPanel = false;
            this.gv_Shift.OptionsMenu.EnableColumnMenu = false;
            this.gv_Shift.OptionsMenu.EnableFooterMenu = false;
            this.gv_Shift.OptionsMenu.EnableGroupPanelMenu = false;
            this.gv_Shift.OptionsMenu.ShowAddNewSummaryItem = DevExpress.Utils.DefaultBoolean.False;
            this.gv_Shift.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gv_Shift.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gv_Shift.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gv_Shift.OptionsMenu.ShowSplitItem = false;
            this.gv_Shift.OptionsView.ColumnAutoWidth = false;
            this.gv_Shift.OptionsView.ShowDetailButtons = false;
            this.gv_Shift.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gv_Shift.OptionsView.ShowGroupPanel = false;
            this.gv_Shift.DoubleClick += new System.EventHandler(this.gv_Shift_DoubleClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(310, 178);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 33);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(229, 178);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 33);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 38);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Shift Code";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(148, 178);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 33);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 68);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 13);
            this.label17.TabIndex = 8;
            this.label17.Text = "Start Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 133);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(313, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "* Make Sure, Shift Timing do not overlap/gapped between shifts ";
            // 
            // frmShiftConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 282);
            this.Controls.Add(this.groupBox3);
            this.Name = "frmShiftConfig";
            this.Text = "Shift Configuration";
            this.Load += new System.EventHandler(this.frmShiftConfig_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gv_Shift)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.TimeEdit txtEndTime;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TimeEdit txtStartTime;
        private System.Windows.Forms.TextBox txtShift;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnCancel;
        private DevExpress.XtraGrid.GridControl gridShift;
        private DevExpress.XtraGrid.Views.Grid.GridView gv_Shift;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox isNight;
        private System.Windows.Forms.Label label2;
    }
}