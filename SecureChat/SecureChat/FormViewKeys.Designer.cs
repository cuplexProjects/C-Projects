namespace SecureChat
{
    partial class FormViewKeys
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
            this.txtPublicKey = new System.Windows.Forms.TextBox();
            this.txtCompleteKey = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblGUID = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtPublicKey.CausesValidation = false;
            this.txtPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPublicKey.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPublicKey.Location = new System.Drawing.Point(3, 19);
            this.txtPublicKey.Multiline = true;
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.ReadOnly = true;
            this.txtPublicKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPublicKey.Size = new System.Drawing.Size(568, 178);
            this.txtPublicKey.TabIndex = 0;
            this.txtPublicKey.WordWrap = false;
            this.txtPublicKey.Enter += new System.EventHandler(this.txtPublicKey_Enter);
            // 
            // txtCompleteKey
            // 
            this.txtCompleteKey.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtCompleteKey.CausesValidation = false;
            this.txtCompleteKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCompleteKey.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompleteKey.Location = new System.Drawing.Point(3, 19);
            this.txtCompleteKey.Multiline = true;
            this.txtCompleteKey.Name = "txtCompleteKey";
            this.txtCompleteKey.ReadOnly = true;
            this.txtCompleteKey.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCompleteKey.Size = new System.Drawing.Size(568, 309);
            this.txtCompleteKey.TabIndex = 1;
            this.txtCompleteKey.WordWrap = false;
            this.txtCompleteKey.Enter += new System.EventHandler(this.txtCompleteKey_Enter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPublicKey);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(574, 200);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Public Key Information";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCompleteKey);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(5, 205);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(574, 331);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Complete Key Set - Keep this private!";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblGUID});
            this.statusStrip1.Location = new System.Drawing.Point(5, 514);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(574, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(41, 17);
            this.toolStripStatusLabel1.Text = "GUID:";
            // 
            // lblGUID
            // 
            this.lblGUID.Name = "lblGUID";
            this.lblGUID.Size = new System.Drawing.Size(29, 17);
            this.lblGUID.Text = "N/A";
            // 
            // FormViewKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 541);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "FormViewKeys";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Asymetric Encryption Key pairs";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormViewKeys_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.TextBox txtCompleteKey;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblGUID;
    }
}