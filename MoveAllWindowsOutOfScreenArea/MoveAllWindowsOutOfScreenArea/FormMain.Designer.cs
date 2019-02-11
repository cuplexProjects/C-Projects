namespace MoveAllWindowsOutOfScreenArea
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxWindows = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMoveWindows = new System.Windows.Forms.Button();
            this.btnFindWindows = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(374, 252);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxWindows);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(374, 198);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Windows out of bounds";
            // 
            // listBoxWindows
            // 
            this.listBoxWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxWindows.FormattingEnabled = true;
            this.listBoxWindows.Location = new System.Drawing.Point(6, 19);
            this.listBoxWindows.Name = "listBoxWindows";
            this.listBoxWindows.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxWindows.Size = new System.Drawing.Size(362, 173);
            this.listBoxWindows.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMoveWindows);
            this.groupBox2.Controls.Add(this.btnFindWindows);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 198);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.groupBox2.MinimumSize = new System.Drawing.Size(100, 25);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(374, 54);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // btnMoveWindows
            // 
            this.btnMoveWindows.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnMoveWindows.Location = new System.Drawing.Point(226, 19);
            this.btnMoveWindows.Name = "btnMoveWindows";
            this.btnMoveWindows.Size = new System.Drawing.Size(141, 25);
            this.btnMoveWindows.TabIndex = 1;
            this.btnMoveWindows.Text = "Move Selected windows";
            this.btnMoveWindows.UseVisualStyleBackColor = true;
            this.btnMoveWindows.Click += new System.EventHandler(this.btnMoveWindows_Click);
            // 
            // btnFindWindows
            // 
            this.btnFindWindows.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFindWindows.Location = new System.Drawing.Point(120, 19);
            this.btnFindWindows.Name = "btnFindWindows";
            this.btnFindWindows.Size = new System.Drawing.Size(100, 25);
            this.btnFindWindows.TabIndex = 0;
            this.btnFindWindows.Text = "Find Windows";
            this.btnFindWindows.UseVisualStyleBackColor = true;
            this.btnFindWindows.Click += new System.EventHandler(this.btnFindWindows_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 262);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Move windows out of bounds";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxWindows;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnMoveWindows;
        private System.Windows.Forms.Button btnFindWindows;


    }
}

