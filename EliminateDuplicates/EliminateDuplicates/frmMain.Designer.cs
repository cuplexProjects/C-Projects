namespace DeleteDuplicateFiles
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.lblSearchProfileName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.lbScanFolders = new System.Windows.Forms.ListBox();
            this.txtFilenameFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkIncludeSubfolders = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbResults = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSearchStatus = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblSearchResults = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblLastWriteTimeInfo = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblCreationTimeInfo = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblFileNameInfo = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblFileSizeInfo = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDirectoryInfo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbDuplicateFiles = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtMasterFilename = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pbFileSearch = new System.Windows.Forms.ProgressBar();
            this.btnDeleteFiles = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.openProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLastProfileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchForDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchDirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPreferredDirPrioListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSearch = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.lblFileHashInfo = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblFileHashesRunning = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnMoveDown);
            this.groupBox1.Controls.Add(this.btnMoveUp);
            this.groupBox1.Controls.Add(this.lbScanFolders);
            this.groupBox1.Controls.Add(this.txtFilenameFilter);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkIncludeSubfolders);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 39);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(679, 228);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Options";
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(604, 79);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(67, 27);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.label9);
            this.flowLayoutPanel1.Controls.Add(this.lblSearchProfileName);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(267, 11);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(324, 30);
            this.flowLayoutPanel1.TabIndex = 16;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 5);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "SearchProfileName:";
            // 
            // lblSearchProfileName
            // 
            this.lblSearchProfileName.AutoSize = true;
            this.lblSearchProfileName.Location = new System.Drawing.Point(150, 5);
            this.lblSearchProfileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchProfileName.Name = "lblSearchProfileName";
            this.lblSearchProfileName.Size = new System.Drawing.Size(31, 17);
            this.lblSearchProfileName.TabIndex = 16;
            this.lblSearchProfileName.Text = "N/A";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(604, 44);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(67, 27);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDown.Location = new System.Drawing.Point(604, 150);
            this.btnMoveDown.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(67, 27);
            this.btnMoveDown.TabIndex = 13;
            this.btnMoveDown.Text = "Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUp.Location = new System.Drawing.Point(604, 116);
            this.btnMoveUp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(67, 27);
            this.btnMoveUp.TabIndex = 12;
            this.btnMoveUp.Text = "Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // lbScanFolders
            // 
            this.lbScanFolders.AllowDrop = true;
            this.lbScanFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbScanFolders.FormattingEnabled = true;
            this.lbScanFolders.ItemHeight = 16;
            this.lbScanFolders.Location = new System.Drawing.Point(8, 44);
            this.lbScanFolders.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbScanFolders.Name = "lbScanFolders";
            this.lbScanFolders.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbScanFolders.Size = new System.Drawing.Size(582, 132);
            this.lbScanFolders.TabIndex = 8;
            this.lbScanFolders.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbScanFolders_DragDrop);
            this.lbScanFolders.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbScanFolders_DragEnter);
            // 
            // txtFilenameFilter
            // 
            this.txtFilenameFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilenameFilter.Location = new System.Drawing.Point(185, 185);
            this.txtFilenameFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilenameFilter.Name = "txtFilenameFilter";
            this.txtFilenameFilter.Size = new System.Drawing.Size(404, 22);
            this.txtFilenameFilter.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 188);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Filter filename expression:";
            // 
            // chkIncludeSubfolders
            // 
            this.chkIncludeSubfolders.AutoSize = true;
            this.chkIncludeSubfolders.Checked = true;
            this.chkIncludeSubfolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeSubfolders.Location = new System.Drawing.Point(96, 22);
            this.chkIncludeSubfolders.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkIncludeSubfolders.Name = "chkIncludeSubfolders";
            this.chkIncludeSubfolders.Size = new System.Drawing.Size(145, 21);
            this.chkIncludeSubfolders.TabIndex = 3;
            this.chkIncludeSubfolders.Text = "Include subfolders";
            this.chkIncludeSubfolders.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Directories:";
            // 
            // lbResults
            // 
            this.lbResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbResults.FormattingEnabled = true;
            this.lbResults.ItemHeight = 16;
            this.lbResults.Location = new System.Drawing.Point(0, 0);
            this.lbResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbResults.Name = "lbResults";
            this.lbResults.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbResults.Size = new System.Drawing.Size(324, 397);
            this.lbResults.TabIndex = 1;
            this.lbResults.SelectedIndexChanged += new System.EventHandler(this.lbResults_SelectedIndexChanged);
            this.lbResults.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbResults_KeyUp);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblSearchStatus);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lblSearchResults);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.splitContainer1);
            this.groupBox2.Controls.Add(this.pbFileSearch);
            this.groupBox2.Location = new System.Drawing.Point(3, 267);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(679, 485);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results";
            // 
            // lblSearchStatus
            // 
            this.lblSearchStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSearchStatus.AutoSize = true;
            this.lblSearchStatus.Location = new System.Drawing.Point(413, 428);
            this.lblSearchStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblSearchStatus.Name = "lblSearchStatus";
            this.lblSearchStatus.Size = new System.Drawing.Size(88, 17);
            this.lblSearchStatus.TabIndex = 12;
            this.lblSearchStatus.Text = "Scaning files";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(359, 428);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Status:";
            // 
            // lblSearchResults
            // 
            this.lblSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSearchResults.AutoSize = true;
            this.lblSearchResults.Location = new System.Drawing.Point(113, 429);
            this.lblSearchResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblSearchResults.Name = "lblSearchResults";
            this.lblSearchResults.Size = new System.Drawing.Size(16, 17);
            this.lblSearchResults.TabIndex = 10;
            this.lblSearchResults.Text = "0";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 429);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Search results:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 23);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbResults);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(654, 397);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 8;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblLastWriteTimeInfo);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.lblCreationTimeInfo);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.lblFileNameInfo);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.lblFileSizeInfo);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.lblDirectoryInfo);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox5.Location = new System.Drawing.Point(0, 258);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(325, 139);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "File Info";
            // 
            // lblLastWriteTimeInfo
            // 
            this.lblLastWriteTimeInfo.AutoSize = true;
            this.lblLastWriteTimeInfo.Location = new System.Drawing.Point(128, 113);
            this.lblLastWriteTimeInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblLastWriteTimeInfo.Name = "lblLastWriteTimeInfo";
            this.lblLastWriteTimeInfo.Size = new System.Drawing.Size(142, 17);
            this.lblLastWriteTimeInfo.TabIndex = 12;
            this.lblLastWriteTimeInfo.Text = "2014-01-01 00:00:00";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 113);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 17);
            this.label11.TabIndex = 11;
            this.label11.Text = "Last Write Time:";
            // 
            // lblCreationTimeInfo
            // 
            this.lblCreationTimeInfo.AutoSize = true;
            this.lblCreationTimeInfo.Location = new System.Drawing.Point(128, 90);
            this.lblCreationTimeInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblCreationTimeInfo.Name = "lblCreationTimeInfo";
            this.lblCreationTimeInfo.Size = new System.Drawing.Size(142, 17);
            this.lblCreationTimeInfo.TabIndex = 10;
            this.lblCreationTimeInfo.Text = "2014-01-01 00:00:00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 90);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 17);
            this.label10.TabIndex = 9;
            this.label10.Text = "CreationTime:";
            // 
            // lblFileNameInfo
            // 
            this.lblFileNameInfo.AutoSize = true;
            this.lblFileNameInfo.Location = new System.Drawing.Point(128, 43);
            this.lblFileNameInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblFileNameInfo.Name = "lblFileNameInfo";
            this.lblFileNameInfo.Size = new System.Drawing.Size(71, 17);
            this.lblFileNameInfo.TabIndex = 8;
            this.lblFileNameInfo.Text = "sample.txt";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 43);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "Filename:";
            // 
            // lblFileSizeInfo
            // 
            this.lblFileSizeInfo.AutoSize = true;
            this.lblFileSizeInfo.Location = new System.Drawing.Point(128, 66);
            this.lblFileSizeInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblFileSizeInfo.Name = "lblFileSizeInfo";
            this.lblFileSizeInfo.Size = new System.Drawing.Size(53, 17);
            this.lblFileSizeInfo.TabIndex = 6;
            this.lblFileSizeInfo.Text = "120 Kb";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 66);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Size:";
            // 
            // lblDirectoryInfo
            // 
            this.lblDirectoryInfo.AutoSize = true;
            this.lblDirectoryInfo.Location = new System.Drawing.Point(128, 20);
            this.lblDirectoryInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblDirectoryInfo.Name = "lblDirectoryInfo";
            this.lblDirectoryInfo.Size = new System.Drawing.Size(72, 17);
            this.lblDirectoryInfo.TabIndex = 4;
            this.lblDirectoryInfo.Text = "c:\\sample\\";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Directory:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lbDuplicateFiles);
            this.groupBox4.Location = new System.Drawing.Point(0, 85);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(7, 4, 7, 4);
            this.groupBox4.Size = new System.Drawing.Size(325, 166);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Duplicates";
            // 
            // lbDuplicateFiles
            // 
            this.lbDuplicateFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDuplicateFiles.FormattingEnabled = true;
            this.lbDuplicateFiles.ItemHeight = 16;
            this.lbDuplicateFiles.Location = new System.Drawing.Point(7, 19);
            this.lbDuplicateFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbDuplicateFiles.Name = "lbDuplicateFiles";
            this.lbDuplicateFiles.Size = new System.Drawing.Size(311, 143);
            this.lbDuplicateFiles.TabIndex = 0;
            this.lbDuplicateFiles.SelectedIndexChanged += new System.EventHandler(this.lbDuplicateFiles_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtMasterFilename);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(325, 78);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Master File";
            // 
            // txtMasterFilename
            // 
            this.txtMasterFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMasterFilename.Location = new System.Drawing.Point(8, 41);
            this.txtMasterFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtMasterFilename.Name = "txtMasterFilename";
            this.txtMasterFilename.ReadOnly = true;
            this.txtMasterFilename.Size = new System.Drawing.Size(308, 22);
            this.txtMasterFilename.TabIndex = 1;
            this.txtMasterFilename.Click += new System.EventHandler(this.txtMasterFilename_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Filename:";
            // 
            // pbFileSearch
            // 
            this.pbFileSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFileSearch.Location = new System.Drawing.Point(12, 453);
            this.pbFileSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbFileSearch.Name = "pbFileSearch";
            this.pbFileSearch.Size = new System.Drawing.Size(659, 25);
            this.pbFileSearch.TabIndex = 7;
            // 
            // btnDeleteFiles
            // 
            this.btnDeleteFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteFiles.Enabled = false;
            this.btnDeleteFiles.Location = new System.Drawing.Point(492, 759);
            this.btnDeleteFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDeleteFiles.Name = "btnDeleteFiles";
            this.btnDeleteFiles.Size = new System.Drawing.Size(193, 34);
            this.btnDeleteFiles.TabIndex = 2;
            this.btnDeleteFiles.Text = "Delete All Selected Copies";
            this.btnDeleteFiles.UseVisualStyleBackColor = true;
            this.btnDeleteFiles.Click += new System.EventHandler(this.btnDeleteFiles_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.directoryScanToolStripMenuItem,
            this.searchDirectoriesToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(7, 6);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(678, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProfileToolStripMenuItem,
            this.toolStripSeparator2,
            this.openProfileToolStripMenuItem,
            this.openLastProfileMenuItem,
            this.toolStripSeparator6,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newProfileToolStripMenuItem
            // 
            this.newProfileToolStripMenuItem.Name = "newProfileToolStripMenuItem";
            this.newProfileToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.newProfileToolStripMenuItem.Text = "New Profile";
            this.newProfileToolStripMenuItem.Click += new System.EventHandler(this.newProfileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(231, 6);
            // 
            // openProfileToolStripMenuItem
            // 
            this.openProfileToolStripMenuItem.Name = "openProfileToolStripMenuItem";
            this.openProfileToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.openProfileToolStripMenuItem.Text = "Open Profile";
            this.openProfileToolStripMenuItem.Click += new System.EventHandler(this.openProfileToolStripMenuItem_Click);
            // 
            // openLastProfileMenuItem
            // 
            this.openLastProfileMenuItem.Name = "openLastProfileMenuItem";
            this.openLastProfileMenuItem.Size = new System.Drawing.Size(234, 26);
            this.openLastProfileMenuItem.Text = "Open Last Profile Used";
            this.openLastProfileMenuItem.Click += new System.EventHandler(this.openLastProfileMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(231, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // directoryScanToolStripMenuItem
            // 
            this.directoryScanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchForDuplicatesToolStripMenuItem,
            this.cancelSearchToolStripMenuItem});
            this.directoryScanToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.directoryScanToolStripMenuItem.Name = "directoryScanToolStripMenuItem";
            this.directoryScanToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.directoryScanToolStripMenuItem.Text = "Search";
            // 
            // searchForDuplicatesToolStripMenuItem
            // 
            this.searchForDuplicatesToolStripMenuItem.Name = "searchForDuplicatesToolStripMenuItem";
            this.searchForDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.searchForDuplicatesToolStripMenuItem.Text = "Search For Duplicates";
            this.searchForDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.searchForDuplicatesToolStripMenuItem_Click);
            // 
            // cancelSearchToolStripMenuItem
            // 
            this.cancelSearchToolStripMenuItem.Name = "cancelSearchToolStripMenuItem";
            this.cancelSearchToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.cancelSearchToolStripMenuItem.Text = "Cancel Search";
            this.cancelSearchToolStripMenuItem.Click += new System.EventHandler(this.cancelSearchToolStripMenuItem_Click);
            // 
            // searchDirectoriesToolStripMenuItem
            // 
            this.searchDirectoriesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDirectoryToolStripMenuItem,
            this.removeSelectedToolStripMenuItem,
            this.toolStripSeparator5,
            this.clearAllToolStripMenuItem});
            this.searchDirectoriesToolStripMenuItem.Name = "searchDirectoriesToolStripMenuItem";
            this.searchDirectoriesToolStripMenuItem.Size = new System.Drawing.Size(141, 24);
            this.searchDirectoriesToolStripMenuItem.Text = "Search Directories";
            // 
            // addDirectoryToolStripMenuItem
            // 
            this.addDirectoryToolStripMenuItem.Name = "addDirectoryToolStripMenuItem";
            this.addDirectoryToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.addDirectoryToolStripMenuItem.Text = "Add";
            this.addDirectoryToolStripMenuItem.Click += new System.EventHandler(this.addDirectoryToolStripMenuItem_Click);
            // 
            // removeSelectedToolStripMenuItem
            // 
            this.removeSelectedToolStripMenuItem.Name = "removeSelectedToolStripMenuItem";
            this.removeSelectedToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.removeSelectedToolStripMenuItem.Text = "Remove Selected";
            this.removeSelectedToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(196, 6);
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            this.clearAllToolStripMenuItem.Text = "Clear All";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setPreferredDirPrioListToolStripMenuItem,
            this.toolStripSeparator3,
            this.clearResultsToolStripMenuItem,
            this.toolStripSeparator4,
            this.optionsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.settingsToolStripMenuItem.Text = "Tools";
            // 
            // setPreferredDirPrioListToolStripMenuItem
            // 
            this.setPreferredDirPrioListToolStripMenuItem.Name = "setPreferredDirPrioListToolStripMenuItem";
            this.setPreferredDirPrioListToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.setPreferredDirPrioListToolStripMenuItem.Text = "Set Preferred Dir Prio List";
            this.setPreferredDirPrioListToolStripMenuItem.Click += new System.EventHandler(this.setPreferredDirPrioListToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(247, 6);
            // 
            // clearResultsToolStripMenuItem
            // 
            this.clearResultsToolStripMenuItem.Name = "clearResultsToolStripMenuItem";
            this.clearResultsToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.clearResultsToolStripMenuItem.Text = "Clear Results";
            this.clearResultsToolStripMenuItem.Click += new System.EventHandler(this.clearResultsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(247, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(207, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(207, 26);
            this.checkForUpdatesToolStripMenuItem.Text = "Check For Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(378, 759);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(107, 34);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblFileHashInfo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFileHashesRunning, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 752);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(338, 52);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 31);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(122, 17);
            this.label15.TabIndex = 13;
            this.label15.Text = "Current File Hash:";
            // 
            // lblFileHashInfo
            // 
            this.lblFileHashInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFileHashInfo.AutoSize = true;
            this.lblFileHashInfo.Location = new System.Drawing.Point(209, 31);
            this.lblFileHashInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblFileHashInfo.Name = "lblFileHashInfo";
            this.lblFileHashInfo.Size = new System.Drawing.Size(20, 17);
            this.lblFileHashInfo.TabIndex = 12;
            this.lblFileHashInfo.Text = "...";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 5);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(197, 17);
            this.label12.TabIndex = 10;
            this.label12.Text = "Parallell File Hashes Running:";
            // 
            // lblFileHashesRunning
            // 
            this.lblFileHashesRunning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFileHashesRunning.AutoSize = true;
            this.lblFileHashesRunning.Location = new System.Drawing.Point(209, 5);
            this.lblFileHashesRunning.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblFileHashesRunning.Name = "lblFileHashesRunning";
            this.lblFileHashesRunning.Size = new System.Drawing.Size(16, 17);
            this.lblFileHashesRunning.TabIndex = 11;
            this.lblFileHashesRunning.Text = "0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 804);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnDeleteFiles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(627, 851);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delete Duplicate Files - Martin Dahl 2015";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResizeBegin += new System.EventHandler(this.frmMain_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkIncludeSubfolders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbResults;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtFilenameFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeleteFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ProgressBar pbFileSearch;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtMasterFilename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbDuplicateFiles;
        private System.Windows.Forms.Label lblFileNameInfo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblFileSizeInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDirectoryInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCreationTimeInfo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblSearchResults;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblSearchStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblLastWriteTimeInfo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox lbScanFolders;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPreferredDirPrioListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem searchDirectoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryScanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchForDuplicatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearResultsToolStripMenuItem;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem openProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblSearchProfileName;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLastProfileMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblFileHashInfo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblFileHashesRunning;
    }
}

