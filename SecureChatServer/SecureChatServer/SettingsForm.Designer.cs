namespace SecureChatServer
{
    partial class SettingsForm
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
            this.grpBoxServerSettings = new System.Windows.Forms.GroupBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerateBasicData = new System.Windows.Forms.Button();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSaveUserProfileData = new System.Windows.Forms.CheckBox();
            this.chkDefaultUserHidden = new System.Windows.Forms.CheckBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblNumberOfUsers = new System.Windows.Forms.Label();
            this.grpBoxServerSettings.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxServerSettings
            // 
            this.grpBoxServerSettings.Controls.Add(this.txtServerPort);
            this.grpBoxServerSettings.Controls.Add(this.label4);
            this.grpBoxServerSettings.Controls.Add(this.btnGenerateBasicData);
            this.grpBoxServerSettings.Controls.Add(this.lblServerStatus);
            this.grpBoxServerSettings.Controls.Add(this.label2);
            this.grpBoxServerSettings.Controls.Add(this.groupBox2);
            this.grpBoxServerSettings.Controls.Add(this.txtServerName);
            this.grpBoxServerSettings.Controls.Add(this.label1);
            this.grpBoxServerSettings.Location = new System.Drawing.Point(8, 8);
            this.grpBoxServerSettings.Name = "grpBoxServerSettings";
            this.grpBoxServerSettings.Size = new System.Drawing.Size(410, 186);
            this.grpBoxServerSettings.TabIndex = 0;
            this.grpBoxServerSettings.TabStop = false;
            this.grpBoxServerSettings.Text = "ServerSettings";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(93, 49);
            this.txtServerPort.MaxLength = 6;
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(60, 20);
            this.txtServerPort.TabIndex = 9;
            this.txtServerPort.Text = "9560";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Server Port:";
            // 
            // btnGenerateBasicData
            // 
            this.btnGenerateBasicData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateBasicData.Location = new System.Drawing.Point(209, 150);
            this.btnGenerateBasicData.Name = "btnGenerateBasicData";
            this.btnGenerateBasicData.Size = new System.Drawing.Size(184, 25);
            this.btnGenerateBasicData.TabIndex = 7;
            this.btnGenerateBasicData.Text = "Generate server parameters";
            this.btnGenerateBasicData.UseVisualStyleBackColor = true;
            this.btnGenerateBasicData.Click += new System.EventHandler(this.btnGenerateBasicData_Click);
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerStatus.Location = new System.Drawing.Point(113, 156);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(59, 13);
            this.lblServerStatus.TabIndex = 6;
            this.lblServerStatus.Text = "-------------";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server status:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSaveUserProfileData);
            this.groupBox2.Controls.Add(this.chkDefaultUserHidden);
            this.groupBox2.Location = new System.Drawing.Point(18, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 69);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User accounts";
            // 
            // chkSaveUserProfileData
            // 
            this.chkSaveUserProfileData.AutoSize = true;
            this.chkSaveUserProfileData.Checked = true;
            this.chkSaveUserProfileData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveUserProfileData.Location = new System.Drawing.Point(6, 42);
            this.chkSaveUserProfileData.Name = "chkSaveUserProfileData";
            this.chkSaveUserProfileData.Size = new System.Drawing.Size(229, 17);
            this.chkSaveUserProfileData.TabIndex = 4;
            this.chkSaveUserProfileData.Text = "Save encrypted user accounts in database";
            this.chkSaveUserProfileData.UseVisualStyleBackColor = true;
            // 
            // chkDefaultUserHidden
            // 
            this.chkDefaultUserHidden.AutoSize = true;
            this.chkDefaultUserHidden.Checked = true;
            this.chkDefaultUserHidden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefaultUserHidden.Location = new System.Drawing.Point(6, 19);
            this.chkDefaultUserHidden.Name = "chkDefaultUserHidden";
            this.chkDefaultUserHidden.Size = new System.Drawing.Size(105, 17);
            this.chkDefaultUserHidden.TabIndex = 3;
            this.chkDefaultUserHidden.Text = "Default not listed";
            this.chkDefaultUserHidden.UseVisualStyleBackColor = true;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(93, 22);
            this.txtServerName.MaxLength = 100;
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(300, 20);
            this.txtServerName.TabIndex = 1;
            this.txtServerName.Text = "My Secure Chat Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(262, 200);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(343, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Number of users:";
            // 
            // lblNumberOfUsers
            // 
            this.lblNumberOfUsers.AutoSize = true;
            this.lblNumberOfUsers.Location = new System.Drawing.Point(105, 206);
            this.lblNumberOfUsers.Name = "lblNumberOfUsers";
            this.lblNumberOfUsers.Size = new System.Drawing.Size(13, 13);
            this.lblNumberOfUsers.TabIndex = 4;
            this.lblNumberOfUsers.Text = "0";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 235);
            this.Controls.Add(this.lblNumberOfUsers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpBoxServerSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.grpBoxServerSettings.ResumeLayout(false);
            this.grpBoxServerSettings.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxServerSettings;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenerateBasicData;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkSaveUserProfileData;
        private System.Windows.Forms.CheckBox chkDefaultUserHidden;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblNumberOfUsers;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label label4;
    }
}