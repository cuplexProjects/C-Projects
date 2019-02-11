namespace DatabaseImport.Forms
{
    partial class frmImportSettings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvImportDataCoupling = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databaseColumnCouplingBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtInputInfo = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportDataCoupling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseColumnCouplingBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvImportDataCoupling);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(5, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(274, 259);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Column Data couplings";
            // 
            // dgvImportDataCoupling
            // 
            this.dgvImportDataCoupling.AllowUserToAddRows = false;
            this.dgvImportDataCoupling.AllowUserToDeleteRows = false;
            this.dgvImportDataCoupling.AllowUserToResizeRows = false;
            this.dgvImportDataCoupling.AutoGenerateColumns = false;
            this.dgvImportDataCoupling.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImportDataCoupling.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn});
            this.dgvImportDataCoupling.DataSource = this.databaseColumnCouplingBindingSource;
            this.dgvImportDataCoupling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvImportDataCoupling.Location = new System.Drawing.Point(5, 18);
            this.dgvImportDataCoupling.Name = "dgvImportDataCoupling";
            this.dgvImportDataCoupling.RowHeadersVisible = false;
            this.dgvImportDataCoupling.Size = new System.Drawing.Size(264, 236);
            this.dgvImportDataCoupling.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.MinimumWidth = 25;
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.MinimumWidth = 25;
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            // 
            // databaseColumnCouplingBindingSource
            // 
            this.databaseColumnCouplingBindingSource.DataSource = typeof(DatabaseImport.Models.DatabaseColumnCoupling);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtInputInfo);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(5, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(274, 91);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import parameters";
            // 
            // txtInputInfo
            // 
            this.txtInputInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInputInfo.Location = new System.Drawing.Point(5, 18);
            this.txtInputInfo.Multiline = true;
            this.txtInputInfo.Name = "txtInputInfo";
            this.txtInputInfo.ReadOnly = true;
            this.txtInputInfo.Size = new System.Drawing.Size(264, 68);
            this.txtInputInfo.TabIndex = 0;
            this.txtInputInfo.Text = "Keywords:\r\n{IMPORT_DATA} = data from file\r\n{SQL_FUNC=\'\'} = sql generated column d" +
    "ata.\r\nExample: {SQL_FUNC=\'Len(Name)\'}";
            // 
            // frmImportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 366);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(150, 200);
            this.Name = "frmImportSettings";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Define Column Data ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImportSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmImportSettings_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvImportDataCoupling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.databaseColumnCouplingBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvImportDataCoupling;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtInputInfo;
        private System.Windows.Forms.BindingSource databaseColumnCouplingBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
    }
}