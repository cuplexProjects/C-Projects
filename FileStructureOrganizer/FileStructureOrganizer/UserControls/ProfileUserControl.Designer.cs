namespace FileStructureOrganizer.UserControls
{
    partial class ProfileUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtProfileName = new System.Windows.Forms.RichTextBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtScanResults = new System.Windows.Forms.RichTextBox();
            this.txtBasePath = new System.Windows.Forms.RichTextBox();
            this.txtSeriesCount = new System.Windows.Forms.RichTextBox();
            this.pnlMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtProfileName
            // 
            this.txtProfileName.BackColor = System.Drawing.SystemColors.Control;
            this.txtProfileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProfileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProfileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProfileName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtProfileName.Location = new System.Drawing.Point(3, 3);
            this.txtProfileName.Multiline = false;
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.ReadOnly = true;
            this.txtProfileName.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtProfileName.Size = new System.Drawing.Size(243, 13);
            this.txtProfileName.TabIndex = 0;
            this.txtProfileName.Text = "Profile name:";
            // 
            // pnlMain
            // 
            this.pnlMain.AutoSize = true;
            this.pnlMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.tableLayoutPanel1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(500, 40);
            this.pnlMain.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.txtScanResults, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBasePath, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtProfileName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSeriesCount, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(498, 38);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // txtScanResults
            // 
            this.txtScanResults.BackColor = System.Drawing.SystemColors.Control;
            this.txtScanResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtScanResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScanResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScanResults.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtScanResults.Location = new System.Drawing.Point(252, 3);
            this.txtScanResults.Multiline = false;
            this.txtScanResults.Name = "txtScanResults";
            this.txtScanResults.ReadOnly = true;
            this.txtScanResults.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtScanResults.Size = new System.Drawing.Size(243, 13);
            this.txtScanResults.TabIndex = 5;
            this.txtScanResults.Text = "Scan results:";
            // 
            // txtBasePath
            // 
            this.txtBasePath.BackColor = System.Drawing.SystemColors.Control;
            this.txtBasePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBasePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBasePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBasePath.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtBasePath.Location = new System.Drawing.Point(3, 22);
            this.txtBasePath.Multiline = false;
            this.txtBasePath.Name = "txtBasePath";
            this.txtBasePath.ReadOnly = true;
            this.txtBasePath.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtBasePath.Size = new System.Drawing.Size(243, 13);
            this.txtBasePath.TabIndex = 3;
            this.txtBasePath.Text = "Base path:";
            // 
            // txtSeriesCount
            // 
            this.txtSeriesCount.BackColor = System.Drawing.SystemColors.Control;
            this.txtSeriesCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSeriesCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSeriesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSeriesCount.Location = new System.Drawing.Point(252, 22);
            this.txtSeriesCount.Multiline = false;
            this.txtSeriesCount.Name = "txtSeriesCount";
            this.txtSeriesCount.ReadOnly = true;
            this.txtSeriesCount.Size = new System.Drawing.Size(243, 13);
            this.txtSeriesCount.TabIndex = 7;
            this.txtSeriesCount.Text = "Series count: ";
            // 
            // ProfileUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.MinimumSize = new System.Drawing.Size(300, 40);
            this.Name = "ProfileUserControl";
            this.Size = new System.Drawing.Size(500, 40);
            this.Load += new System.EventHandler(this.ProfileUserControl_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtProfileName;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.RichTextBox txtBasePath;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox txtScanResults;
        private System.Windows.Forms.RichTextBox txtSeriesCount;
    }
}
