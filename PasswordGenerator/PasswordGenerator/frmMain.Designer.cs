namespace PasswordGenerator
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
            this.btnGeneratePassword = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGeneratedPassword = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtPasswordLength = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // btnGeneratePassword
            // 
            this.btnGeneratePassword.Location = new System.Drawing.Point(308, 194);
            this.btnGeneratePassword.Name = "btnGeneratePassword";
            this.btnGeneratePassword.Size = new System.Drawing.Size(145, 27);
            this.btnGeneratePassword.TabIndex = 0;
            this.btnGeneratePassword.Text = "Generate Password";
            this.btnGeneratePassword.UseVisualStyleBackColor = true;
            this.btnGeneratePassword.Click += new System.EventHandler(this.btnGeneratePassword_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Password length:";
            // 
            // txtGeneratedPassword
            // 
            this.txtGeneratedPassword.Location = new System.Drawing.Point(15, 32);
            this.txtGeneratedPassword.Multiline = true;
            this.txtGeneratedPassword.Name = "txtGeneratedPassword";
            this.txtGeneratedPassword.ReadOnly = true;
            this.txtGeneratedPassword.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGeneratedPassword.Size = new System.Drawing.Size(437, 156);
            this.txtGeneratedPassword.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(222, 194);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 27);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtPasswordLength
            // 
            this.txtPasswordLength.Location = new System.Drawing.Point(106, 6);
            this.txtPasswordLength.Mask = "00000";
            this.txtPasswordLength.Name = "txtPasswordLength";
            this.txtPasswordLength.Size = new System.Drawing.Size(41, 20);
            this.txtPasswordLength.TabIndex = 5;
            this.txtPasswordLength.Text = "32";
            this.txtPasswordLength.ValidatingType = typeof(int);
            this.txtPasswordLength.Click += new System.EventHandler(this.txtPasswordLength_Click);
            this.txtPasswordLength.Enter += new System.EventHandler(this.txtPasswordLength_Enter);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 227);
            this.Controls.Add(this.txtPasswordLength);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtGeneratedPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGeneratePassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGeneratePassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGeneratedPassword;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.MaskedTextBox txtPasswordLength;
    }
}

