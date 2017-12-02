namespace GenerateOvpnFile
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbProtocol = new System.Windows.Forms.ComboBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCipher = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCompression = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbTPSAuth = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbInterface = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowseTaFile = new System.Windows.Forms.GroupBox();
            this.lblFileGenerarationStatus = new System.Windows.Forms.Label();
            this.btnBrowseOutputDir = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnGenerateCompactFile = new System.Windows.Forms.Button();
            this.btnBrowseTaPath = new System.Windows.Forms.Button();
            this.txtTLSPath = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnBrowseClientKey = new System.Windows.Forms.Button();
            this.txtClientKey = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnBrowseClientCert = new System.Windows.Forms.Button();
            this.txtClientCertPath = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBrowseCA = new System.Windows.Forms.Button();
            this.txtCaPath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnRevertSettings = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.btnBrowseTaFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtHost);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cbProtocol);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbCipher);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbCompression);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbTPSAuth);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbInterface);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(765, 299);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Properties";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(191, 30);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtName.MaxLength = 255;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(340, 22);
            this.txtName.TabIndex = 1;
            this.txtName.Text = "YourName";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 38);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 17);
            this.label12.TabIndex = 15;
            this.label12.Text = "Name:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(191, 62);
            this.txtHost.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtHost.MaxLength = 255;
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(520, 22);
            this.txtHost.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 65);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 17);
            this.label8.TabIndex = 13;
            this.label8.Text = "Remote Host:";
            // 
            // cbProtocol
            // 
            this.cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProtocol.FormattingEnabled = true;
            this.cbProtocol.Location = new System.Drawing.Point(191, 159);
            this.cbProtocol.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbProtocol.Name = "cbProtocol";
            this.cbProtocol.Size = new System.Drawing.Size(79, 24);
            this.cbProtocol.TabIndex = 5;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(191, 127);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPort.MaxLength = 6;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(79, 22);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "1194";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 130);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Server Port:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 262);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Cipher:";
            // 
            // cbCipher
            // 
            this.cbCipher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCipher.FormattingEnabled = true;
            this.cbCipher.Location = new System.Drawing.Point(191, 258);
            this.cbCipher.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCipher.Name = "cbCipher";
            this.cbCipher.Size = new System.Drawing.Size(163, 24);
            this.cbCipher.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 229);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Compression:";
            // 
            // cbCompression
            // 
            this.cbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompression.FormattingEnabled = true;
            this.cbCompression.Location = new System.Drawing.Point(191, 225);
            this.cbCompression.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbCompression.Name = "cbCompression";
            this.cbCompression.Size = new System.Drawing.Size(163, 24);
            this.cbCompression.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 196);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Extra HMAC-verification:";
            // 
            // cbTPSAuth
            // 
            this.cbTPSAuth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTPSAuth.FormattingEnabled = true;
            this.cbTPSAuth.Location = new System.Drawing.Point(191, 192);
            this.cbTPSAuth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTPSAuth.Name = "cbTPSAuth";
            this.cbTPSAuth.Size = new System.Drawing.Size(163, 24);
            this.cbTPSAuth.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 162);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Protocol:";
            // 
            // cbInterface
            // 
            this.cbInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInterface.FormattingEnabled = true;
            this.cbInterface.Location = new System.Drawing.Point(191, 94);
            this.cbInterface.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbInterface.Name = "cbInterface";
            this.cbInterface.Size = new System.Drawing.Size(79, 24);
            this.cbInterface.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 97);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Interface:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 31);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "CA Path:";
            // 
            // btnBrowseTaFile
            // 
            this.btnBrowseTaFile.Controls.Add(this.btnRevertSettings);
            this.btnBrowseTaFile.Controls.Add(this.lblFileGenerarationStatus);
            this.btnBrowseTaFile.Controls.Add(this.btnBrowseOutputDir);
            this.btnBrowseTaFile.Controls.Add(this.label13);
            this.btnBrowseTaFile.Controls.Add(this.btnGenerateCompactFile);
            this.btnBrowseTaFile.Controls.Add(this.btnBrowseTaPath);
            this.btnBrowseTaFile.Controls.Add(this.txtTLSPath);
            this.btnBrowseTaFile.Controls.Add(this.label11);
            this.btnBrowseTaFile.Controls.Add(this.btnBrowseClientKey);
            this.btnBrowseTaFile.Controls.Add(this.txtClientKey);
            this.btnBrowseTaFile.Controls.Add(this.label10);
            this.btnBrowseTaFile.Controls.Add(this.btnBrowseClientCert);
            this.btnBrowseTaFile.Controls.Add(this.txtClientCertPath);
            this.btnBrowseTaFile.Controls.Add(this.label9);
            this.btnBrowseTaFile.Controls.Add(this.btnBrowseCA);
            this.btnBrowseTaFile.Controls.Add(this.txtCaPath);
            this.btnBrowseTaFile.Controls.Add(this.label7);
            this.btnBrowseTaFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseTaFile.Location = new System.Drawing.Point(7, 305);
            this.btnBrowseTaFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseTaFile.Name = "btnBrowseTaFile";
            this.btnBrowseTaFile.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseTaFile.Size = new System.Drawing.Size(765, 215);
            this.btnBrowseTaFile.TabIndex = 14;
            this.btnBrowseTaFile.TabStop = false;
            this.btnBrowseTaFile.Text = "Certificates";
            // 
            // lblFileGenerarationStatus
            // 
            this.lblFileGenerarationStatus.AutoSize = true;
            this.lblFileGenerarationStatus.Location = new System.Drawing.Point(84, 183);
            this.lblFileGenerarationStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileGenerarationStatus.Name = "lblFileGenerarationStatus";
            this.lblFileGenerarationStatus.Size = new System.Drawing.Size(171, 17);
            this.lblFileGenerarationStatus.TabIndex = 29;
            this.lblFileGenerarationStatus.Text = "SCSXUvXUKPkbbjqxcqVS";
            // 
            // btnBrowseOutputDir
            // 
            this.btnBrowseOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutputDir.Location = new System.Drawing.Point(449, 177);
            this.btnBrowseOutputDir.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseOutputDir.Name = "btnBrowseOutputDir";
            this.btnBrowseOutputDir.Size = new System.Drawing.Size(133, 31);
            this.btnBrowseOutputDir.TabIndex = 10;
            this.btnBrowseOutputDir.Text = "Browse to file";
            this.btnBrowseOutputDir.UseVisualStyleBackColor = true;
            this.btnBrowseOutputDir.Click += new System.EventHandler(this.btnBrowseOutputDir_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 183);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(52, 17);
            this.label13.TabIndex = 26;
            this.label13.Text = "Status:";
            // 
            // btnGenerateCompactFile
            // 
            this.btnGenerateCompactFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateCompactFile.Location = new System.Drawing.Point(591, 177);
            this.btnGenerateCompactFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerateCompactFile.Name = "btnGenerateCompactFile";
            this.btnGenerateCompactFile.Size = new System.Drawing.Size(167, 31);
            this.btnGenerateCompactFile.TabIndex = 11;
            this.btnGenerateCompactFile.Text = "Save OVPN File";
            this.btnGenerateCompactFile.UseVisualStyleBackColor = true;
            this.btnGenerateCompactFile.Click += new System.EventHandler(this.btnGenerateCompactFile_Click);
            // 
            // btnBrowseTaPath
            // 
            this.btnBrowseTaPath.Location = new System.Drawing.Point(665, 118);
            this.btnBrowseTaPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseTaPath.Name = "btnBrowseTaPath";
            this.btnBrowseTaPath.Size = new System.Drawing.Size(47, 25);
            this.btnBrowseTaPath.TabIndex = 24;
            this.btnBrowseTaPath.Text = "..";
            this.btnBrowseTaPath.UseVisualStyleBackColor = true;
            this.btnBrowseTaPath.Click += new System.EventHandler(this.btnBrowseTaPath_Click);
            // 
            // txtTLSPath
            // 
            this.txtTLSPath.Location = new System.Drawing.Point(191, 119);
            this.txtTLSPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTLSPath.Name = "txtTLSPath";
            this.txtTLSPath.ReadOnly = true;
            this.txtTLSPath.Size = new System.Drawing.Size(465, 22);
            this.txtTLSPath.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(23, 123);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 17);
            this.label11.TabIndex = 22;
            this.label11.Text = "TLS-auth key:";
            // 
            // btnBrowseClientKey
            // 
            this.btnBrowseClientKey.Location = new System.Drawing.Point(665, 86);
            this.btnBrowseClientKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseClientKey.Name = "btnBrowseClientKey";
            this.btnBrowseClientKey.Size = new System.Drawing.Size(47, 25);
            this.btnBrowseClientKey.TabIndex = 21;
            this.btnBrowseClientKey.Text = "..";
            this.btnBrowseClientKey.UseVisualStyleBackColor = true;
            this.btnBrowseClientKey.Click += new System.EventHandler(this.btnBrowseClientKey_Click);
            // 
            // txtClientKey
            // 
            this.txtClientKey.Location = new System.Drawing.Point(191, 87);
            this.txtClientKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtClientKey.Name = "txtClientKey";
            this.txtClientKey.ReadOnly = true;
            this.txtClientKey.Size = new System.Drawing.Size(465, 22);
            this.txtClientKey.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 91);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "Client Key:";
            // 
            // btnBrowseClientCert
            // 
            this.btnBrowseClientCert.Location = new System.Drawing.Point(665, 54);
            this.btnBrowseClientCert.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseClientCert.Name = "btnBrowseClientCert";
            this.btnBrowseClientCert.Size = new System.Drawing.Size(47, 25);
            this.btnBrowseClientCert.TabIndex = 18;
            this.btnBrowseClientCert.Text = "..";
            this.btnBrowseClientCert.UseVisualStyleBackColor = true;
            this.btnBrowseClientCert.Click += new System.EventHandler(this.btnBrowseClientCert_Click);
            // 
            // txtClientCertPath
            // 
            this.txtClientCertPath.Location = new System.Drawing.Point(191, 55);
            this.txtClientCertPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtClientCertPath.Name = "txtClientCertPath";
            this.txtClientCertPath.ReadOnly = true;
            this.txtClientCertPath.Size = new System.Drawing.Size(465, 22);
            this.txtClientCertPath.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 59);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(114, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "Client Certificate:";
            // 
            // btnBrowseCA
            // 
            this.btnBrowseCA.Location = new System.Drawing.Point(665, 22);
            this.btnBrowseCA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowseCA.Name = "btnBrowseCA";
            this.btnBrowseCA.Size = new System.Drawing.Size(47, 25);
            this.btnBrowseCA.TabIndex = 15;
            this.btnBrowseCA.Text = "..";
            this.btnBrowseCA.UseVisualStyleBackColor = true;
            this.btnBrowseCA.Click += new System.EventHandler(this.btnBrowseCA_Click);
            // 
            // txtCaPath
            // 
            this.txtCaPath.Location = new System.Drawing.Point(191, 23);
            this.txtCaPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCaPath.Name = "txtCaPath";
            this.txtCaPath.ReadOnly = true;
            this.txtCaPath.Size = new System.Drawing.Size(465, 22);
            this.txtCaPath.TabIndex = 15;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnRevertSettings
            // 
            this.btnRevertSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRevertSettings.Location = new System.Drawing.Point(308, 177);
            this.btnRevertSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnRevertSettings.Name = "btnRevertSettings";
            this.btnRevertSettings.Size = new System.Drawing.Size(133, 31);
            this.btnRevertSettings.TabIndex = 30;
            this.btnRevertSettings.Text = "Revert Settings";
            this.btnRevertSettings.UseVisualStyleBackColor = true;
            this.btnRevertSettings.Click += new System.EventHandler(this.btnRevertSettings_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 526);
            this.Controls.Add(this.btnBrowseTaFile);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(794, 561);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open VPN Profile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.btnBrowseTaFile.ResumeLayout(false);
            this.btnBrowseTaFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbCipher;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCompression;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbTPSAuth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbInterface;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbProtocol;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox btnBrowseTaFile;
        private System.Windows.Forms.Button btnBrowseTaPath;
        private System.Windows.Forms.TextBox txtTLSPath;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnBrowseClientKey;
        private System.Windows.Forms.TextBox txtClientKey;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnBrowseClientCert;
        private System.Windows.Forms.TextBox txtClientCertPath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnBrowseCA;
        private System.Windows.Forms.TextBox txtCaPath;
        private System.Windows.Forms.Button btnGenerateCompactFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblFileGenerarationStatus;
        private System.Windows.Forms.Button btnBrowseOutputDir;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnRevertSettings;
    }
}

