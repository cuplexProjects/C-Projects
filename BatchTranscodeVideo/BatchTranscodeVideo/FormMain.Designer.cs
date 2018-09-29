namespace BatchTranscodeVideo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstProfile = new System.Windows.Forms.ComboBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdateProfiles = new System.Windows.Forms.Button();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.pBarEncoding = new System.Windows.Forms.ProgressBar();
            this.txtEncodingProgress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtOutputDirectory);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.MinimumSize = new System.Drawing.Size(300, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(716, 322);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files to convert";
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(1031, 96);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 24);
            this.button5.TabIndex = 10;
            this.button5.Text = "Remove All";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(1031, 65);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 24);
            this.button4.TabIndex = 9;
            this.button4.Text = "Remove";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(597, 262);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(113, 24);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(6, 273);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Output Folder";
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputDirectory.Location = new System.Drawing.Point(9, 291);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.ReadOnly = true;
            this.txtOutputDirectory.Size = new System.Drawing.Size(701, 20);
            this.txtOutputDirectory.TabIndex = 6;
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Location = new System.Drawing.Point(9, 41);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(697, 207);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Encoding";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Size";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1031, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add Files";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select file(s) to encode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Profile";
            // 
            // lstProfile
            // 
            this.lstProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstProfile.FormattingEnabled = true;
            this.lstProfile.Location = new System.Drawing.Point(9, 35);
            this.lstProfile.Name = "lstProfile";
            this.lstProfile.Size = new System.Drawing.Size(150, 21);
            this.lstProfile.TabIndex = 3;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(51, 149);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(108, 24);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnUpdateProfiles);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.btnShowLog);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.btnStart);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lstProfile);
            this.groupBox2.Location = new System.Drawing.Point(734, 12);
            this.groupBox2.MinimumSize = new System.Drawing.Size(170, 300);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 322);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Conversion Settings";
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button7.Location = new System.Drawing.Point(89, 292);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(70, 24);
            this.button7.TabIndex = 7;
            this.button7.Text = "Open";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // btnShowLog
            // 
            this.btnShowLog.Location = new System.Drawing.Point(51, 209);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(108, 24);
            this.btnShowLog.TabIndex = 6;
            this.btnShowLog.Text = "Show FFMpeg log";
            this.btnShowLog.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 65);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Merge Files";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(51, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 24);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUpdateProfiles
            // 
            this.btnUpdateProfiles.Location = new System.Drawing.Point(51, 88);
            this.btnUpdateProfiles.Name = "btnUpdateProfiles";
            this.btnUpdateProfiles.Size = new System.Drawing.Size(108, 24);
            this.btnUpdateProfiles.TabIndex = 9;
            this.btnUpdateProfiles.Text = "Reload Profiles";
            this.btnUpdateProfiles.UseVisualStyleBackColor = true;
            // 
            // grpProgress
            // 
            this.grpProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProgress.Controls.Add(this.txtEncodingProgress);
            this.grpProgress.Controls.Add(this.pBarEncoding);
            this.grpProgress.Location = new System.Drawing.Point(12, 340);
            this.grpProgress.MaximumSize = new System.Drawing.Size(0, 80);
            this.grpProgress.MinimumSize = new System.Drawing.Size(150, 70);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(901, 70);
            this.grpProgress.TabIndex = 7;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress";
            // 
            // pBarEncoding
            // 
            this.pBarEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBarEncoding.Location = new System.Drawing.Point(5, 19);
            this.pBarEncoding.Name = "pBarEncoding";
            this.pBarEncoding.Size = new System.Drawing.Size(885, 20);
            this.pBarEncoding.TabIndex = 0;
            // 
            // txtEncodingProgress
            // 
            this.txtEncodingProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncodingProgress.Location = new System.Drawing.Point(5, 44);
            this.txtEncodingProgress.Name = "txtEncodingProgress";
            this.txtEncodingProgress.ReadOnly = true;
            this.txtEncodingProgress.Size = new System.Drawing.Size(885, 20);
            this.txtEncodingProgress.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 38);
            this.label3.TabIndex = 10;
            this.label3.Text = "Open output directory in windows explorer";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 411);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(750, 450);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batch Transcode mp4 video with ffmpeg and Cuda";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox lstProfile;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button btnShowLog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdateProfiles;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.TextBox txtEncodingProgress;
        private System.Windows.Forms.ProgressBar pBarEncoding;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

