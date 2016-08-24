namespace FileSystemImage
{
    partial class frmSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearch));
            this.btnSearch = new System.Windows.Forms.Button();
            this.chkRegexp = new System.Windows.Forms.CheckBox();
            this.lblFilename = new System.Windows.Forms.Label();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.grpSearch = new System.Windows.Forms.GroupBox();
            this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSearchResCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpSearch.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(314, 32);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(65, 25);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkRegexp
            // 
            this.chkRegexp.AutoSize = true;
            this.chkRegexp.Location = new System.Drawing.Point(133, 17);
            this.chkRegexp.Name = "chkRegexp";
            this.chkRegexp.Size = new System.Drawing.Size(175, 17);
            this.chkRegexp.TabIndex = 1;
            this.chkRegexp.Text = "Search with Regular expression";
            this.chkRegexp.UseVisualStyleBackColor = true;
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilename.Location = new System.Drawing.Point(6, 44);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(106, 13);
            this.lblFilename.TabIndex = 2;
            this.lblFilename.Text = "Filename / regex:";
            // 
            // lstResults
            // 
            this.lstResults.FormattingEnabled = true;
            this.lstResults.Location = new System.Drawing.Point(12, 108);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(385, 225);
            this.lstResults.TabIndex = 3;
            this.lstResults.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstResults_KeyPress);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(6, 63);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(373, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // grpSearch
            // 
            this.grpSearch.Controls.Add(this.chkIgnoreCase);
            this.grpSearch.Controls.Add(this.lblFilename);
            this.grpSearch.Controls.Add(this.btnSearch);
            this.grpSearch.Controls.Add(this.txtSearch);
            this.grpSearch.Controls.Add(this.chkRegexp);
            this.grpSearch.Location = new System.Drawing.Point(12, 12);
            this.grpSearch.Name = "grpSearch";
            this.grpSearch.Size = new System.Drawing.Size(385, 92);
            this.grpSearch.TabIndex = 0;
            this.grpSearch.TabStop = false;
            this.grpSearch.Text = "Search parameters";
            // 
            // chkIgnoreCase
            // 
            this.chkIgnoreCase.AutoSize = true;
            this.chkIgnoreCase.Checked = true;
            this.chkIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreCase.Location = new System.Drawing.Point(133, 40);
            this.chkIgnoreCase.Name = "chkIgnoreCase";
            this.chkIgnoreCase.Size = new System.Drawing.Size(82, 17);
            this.chkIgnoreCase.TabIndex = 3;
            this.chkIgnoreCase.Text = "Ignore case";
            this.chkIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripSearchResCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 340);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(409, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(82, 17);
            this.toolStripStatusLabel1.Text = "Search results:";
            // 
            // toolStripSearchResCount
            // 
            this.toolStripSearchResCount.Name = "toolStripSearchResCount";
            this.toolStripSearchResCount.Size = new System.Drawing.Size(13, 17);
            this.toolStripSearchResCount.Text = "0";
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 362);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lstResults);
            this.Controls.Add(this.grpSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(425, 400);
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            this.Load += new System.EventHandler(this.frmSearch_Load);
            this.Resize += new System.EventHandler(this.frmSearch_Resize);
            this.grpSearch.ResumeLayout(false);
            this.grpSearch.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox chkRegexp;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.GroupBox grpSearch;
        private System.Windows.Forms.CheckBox chkIgnoreCase;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSearchResCount;
    }
}