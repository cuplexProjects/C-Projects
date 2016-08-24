namespace SecureChat
{
    partial class FormCreateNewUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateNewUser));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picPassword2Status = new System.Windows.Forms.PictureBox();
            this.picPasswordStatus = new System.Windows.Forms.PictureBox();
            this.picUserNameStatus = new System.Windows.Forms.PictureBox();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPasswordConfirm = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.lblKeyGenStatus = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword2Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPasswordStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUserNameStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.picPassword2Status);
            this.groupBox1.Controls.Add(this.picPasswordStatus);
            this.groupBox1.Controls.Add(this.picUserNameStatus);
            this.groupBox1.Controls.Add(this.lblServerStatus);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtPasswordConfirm);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Configuration";
            // 
            // picPassword2Status
            // 
            this.picPassword2Status.Location = new System.Drawing.Point(363, 73);
            this.picPassword2Status.Name = "picPassword2Status";
            this.picPassword2Status.Size = new System.Drawing.Size(20, 20);
            this.picPassword2Status.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPassword2Status.TabIndex = 11;
            this.picPassword2Status.TabStop = false;
            // 
            // picPasswordStatus
            // 
            this.picPasswordStatus.Location = new System.Drawing.Point(363, 48);
            this.picPasswordStatus.Name = "picPasswordStatus";
            this.picPasswordStatus.Size = new System.Drawing.Size(20, 20);
            this.picPasswordStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPasswordStatus.TabIndex = 10;
            this.picPasswordStatus.TabStop = false;
            // 
            // picUserNameStatus
            // 
            this.picUserNameStatus.InitialImage = null;
            this.picUserNameStatus.Location = new System.Drawing.Point(363, 23);
            this.picUserNameStatus.Name = "picUserNameStatus";
            this.picUserNameStatus.Size = new System.Drawing.Size(20, 20);
            this.picUserNameStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picUserNameStatus.TabIndex = 9;
            this.picUserNameStatus.TabStop = false;
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Location = new System.Drawing.Point(104, 111);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(67, 13);
            this.lblServerStatus.TabIndex = 7;
            this.lblServerStatus.Text = "....................";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Server status:";
            // 
            // txtPasswordConfirm
            // 
            this.txtPasswordConfirm.Location = new System.Drawing.Point(107, 73);
            this.txtPasswordConfirm.MaxLength = 256;
            this.txtPasswordConfirm.Name = "txtPasswordConfirm";
            this.txtPasswordConfirm.PasswordChar = '*';
            this.txtPasswordConfirm.Size = new System.Drawing.Size(250, 20);
            this.txtPasswordConfirm.TabIndex = 5;
            this.txtPasswordConfirm.TextChanged += new System.EventHandler(this.txtPasswordConfirm_TextChanged);
            this.txtPasswordConfirm.Enter += new System.EventHandler(this.txtPasswordConfirm_Enter);
            this.txtPasswordConfirm.Validating += new System.ComponentModel.CancelEventHandler(this.txtPasswordConfirm_Validating);
            this.txtPasswordConfirm.Validated += new System.EventHandler(this.txtPasswordConfirm_Validated);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Confirm Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(107, 48);
            this.txtPassword.MaxLength = 256;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(250, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtPassword_Validating);
            this.txtPassword.Validated += new System.EventHandler(this.txtPassword_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(107, 22);
            this.txtUserName.MaxLength = 64;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(250, 20);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserName_Validating);
            this.txtUserName.Validated += new System.EventHandler(this.txtUserName_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // btnCreateUser
            // 
            this.btnCreateUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateUser.Location = new System.Drawing.Point(195, 155);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(100, 28);
            this.btnCreateUser.TabIndex = 1;
            this.btnCreateUser.Text = "Create User";
            this.btnCreateUser.UseVisualStyleBackColor = true;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(301, 155);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // statusImageList
            // 
            this.statusImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("statusImageList.ImageStream")));
            this.statusImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.statusImageList.Images.SetKeyName(0, "Add.ico");
            this.statusImageList.Images.SetKeyName(1, "Check.ico");
            this.statusImageList.Images.SetKeyName(2, "Cancel.ico");
            // 
            // lblKeyGenStatus
            // 
            this.lblKeyGenStatus.AutoSize = true;
            this.lblKeyGenStatus.Location = new System.Drawing.Point(15, 163);
            this.lblKeyGenStatus.Name = "lblKeyGenStatus";
            this.lblKeyGenStatus.Size = new System.Drawing.Size(82, 13);
            this.lblKeyGenStatus.TabIndex = 7;
            this.lblKeyGenStatus.Text = "-------------------------";
            // 
            // FormCreateNewUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 192);
            this.Controls.Add(this.lblKeyGenStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreateUser);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCreateNewUser";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New User";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCreateNewUser_FormClosing);
            this.Load += new System.EventHandler(this.FormCreateNewUser_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPassword2Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPasswordStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUserNameStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCreateUser;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblServerStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPasswordConfirm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picPassword2Status;
        private System.Windows.Forms.PictureBox picPasswordStatus;
        private System.Windows.Forms.PictureBox picUserNameStatus;
        private System.Windows.Forms.ImageList statusImageList;
        private System.Windows.Forms.Label lblKeyGenStatus;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}