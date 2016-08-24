namespace AlterlogicWebmailKeygen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtUserLimit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdPerUser = new System.Windows.Forms.RadioButton();
            this.rdUnlimited = new System.Windows.Forms.RadioButton();
            this.txtGeneratedKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dtPickerExpireDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtUserLimit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rdPerUser);
            this.groupBox1.Controls.Add(this.rdUnlimited);
            this.groupBox1.Controls.Add(this.txtGeneratedKey);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnGenerate);
            this.groupBox1.Controls.Add(this.dtPickerExpireDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "key generator properties";
            // 
            // txtUserLimit
            // 
            this.txtUserLimit.Enabled = false;
            this.txtUserLimit.Location = new System.Drawing.Point(89, 71);
            this.txtUserLimit.MaxLength = 6;
            this.txtUserLimit.Name = "txtUserLimit";
            this.txtUserLimit.Size = new System.Drawing.Size(85, 22);
            this.txtUserLimit.TabIndex = 8;
            this.txtUserLimit.Text = "100";
            this.txtUserLimit.TextChanged += new System.EventHandler(this.txtUserLimit_TextChanged);
            this.txtUserLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserLimit_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "User Limit:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Expire date:";
            // 
            // rdPerUser
            // 
            this.rdPerUser.AutoSize = true;
            this.rdPerUser.Location = new System.Drawing.Point(89, 46);
            this.rdPerUser.Name = "rdPerUser";
            this.rdPerUser.Size = new System.Drawing.Size(88, 17);
            this.rdPerUser.TabIndex = 5;
            this.rdPerUser.Text = "User Limited";
            this.rdPerUser.UseVisualStyleBackColor = true;
            this.rdPerUser.CheckedChanged += new System.EventHandler(this.rdPerUser_CheckedChanged);
            // 
            // rdUnlimited
            // 
            this.rdUnlimited.AutoSize = true;
            this.rdUnlimited.Checked = true;
            this.rdUnlimited.Location = new System.Drawing.Point(89, 23);
            this.rdUnlimited.Name = "rdUnlimited";
            this.rdUnlimited.Size = new System.Drawing.Size(156, 17);
            this.rdUnlimited.TabIndex = 4;
            this.rdUnlimited.TabStop = true;
            this.rdUnlimited.Text = "Unlimited - Server License";
            this.rdUnlimited.UseVisualStyleBackColor = true;
            this.rdUnlimited.CheckedChanged += new System.EventHandler(this.rdUnlimited_CheckedChanged);
            // 
            // txtGeneratedKey
            // 
            this.txtGeneratedKey.BackColor = System.Drawing.SystemColors.Window;
            this.txtGeneratedKey.Location = new System.Drawing.Point(12, 133);
            this.txtGeneratedKey.Name = "txtGeneratedKey";
            this.txtGeneratedKey.ReadOnly = true;
            this.txtGeneratedKey.Size = new System.Drawing.Size(335, 22);
            this.txtGeneratedKey.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "License Type:";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(353, 133);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(64, 24);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dtPickerExpireDate
            // 
            this.dtPickerExpireDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPickerExpireDate.Location = new System.Drawing.Point(87, 99);
            this.dtPickerExpireDate.MaxDate = new System.DateTime(2100, 1, 1, 0, 0, 0, 0);
            this.dtPickerExpireDate.MinDate = new System.DateTime(2014, 1, 1, 0, 0, 0, 0);
            this.dtPickerExpireDate.Name = "dtPickerExpireDate";
            this.dtPickerExpireDate.Size = new System.Drawing.Size(90, 22);
            this.dtPickerExpireDate.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 176);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Afterlogic Webmail pro key generator";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdPerUser;
        private System.Windows.Forms.RadioButton rdUnlimited;
        private System.Windows.Forms.TextBox txtGeneratedKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DateTimePicker dtPickerExpireDate;
        private System.Windows.Forms.TextBox txtUserLimit;
        private System.Windows.Forms.Label label3;
    }
}

