namespace KeyFileGenerator
{
    partial class Form1
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFileSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbKb = new System.Windows.Forms.RadioButton();
            this.rbMb = new System.Windows.Forms.RadioButton();
            this.rbGb = new System.Windows.Forms.RadioButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(232, 73);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(94, 35);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File Path:";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(3, 25);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(285, 20);
            this.txtFilePath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(294, 20);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(32, 28);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFileSize
            // 
            this.txtFileSize.Location = new System.Drawing.Point(3, 73);
            this.txtFileSize.MaxLength = 10;
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.Size = new System.Drawing.Size(90, 20);
            this.txtFileSize.TabIndex = 4;
            this.txtFileSize.Text = "0";
            this.txtFileSize.TextChanged += new System.EventHandler(this.txtFileSize_TextChanged);
            this.txtFileSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFileSize_KeyDown);
            this.txtFileSize.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtFileSize_PreviewKeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "File Size:";
            // 
            // rbKb
            // 
            this.rbKb.AutoSize = true;
            this.rbKb.Checked = true;
            this.rbKb.Location = new System.Drawing.Point(55, 51);
            this.rbKb.Name = "rbKb";
            this.rbKb.Size = new System.Drawing.Size(38, 17);
            this.rbKb.TabIndex = 6;
            this.rbKb.TabStop = true;
            this.rbKb.Text = "Kb";
            this.rbKb.UseVisualStyleBackColor = true;
            // 
            // rbMb
            // 
            this.rbMb.AutoSize = true;
            this.rbMb.Location = new System.Drawing.Point(99, 51);
            this.rbMb.Name = "rbMb";
            this.rbMb.Size = new System.Drawing.Size(40, 17);
            this.rbMb.TabIndex = 7;
            this.rbMb.Text = "Mb";
            this.rbMb.UseVisualStyleBackColor = true;
            // 
            // rbGb
            // 
            this.rbGb.AutoSize = true;
            this.rbGb.Location = new System.Drawing.Point(145, 51);
            this.rbGb.Name = "rbGb";
            this.rbGb.Size = new System.Drawing.Size(39, 17);
            this.rbGb.TabIndex = 8;
            this.rbGb.Text = "Gb";
            this.rbGb.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 120);
            this.Controls.Add(this.rbGb);
            this.Controls.Add(this.rbMb);
            this.Controls.Add(this.rbKb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFileSize);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Key File Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFileSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbKb;
        private System.Windows.Forms.RadioButton rbMb;
        private System.Windows.Forms.RadioButton rbGb;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

