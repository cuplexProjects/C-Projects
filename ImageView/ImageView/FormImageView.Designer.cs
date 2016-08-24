namespace ImageView
{
    partial class FormImageView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImageView));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyFilepathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithDefaultProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(484, 361);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseClick);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyFilepathToolStripMenuItem,
            this.openWithDefaultProgramToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 48);
            // 
            // copyFilepathToolStripMenuItem
            // 
            this.copyFilepathToolStripMenuItem.Name = "copyFilepathToolStripMenuItem";
            this.copyFilepathToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.copyFilepathToolStripMenuItem.Text = "Copy Filepath";
            this.copyFilepathToolStripMenuItem.Click += new System.EventHandler(this.copyFilepathToolStripMenuItem_Click);
            // 
            // openWithDefaultProgramToolStripMenuItem
            // 
            this.openWithDefaultProgramToolStripMenuItem.Name = "openWithDefaultProgramToolStripMenuItem";
            this.openWithDefaultProgramToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.openWithDefaultProgramToolStripMenuItem.Text = "Open In Default App";
            this.openWithDefaultProgramToolStripMenuItem.Click += new System.EventHandler(this.openWithDefaultProgramToolStripMenuItem_Click);
            // 
            // FormImageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.pictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "FormImageView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image Viewer";
            this.Activated += new System.EventHandler(this.FormImageView_Activated);
            this.Deactivate += new System.EventHandler(this.FormImageView_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormImageView_FormClosed);
            this.Load += new System.EventHandler(this.FormImageView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyFilepathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWithDefaultProgramToolStripMenuItem;
    }
}