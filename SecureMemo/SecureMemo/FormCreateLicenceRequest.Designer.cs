namespace SecureMemo
{
    partial class FormCreateLicenceRequest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreateLicenceRequest));
            this.createLicenceRequestControl1 = new GeneralToolkitLib.UserControls.CreateLicenceRequestControl();
            this.SuspendLayout();
            // 
            // createLicenceRequestControl1
            // 
            this.createLicenceRequestControl1.Location = new System.Drawing.Point(12, 12);
            this.createLicenceRequestControl1.Name = "createLicenceRequestControl1";
            this.createLicenceRequestControl1.Size = new System.Drawing.Size(473, 399);
            this.createLicenceRequestControl1.TabIndex = 0;
            // 
            // FormCreateLicenceRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 410);
            this.Controls.Add(this.createLicenceRequestControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCreateLicenceRequest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Licence Request";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private GeneralToolkitLib.UserControls.CreateLicenceRequestControl createLicenceRequestControl1;
    }
}