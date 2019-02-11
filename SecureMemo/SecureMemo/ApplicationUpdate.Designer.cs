namespace SecureMemo
{
    partial class ApplicationUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationUpdate));
            this.grpVersionInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.lblTrademark = new System.Windows.Forms.Label();
            this.lblBuildDate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCheckForUpdates = new System.Windows.Forms.Button();
            this.picBoxApplicationLogo = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblLinkEmail = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.progressWaitControl1 = new GeneralToolkitLib.UserControls.ProgressWaitControl();
            this.grpVersionInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxApplicationLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // grpVersionInfo
            // 
            this.grpVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpVersionInfo.Controls.Add(this.tableLayoutPanel1);
            this.grpVersionInfo.Location = new System.Drawing.Point(12, 183);
            this.grpVersionInfo.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.grpVersionInfo.Name = "grpVersionInfo";
            this.grpVersionInfo.Size = new System.Drawing.Size(400, 93);
            this.grpVersionInfo.TabIndex = 0;
            this.grpVersionInfo.TabStop = false;
            this.grpVersionInfo.Text = "Version Information";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.45928F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.54072F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTrademark, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblBuildDate, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentVersion, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(394, 74);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Trademark";
            // 
            // lblTrademark
            // 
            this.lblTrademark.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTrademark.AutoSize = true;
            this.lblTrademark.Location = new System.Drawing.Point(150, 56);
            this.lblTrademark.Name = "lblTrademark";
            this.lblTrademark.Size = new System.Drawing.Size(16, 13);
            this.lblTrademark.TabIndex = 7;
            this.lblTrademark.Text = "...";
            // 
            // lblBuildDate
            // 
            this.lblBuildDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblBuildDate.AutoSize = true;
            this.lblBuildDate.Location = new System.Drawing.Point(150, 31);
            this.lblBuildDate.Name = "lblBuildDate";
            this.lblBuildDate.Size = new System.Drawing.Size(16, 13);
            this.lblBuildDate.TabIndex = 6;
            this.lblBuildDate.Text = "...";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Build Date";
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Location = new System.Drawing.Point(150, 6);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(16, 13);
            this.lblCurrentVersion.TabIndex = 4;
            this.lblCurrentVersion.Text = "...";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Current Version";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(337, 299);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCheckForUpdates
            // 
            this.btnCheckForUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckForUpdates.Location = new System.Drawing.Point(187, 299);
            this.btnCheckForUpdates.Name = "btnCheckForUpdates";
            this.btnCheckForUpdates.Size = new System.Drawing.Size(144, 25);
            this.btnCheckForUpdates.TabIndex = 2;
            this.btnCheckForUpdates.Text = "Check For Updates";
            this.btnCheckForUpdates.UseVisualStyleBackColor = true;
            this.btnCheckForUpdates.Click += new System.EventHandler(this.btnCheckForUpdates_Click);
            // 
            // picBoxApplicationLogo
            // 
            this.picBoxApplicationLogo.ErrorImage = null;
            this.picBoxApplicationLogo.Image = ((System.Drawing.Image)(resources.GetObject("picBoxApplicationLogo.Image")));
            this.picBoxApplicationLogo.InitialImage = null;
            this.picBoxApplicationLogo.Location = new System.Drawing.Point(12, 12);
            this.picBoxApplicationLogo.Name = "picBoxApplicationLogo";
            this.picBoxApplicationLogo.Size = new System.Drawing.Size(400, 100);
            this.picBoxApplicationLogo.TabIndex = 3;
            this.picBoxApplicationLogo.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Author:";
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(59, 132);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(61, 13);
            this.lblAuthor.TabIndex = 5;
            this.lblAuthor.Text = "Martin Dahl";
            // 
            // lblLinkEmail
            // 
            this.lblLinkEmail.AutoSize = true;
            this.lblLinkEmail.Location = new System.Drawing.Point(59, 150);
            this.lblLinkEmail.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.lblLinkEmail.Name = "lblLinkEmail";
            this.lblLinkEmail.Size = new System.Drawing.Size(19, 13);
            this.lblLinkEmail.TabIndex = 6;
            this.lblLinkEmail.TabStop = true;
            this.lblLinkEmail.Text = "....";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Email:";
            // 
            // progressWaitControl1
            // 
            this.progressWaitControl1.Active = true;
            this.progressWaitControl1.BackColor = System.Drawing.Color.GhostWhite;
            this.progressWaitControl1.Location = new System.Drawing.Point(12, 284);
            this.progressWaitControl1.Name = "progressWaitControl1";
            this.progressWaitControl1.Size = new System.Drawing.Size(160, 46);
            this.progressWaitControl1.TabIndex = 8;
            this.progressWaitControl1.Load += new System.EventHandler(this.progressWaitControl1_Load);
            // 
            // ApplicationUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 336);
            this.Controls.Add(this.progressWaitControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLinkEmail);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.picBoxApplicationLogo);
            this.Controls.Add(this.btnCheckForUpdates);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grpVersionInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApplicationUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Secure Memo Update";
            this.Load += new System.EventHandler(this.ApplicationUpdate_Load);
            this.grpVersionInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxApplicationLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpVersionInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblBuildDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCheckForUpdates;
        private System.Windows.Forms.PictureBox picBoxApplicationLogo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.LinkLabel lblLinkEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblTrademark;
        private System.Windows.Forms.Label label2;
        private GeneralToolkitLib.UserControls.ProgressWaitControl progressWaitControl1;
    }
}