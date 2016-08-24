namespace DeleteDuplicateFiles
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpBoxAppLogSettings = new System.Windows.Forms.GroupBox();
            this.lblLogFileDiskUsage = new System.Windows.Forms.Label();
            this.lblNumberOfLogFiles = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClearAllLogFiles = new System.Windows.Forms.Button();
            this.btnOpenDataFolderInExplorer = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnOptimizeDb = new System.Windows.Forms.Button();
            this.chkIgnoreHiddenFilesAndDirs = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericMaximumNoOfHashThreads = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioPermanetDelete = new System.Windows.Forms.RadioButton();
            this.radioRecycleBin = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioMD5 = new System.Windows.Forms.RadioButton();
            this.radioCRC32 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioOldestDate = new System.Windows.Forms.RadioButton();
            this.radioNewestDate = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ThreadsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkIgnoreSystemFiles = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.grpBoxAppLogSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericMaximumNoOfHashThreads)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(286, 298);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 28);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(367, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkIgnoreSystemFiles);
            this.groupBox1.Controls.Add(this.grpBoxAppLogSettings);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnOptimizeDb);
            this.groupBox1.Controls.Add(this.chkIgnoreHiddenFilesAndDirs);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numericMaximumNoOfHashThreads);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 274);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Program settings";
            // 
            // grpBoxAppLogSettings
            // 
            this.grpBoxAppLogSettings.Controls.Add(this.lblLogFileDiskUsage);
            this.grpBoxAppLogSettings.Controls.Add(this.lblNumberOfLogFiles);
            this.grpBoxAppLogSettings.Controls.Add(this.label8);
            this.grpBoxAppLogSettings.Controls.Add(this.label7);
            this.grpBoxAppLogSettings.Controls.Add(this.btnClearAllLogFiles);
            this.grpBoxAppLogSettings.Controls.Add(this.btnOpenDataFolderInExplorer);
            this.grpBoxAppLogSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxAppLogSettings.Location = new System.Drawing.Point(6, 185);
            this.grpBoxAppLogSettings.Name = "grpBoxAppLogSettings";
            this.grpBoxAppLogSettings.Size = new System.Drawing.Size(422, 79);
            this.grpBoxAppLogSettings.TabIndex = 17;
            this.grpBoxAppLogSettings.TabStop = false;
            this.grpBoxAppLogSettings.Text = "Application Log Files";
            // 
            // lblLogFileDiskUsage
            // 
            this.lblLogFileDiskUsage.AutoSize = true;
            this.lblLogFileDiskUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogFileDiskUsage.Location = new System.Drawing.Point(119, 44);
            this.lblLogFileDiskUsage.Name = "lblLogFileDiskUsage";
            this.lblLogFileDiskUsage.Size = new System.Drawing.Size(29, 13);
            this.lblLogFileDiskUsage.TabIndex = 21;
            this.lblLogFileDiskUsage.Text = "0 Kb";
            // 
            // lblNumberOfLogFiles
            // 
            this.lblNumberOfLogFiles.AutoSize = true;
            this.lblNumberOfLogFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumberOfLogFiles.Location = new System.Drawing.Point(119, 25);
            this.lblNumberOfLogFiles.Name = "lblNumberOfLogFiles";
            this.lblNumberOfLogFiles.Size = new System.Drawing.Size(13, 13);
            this.lblNumberOfLogFiles.TabIndex = 20;
            this.lblNumberOfLogFiles.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 44);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Disk Space Usage:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 25);
            this.label7.Margin = new System.Windows.Forms.Padding(3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Number of Log Files:";
            // 
            // btnClearAllLogFiles
            // 
            this.btnClearAllLogFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearAllLogFiles.Location = new System.Drawing.Point(295, 46);
            this.btnClearAllLogFiles.Name = "btnClearAllLogFiles";
            this.btnClearAllLogFiles.Size = new System.Drawing.Size(121, 25);
            this.btnClearAllLogFiles.TabIndex = 17;
            this.btnClearAllLogFiles.Text = "Remove All Log Files";
            this.btnClearAllLogFiles.UseVisualStyleBackColor = true;
            this.btnClearAllLogFiles.Click += new System.EventHandler(this.btnClearAllLogFiles_Click);
            // 
            // btnOpenDataFolderInExplorer
            // 
            this.btnOpenDataFolderInExplorer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDataFolderInExplorer.Location = new System.Drawing.Point(295, 15);
            this.btnOpenDataFolderInExplorer.Name = "btnOpenDataFolderInExplorer";
            this.btnOpenDataFolderInExplorer.Size = new System.Drawing.Size(121, 25);
            this.btnOpenDataFolderInExplorer.TabIndex = 16;
            this.btnOpenDataFolderInExplorer.Text = "Open File Location";
            this.btnOpenDataFolderInExplorer.UseVisualStyleBackColor = true;
            this.btnOpenDataFolderInExplorer.Click += new System.EventHandler(this.btnOpenDataFolderInExplorer_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(15, 160);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(245, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Remove deleted files from hash database:";
            // 
            // btnOptimizeDb
            // 
            this.btnOptimizeDb.Location = new System.Drawing.Point(277, 154);
            this.btnOptimizeDb.Name = "btnOptimizeDb";
            this.btnOptimizeDb.Size = new System.Drawing.Size(75, 25);
            this.btnOptimizeDb.TabIndex = 15;
            this.btnOptimizeDb.Text = "Run";
            this.btnOptimizeDb.UseVisualStyleBackColor = true;
            this.btnOptimizeDb.Click += new System.EventHandler(this.btnOptimizeDb_Click);
            // 
            // chkIgnoreHiddenFilesAndDirs
            // 
            this.chkIgnoreHiddenFilesAndDirs.AutoSize = true;
            this.chkIgnoreHiddenFilesAndDirs.Location = new System.Drawing.Point(143, 83);
            this.chkIgnoreHiddenFilesAndDirs.Name = "chkIgnoreHiddenFilesAndDirs";
            this.chkIgnoreHiddenFilesAndDirs.Size = new System.Drawing.Size(184, 17);
            this.chkIgnoreHiddenFilesAndDirs.TabIndex = 14;
            this.chkIgnoreHiddenFilesAndDirs.Text = "Ignore hidden files and directories";
            this.chkIgnoreHiddenFilesAndDirs.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(15, 84);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Search filter:";
            // 
            // numericMaximumNoOfHashThreads
            // 
            this.numericMaximumNoOfHashThreads.Location = new System.Drawing.Point(301, 128);
            this.numericMaximumNoOfHashThreads.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericMaximumNoOfHashThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericMaximumNoOfHashThreads.Name = "numericMaximumNoOfHashThreads";
            this.numericMaximumNoOfHashThreads.Size = new System.Drawing.Size(42, 20);
            this.numericMaximumNoOfHashThreads.TabIndex = 12;
            this.ThreadsToolTip.SetToolTip(this.numericMaximumNoOfHashThreads, "The optimal value is usually the same as the number of logical processors\r\nyour C" +
        "PU has available but disk performance also must be taken into consideration.");
            this.numericMaximumNoOfHashThreads.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 130);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(280, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Maximum number of simultaneous  hash threads:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radioPermanetDelete);
            this.panel3.Controls.Add(this.radioRecycleBin);
            this.panel3.Location = new System.Drawing.Point(143, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(285, 18);
            this.panel3.TabIndex = 10;
            // 
            // radioPermanetDelete
            // 
            this.radioPermanetDelete.AutoSize = true;
            this.radioPermanetDelete.Checked = true;
            this.radioPermanetDelete.Location = new System.Drawing.Point(0, 0);
            this.radioPermanetDelete.Name = "radioPermanetDelete";
            this.radioPermanetDelete.Size = new System.Drawing.Size(108, 17);
            this.radioPermanetDelete.TabIndex = 7;
            this.radioPermanetDelete.TabStop = true;
            this.radioPermanetDelete.Text = "Permanent delete";
            this.radioPermanetDelete.UseVisualStyleBackColor = true;
            // 
            // radioRecycleBin
            // 
            this.radioRecycleBin.AutoSize = true;
            this.radioRecycleBin.Location = new System.Drawing.Point(134, 0);
            this.radioRecycleBin.Name = "radioRecycleBin";
            this.radioRecycleBin.Size = new System.Drawing.Size(81, 17);
            this.radioRecycleBin.TabIndex = 8;
            this.radioRecycleBin.Text = "Recycle bin";
            this.radioRecycleBin.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioMD5);
            this.panel2.Controls.Add(this.radioCRC32);
            this.panel2.Location = new System.Drawing.Point(143, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(285, 18);
            this.panel2.TabIndex = 10;
            // 
            // radioMD5
            // 
            this.radioMD5.AutoSize = true;
            this.radioMD5.Checked = true;
            this.radioMD5.Location = new System.Drawing.Point(0, 0);
            this.radioMD5.Name = "radioMD5";
            this.radioMD5.Size = new System.Drawing.Size(48, 17);
            this.radioMD5.TabIndex = 0;
            this.radioMD5.TabStop = true;
            this.radioMD5.Text = "MD5";
            this.radioMD5.UseVisualStyleBackColor = true;
            // 
            // radioCRC32
            // 
            this.radioCRC32.AutoSize = true;
            this.radioCRC32.Location = new System.Drawing.Point(134, 0);
            this.radioCRC32.Name = "radioCRC32";
            this.radioCRC32.Size = new System.Drawing.Size(59, 17);
            this.radioCRC32.TabIndex = 1;
            this.radioCRC32.Text = "CRC32";
            this.radioCRC32.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioOldestDate);
            this.panel1.Controls.Add(this.radioNewestDate);
            this.panel1.Location = new System.Drawing.Point(143, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 18);
            this.panel1.TabIndex = 9;
            // 
            // radioOldestDate
            // 
            this.radioOldestDate.AutoSize = true;
            this.radioOldestDate.Checked = true;
            this.radioOldestDate.Location = new System.Drawing.Point(0, 0);
            this.radioOldestDate.Name = "radioOldestDate";
            this.radioOldestDate.Size = new System.Drawing.Size(121, 17);
            this.radioOldestDate.TabIndex = 4;
            this.radioOldestDate.TabStop = true;
            this.radioOldestDate.Text = "Oldest modified date";
            this.radioOldestDate.UseVisualStyleBackColor = true;
            // 
            // radioNewestDate
            // 
            this.radioNewestDate.AutoSize = true;
            this.radioNewestDate.Location = new System.Drawing.Point(134, 0);
            this.radioNewestDate.Name = "radioNewestDate";
            this.radioNewestDate.Size = new System.Drawing.Size(127, 17);
            this.radioNewestDate.TabIndex = 5;
            this.radioNewestDate.Text = "Newest modified date";
            this.radioNewestDate.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Deletion method:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Base master file on:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Hash Algorithm:";
            // 
            // chkIgnoreSystemFiles
            // 
            this.chkIgnoreSystemFiles.AutoSize = true;
            this.chkIgnoreSystemFiles.Location = new System.Drawing.Point(143, 105);
            this.chkIgnoreSystemFiles.Name = "chkIgnoreSystemFiles";
            this.chkIgnoreSystemFiles.Size = new System.Drawing.Size(112, 17);
            this.chkIgnoreSystemFiles.TabIndex = 18;
            this.chkIgnoreSystemFiles.Text = "Ignore system files";
            this.chkIgnoreSystemFiles.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 331);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpBoxAppLogSettings.ResumeLayout(false);
            this.grpBoxAppLogSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericMaximumNoOfHashThreads)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioNewestDate;
        private System.Windows.Forms.RadioButton radioOldestDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioCRC32;
        private System.Windows.Forms.RadioButton radioMD5;
        private System.Windows.Forms.RadioButton radioRecycleBin;
        private System.Windows.Forms.RadioButton radioPermanetDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numericMaximumNoOfHashThreads;
        private System.Windows.Forms.ToolTip ThreadsToolTip;
        private System.Windows.Forms.CheckBox chkIgnoreHiddenFilesAndDirs;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnOptimizeDb;
        private System.Windows.Forms.GroupBox grpBoxAppLogSettings;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClearAllLogFiles;
        private System.Windows.Forms.Button btnOpenDataFolderInExplorer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLogFileDiskUsage;
        private System.Windows.Forms.Label lblNumberOfLogFiles;
        private System.Windows.Forms.CheckBox chkIgnoreSystemFiles;
    }
}