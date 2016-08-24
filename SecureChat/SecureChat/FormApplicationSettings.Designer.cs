namespace SecureChat
{
    partial class FormApplicationSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApplicationSettings));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxUserSettings = new System.Windows.Forms.GroupBox();
            this.txtAPIUrl = new System.Windows.Forms.TextBox();
            this.lblGUID = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.imageListLutton = new System.Windows.Forms.ImageList(this.components);
            this.imageListNetworkStatus = new System.Windows.Forms.ImageList(this.components);
            this.lblConnStatusText = new System.Windows.Forms.Label();
            this.toolTipRedreshConnStatus = new System.Windows.Forms.ToolTip(this.components);
            this.networkStatusControl1 = new SecureChat.CustomControls.NetworkStatusControl();
            this.groupBoxUserSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(297, 60);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(403, 60);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBoxUserSettings
            // 
            this.groupBoxUserSettings.Controls.Add(this.txtAPIUrl);
            this.groupBoxUserSettings.Controls.Add(this.lblGUID);
            this.groupBoxUserSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxUserSettings.Location = new System.Drawing.Point(5, 5);
            this.groupBoxUserSettings.Name = "groupBoxUserSettings";
            this.groupBoxUserSettings.Padding = new System.Windows.Forms.Padding(5);
            this.groupBoxUserSettings.Size = new System.Drawing.Size(509, 58);
            this.groupBoxUserSettings.TabIndex = 2;
            this.groupBoxUserSettings.TabStop = false;
            this.groupBoxUserSettings.Text = "Application Settings";
            // 
            // txtAPIUrl
            // 
            this.txtAPIUrl.Location = new System.Drawing.Point(68, 21);
            this.txtAPIUrl.MaxLength = 1024;
            this.txtAPIUrl.Name = "txtAPIUrl";
            this.txtAPIUrl.Size = new System.Drawing.Size(429, 20);
            this.txtAPIUrl.TabIndex = 2;
            this.txtAPIUrl.Text = "https://api.cuplex.se";
            // 
            // lblGUID
            // 
            this.lblGUID.AutoSize = true;
            this.lblGUID.Location = new System.Drawing.Point(10, 25);
            this.lblGUID.Name = "lblGUID";
            this.lblGUID.Size = new System.Drawing.Size(52, 13);
            this.lblGUID.TabIndex = 0;
            this.lblGUID.Text = "API Host:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblConnStatusText);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.networkStatusControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(509, 94);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network And User Status";
            // 
            // btnRefresh
            // 
            this.btnRefresh.AccessibleDescription = "Refresh";
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.ForeColor = System.Drawing.Color.Transparent;
            this.btnRefresh.ImageIndex = 4;
            this.btnRefresh.ImageList = this.imageListLutton;
            this.btnRefresh.Location = new System.Drawing.Point(463, 11);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(40, 40);
            this.btnRefresh.TabIndex = 3;
            this.toolTipRedreshConnStatus.SetToolTip(this.btnRefresh, "Check Connection Status");
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // imageListLutton
            // 
            this.imageListLutton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLutton.ImageStream")));
            this.imageListLutton.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLutton.Images.SetKeyName(0, "Hopstarter-Button-Button-Refresh.ico");
            this.imageListLutton.Images.SetKeyName(1, "Hopstarter-Soft-Scraps-Button-Refresh.ico");
            this.imageListLutton.Images.SetKeyName(2, "Oxygen-Icons.org-Oxygen-Actions-view-refresh.ico");
            this.imageListLutton.Images.SetKeyName(3, "Custom-Icon-Design-Mono-General-4-Refresh.ico");
            this.imageListLutton.Images.SetKeyName(4, "Hopstarter-Soft-Scraps-Network-Refresh.ico");
            this.imageListLutton.Images.SetKeyName(5, "Rafiqul-Hassan-Blogger-Refresh-2.ico");
            // 
            // imageListNetworkStatus
            // 
            this.imageListNetworkStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListNetworkStatus.ImageStream")));
            this.imageListNetworkStatus.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListNetworkStatus.Images.SetKeyName(0, "Gakuseisean-Ivista-2-Network-Globe-Connected.ico");
            this.imageListNetworkStatus.Images.SetKeyName(1, "Gakuseisean-Ivista-2-Network-Globe-Disconnected.ico");
            this.imageListNetworkStatus.Images.SetKeyName(2, "Oxygen-Icons.org-Oxygen-Devices-network-wired.ico");
            // 
            // lblConnStatusText
            // 
            this.lblConnStatusText.AutoSize = true;
            this.lblConnStatusText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConnStatusText.Location = new System.Drawing.Point(12, 54);
            this.lblConnStatusText.Name = "lblConnStatusText";
            this.lblConnStatusText.Size = new System.Drawing.Size(79, 13);
            this.lblConnStatusText.TabIndex = 2;
            this.lblConnStatusText.Text = "Not Connected";
            // 
            // toolTipRedreshConnStatus
            // 
            this.toolTipRedreshConnStatus.IsBalloon = true;
            this.toolTipRedreshConnStatus.ToolTipTitle = "Verify connection to API";
            // 
            // networkStatusControl1
            // 
            this.networkStatusControl1.BackColor = System.Drawing.Color.White;
            this.networkStatusControl1.ConnectedImageIndex = 1;
            this.networkStatusControl1.Location = new System.Drawing.Point(15, 19);
            this.networkStatusControl1.Name = "networkStatusControl1";
            this.networkStatusControl1.PendingImageIndex = 2;
            this.networkStatusControl1.Size = new System.Drawing.Size(32, 32);
            this.networkStatusControl1.StatusImageList = this.imageListNetworkStatus;
            this.networkStatusControl1.TabIndex = 1;
            // 
            // frmApplicationSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 162);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxUserSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmApplicationSettings";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBoxUserSettings.ResumeLayout(false);
            this.groupBoxUserSettings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBoxUserSettings;
        private System.Windows.Forms.TextBox txtAPIUrl;
        private System.Windows.Forms.Label lblGUID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ImageList imageListNetworkStatus;
        private CustomControls.NetworkStatusControl networkStatusControl1;
        private System.Windows.Forms.Label lblConnStatusText;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ImageList imageListLutton;
        private System.Windows.Forms.ToolTip toolTipRedreshConnStatus;
    }
}