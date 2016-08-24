namespace SearchForDuplicates
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.lstDuplicates = new System.Windows.Forms.ListBox();
            this.lstFileLocations = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDuplicateCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClearDirectories = new System.Windows.Forms.Button();
            this.btnRemoveDirectory = new System.Windows.Forms.Button();
            this.lstDirectories = new System.Windows.Forms.ListBox();
            this.grpCompareOn = new System.Windows.Forms.GroupBox();
            this.chkFilenameFilter = new System.Windows.Forms.CheckBox();
            this.txtFilenameFilter = new System.Windows.Forms.TextBox();
            this.radioFileContent = new System.Windows.Forms.RadioButton();
            this.radioFileName = new System.Windows.Forms.RadioButton();
            this.btnBrowseDirectory = new System.Windows.Forms.Button();
            this.btnChecksumUtil = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnClear = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.grpSearchProgress = new System.Windows.Forms.GroupBox();
            this.lblkBytesRead = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblFilesProcessed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAbout = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAllwaysOnTop = new System.Windows.Forms.CheckBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.bwLoadFileHashTable = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.grpCompareOn.SuspendLayout();
            this.grpSearchProgress.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstDuplicates
            // 
            this.lstDuplicates.FormattingEnabled = true;
            this.lstDuplicates.HorizontalScrollbar = true;
            this.lstDuplicates.Location = new System.Drawing.Point(12, 32);
            this.lstDuplicates.Name = "lstDuplicates";
            this.lstDuplicates.Size = new System.Drawing.Size(309, 238);
            this.lstDuplicates.Sorted = true;
            this.lstDuplicates.TabIndex = 1;
            this.lstDuplicates.SelectedIndexChanged += new System.EventHandler(this.lstDuplicates_SelectedIndexChanged);
            // 
            // lstFileLocations
            // 
            this.lstFileLocations.FormattingEnabled = true;
            this.lstFileLocations.HorizontalScrollbar = true;
            this.lstFileLocations.Location = new System.Drawing.Point(12, 287);
            this.lstFileLocations.Name = "lstFileLocations";
            this.lstFileLocations.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFileLocations.Size = new System.Drawing.Size(309, 108);
            this.lstFileLocations.Sorted = true;
            this.lstFileLocations.TabIndex = 2;
            this.lstFileLocations.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstFileLocations_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDuplicateCount);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lstDuplicates);
            this.groupBox1.Controls.Add(this.lstFileLocations);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 406);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Duplicates found";
            // 
            // lblDuplicateCount
            // 
            this.lblDuplicateCount.AutoSize = true;
            this.lblDuplicateCount.Location = new System.Drawing.Point(260, 16);
            this.lblDuplicateCount.Name = "lblDuplicateCount";
            this.lblDuplicateCount.Size = new System.Drawing.Size(0, 13);
            this.lblDuplicateCount.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 271);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "File locations";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Duplicates";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(615, 389);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.label7);
            this.grpSettings.Controls.Add(this.btnClearDirectories);
            this.grpSettings.Controls.Add(this.btnRemoveDirectory);
            this.grpSettings.Controls.Add(this.lstDirectories);
            this.grpSettings.Controls.Add(this.grpCompareOn);
            this.grpSettings.Controls.Add(this.btnBrowseDirectory);
            this.grpSettings.Location = new System.Drawing.Point(352, 8);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(333, 202);
            this.grpSettings.TabIndex = 7;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Search directories";
            // 
            // btnClearDirectories
            // 
            this.btnClearDirectories.Location = new System.Drawing.Point(272, 84);
            this.btnClearDirectories.Name = "btnClearDirectories";
            this.btnClearDirectories.Size = new System.Drawing.Size(54, 20);
            this.btnClearDirectories.TabIndex = 12;
            this.btnClearDirectories.Text = "Clear";
            this.btnClearDirectories.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClearDirectories.UseVisualStyleBackColor = true;
            this.btnClearDirectories.Click += new System.EventHandler(this.btnClearDirectories_Click);
            // 
            // btnRemoveDirectory
            // 
            this.btnRemoveDirectory.Location = new System.Drawing.Point(272, 58);
            this.btnRemoveDirectory.Name = "btnRemoveDirectory";
            this.btnRemoveDirectory.Size = new System.Drawing.Size(54, 20);
            this.btnRemoveDirectory.TabIndex = 11;
            this.btnRemoveDirectory.Text = "Delete";
            this.btnRemoveDirectory.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRemoveDirectory.UseVisualStyleBackColor = true;
            this.btnRemoveDirectory.Click += new System.EventHandler(this.btnRemoveDirectory_Click);
            // 
            // lstDirectories
            // 
            this.lstDirectories.AllowDrop = true;
            this.lstDirectories.FormattingEnabled = true;
            this.lstDirectories.HorizontalScrollbar = true;
            this.lstDirectories.Location = new System.Drawing.Point(9, 32);
            this.lstDirectories.Name = "lstDirectories";
            this.lstDirectories.Size = new System.Drawing.Size(257, 95);
            this.lstDirectories.TabIndex = 10;
            this.toolTipMain.SetToolTip(this.lstDirectories, "Drag and drop directories here or use the browse button.");
            // 
            // grpCompareOn
            // 
            this.grpCompareOn.Controls.Add(this.chkFilenameFilter);
            this.grpCompareOn.Controls.Add(this.txtFilenameFilter);
            this.grpCompareOn.Controls.Add(this.radioFileContent);
            this.grpCompareOn.Controls.Add(this.radioFileName);
            this.grpCompareOn.Location = new System.Drawing.Point(9, 134);
            this.grpCompareOn.Name = "grpCompareOn";
            this.grpCompareOn.Size = new System.Drawing.Size(317, 62);
            this.grpCompareOn.TabIndex = 9;
            this.grpCompareOn.TabStop = false;
            this.grpCompareOn.Text = "Comparison method";
            // 
            // chkFilenameFilter
            // 
            this.chkFilenameFilter.AutoSize = true;
            this.chkFilenameFilter.Location = new System.Drawing.Point(216, 19);
            this.chkFilenameFilter.Name = "chkFilenameFilter";
            this.chkFilenameFilter.Size = new System.Drawing.Size(96, 17);
            this.chkFilenameFilter.TabIndex = 17;
            this.chkFilenameFilter.Text = "File format filter";
            this.chkFilenameFilter.UseVisualStyleBackColor = true;
            this.chkFilenameFilter.CheckedChanged += new System.EventHandler(this.chkFilenameFilter_CheckedChanged);
            // 
            // txtFilenameFilter
            // 
            this.txtFilenameFilter.Enabled = false;
            this.txtFilenameFilter.Location = new System.Drawing.Point(216, 39);
            this.txtFilenameFilter.MaxLength = 1024;
            this.txtFilenameFilter.Name = "txtFilenameFilter";
            this.txtFilenameFilter.Size = new System.Drawing.Size(89, 20);
            this.txtFilenameFilter.TabIndex = 16;
            this.toolTipMain.SetToolTip(this.txtFilenameFilter, "Example mp3;jpg;gif");
            // 
            // radioFileContent
            // 
            this.radioFileContent.AutoSize = true;
            this.radioFileContent.Checked = true;
            this.radioFileContent.Location = new System.Drawing.Point(15, 18);
            this.radioFileContent.Name = "radioFileContent";
            this.radioFileContent.Size = new System.Drawing.Size(80, 17);
            this.radioFileContent.TabIndex = 15;
            this.radioFileContent.TabStop = true;
            this.radioFileContent.Text = "File content";
            this.radioFileContent.UseVisualStyleBackColor = true;
            // 
            // radioFileName
            // 
            this.radioFileName.AutoSize = true;
            this.radioFileName.Location = new System.Drawing.Point(101, 18);
            this.radioFileName.Name = "radioFileName";
            this.radioFileName.Size = new System.Drawing.Size(70, 17);
            this.radioFileName.TabIndex = 13;
            this.radioFileName.Text = "File name";
            this.radioFileName.UseVisualStyleBackColor = true;
            // 
            // btnBrowseDirectory
            // 
            this.btnBrowseDirectory.Location = new System.Drawing.Point(272, 32);
            this.btnBrowseDirectory.Name = "btnBrowseDirectory";
            this.btnBrowseDirectory.Size = new System.Drawing.Size(54, 20);
            this.btnBrowseDirectory.TabIndex = 9;
            this.btnBrowseDirectory.Text = "Browse";
            this.btnBrowseDirectory.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBrowseDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseDirectory.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnChecksumUtil
            // 
            this.btnChecksumUtil.Location = new System.Drawing.Point(352, 389);
            this.btnChecksumUtil.Name = "btnChecksumUtil";
            this.btnChecksumUtil.Size = new System.Drawing.Size(103, 25);
            this.btnChecksumUtil.TabIndex = 8;
            this.btnChecksumUtil.Text = "Open chksum util";
            this.btnChecksumUtil.UseVisualStyleBackColor = true;
            this.btnChecksumUtil.Click += new System.EventHandler(this.btnChecksumUtil_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(457, 389);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(7, 67);
            this.progressBar.Name = "progressBar";
            this.progressBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressBar.Size = new System.Drawing.Size(320, 13);
            this.progressBar.TabIndex = 10;
            // 
            // grpSearchProgress
            // 
            this.grpSearchProgress.Controls.Add(this.lblkBytesRead);
            this.grpSearchProgress.Controls.Add(this.label6);
            this.grpSearchProgress.Controls.Add(this.lblFilesProcessed);
            this.grpSearchProgress.Controls.Add(this.label1);
            this.grpSearchProgress.Controls.Add(this.progressBar);
            this.grpSearchProgress.Location = new System.Drawing.Point(352, 220);
            this.grpSearchProgress.Name = "grpSearchProgress";
            this.grpSearchProgress.Size = new System.Drawing.Size(333, 86);
            this.grpSearchProgress.TabIndex = 11;
            this.grpSearchProgress.TabStop = false;
            this.grpSearchProgress.Text = "Search Progress";
            // 
            // lblkBytesRead
            // 
            this.lblkBytesRead.AutoSize = true;
            this.lblkBytesRead.Location = new System.Drawing.Point(90, 41);
            this.lblkBytesRead.Name = "lblkBytesRead";
            this.lblkBytesRead.Size = new System.Drawing.Size(13, 13);
            this.lblkBytesRead.TabIndex = 14;
            this.lblkBytesRead.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "kBytes read:";
            // 
            // lblFilesProcessed
            // 
            this.lblFilesProcessed.AutoSize = true;
            this.lblFilesProcessed.Location = new System.Drawing.Point(90, 18);
            this.lblFilesProcessed.Name = "lblFilesProcessed";
            this.lblFilesProcessed.Size = new System.Drawing.Size(13, 13);
            this.lblFilesProcessed.TabIndex = 12;
            this.lblFilesProcessed.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Files processed:";
            // 
            // toolTipMain
            // 
            this.toolTipMain.AutoPopDelay = 7500;
            this.toolTipMain.InitialDelay = 500;
            this.toolTipMain.ReshowDelay = 100;
            this.toolTipMain.ShowAlways = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.lblAbout});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(694, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 12;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // lblAbout
            // 
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblAbout.Size = new System.Drawing.Size(679, 17);
            this.lblAbout.Spring = true;
            this.lblAbout.Text = "Made by Martin Dahl 2014";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(534, 389);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 25);
            this.btnStop.TabIndex = 13;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkAllwaysOnTop);
            this.groupBox2.Controls.Add(this.btnAbout);
            this.groupBox2.Location = new System.Drawing.Point(352, 312);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 71);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Misc";
            // 
            // chkAllwaysOnTop
            // 
            this.chkAllwaysOnTop.AutoSize = true;
            this.chkAllwaysOnTop.Location = new System.Drawing.Point(227, 17);
            this.chkAllwaysOnTop.Name = "chkAllwaysOnTop";
            this.chkAllwaysOnTop.Size = new System.Drawing.Size(94, 17);
            this.chkAllwaysOnTop.TabIndex = 19;
            this.chkAllwaysOnTop.Text = "Allways on top";
            this.chkAllwaysOnTop.UseVisualStyleBackColor = true;
            this.chkAllwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAllwaysOnTop_CheckedChanged);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(227, 40);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(94, 25);
            this.btnAbout.TabIndex = 9;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // bwLoadFileHashTable
            // 
            this.bwLoadFileHashTable.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwLoadFileHashTable_DoWork);
            this.bwLoadFileHashTable.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwLoadFileHashTable_RunWorkerCompleted);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 442);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.grpSearchProgress);
            this.Controls.Add(this.btnChecksumUtil);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search for duplicates";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Disposed += new System.EventHandler(this.frmMain_Disposed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpCompareOn.ResumeLayout(false);
            this.grpCompareOn.PerformLayout();
            this.grpSearchProgress.ResumeLayout(false);
            this.grpSearchProgress.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstDuplicates;
        private System.Windows.Forms.ListBox lstFileLocations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.Button btnBrowseDirectory;
        private System.Windows.Forms.Button btnChecksumUtil;
        private System.Windows.Forms.GroupBox grpCompareOn;
        private System.Windows.Forms.RadioButton radioFileContent;
        private System.Windows.Forms.RadioButton radioFileName;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblDuplicateCount;
        private System.Windows.Forms.GroupBox grpSearchProgress;
        private System.Windows.Forms.Label lblkBytesRead;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblFilesProcessed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.ListBox lstDirectories;
        private System.Windows.Forms.Button btnClearDirectories;
        private System.Windows.Forms.Button btnRemoveDirectory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFilenameFilter;
        private System.Windows.Forms.CheckBox chkFilenameFilter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblAbout;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.CheckBox chkAllwaysOnTop;
        private System.ComponentModel.BackgroundWorker bwLoadFileHashTable;
    }
}