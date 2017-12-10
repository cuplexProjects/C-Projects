namespace ImageView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThumbnailSettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentDbSize = new System.Windows.Forms.Label();
            this.lblOptimalDbSpace = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblWaistedSpace = new System.Windows.Forms.Label();
            this.lblCachedItems = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRunDefragmentJob = new System.Windows.Forms.Button();
            this.txtDefragInfo = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtReduceItemsInfo = new System.Windows.Forms.TextBox();
            this.btnReduceItemsCached = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtClearDatabaseInfo = new System.Windows.Forms.TextBox();
            this.btnClearDatabase = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
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
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(327, 523);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 38);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblWaistedSpace, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCurrentDbSize, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblOptimalDbSpace, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblCachedItems, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 21);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(413, 118);
            this.tableLayoutPanel1.TabIndex = 0;
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
            // lblOptimalDbSpace
            // 
            this.lblOptimalDbSpace.AutoSize = true;
            this.lblOptimalDbSpace.Location = new System.Drawing.Point(209, 34);
            this.lblOptimalDbSpace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.lblOptimalDbSpace.Name = "lblOptimalDbSpace";
            this.lblOptimalDbSpace.Size = new System.Drawing.Size(20, 17);
            this.lblOptimalDbSpace.TabIndex = 2;
            this.lblOptimalDbSpace.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Cached Thumbnails:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 34);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Optimal Database Size";
            // 
            // lblWaistedSpace
            // 
            this.lblWaistedSpace.AutoSize = true;
            this.lblWaistedSpace.Location = new System.Drawing.Point(209, 64);
            this.lblWaistedSpace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.lblWaistedSpace.Name = "lblWaistedSpace";
            this.lblWaistedSpace.Size = new System.Drawing.Size(20, 17);
            this.lblWaistedSpace.TabIndex = 5;
            this.lblWaistedSpace.Text = "...";
            // 
            // lblCachedItems
            // 
            this.lblCachedItems.AutoSize = true;
            this.lblCachedItems.Location = new System.Drawing.Point(209, 94);
            this.lblCachedItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.lblCachedItems.Name = "lblCachedItems";
            this.lblCachedItems.Size = new System.Drawing.Size(20, 17);
            this.lblCachedItems.TabIndex = 6;
            this.lblCachedItems.Text = "...";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 64);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "Waisted space:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(12, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 330);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tools";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtDefragInfo);
            this.groupBox3.Controls.Add(this.btnRunDefragmentJob);
            this.groupBox3.Location = new System.Drawing.Point(12, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(407, 90);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Defragment";
            // 
            // btnRunDefragmentJob
            // 
            this.btnRunDefragmentJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunDefragmentJob.Location = new System.Drawing.Point(281, 50);
            this.btnRunDefragmentJob.Name = "btnRunDefragmentJob";
            this.btnRunDefragmentJob.Size = new System.Drawing.Size(120, 34);
            this.btnRunDefragmentJob.TabIndex = 2;
            this.btnRunDefragmentJob.Text = "Defrag";
            this.btnRunDefragmentJob.UseVisualStyleBackColor = true;
            // 
            // txtDefragInfo
            // 
            this.txtDefragInfo.Location = new System.Drawing.Point(6, 21);
            this.txtDefragInfo.Multiline = true;
            this.txtDefragInfo.Name = "txtDefragInfo";
            this.txtDefragInfo.ReadOnly = true;
            this.txtDefragInfo.Size = new System.Drawing.Size(269, 63);
            this.txtDefragInfo.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtReduceItemsInfo);
            this.groupBox4.Controls.Add(this.btnReduceItemsCached);
            this.groupBox4.Location = new System.Drawing.Point(12, 117);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(407, 90);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Reduce cached Items";
            // 
            // txtReduceItemsInfo
            // 
            this.txtReduceItemsInfo.Location = new System.Drawing.Point(6, 21);
            this.txtReduceItemsInfo.Multiline = true;
            this.txtReduceItemsInfo.Name = "txtReduceItemsInfo";
            this.txtReduceItemsInfo.ReadOnly = true;
            this.txtReduceItemsInfo.Size = new System.Drawing.Size(269, 63);
            this.txtReduceItemsInfo.TabIndex = 3;
            // 
            // btnReduceItemsCached
            // 
            this.btnReduceItemsCached.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReduceItemsCached.Location = new System.Drawing.Point(281, 50);
            this.btnReduceItemsCached.Name = "btnReduceItemsCached";
            this.btnReduceItemsCached.Size = new System.Drawing.Size(120, 34);
            this.btnReduceItemsCached.TabIndex = 2;
            this.btnReduceItemsCached.Text = "Reduce items";
            this.btnReduceItemsCached.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtClearDatabaseInfo);
            this.groupBox5.Controls.Add(this.btnClearDatabase);
            this.groupBox5.Location = new System.Drawing.Point(12, 213);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(407, 90);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Clear Database";
            // 
            // txtClearDatabaseInfo
            // 
            this.txtClearDatabaseInfo.Location = new System.Drawing.Point(6, 21);
            this.txtClearDatabaseInfo.Multiline = true;
            this.txtClearDatabaseInfo.Name = "txtClearDatabaseInfo";
            this.txtClearDatabaseInfo.ReadOnly = true;
            this.txtClearDatabaseInfo.Size = new System.Drawing.Size(269, 63);
            this.txtClearDatabaseInfo.TabIndex = 3;
            // 
            // btnClearDatabase
            // 
            this.btnClearDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearDatabase.Location = new System.Drawing.Point(281, 50);
            this.btnClearDatabase.Name = "btnClearDatabase";
            this.btnClearDatabase.Size = new System.Drawing.Size(120, 34);
            this.btnClearDatabase.TabIndex = 2;
            this.btnClearDatabase.Text = "Clear Database";
            this.btnClearDatabase.UseVisualStyleBackColor = true;
            // 
            // FormThumbnailSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 573);
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
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblWaistedSpace;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentDbSize;
        private System.Windows.Forms.Label lblOptimalDbSpace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCachedItems;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtDefragInfo;
        private System.Windows.Forms.Button btnRunDefragmentJob;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtClearDatabaseInfo;
        private System.Windows.Forms.Button btnClearDatabase;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtReduceItemsInfo;
        private System.Windows.Forms.Button btnReduceItemsCached;
    }
}