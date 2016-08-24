namespace IpLocationUpdate
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.drpSQLServer = new System.Windows.Forms.ComboBox();
            this.drpDatabase = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.drpDBTable = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSQLConnectionStatus = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.drpAuthentication = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnParseFile = new System.Windows.Forms.Button();
            this.txtGeoIpStatus = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGeoIpInputFile = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pbarTransfer = new System.Windows.Forms.ProgressBar();
            this.txtUpdateDBStatus = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnAbortTransfer = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(130, 230);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(84, 22);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(90, 78);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(207, 20);
            this.txtUserName.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(90, 104);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(207, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "SQL Server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "User name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Authentication:";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(220, 230);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(84, 22);
            this.btnDisconnect.TabIndex = 7;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDiscover);
            this.groupBox1.Controls.Add(this.drpSQLServer);
            this.groupBox1.Controls.Add(this.drpDatabase);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.drpDBTable);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtSQLConnectionStatus);
            this.groupBox1.Controls.Add(this.btnDisconnect);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.drpAuthentication);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 260);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DB Connection";
            // 
            // btnDiscover
            // 
            this.btnDiscover.Location = new System.Drawing.Point(9, 230);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(100, 22);
            this.btnDiscover.TabIndex = 16;
            this.btnDiscover.Text = "List SQL Servers";
            this.btnDiscover.UseVisualStyleBackColor = true;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
            // 
            // drpSQLServer
            // 
            this.drpSQLServer.FormattingEnabled = true;
            this.drpSQLServer.Items.AddRange(new object[] {
            "localhost"});
            this.drpSQLServer.Location = new System.Drawing.Point(90, 24);
            this.drpSQLServer.Name = "drpSQLServer";
            this.drpSQLServer.Size = new System.Drawing.Size(207, 21);
            this.drpSQLServer.TabIndex = 15;
            // 
            // drpDatabase
            // 
            this.drpDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpDatabase.Enabled = false;
            this.drpDatabase.FormattingEnabled = true;
            this.drpDatabase.Location = new System.Drawing.Point(90, 130);
            this.drpDatabase.Name = "drpDatabase";
            this.drpDatabase.Size = new System.Drawing.Size(207, 21);
            this.drpDatabase.TabIndex = 14;
            this.drpDatabase.SelectedIndexChanged += new System.EventHandler(this.drpDatabase_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 133);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Database:";
            // 
            // drpDBTable
            // 
            this.drpDBTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpDBTable.Enabled = false;
            this.drpDBTable.FormattingEnabled = true;
            this.drpDBTable.Location = new System.Drawing.Point(90, 157);
            this.drpDBTable.Name = "drpDBTable";
            this.drpDBTable.Size = new System.Drawing.Size(207, 21);
            this.drpDBTable.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Table:";
            // 
            // txtSQLConnectionStatus
            // 
            this.txtSQLConnectionStatus.Location = new System.Drawing.Point(90, 197);
            this.txtSQLConnectionStatus.Name = "txtSQLConnectionStatus";
            this.txtSQLConnectionStatus.ReadOnly = true;
            this.txtSQLConnectionStatus.Size = new System.Drawing.Size(207, 20);
            this.txtSQLConnectionStatus.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password:";
            // 
            // drpAuthentication
            // 
            this.drpAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpAuthentication.FormattingEnabled = true;
            this.drpAuthentication.Location = new System.Drawing.Point(90, 51);
            this.drpAuthentication.Name = "drpAuthentication";
            this.drpAuthentication.Size = new System.Drawing.Size(207, 21);
            this.drpAuthentication.TabIndex = 7;
            this.drpAuthentication.SelectedIndexChanged += new System.EventHandler(this.drpAuthentication_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnParseFile);
            this.groupBox2.Controls.Add(this.txtGeoIpStatus);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtGeoIpInputFile);
            this.groupBox2.Controls.Add(this.btnOpenFile);
            this.groupBox2.Location = new System.Drawing.Point(338, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 110);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Geo-IP";
            // 
            // btnParseFile
            // 
            this.btnParseFile.Location = new System.Drawing.Point(194, 78);
            this.btnParseFile.Name = "btnParseFile";
            this.btnParseFile.Size = new System.Drawing.Size(100, 22);
            this.btnParseFile.TabIndex = 13;
            this.btnParseFile.Text = "Parse file";
            this.btnParseFile.UseVisualStyleBackColor = true;
            this.btnParseFile.Click += new System.EventHandler(this.btnParseFile_Click);
            // 
            // txtGeoIpStatus
            // 
            this.txtGeoIpStatus.Location = new System.Drawing.Point(88, 51);
            this.txtGeoIpStatus.Name = "txtGeoIpStatus";
            this.txtGeoIpStatus.ReadOnly = true;
            this.txtGeoIpStatus.Size = new System.Drawing.Size(207, 20);
            this.txtGeoIpStatus.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Status:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Input file:";
            // 
            // txtGeoIpInputFile
            // 
            this.txtGeoIpInputFile.Location = new System.Drawing.Point(88, 25);
            this.txtGeoIpInputFile.Name = "txtGeoIpInputFile";
            this.txtGeoIpInputFile.ReadOnly = true;
            this.txtGeoIpInputFile.Size = new System.Drawing.Size(207, 20);
            this.txtGeoIpInputFile.TabIndex = 4;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(88, 78);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(100, 22);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Open file";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(204, 113);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(100, 25);
            this.btnTransfer.TabIndex = 10;
            this.btnTransfer.Text = "Begin transfer";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAbortTransfer);
            this.groupBox3.Controls.Add(this.pbarTransfer);
            this.groupBox3.Controls.Add(this.txtUpdateDBStatus);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.btnTransfer);
            this.groupBox3.Location = new System.Drawing.Point(338, 128);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(310, 144);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Update DB";
            // 
            // pbarTransfer
            // 
            this.pbarTransfer.Location = new System.Drawing.Point(6, 120);
            this.pbarTransfer.Name = "pbarTransfer";
            this.pbarTransfer.Size = new System.Drawing.Size(182, 18);
            this.pbarTransfer.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbarTransfer.TabIndex = 15;
            // 
            // txtUpdateDBStatus
            // 
            this.txtUpdateDBStatus.Location = new System.Drawing.Point(88, 27);
            this.txtUpdateDBStatus.Multiline = true;
            this.txtUpdateDBStatus.Name = "txtUpdateDBStatus";
            this.txtUpdateDBStatus.ReadOnly = true;
            this.txtUpdateDBStatus.Size = new System.Drawing.Size(207, 80);
            this.txtUpdateDBStatus.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Status:";
            // 
            // btnAbortTransfer
            // 
            this.btnAbortTransfer.Enabled = false;
            this.btnAbortTransfer.Location = new System.Drawing.Point(9, 58);
            this.btnAbortTransfer.Name = "btnAbortTransfer";
            this.btnAbortTransfer.Size = new System.Drawing.Size(61, 22);
            this.btnAbortTransfer.TabIndex = 16;
            this.btnAbortTransfer.Text = "Abort";
            this.btnAbortTransfer.UseVisualStyleBackColor = true;
            this.btnAbortTransfer.Click += new System.EventHandler(this.btnAbortTransfer_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 282);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IpLocation update program";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox drpAuthentication;
        private System.Windows.Forms.TextBox txtSQLConnectionStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtGeoIpStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGeoIpInputFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnParseFile;
        private System.Windows.Forms.ComboBox drpDBTable;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtUpdateDBStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox drpDatabase;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox drpSQLServer;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.ProgressBar pbarTransfer;
        private System.Windows.Forms.Button btnAbortTransfer;
    }
}

