namespace SecureMemo
{
    partial class FormRestoreBackup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRestoreBackup));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewBackupoFiles = new System.Windows.Forms.ListView();
            this.FilenameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(347, 323);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestore.Enabled = false;
            this.btnRestore.Location = new System.Drawing.Point(188, 323);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(150, 27);
            this.btnRestore.TabIndex = 3;
            this.btnRestore.Text = "Restore Selected File";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewBackupoFiles);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 300);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available backups";
            // 
            // listViewBackupoFiles
            // 
            this.listViewBackupoFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FilenameHeader,
            this.DateHeader});
            this.listViewBackupoFiles.FullRowSelect = true;
            this.listViewBackupoFiles.Location = new System.Drawing.Point(6, 19);
            this.listViewBackupoFiles.MultiSelect = false;
            this.listViewBackupoFiles.Name = "listViewBackupoFiles";
            this.listViewBackupoFiles.Size = new System.Drawing.Size(404, 275);
            this.listViewBackupoFiles.TabIndex = 0;
            this.listViewBackupoFiles.UseCompatibleStateImageBehavior = false;
            this.listViewBackupoFiles.View = System.Windows.Forms.View.Details;
            this.listViewBackupoFiles.SelectedIndexChanged += new System.EventHandler(this.listViewBackupoFiles_SelectedIndexChanged);
            // 
            // FilenameHeader
            // 
            this.FilenameHeader.Text = "Filename";
            this.FilenameHeader.Width = 250;
            // 
            // DateHeader
            // 
            this.DateHeader.Text = "Date";
            this.DateHeader.Width = 150;
            // 
            // FormRestoreBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 362);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRestore);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRestoreBackup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Restore Backup";
            this.Load += new System.EventHandler(this.frmRestoreBackup_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewBackupoFiles;
        private System.Windows.Forms.ColumnHeader FilenameHeader;
        private System.Windows.Forms.ColumnHeader DateHeader;
    }
}