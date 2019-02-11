namespace SecureMemo
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtFontSize = new System.Windows.Forms.RichTextBox();
            this.txtFontFamily = new System.Windows.Forms.RichTextBox();
            this.txtFontStyle = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnShowFontDialog = new System.Windows.Forms.Button();
            this.chkAlwaysOntop = new System.Windows.Forms.CheckBox();
            this.numericUpDownTabPages = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpDatabaseSettings = new System.Windows.Forms.GroupBox();
            this.btnBrowseFolder = new System.Windows.Forms.Button();
            this.txtSyncDatabaseDirectory = new System.Windows.Forms.TextBox();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.chkSyncDatabase = new System.Windows.Forms.CheckBox();
            this.folderBrowseForSyncDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabPages)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpDatabaseSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(227, 287);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 27);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(308, 287);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Controls.Add(this.btnShowFontDialog);
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 110);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font Settings ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.Controls.Add(this.txtFontSize, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtFontFamily, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtFontStyle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(358, 47);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // txtFontSize
            // 
            this.txtFontSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFontSize.Location = new System.Drawing.Point(304, 19);
            this.txtFontSize.Multiline = false;
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.ReadOnly = true;
            this.txtFontSize.Size = new System.Drawing.Size(51, 25);
            this.txtFontSize.TabIndex = 14;
            this.txtFontSize.Text = "";
            // 
            // txtFontFamily
            // 
            this.txtFontFamily.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFontFamily.Location = new System.Drawing.Point(3, 19);
            this.txtFontFamily.Multiline = false;
            this.txtFontFamily.Name = "txtFontFamily";
            this.txtFontFamily.ReadOnly = true;
            this.txtFontFamily.Size = new System.Drawing.Size(205, 25);
            this.txtFontFamily.TabIndex = 12;
            this.txtFontFamily.Text = "";
            // 
            // txtFontStyle
            // 
            this.txtFontStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFontStyle.Location = new System.Drawing.Point(214, 19);
            this.txtFontStyle.Multiline = false;
            this.txtFontStyle.Name = "txtFontStyle";
            this.txtFontStyle.ReadOnly = true;
            this.txtFontStyle.Size = new System.Drawing.Size(84, 25);
            this.txtFontStyle.TabIndex = 13;
            this.txtFontStyle.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(304, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "Size:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Font Family:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(214, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Style:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // btnShowFontDialog
            // 
            this.btnShowFontDialog.Location = new System.Drawing.Point(249, 76);
            this.btnShowFontDialog.Name = "btnShowFontDialog";
            this.btnShowFontDialog.Size = new System.Drawing.Size(115, 27);
            this.btnShowFontDialog.TabIndex = 9;
            this.btnShowFontDialog.Text = "Select New Font";
            this.btnShowFontDialog.UseVisualStyleBackColor = true;
            this.btnShowFontDialog.Click += new System.EventHandler(this.btnShowFontDialog_Click);
            // 
            // chkAlwaysOntop
            // 
            this.chkAlwaysOntop.AutoSize = true;
            this.chkAlwaysOntop.Location = new System.Drawing.Point(249, 25);
            this.chkAlwaysOntop.Name = "chkAlwaysOntop";
            this.chkAlwaysOntop.Size = new System.Drawing.Size(98, 17);
            this.chkAlwaysOntop.TabIndex = 11;
            this.chkAlwaysOntop.Text = "Always Ontop";
            this.chkAlwaysOntop.UseVisualStyleBackColor = true;
            // 
            // numericUpDownTabPages
            // 
            this.numericUpDownTabPages.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownTabPages.Location = new System.Drawing.Point(152, 23);
            this.numericUpDownTabPages.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTabPages.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTabPages.Name = "numericUpDownTabPages";
            this.numericUpDownTabPages.Size = new System.Drawing.Size(50, 22);
            this.numericUpDownTabPages.TabIndex = 10;
            this.numericUpDownTabPages.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Default Empty Tab Pages:";
            // 
            // fontDialog1
            // 
            this.fontDialog1.AllowVerticalFonts = false;
            this.fontDialog1.FontMustExist = true;
            this.fontDialog1.MaxSize = 64;
            this.fontDialog1.MinSize = 4;
            this.fontDialog1.ShowEffects = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDownTabPages);
            this.groupBox2.Controls.Add(this.chkAlwaysOntop);
            this.groupBox2.Location = new System.Drawing.Point(7, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 60);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "General Settings";
            // 
            // grpDatabaseSettings
            // 
            this.grpDatabaseSettings.Controls.Add(this.btnBrowseFolder);
            this.grpDatabaseSettings.Controls.Add(this.txtSyncDatabaseDirectory);
            this.grpDatabaseSettings.Controls.Add(this.lblDirectory);
            this.grpDatabaseSettings.Controls.Add(this.chkSyncDatabase);
            this.grpDatabaseSettings.Location = new System.Drawing.Point(7, 194);
            this.grpDatabaseSettings.Name = "grpDatabaseSettings";
            this.grpDatabaseSettings.Size = new System.Drawing.Size(370, 85);
            this.grpDatabaseSettings.TabIndex = 14;
            this.grpDatabaseSettings.TabStop = false;
            this.grpDatabaseSettings.Text = "Sync database settings";
            // 
            // btnBrowseFolder
            // 
            this.btnBrowseFolder.Enabled = false;
            this.btnBrowseFolder.Location = new System.Drawing.Point(331, 47);
            this.btnBrowseFolder.Name = "btnBrowseFolder";
            this.btnBrowseFolder.Size = new System.Drawing.Size(30, 22);
            this.btnBrowseFolder.TabIndex = 3;
            this.btnBrowseFolder.Text = "..";
            this.btnBrowseFolder.UseVisualStyleBackColor = true;
            this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
            // 
            // txtSyncDatabaseDirectory
            // 
            this.txtSyncDatabaseDirectory.Location = new System.Drawing.Point(70, 47);
            this.txtSyncDatabaseDirectory.Name = "txtSyncDatabaseDirectory";
            this.txtSyncDatabaseDirectory.ReadOnly = true;
            this.txtSyncDatabaseDirectory.Size = new System.Drawing.Size(255, 22);
            this.txtSyncDatabaseDirectory.TabIndex = 2;
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(10, 50);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(56, 13);
            this.lblDirectory.TabIndex = 1;
            this.lblDirectory.Text = "Directory:";
            // 
            // chkSyncDatabase
            // 
            this.chkSyncDatabase.AutoSize = true;
            this.chkSyncDatabase.Location = new System.Drawing.Point(10, 25);
            this.chkSyncDatabase.Name = "chkSyncDatabase";
            this.chkSyncDatabase.Size = new System.Drawing.Size(224, 17);
            this.chkSyncDatabase.TabIndex = 0;
            this.chkSyncDatabase.Text = "Synchronize database to shared folder";
            this.chkSyncDatabase.UseVisualStyleBackColor = true;
            this.chkSyncDatabase.CheckedChanged += new System.EventHandler(this.chkSyncDatabase_CheckedChanged);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 326);
            this.Controls.Add(this.grpDatabaseSettings);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabPages)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpDatabaseSettings.ResumeLayout(false);
            this.grpDatabaseSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAlwaysOntop;
        private System.Windows.Forms.NumericUpDown numericUpDownTabPages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button btnShowFontDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox txtFontFamily;
        private System.Windows.Forms.RichTextBox txtFontSize;
        private System.Windows.Forms.RichTextBox txtFontStyle;
        private System.Windows.Forms.GroupBox grpDatabaseSettings;
        private System.Windows.Forms.Button btnBrowseFolder;
        private System.Windows.Forms.TextBox txtSyncDatabaseDirectory;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.CheckBox chkSyncDatabase;
        private System.Windows.Forms.FolderBrowserDialog folderBrowseForSyncDirectory;
    }
}