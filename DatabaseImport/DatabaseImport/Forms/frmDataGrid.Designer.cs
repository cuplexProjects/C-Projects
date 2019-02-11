namespace DatabaseImport.Forms
{
    partial class frmDataGrid
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
            this.panelDgvContainer = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelDgvContainer
            // 
            this.panelDgvContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDgvContainer.Location = new System.Drawing.Point(0, 0);
            this.panelDgvContainer.Name = "panelDgvContainer";
            this.panelDgvContainer.Padding = new System.Windows.Forms.Padding(5);
            this.panelDgvContainer.Size = new System.Drawing.Size(484, 366);
            this.panelDgvContainer.TabIndex = 0;
            // 
            // frmDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 366);
            this.Controls.Add(this.panelDgvContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(250, 200);
            this.Name = "frmDataGrid";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Table Column Structure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDataGrid_FormClosing);
            this.Load += new System.EventHandler(this.frmDataGrid_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDgvContainer;
    }
}