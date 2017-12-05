namespace FileChecksumUtil
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBarHash = new System.Windows.Forms.ProgressBar();
            this.txtSHA256 = new System.Windows.Forms.TextBox();
            this.txtCRC32 = new System.Windows.Forms.TextBox();
            this.radioButtonMD5 = new System.Windows.Forms.RadioButton();
            this.radioButtonCRC32 = new System.Windows.Forms.RadioButton();
            this.radioButtonSha256 = new System.Windows.Forms.RadioButton();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.txtMD5 = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorkerCalculateHash = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtInputFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 211);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBarHash);
            this.groupBox2.Controls.Add(this.txtSHA256);
            this.groupBox2.Controls.Add(this.txtCRC32);
            this.groupBox2.Controls.Add(this.radioButtonMD5);
            this.groupBox2.Controls.Add(this.radioButtonCRC32);
            this.groupBox2.Controls.Add(this.radioButtonSha256);
            this.groupBox2.Controls.Add(this.btnCalculate);
            this.groupBox2.Controls.Add(this.txtMD5);
            this.groupBox2.Location = new System.Drawing.Point(6, 51);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 149);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Result View";
            // 
            // progressBarHash
            // 
            this.progressBarHash.Location = new System.Drawing.Point(12, 118);
            this.progressBarHash.Name = "progressBarHash";
            this.progressBarHash.Size = new System.Drawing.Size(347, 23);
            this.progressBarHash.TabIndex = 9;
            // 
            // txtSHA256
            // 
            this.txtSHA256.Location = new System.Drawing.Point(101, 79);
            this.txtSHA256.Name = "txtSHA256";
            this.txtSHA256.ReadOnly = true;
            this.txtSHA256.Size = new System.Drawing.Size(351, 27);
            this.txtSHA256.TabIndex = 8;
            this.txtSHA256.Click += new System.EventHandler(this.txtPath_Click);
            // 
            // txtCRC32
            // 
            this.txtCRC32.Location = new System.Drawing.Point(101, 50);
            this.txtCRC32.Name = "txtCRC32";
            this.txtCRC32.ReadOnly = true;
            this.txtCRC32.Size = new System.Drawing.Size(351, 27);
            this.txtCRC32.TabIndex = 7;
            this.txtCRC32.Click += new System.EventHandler(this.txtPath_Click);
            // 
            // radioButtonMD5
            // 
            this.radioButtonMD5.AutoSize = true;
            this.radioButtonMD5.Checked = true;
            this.radioButtonMD5.Location = new System.Drawing.Point(12, 22);
            this.radioButtonMD5.Name = "radioButtonMD5";
            this.radioButtonMD5.Size = new System.Drawing.Size(62, 24);
            this.radioButtonMD5.TabIndex = 1;
            this.radioButtonMD5.TabStop = true;
            this.radioButtonMD5.Text = "MD5";
            this.radioButtonMD5.UseVisualStyleBackColor = true;
            // 
            // radioButtonCRC32
            // 
            this.radioButtonCRC32.AutoSize = true;
            this.radioButtonCRC32.Location = new System.Drawing.Point(12, 51);
            this.radioButtonCRC32.Name = "radioButtonCRC32";
            this.radioButtonCRC32.Size = new System.Drawing.Size(73, 24);
            this.radioButtonCRC32.TabIndex = 5;
            this.radioButtonCRC32.Text = "CRC32";
            this.radioButtonCRC32.UseVisualStyleBackColor = true;
            // 
            // radioButtonSha256
            // 
            this.radioButtonSha256.AutoSize = true;
            this.radioButtonSha256.Location = new System.Drawing.Point(12, 80);
            this.radioButtonSha256.Name = "radioButtonSha256";
            this.radioButtonSha256.Size = new System.Drawing.Size(83, 24);
            this.radioButtonSha256.TabIndex = 6;
            this.radioButtonSha256.Text = "SHA256";
            this.radioButtonSha256.UseVisualStyleBackColor = true;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Enabled = false;
            this.btnCalculate.Location = new System.Drawing.Point(375, 115);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 26);
            this.btnCalculate.TabIndex = 2;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // txtMD5
            // 
            this.txtMD5.Location = new System.Drawing.Point(101, 21);
            this.txtMD5.Name = "txtMD5";
            this.txtMD5.ReadOnly = true;
            this.txtMD5.Size = new System.Drawing.Size(351, 27);
            this.txtMD5.TabIndex = 3;
            this.txtMD5.Click += new System.EventHandler(this.txtPath_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(429, 21);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(35, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtInputFile
            // 
            this.txtInputFile.AllowDrop = true;
            this.txtInputFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtInputFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtInputFile.Location = new System.Drawing.Point(49, 22);
            this.txtInputFile.MaxLength = 1024;
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(374, 27);
            this.txtInputFile.TabIndex = 4;
            this.txtInputFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragDrop);
            this.txtInputFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragEnter);
            this.txtInputFile.Validating += new System.ComponentModel.CancelEventHandler(this.txtInputFile_Validating);
            this.txtInputFile.Validated += new System.EventHandler(this.txtInputFile_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            // 
            // backgroundWorkerCalculateHash
            // 
            this.backgroundWorkerCalculateHash.WorkerSupportsCancellation = true;
            this.backgroundWorkerCalculateHash.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalculateHash_DoWork);
            this.backgroundWorkerCalculateHash.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalculateHash_RunWorkerCompleted);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 221);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 4, 5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Checksum Util";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.RadioButton radioButtonSha256;
        private System.Windows.Forms.RadioButton radioButtonCRC32;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.TextBox txtMD5;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.RadioButton radioButtonMD5;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCalculateHash;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSHA256;
        private System.Windows.Forms.TextBox txtCRC32;
        private System.Windows.Forms.ProgressBar progressBarHash;
    }
}

