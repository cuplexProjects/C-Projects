namespace PrefetchFile
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnReadFile = new System.Windows.Forms.Button();
            this.lblFilename = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.barSelector = new PrefetchFile.UserControls.SelectionBar();
            this.lblFileReadRange = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnReadFile
            // 
            this.btnReadFile.Enabled = false;
            this.btnReadFile.Location = new System.Drawing.Point(430, 132);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(95, 29);
            this.btnReadFile.TabIndex = 0;
            this.btnReadFile.Text = "Read File";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(12, 9);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(69, 17);
            this.lblFilename.TabIndex = 1;
            this.lblFilename.Text = "Filename:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 100);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(513, 26);
            this.progressBar.TabIndex = 3;
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(87, 6);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(385, 22);
            this.txtFilename.TabIndex = 5;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(478, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(47, 29);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // barSelector
            // 
            this.barSelector.Location = new System.Drawing.Point(15, 47);
            this.barSelector.Name = "barSelector";
            this.barSelector.SelectionStart = 0;
            this.barSelector.Size = new System.Drawing.Size(510, 36);
            this.barSelector.TabIndex = 7;
            this.barSelector.Text = "barSelector1";
            this.barSelector.SelectionChanged += new System.EventHandler(this.barSelector_SelectionChanged);
            // 
            // lblFileReadRange
            // 
            this.lblFileReadRange.AutoSize = true;
            this.lblFileReadRange.Location = new System.Drawing.Point(12, 138);
            this.lblFileReadRange.Name = "lblFileReadRange";
            this.lblFileReadRange.Size = new System.Drawing.Size(120, 17);
            this.lblFileReadRange.TabIndex = 8;
            this.lblFileReadRange.Text = "lblFileReadRange";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 173);
            this.Controls.Add(this.lblFileReadRange);
            this.Controls.Add(this.barSelector);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblFilename);
            this.Controls.Add(this.btnReadFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prefetch File";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnBrowse;
        private UserControls.SelectionBar barSelector;
        private System.Windows.Forms.Label lblFileReadRange;
    }
}

