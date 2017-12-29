﻿namespace ImageView
{
    partial class FormThumbnailSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThumbnailSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentDbSize = new System.Windows.Forms.Label();
            this.lblCachedItems = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClearDatabase = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnReduceCachSize = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRunDefragmentJob = new System.Windows.Forms.Button();
            this.numericSize = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemoveFilesNotFound = new System.Windows.Forms.Button();
            this.toolTipOptimize = new System.Windows.Forms.ToolTip(this.components);
            this.lblInfo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSize)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(435, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data usage summary";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentDbSize, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblCachedItems, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 21);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(413, 65);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Cached Thumbnails:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thumbnail Database Size:";
            // 
            // lblCurrentDbSize
            // 
            this.lblCurrentDbSize.AutoSize = true;
            this.lblCurrentDbSize.Location = new System.Drawing.Point(209, 4);
            this.lblCurrentDbSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.lblCurrentDbSize.Name = "lblCurrentDbSize";
            this.lblCurrentDbSize.Size = new System.Drawing.Size(20, 17);
            this.lblCurrentDbSize.TabIndex = 1;
            this.lblCurrentDbSize.Text = "...";
            // 
            // lblCachedItems
            // 
            this.lblCachedItems.AutoSize = true;
            this.lblCachedItems.Location = new System.Drawing.Point(209, 34);
            this.lblCachedItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.lblCachedItems.Name = "lblCachedItems";
            this.lblCachedItems.Size = new System.Drawing.Size(20, 17);
            this.lblCachedItems.TabIndex = 6;
            this.lblCachedItems.Text = "...";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(327, 338);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 38);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(12, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 229);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tools";
            // 
            // btnClearDatabase
            // 
            this.btnClearDatabase.Enabled = false;
            this.btnClearDatabase.Location = new System.Drawing.Point(281, 22);
            this.btnClearDatabase.Name = "btnClearDatabase";
            this.btnClearDatabase.Size = new System.Drawing.Size(120, 34);
            this.btnClearDatabase.TabIndex = 2;
            this.btnClearDatabase.Text = "Clear Database";
            this.btnClearDatabase.UseVisualStyleBackColor = true;
            this.btnClearDatabase.Click += new System.EventHandler(this.btnClearDatabase_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.numericSize);
            this.groupBox4.Controls.Add(this.btnReduceCachSize);
            this.groupBox4.Location = new System.Drawing.Point(12, 93);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(407, 130);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Reduce cached Items";
            // 
            // btnReduceCachSize
            // 
            this.btnReduceCachSize.Enabled = false;
            this.btnReduceCachSize.Location = new System.Drawing.Point(251, 26);
            this.btnReduceCachSize.Name = "btnReduceCachSize";
            this.btnReduceCachSize.Size = new System.Drawing.Size(150, 34);
            this.btnReduceCachSize.TabIndex = 2;
            this.btnReduceCachSize.Text = "Reduce cache size";
            this.btnReduceCachSize.UseVisualStyleBackColor = true;
            this.btnReduceCachSize.Click += new System.EventHandler(this.btnReduceCachSize_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRemoveFilesNotFound);
            this.groupBox3.Controls.Add(this.btnClearDatabase);
            this.groupBox3.Controls.Add(this.btnRunDefragmentJob);
            this.groupBox3.Location = new System.Drawing.Point(12, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(407, 67);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General";
            // 
            // btnRunDefragmentJob
            // 
            this.btnRunDefragmentJob.Location = new System.Drawing.Point(6, 22);
            this.btnRunDefragmentJob.Name = "btnRunDefragmentJob";
            this.btnRunDefragmentJob.Size = new System.Drawing.Size(120, 34);
            this.btnRunDefragmentJob.TabIndex = 2;
            this.btnRunDefragmentJob.Text = "Defrag";
            this.btnRunDefragmentJob.UseVisualStyleBackColor = true;
            this.btnRunDefragmentJob.Click += new System.EventHandler(this.btnRunDefragmentJob_Click);
            // 
            // numericSize
            // 
            this.numericSize.Location = new System.Drawing.Point(82, 33);
            this.numericSize.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericSize.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericSize.Name = "numericSize";
            this.numericSize.Size = new System.Drawing.Size(99, 22);
            this.numericSize.TabIndex = 5;
            this.numericSize.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mb";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(388, 39);
            this.label3.TabIndex = 8;
            this.label3.Text = "When shrinking the thumbnail database size the deletion order is by thumbnail siz" +
    "e ";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // btnRemoveFilesNotFound
            // 
            this.btnRemoveFilesNotFound.Location = new System.Drawing.Point(142, 22);
            this.btnRemoveFilesNotFound.Name = "btnRemoveFilesNotFound";
            this.btnRemoveFilesNotFound.Size = new System.Drawing.Size(120, 34);
            this.btnRemoveFilesNotFound.TabIndex = 3;
            this.btnRemoveFilesNotFound.Text = "Optimize";
            this.toolTipOptimize.SetToolTip(this.btnRemoveFilesNotFound, "This action will verify file access to every thumbnail item and remove thumbnails" +
        " which parent image is unaccessable.");
            this.btnRemoveFilesNotFound.UseVisualStyleBackColor = true;
            this.btnRemoveFilesNotFound.Click += new System.EventHandler(this.btnRemoveFilesNotFound_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(15, 406);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(20, 17);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "New Size";
            // 
            // FormThumbnailSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 387);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormThumbnailSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thumbnail Image cache settings";
            this.Load += new System.EventHandler(this.FormThumbnailSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentDbSize;
        private System.Windows.Forms.Label lblCachedItems;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRunDefragmentJob;
        private System.Windows.Forms.Button btnClearDatabase;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnReduceCachSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRemoveFilesNotFound;
        private System.Windows.Forms.ToolTip toolTipOptimize;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label label5;
    }
}