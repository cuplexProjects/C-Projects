namespace Geo_IpImport
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbASN = new System.Windows.Forms.RadioButton();
            this.rbCountry = new System.Windows.Forms.RadioButton();
            this.rbCity = new System.Windows.Forms.RadioButton();
            this.txtCSVDirectory = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkContinueOnError = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbConnectionString = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnTestDBConn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtProgressInfo = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabelConnStr = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.txtCSVDirectory);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Location = new System.Drawing.Point(12, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 123);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Geo-IP CSV File";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbASN);
            this.panel1.Controls.Add(this.rbCountry);
            this.panel1.Controls.Add(this.rbCity);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 67);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 53);
            this.panel1.TabIndex = 3;
            // 
            // rbASN
            // 
            this.rbASN.AutoSize = true;
            this.rbASN.Location = new System.Drawing.Point(9, 26);
            this.rbASN.Name = "rbASN";
            this.rbASN.Size = new System.Drawing.Size(166, 17);
            this.rbASN.TabIndex = 2;
            this.rbASN.Text = "Autonomous System Numbers";
            this.rbASN.UseVisualStyleBackColor = true;
            // 
            // rbCountry
            // 
            this.rbCountry.AutoSize = true;
            this.rbCountry.Checked = true;
            this.rbCountry.Location = new System.Drawing.Point(9, 3);
            this.rbCountry.Name = "rbCountry";
            this.rbCountry.Size = new System.Drawing.Size(61, 17);
            this.rbCountry.TabIndex = 1;
            this.rbCountry.TabStop = true;
            this.rbCountry.Text = "Country";
            this.rbCountry.UseVisualStyleBackColor = true;
            // 
            // rbCity
            // 
            this.rbCity.AutoSize = true;
            this.rbCity.Location = new System.Drawing.Point(76, 3);
            this.rbCity.Name = "rbCity";
            this.rbCity.Size = new System.Drawing.Size(42, 17);
            this.rbCity.TabIndex = 0;
            this.rbCity.Text = "City";
            this.rbCity.UseVisualStyleBackColor = true;
            // 
            // txtCSVDirectory
            // 
            this.txtCSVDirectory.Location = new System.Drawing.Point(9, 38);
            this.txtCSVDirectory.Name = "txtCSVDirectory";
            this.txtCSVDirectory.Size = new System.Drawing.Size(312, 20);
            this.txtCSVDirectory.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Import location:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(277, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(44, 25);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnImport
            // 
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(261, 25);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 25);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(339, 16);
            this.progressBar1.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(180, 25);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkContinueOnError
            // 
            this.chkContinueOnError.AutoSize = true;
            this.chkContinueOnError.Checked = true;
            this.chkContinueOnError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkContinueOnError.Location = new System.Drawing.Point(12, 30);
            this.chkContinueOnError.Name = "chkContinueOnError";
            this.chkContinueOnError.Size = new System.Drawing.Size(156, 17);
            this.chkContinueOnError.TabIndex = 6;
            this.chkContinueOnError.Text = "Continue on row parse error";
            this.chkContinueOnError.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbConnectionString);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.btnTestDBConn);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 75);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database connection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Connection String:";
            // 
            // cbConnectionString
            // 
            this.cbConnectionString.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnectionString.FormattingEnabled = true;
            this.cbConnectionString.Location = new System.Drawing.Point(106, 45);
            this.cbConnectionString.Name = "cbConnectionString";
            this.cbConnectionString.Size = new System.Drawing.Size(215, 21);
            this.cbConnectionString.TabIndex = 8;
            this.cbConnectionString.SelectedIndexChanged += new System.EventHandler(this.cbConnectionString_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(52, 26);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(27, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "N/A";
            // 
            // btnTestDBConn
            // 
            this.btnTestDBConn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestDBConn.Location = new System.Drawing.Point(226, 14);
            this.btnTestDBConn.Name = "btnTestDBConn";
            this.btnTestDBConn.Size = new System.Drawing.Size(95, 25);
            this.btnTestDBConn.TabIndex = 6;
            this.btnTestDBConn.Text = "Test Connection";
            this.btnTestDBConn.UseVisualStyleBackColor = true;
            this.btnTestDBConn.Click += new System.EventHandler(this.btnTestDBConn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.btnImport);
            this.panel2.Controls.Add(this.chkContinueOnError);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Location = new System.Drawing.Point(0, 342);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(355, 54);
            this.panel2.TabIndex = 8;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtProgressInfo);
            this.groupBox3.Location = new System.Drawing.Point(12, 225);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 112);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Import progress";
            // 
            // txtProgressInfo
            // 
            this.txtProgressInfo.Location = new System.Drawing.Point(9, 19);
            this.txtProgressInfo.Multiline = true;
            this.txtProgressInfo.Name = "txtProgressInfo";
            this.txtProgressInfo.ReadOnly = true;
            this.txtProgressInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProgressInfo.Size = new System.Drawing.Size(312, 80);
            this.txtProgressInfo.TabIndex = 3;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabelConnStr});
            this.statusStrip.Location = new System.Drawing.Point(0, 399);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(358, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 10;
            // 
            // StatusLabelConnStr
            // 
            this.StatusLabelConnStr.Name = "StatusLabelConnStr";
            this.StatusLabelConnStr.Size = new System.Drawing.Size(32, 17);
            this.StatusLabelConnStr.Text = "-----";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 421);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geo-IP SQL Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtCSVDirectory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkContinueOnError;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbASN;
        private System.Windows.Forms.RadioButton rbCountry;
        private System.Windows.Forms.RadioButton rbCity;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnTestDBConn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtProgressInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbConnectionString;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabelConnStr;
    }
}

