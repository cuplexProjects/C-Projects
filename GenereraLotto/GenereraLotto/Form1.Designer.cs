namespace GenereraLotto
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.lblGeneratedDescr = new System.Windows.Forms.Label();
            this.lnkAbout = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(334, 431);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(77, 29);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generera";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(251, 431);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 29);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Rensa";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pnlBoard
            // 
            this.pnlBoard.BackColor = System.Drawing.Color.White;
            this.pnlBoard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBoard.Location = new System.Drawing.Point(12, 12);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(400, 400);
            this.pnlBoard.TabIndex = 2;
            this.pnlBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBoard_Paint);
            // 
            // lblGeneratedDescr
            // 
            this.lblGeneratedDescr.AutoSize = true;
            this.lblGeneratedDescr.Location = new System.Drawing.Point(12, 415);
            this.lblGeneratedDescr.Name = "lblGeneratedDescr";
            this.lblGeneratedDescr.Size = new System.Drawing.Size(31, 13);
            this.lblGeneratedDescr.TabIndex = 3;
            this.lblGeneratedDescr.Text = "........";
            // 
            // lnkAbout
            // 
            this.lnkAbout.AutoSize = true;
            this.lnkAbout.Location = new System.Drawing.Point(9, 450);
            this.lnkAbout.Name = "lnkAbout";
            this.lnkAbout.Size = new System.Drawing.Size(81, 13);
            this.lnkAbout.TabIndex = 4;
            this.lnkAbout.TabStop = true;
            this.lnkAbout.Text = "Om programmet";
            this.lnkAbout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAbout_LinkClicked);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 472);
            this.Controls.Add(this.lnkAbout);
            this.Controls.Add(this.lblGeneratedDescr);
            this.Controls.Add(this.pnlBoard);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generera lottorad";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel pnlBoard;
        private System.Windows.Forms.Label lblGeneratedDescr;
        private System.Windows.Forms.LinkLabel lnkAbout;
    }
}

