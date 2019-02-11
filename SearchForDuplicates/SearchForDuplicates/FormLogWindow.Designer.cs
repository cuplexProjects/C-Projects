namespace SearchForDuplicates
{
    partial class FormLogWindow
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
            this.btnClose = new System.Windows.Forms.Button();
            this.lstLogs = new System.Windows.Forms.ListBox();
            this.txtLogData = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(183, 242);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(135, 25);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lstLogs
            // 
            this.lstLogs.FormattingEnabled = true;
            this.lstLogs.Location = new System.Drawing.Point(12, 12);
            this.lstLogs.Name = "lstLogs";
            this.lstLogs.Size = new System.Drawing.Size(152, 225);
            this.lstLogs.TabIndex = 1;
            this.lstLogs.SelectedIndexChanged += new System.EventHandler(this.lstLogs_SelectedIndexChanged);
            // 
            // txtLogData
            // 
            this.txtLogData.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtLogData.Location = new System.Drawing.Point(170, 12);
            this.txtLogData.Multiline = true;
            this.txtLogData.Name = "txtLogData";
            this.txtLogData.ReadOnly = true;
            this.txtLogData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogData.Size = new System.Drawing.Size(318, 224);
            this.txtLogData.TabIndex = 2;
            // 
            // LogWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 276);
            this.Controls.Add(this.txtLogData);
            this.Controls.Add(this.lstLogs);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LogWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log Window";
            this.Load += new System.EventHandler(this.LogWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox lstLogs;
        private System.Windows.Forms.TextBox txtLogData;
    }
}