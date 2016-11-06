namespace ImageView
{
    partial class FormThumbnailView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThumbnailView));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.grpBoxControls = new System.Windows.Forms.GroupBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.picBoxMaximized = new System.Windows.Forms.PictureBox();
            this.btnScanDirectory = new System.Windows.Forms.Button();
            this.grpBoxControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMaximized)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Location = new System.Drawing.Point(5, 526);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(772, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 70);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(768, 452);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // grpBoxControls
            // 
            this.grpBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxControls.Controls.Add(this.btnScanDirectory);
            this.grpBoxControls.Controls.Add(this.btnSettings);
            this.grpBoxControls.Controls.Add(this.btnGenerate);
            this.grpBoxControls.Location = new System.Drawing.Point(5, 8);
            this.grpBoxControls.Name = "grpBoxControls";
            this.grpBoxControls.Size = new System.Drawing.Size(772, 55);
            this.grpBoxControls.TabIndex = 2;
            this.grpBoxControls.TabStop = false;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(101, 19);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(92, 30);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(3, 19);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(92, 30);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // picBoxMaximized
            // 
            this.picBoxMaximized.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxMaximized.Location = new System.Drawing.Point(9, 69);
            this.picBoxMaximized.Name = "picBoxMaximized";
            this.picBoxMaximized.Size = new System.Drawing.Size(764, 453);
            this.picBoxMaximized.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxMaximized.TabIndex = 0;
            this.picBoxMaximized.TabStop = false;
            this.picBoxMaximized.Click += new System.EventHandler(this.picBoxMaximized_Click);
            // 
            // btnScanDirectory
            // 
            this.btnScanDirectory.Location = new System.Drawing.Point(199, 19);
            this.btnScanDirectory.Name = "btnScanDirectory";
            this.btnScanDirectory.Size = new System.Drawing.Size(120, 30);
            this.btnScanDirectory.TabIndex = 2;
            this.btnScanDirectory.Text = "Scan Directory";
            this.btnScanDirectory.UseVisualStyleBackColor = true;
            this.btnScanDirectory.Click += new System.EventHandler(this.btnScanDirectory_Click);
            // 
            // FormThumbnailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.grpBoxControls);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.picBoxMaximized);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FormThumbnailView";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thumbnails";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormThumbnailView_FormClosing);
            this.Load += new System.EventHandler(this.FormThumbnailView_Load);
            this.grpBoxControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMaximized)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox grpBoxControls;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.PictureBox picBoxMaximized;
        private System.Windows.Forms.Button btnScanDirectory;
    }
}