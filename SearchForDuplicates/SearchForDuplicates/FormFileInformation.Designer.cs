using System;
namespace SearchForDuplicates
{
    partial class FormFileInformation
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
            this.grpFileInformation = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCompressed = new System.Windows.Forms.CheckBox();
            this.chkSystem = new System.Windows.Forms.CheckBox();
            this.chkHidden = new System.Windows.Forms.CheckBox();
            this.chkWriteProtected = new System.Windows.Forms.CheckBox();
            this.txtModifiedDate = new System.Windows.Forms.TextBox();
            this.txtCreatedDate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFileSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.BtnClose = new System.Windows.Forms.Button();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.grpFileInformation.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpFileInformation
            // 
            this.grpFileInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grpFileInformation.Controls.Add(this.groupBox1);
            this.grpFileInformation.Controls.Add(this.txtModifiedDate);
            this.grpFileInformation.Controls.Add(this.txtCreatedDate);
            this.grpFileInformation.Controls.Add(this.label7);
            this.grpFileInformation.Controls.Add(this.label6);
            this.grpFileInformation.Controls.Add(this.label4);
            this.grpFileInformation.Controls.Add(this.label3);
            this.grpFileInformation.Controls.Add(this.txtFileSize);
            this.grpFileInformation.Controls.Add(this.label2);
            this.grpFileInformation.Controls.Add(this.txtFilePath);
            this.grpFileInformation.Controls.Add(this.label1);
            this.grpFileInformation.Controls.Add(this.txtFileName);
            this.grpFileInformation.Controls.Add(this.BtnClose);
            this.grpFileInformation.Controls.Add(this.lstFiles);
            this.grpFileInformation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.grpFileInformation.Location = new System.Drawing.Point(0, 0);
            this.grpFileInformation.Margin = new System.Windows.Forms.Padding(0);
            this.grpFileInformation.Name = "grpFileInformation";
            this.grpFileInformation.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.grpFileInformation.Size = new System.Drawing.Size(445, 275);
            this.grpFileInformation.TabIndex = 0;
            this.grpFileInformation.TabStop = false;
            this.grpFileInformation.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grpFileInformation_MouseMove);
            this.grpFileInformation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grpFileInformation_MouseUp);
            this.grpFileInformation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grpFileInformation_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkCompressed);
            this.groupBox1.Controls.Add(this.chkSystem);
            this.groupBox1.Controls.Add(this.chkHidden);
            this.groupBox1.Controls.Add(this.chkWriteProtected);
            this.groupBox1.Location = new System.Drawing.Point(198, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 71);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Attributes";
            // 
            // chkCompressed
            // 
            this.chkCompressed.AutoCheck = false;
            this.chkCompressed.AutoSize = true;
            this.chkCompressed.Location = new System.Drawing.Point(114, 42);
            this.chkCompressed.Name = "chkCompressed";
            this.chkCompressed.Size = new System.Drawing.Size(84, 17);
            this.chkCompressed.TabIndex = 18;
            this.chkCompressed.Text = "Compressed";
            this.chkCompressed.UseVisualStyleBackColor = true;
            // 
            // chkSystem
            // 
            this.chkSystem.AutoCheck = false;
            this.chkSystem.AutoSize = true;
            this.chkSystem.Location = new System.Drawing.Point(6, 42);
            this.chkSystem.Name = "chkSystem";
            this.chkSystem.Size = new System.Drawing.Size(60, 17);
            this.chkSystem.TabIndex = 17;
            this.chkSystem.Text = "System";
            this.chkSystem.UseVisualStyleBackColor = true;
            // 
            // chkHidden
            // 
            this.chkHidden.AutoCheck = false;
            this.chkHidden.AutoSize = true;
            this.chkHidden.Location = new System.Drawing.Point(114, 19);
            this.chkHidden.Name = "chkHidden";
            this.chkHidden.Size = new System.Drawing.Size(60, 17);
            this.chkHidden.TabIndex = 16;
            this.chkHidden.Text = "Hidden";
            this.chkHidden.UseVisualStyleBackColor = true;
            // 
            // chkWriteProtected
            // 
            this.chkWriteProtected.AutoCheck = false;
            this.chkWriteProtected.AutoSize = true;
            this.chkWriteProtected.Location = new System.Drawing.Point(6, 19);
            this.chkWriteProtected.Name = "chkWriteProtected";
            this.chkWriteProtected.Size = new System.Drawing.Size(100, 17);
            this.chkWriteProtected.TabIndex = 15;
            this.chkWriteProtected.Text = "Write Protected";
            this.chkWriteProtected.UseVisualStyleBackColor = true;
            // 
            // txtModifiedDate
            // 
            this.txtModifiedDate.BackColor = System.Drawing.Color.White;
            this.txtModifiedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtModifiedDate.Location = new System.Drawing.Point(255, 132);
            this.txtModifiedDate.Name = "txtModifiedDate";
            this.txtModifiedDate.ReadOnly = true;
            this.txtModifiedDate.Size = new System.Drawing.Size(117, 20);
            this.txtModifiedDate.TabIndex = 13;
            // 
            // txtCreatedDate
            // 
            this.txtCreatedDate.BackColor = System.Drawing.Color.White;
            this.txtCreatedDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreatedDate.Location = new System.Drawing.Point(255, 106);
            this.txtCreatedDate.Name = "txtCreatedDate";
            this.txtCreatedDate.ReadOnly = true;
            this.txtCreatedDate.Size = new System.Drawing.Size(117, 20);
            this.txtCreatedDate.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(195, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Modified:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(195, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Created:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Files:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "File Size:";
            // 
            // txtFileSize
            // 
            this.txtFileSize.BackColor = System.Drawing.Color.White;
            this.txtFileSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileSize.Location = new System.Drawing.Point(255, 80);
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.ReadOnly = true;
            this.txtFileSize.Size = new System.Drawing.Size(117, 20);
            this.txtFileSize.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Path:";
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.Color.White;
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath.Location = new System.Drawing.Point(255, 54);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(175, 20);
            this.txtFilePath.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "File Name:";
            // 
            // txtFileName
            // 
            this.txtFileName.BackColor = System.Drawing.Color.White;
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileName.Location = new System.Drawing.Point(255, 28);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(175, 20);
            this.txtFileName.TabIndex = 2;
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(347, 245);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(83, 21);
            this.BtnClose.TabIndex = 1;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.HorizontalScrollbar = true;
            this.lstFiles.Location = new System.Drawing.Point(8, 28);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(178, 238);
            this.lstFiles.TabIndex = 0;
            this.lstFiles.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            // 
            // FileInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(447, 278);
            this.ControlBox = false;
            this.Controls.Add(this.grpFileInformation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileInformation";
            this.Opacity = 0.9;
            this.Text = "File Information";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Gray;
            this.Load += new System.EventHandler(this.FileInformation_Load);
            this.grpFileInformation.ResumeLayout(false);
            this.grpFileInformation.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFileInformation;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFileSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.TextBox txtModifiedDate;
        private System.Windows.Forms.TextBox txtCreatedDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCompressed;
        private System.Windows.Forms.CheckBox chkSystem;
        private System.Windows.Forms.CheckBox chkHidden;
        private System.Windows.Forms.CheckBox chkWriteProtected;
    }
}