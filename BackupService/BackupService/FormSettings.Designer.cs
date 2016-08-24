namespace BackupService
{
    partial class FormSettings
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewBackupFolders = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSetPassword = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDestinationFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButtonMonth = new System.Windows.Forms.RadioButton();
            this.radioButtonWeek = new System.Windows.Forms.RadioButton();
            this.radioButtonOne = new System.Windows.Forms.RadioButton();
            this.radioButtonDay = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dtPickerStartTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtPickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.btnBackup = new System.Windows.Forms.Button();
            this.saveFileDialogSetDestination = new System.Windows.Forms.SaveFileDialog();
            this.toolTipFileDrop = new System.Windows.Forms.ToolTip(this.components);
            this.backupFolderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.customPanel3 = new BackupService.UserControls.CustomPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.customPanel2 = new BackupService.UserControls.CustomPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.customPanel1 = new BackupService.UserControls.CustomPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.fullPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subFoldersDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBackupFolders)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backupFolderBindingSource)).BeginInit();
            this.customPanel3.SuspendLayout();
            this.customPanel2.SuspendLayout();
            this.customPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewBackupFolders);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(275, 0);
            this.groupBox1.MinimumSize = new System.Drawing.Size(200, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(549, 452);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Backup sources";
            // 
            // dataGridViewBackupFolders
            // 
            this.dataGridViewBackupFolders.AllowDrop = true;
            this.dataGridViewBackupFolders.AllowUserToAddRows = false;
            this.dataGridViewBackupFolders.AllowUserToResizeRows = false;
            this.dataGridViewBackupFolders.AutoGenerateColumns = false;
            this.dataGridViewBackupFolders.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridViewBackupFolders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewBackupFolders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fullPathDataGridViewTextBoxColumn,
            this.subFoldersDataGridViewTextBoxColumn,
            this.filesDataGridViewTextBoxColumn,
            this.sizeDataGridViewTextBoxColumn});
            this.dataGridViewBackupFolders.DataSource = this.backupFolderBindingSource;
            this.dataGridViewBackupFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBackupFolders.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewBackupFolders.Location = new System.Drawing.Point(5, 23);
            this.dataGridViewBackupFolders.Name = "dataGridViewBackupFolders";
            this.dataGridViewBackupFolders.ReadOnly = true;
            this.dataGridViewBackupFolders.RowHeadersVisible = false;
            this.dataGridViewBackupFolders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBackupFolders.Size = new System.Drawing.Size(539, 424);
            this.dataGridViewBackupFolders.TabIndex = 0;
            this.toolTipFileDrop.SetToolTip(this.dataGridViewBackupFolders, "Drag folders here from Windows file explorer");
            this.dataGridViewBackupFolders.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridViewBackupFolders_UserDeletingRow);
            this.dataGridViewBackupFolders.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewBackupFolders_DragDrop);
            this.dataGridViewBackupFolders.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridViewBackupFolders_DragEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(5, 457);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(824, 150);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Restore";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(834, 612);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(824, 452);
            this.panel2.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSetPassword);
            this.groupBox3.Controls.Add(this.btnLoadSettings);
            this.groupBox3.Controls.Add(this.btnSaveSettings);
            this.groupBox3.Controls.Add(this.panel7);
            this.groupBox3.Controls.Add(this.panel5);
            this.groupBox3.Controls.Add(this.panel4);
            this.groupBox3.Controls.Add(this.btnBackup);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.MinimumSize = new System.Drawing.Size(275, 450);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(275, 452);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Backup schedule and settings";
            // 
            // btnSetPassword
            // 
            this.btnSetPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetPassword.Location = new System.Drawing.Point(24, 388);
            this.btnSetPassword.Name = "btnSetPassword";
            this.btnSetPassword.Size = new System.Drawing.Size(107, 26);
            this.btnSetPassword.TabIndex = 14;
            this.btnSetPassword.Text = "Set password";
            this.btnSetPassword.UseVisualStyleBackColor = true;
            this.btnSetPassword.Click += new System.EventHandler(this.btnSetPassword_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSettings.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSettings.Location = new System.Drawing.Point(61, 420);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(100, 26);
            this.btnLoadSettings.TabIndex = 13;
            this.btnLoadSettings.Text = "Load settings";
            this.btnLoadSettings.UseVisualStyleBackColor = true;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveSettings.Location = new System.Drawing.Point(167, 420);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(100, 26);
            this.btnSaveSettings.TabIndex = 12;
            this.btnSaveSettings.Text = "Save settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnClear);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Controls.Add(this.txtDestinationFile);
            this.panel7.Controls.Add(this.btnBrowse);
            this.panel7.Controls.Add(this.customPanel3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 242);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(269, 128);
            this.panel7.TabIndex = 11;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(72, 85);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(71, 26);
            this.btnClear.TabIndex = 19;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "DestinationFile:";
            // 
            // txtDestinationFile
            // 
            this.txtDestinationFile.Location = new System.Drawing.Point(6, 52);
            this.txtDestinationFile.Margin = new System.Windows.Forms.Padding(5);
            this.txtDestinationFile.Name = "txtDestinationFile";
            this.txtDestinationFile.ReadOnly = true;
            this.txtDestinationFile.Size = new System.Drawing.Size(229, 25);
            this.txtDestinationFile.TabIndex = 17;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(149, 85);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(97, 26);
            this.btnBrowse.TabIndex = 16;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.radioButtonMonth);
            this.panel5.Controls.Add(this.customPanel2);
            this.panel5.Controls.Add(this.radioButtonWeek);
            this.panel5.Controls.Add(this.radioButtonOne);
            this.panel5.Controls.Add(this.radioButtonDay);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(3, 96);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(269, 146);
            this.panel5.TabIndex = 10;
            // 
            // radioButtonMonth
            // 
            this.radioButtonMonth.AutoSize = true;
            this.radioButtonMonth.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonMonth.Location = new System.Drawing.Point(11, 119);
            this.radioButtonMonth.Name = "radioButtonMonth";
            this.radioButtonMonth.Size = new System.Drawing.Size(73, 21);
            this.radioButtonMonth.TabIndex = 7;
            this.radioButtonMonth.Text = "Monthly";
            this.radioButtonMonth.UseVisualStyleBackColor = true;
            // 
            // radioButtonWeek
            // 
            this.radioButtonWeek.AutoSize = true;
            this.radioButtonWeek.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonWeek.Location = new System.Drawing.Point(9, 93);
            this.radioButtonWeek.Name = "radioButtonWeek";
            this.radioButtonWeek.Size = new System.Drawing.Size(67, 21);
            this.radioButtonWeek.TabIndex = 6;
            this.radioButtonWeek.Text = "Weekly";
            this.radioButtonWeek.UseVisualStyleBackColor = true;
            // 
            // radioButtonOne
            // 
            this.radioButtonOne.AutoSize = true;
            this.radioButtonOne.Checked = true;
            this.radioButtonOne.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonOne.Location = new System.Drawing.Point(11, 41);
            this.radioButtonOne.Name = "radioButtonOne";
            this.radioButtonOne.Size = new System.Drawing.Size(79, 21);
            this.radioButtonOne.TabIndex = 3;
            this.radioButtonOne.TabStop = true;
            this.radioButtonOne.Text = "One time";
            this.radioButtonOne.UseVisualStyleBackColor = true;
            // 
            // radioButtonDay
            // 
            this.radioButtonDay.AutoSize = true;
            this.radioButtonDay.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonDay.Location = new System.Drawing.Point(11, 67);
            this.radioButtonDay.Name = "radioButtonDay";
            this.radioButtonDay.Size = new System.Drawing.Size(54, 21);
            this.radioButtonDay.TabIndex = 4;
            this.radioButtonDay.Text = "Daily";
            this.radioButtonDay.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dtPickerStartTime);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.customPanel1);
            this.panel4.Controls.Add(this.dtPickerStartDate);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(269, 75);
            this.panel4.TabIndex = 9;
            // 
            // dtPickerStartTime
            // 
            this.dtPickerStartTime.CalendarFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerStartTime.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtPickerStartTime.Location = new System.Drawing.Point(166, 32);
            this.dtPickerStartTime.Name = "dtPickerStartTime";
            this.dtPickerStartTime.ShowUpDown = true;
            this.dtPickerStartTime.Size = new System.Drawing.Size(80, 25);
            this.dtPickerStartTime.TabIndex = 8;
            this.dtPickerStartTime.Value = new System.DateTime(2014, 4, 11, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Start:";
            // 
            // dtPickerStartDate
            // 
            this.dtPickerStartDate.CalendarFont = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerStartDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPickerStartDate.Location = new System.Drawing.Point(52, 34);
            this.dtPickerStartDate.Name = "dtPickerStartDate";
            this.dtPickerStartDate.Size = new System.Drawing.Size(105, 25);
            this.dtPickerStartDate.TabIndex = 7;
            // 
            // btnBackup
            // 
            this.btnBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackup.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackup.Location = new System.Drawing.Point(137, 388);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(130, 26);
            this.btnBackup.TabIndex = 0;
            this.btnBackup.Text = "Run backup now";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // toolTipFileDrop
            // 
            this.toolTipFileDrop.ToolTipTitle = "Drag and drop";
            // 
            // backupFolderBindingSource
            // 
            this.backupFolderBindingSource.AllowNew = true;
            this.backupFolderBindingSource.DataSource = typeof(BackupService.Settings.BackupFolder);
            // 
            // customPanel3
            // 
            this.customPanel3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.customPanel3.BorderColor = System.Drawing.Color.Silver;
            this.customPanel3.BorderWidth = 2;
            this.customPanel3.Controls.Add(this.label7);
            this.customPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.customPanel3.Location = new System.Drawing.Point(0, 0);
            this.customPanel3.Name = "customPanel3";
            this.customPanel3.Size = new System.Drawing.Size(269, 27);
            this.customPanel3.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(80)))), ((int)(((byte)(134)))));
            this.label7.Location = new System.Drawing.Point(10, 5);
            this.label7.Margin = new System.Windows.Forms.Padding(5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 17);
            this.label7.TabIndex = 1;
            this.label7.Text = "Backup destination";
            // 
            // customPanel2
            // 
            this.customPanel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.customPanel2.BorderColor = System.Drawing.Color.Silver;
            this.customPanel2.BorderWidth = 2;
            this.customPanel2.Controls.Add(this.label6);
            this.customPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customPanel2.Location = new System.Drawing.Point(0, 0);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.Size = new System.Drawing.Size(269, 27);
            this.customPanel2.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(80)))), ((int)(((byte)(134)))));
            this.label6.Location = new System.Drawing.Point(10, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Recurrence";
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.customPanel1.BorderColor = System.Drawing.Color.Silver;
            this.customPanel1.BorderWidth = 2;
            this.customPanel1.Controls.Add(this.label5);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(269, 27);
            this.customPanel1.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(80)))), ((int)(((byte)(134)))));
            this.label5.Location = new System.Drawing.Point(10, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "When to run";
            // 
            // fullPathDataGridViewTextBoxColumn
            // 
            this.fullPathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fullPathDataGridViewTextBoxColumn.DataPropertyName = "FullPath";
            this.fullPathDataGridViewTextBoxColumn.HeaderText = "Path";
            this.fullPathDataGridViewTextBoxColumn.Name = "fullPathDataGridViewTextBoxColumn";
            this.fullPathDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // subFoldersDataGridViewTextBoxColumn
            // 
            this.subFoldersDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.subFoldersDataGridViewTextBoxColumn.DataPropertyName = "SubFolders";
            this.subFoldersDataGridViewTextBoxColumn.HeaderText = "SubFolders";
            this.subFoldersDataGridViewTextBoxColumn.Name = "subFoldersDataGridViewTextBoxColumn";
            this.subFoldersDataGridViewTextBoxColumn.ReadOnly = true;
            this.subFoldersDataGridViewTextBoxColumn.Width = 98;
            // 
            // filesDataGridViewTextBoxColumn
            // 
            this.filesDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.filesDataGridViewTextBoxColumn.DataPropertyName = "Files";
            this.filesDataGridViewTextBoxColumn.HeaderText = "Files";
            this.filesDataGridViewTextBoxColumn.Name = "filesDataGridViewTextBoxColumn";
            this.filesDataGridViewTextBoxColumn.ReadOnly = true;
            this.filesDataGridViewTextBoxColumn.Width = 58;
            // 
            // sizeDataGridViewTextBoxColumn
            // 
            this.sizeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sizeDataGridViewTextBoxColumn.DataPropertyName = "Size";
            this.sizeDataGridViewTextBoxColumn.HeaderText = "Size";
            this.sizeDataGridViewTextBoxColumn.Name = "sizeDataGridViewTextBoxColumn";
            this.sizeDataGridViewTextBoxColumn.ReadOnly = true;
            this.sizeDataGridViewTextBoxColumn.Width = 56;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 612);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(750, 650);
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Backup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSettings_FormClosing);
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBackupFolders)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backupFolderBindingSource)).EndInit();
            this.customPanel3.ResumeLayout(false);
            this.customPanel3.PerformLayout();
            this.customPanel2.ResumeLayout(false);
            this.customPanel2.PerformLayout();
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton radioButtonMonth;
        private System.Windows.Forms.RadioButton radioButtonWeek;
        private System.Windows.Forms.RadioButton radioButtonOne;
        private System.Windows.Forms.RadioButton radioButtonDay;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DateTimePicker dtPickerStartTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtPickerStartDate;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSetDestination;
        private BackupService.UserControls.CustomPanel customPanel1;
        private BackupService.UserControls.CustomPanel customPanel3;
        private System.Windows.Forms.Label label7;
        private BackupService.UserControls.CustomPanel customPanel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDestinationFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolTip toolTipFileDrop;
        private System.Windows.Forms.Button btnLoadSettings;
        private System.Windows.Forms.Button btnSetPassword;
        private System.Windows.Forms.DataGridView dataGridViewBackupFolders;
        private System.Windows.Forms.BindingSource backupFolderBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullPathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subFoldersDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeDataGridViewTextBoxColumn;
    }
}