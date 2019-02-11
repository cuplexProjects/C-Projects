namespace SecureMemo
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.btnOk = new System.Windows.Forms.Button();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.licenceInfoControl1 = new GeneralToolkitLib.UserControls.LicenceInfoControl();
            this.lblAppInfo1 = new System.Windows.Forms.Label();
            this.lblAppInfo2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(272, 319);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppTitle.Location = new System.Drawing.Point(10, 9);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(101, 25);
            this.lblAppTitle.TabIndex = 4;
            this.lblAppTitle.Text = "AppName";
            // 
            // licenceInfoControl1
            // 
            this.licenceInfoControl1.Location = new System.Drawing.Point(10, 78);
            this.licenceInfoControl1.Name = "licenceInfoControl1";
            this.licenceInfoControl1.NotRegisteredInfoText = null;
            this.licenceInfoControl1.Size = new System.Drawing.Size(362, 235);
            this.licenceInfoControl1.TabIndex = 6;
            this.licenceInfoControl1.Load += new System.EventHandler(this.licenceInfoControl1_Load);
            // 
            // lblAppInfo1
            // 
            this.lblAppInfo1.AutoSize = true;
            this.lblAppInfo1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppInfo1.Location = new System.Drawing.Point(12, 34);
            this.lblAppInfo1.Name = "lblAppInfo1";
            this.lblAppInfo1.Size = new System.Drawing.Size(106, 17);
            this.lblAppInfo1.TabIndex = 7;
            this.lblAppInfo1.Text = "Application info1";
            // 
            // lblAppInfo2
            // 
            this.lblAppInfo2.AutoSize = true;
            this.lblAppInfo2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppInfo2.Location = new System.Drawing.Point(12, 51);
            this.lblAppInfo2.Name = "lblAppInfo2";
            this.lblAppInfo2.Size = new System.Drawing.Size(106, 17);
            this.lblAppInfo2.TabIndex = 8;
            this.lblAppInfo2.Text = "Application info2";
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 356);
            this.Controls.Add(this.lblAppInfo2);
            this.Controls.Add(this.lblAppInfo1);
            this.Controls.Add(this.licenceInfoControl1);
            this.Controls.Add(this.lblAppTitle);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblAppTitle;
        private GeneralToolkitLib.UserControls.LicenceInfoControl licenceInfoControl1;
        private System.Windows.Forms.Label lblAppInfo1;
        private System.Windows.Forms.Label lblAppInfo2;
    }
}