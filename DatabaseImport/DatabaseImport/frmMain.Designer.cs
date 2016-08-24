namespace DatabaseImport
{
    partial class frmMain
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbTables = new System.Windows.Forms.ComboBox();
            this.chkWindowsAuthentication = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDatabases = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnListDatabases = new System.Windows.Forms.Button();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnImport = new System.Windows.Forms.Button();
            this.dgvTableStructure = new System.Windows.Forms.DataGridView();
            this.columnNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ordinalPositionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDefaultDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isNullableDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.characterMaximumLengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sQLColumnStructureBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panelColumnStructure = new System.Windows.Forms.Panel();
            this.btnOpenDatagridForm = new System.Windows.Forms.Button();
            this.btnReturnDatagrid = new System.Windows.Forms.Button();
            this.btnImportSettings = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableStructure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sQLColumnStructureBindingSource)).BeginInit();
            this.panelColumnStructure.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtInputFile);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(339, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import File Configuration";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(80, 49);
            this.textBox1.MaxLength = 10;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(60, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "\\r\\n";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Seraprator:";
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(38, 23);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.ReadOnly = true;
            this.txtInputFile.Size = new System.Drawing.Size(245, 20);
            this.txtInputFile.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(289, 20);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(44, 25);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.cbTables);
            this.groupBox2.Controls.Add(this.chkWindowsAuthentication);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbDatabases);
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.txtUsername);
            this.groupBox2.Controls.Add(this.btnListDatabases);
            this.groupBox2.Controls.Add(this.txtServerName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 218);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SQL Server";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Table:";
            // 
            // cbTables
            // 
            this.cbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTables.Enabled = false;
            this.cbTables.FormattingEnabled = true;
            this.cbTables.Location = new System.Drawing.Point(82, 133);
            this.cbTables.Name = "cbTables";
            this.cbTables.Size = new System.Drawing.Size(239, 21);
            this.cbTables.TabIndex = 9;
            this.cbTables.SelectedIndexChanged += new System.EventHandler(this.cbTables_SelectedIndexChanged);
            // 
            // chkWindowsAuthentication
            // 
            this.chkWindowsAuthentication.AutoSize = true;
            this.chkWindowsAuthentication.Checked = true;
            this.chkWindowsAuthentication.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsAuthentication.Location = new System.Drawing.Point(9, 184);
            this.chkWindowsAuthentication.Name = "chkWindowsAuthentication";
            this.chkWindowsAuthentication.Size = new System.Drawing.Size(159, 17);
            this.chkWindowsAuthentication.TabIndex = 8;
            this.chkWindowsAuthentication.Text = "Use windows authentication";
            this.chkWindowsAuthentication.UseVisualStyleBackColor = true;
            this.chkWindowsAuthentication.CheckedChanged += new System.EventHandler(this.chkWindowsAuthentication_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Database:";
            // 
            // cbDatabases
            // 
            this.cbDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDatabases.Enabled = false;
            this.cbDatabases.FormattingEnabled = true;
            this.cbDatabases.Location = new System.Drawing.Point(82, 106);
            this.cbDatabases.Name = "cbDatabases";
            this.cbDatabases.Size = new System.Drawing.Size(239, 21);
            this.cbDatabases.TabIndex = 6;
            this.cbDatabases.SelectedIndexChanged += new System.EventHandler(this.cbDatabases_SelectedIndexChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(82, 80);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(239, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // txtUsername
            // 
            this.txtUsername.Enabled = false;
            this.txtUsername.Location = new System.Drawing.Point(82, 54);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(239, 20);
            this.txtUsername.TabIndex = 4;
            // 
            // btnListDatabases
            // 
            this.btnListDatabases.Location = new System.Drawing.Point(204, 179);
            this.btnListDatabases.Name = "btnListDatabases";
            this.btnListDatabases.Size = new System.Drawing.Size(117, 25);
            this.btnListDatabases.TabIndex = 0;
            this.btnListDatabases.Text = "List Databases";
            this.btnListDatabases.UseVisualStyleBackColor = true;
            this.btnListDatabases.Click += new System.EventHandler(this.btnListDatabases_Click);
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(82, 28);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(239, 20);
            this.txtServerName.TabIndex = 3;
            this.txtServerName.Text = "localhost";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Server name:";
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(159, 511);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 489);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(339, 16);
            this.progressBar1.TabIndex = 7;
            // 
            // btnImport
            // 
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(241, 511);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(104, 25);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // dgvTableStructure
            // 
            this.dgvTableStructure.AllowUserToAddRows = false;
            this.dgvTableStructure.AllowUserToDeleteRows = false;
            this.dgvTableStructure.AllowUserToResizeRows = false;
            this.dgvTableStructure.AutoGenerateColumns = false;
            this.dgvTableStructure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableStructure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNameDataGridViewTextBoxColumn,
            this.ordinalPositionDataGridViewTextBoxColumn,
            this.columnDefaultDataGridViewTextBoxColumn,
            this.isNullableDataGridViewCheckBoxColumn,
            this.dataTypeDataGridViewTextBoxColumn,
            this.characterMaximumLengthDataGridViewTextBoxColumn});
            this.dgvTableStructure.DataSource = this.sQLColumnStructureBindingSource;
            this.dgvTableStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTableStructure.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTableStructure.Location = new System.Drawing.Point(0, 0);
            this.dgvTableStructure.MultiSelect = false;
            this.dgvTableStructure.Name = "dgvTableStructure";
            this.dgvTableStructure.ReadOnly = true;
            this.dgvTableStructure.RowHeadersVisible = false;
            this.dgvTableStructure.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTableStructure.Size = new System.Drawing.Size(339, 115);
            this.dgvTableStructure.TabIndex = 9;
            // 
            // columnNameDataGridViewTextBoxColumn
            // 
            this.columnNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnNameDataGridViewTextBoxColumn.DataPropertyName = "ColumnName";
            this.columnNameDataGridViewTextBoxColumn.HeaderText = "ColumnName";
            this.columnNameDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.columnNameDataGridViewTextBoxColumn.Name = "columnNameDataGridViewTextBoxColumn";
            this.columnNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ordinalPositionDataGridViewTextBoxColumn
            // 
            this.ordinalPositionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ordinalPositionDataGridViewTextBoxColumn.DataPropertyName = "OrdinalPosition";
            this.ordinalPositionDataGridViewTextBoxColumn.HeaderText = "OrdinalPosition";
            this.ordinalPositionDataGridViewTextBoxColumn.MinimumWidth = 20;
            this.ordinalPositionDataGridViewTextBoxColumn.Name = "ordinalPositionDataGridViewTextBoxColumn";
            this.ordinalPositionDataGridViewTextBoxColumn.ReadOnly = true;
            this.ordinalPositionDataGridViewTextBoxColumn.Width = 102;
            // 
            // columnDefaultDataGridViewTextBoxColumn
            // 
            this.columnDefaultDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDefaultDataGridViewTextBoxColumn.DataPropertyName = "ColumnDefault";
            this.columnDefaultDataGridViewTextBoxColumn.FillWeight = 50F;
            this.columnDefaultDataGridViewTextBoxColumn.HeaderText = "ColumnDefault";
            this.columnDefaultDataGridViewTextBoxColumn.MinimumWidth = 25;
            this.columnDefaultDataGridViewTextBoxColumn.Name = "columnDefaultDataGridViewTextBoxColumn";
            this.columnDefaultDataGridViewTextBoxColumn.ReadOnly = true;
            this.columnDefaultDataGridViewTextBoxColumn.Width = 101;
            // 
            // isNullableDataGridViewCheckBoxColumn
            // 
            this.isNullableDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.isNullableDataGridViewCheckBoxColumn.DataPropertyName = "IsNullable";
            this.isNullableDataGridViewCheckBoxColumn.HeaderText = "IsNullable";
            this.isNullableDataGridViewCheckBoxColumn.MinimumWidth = 25;
            this.isNullableDataGridViewCheckBoxColumn.Name = "isNullableDataGridViewCheckBoxColumn";
            this.isNullableDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isNullableDataGridViewCheckBoxColumn.Width = 59;
            // 
            // dataTypeDataGridViewTextBoxColumn
            // 
            this.dataTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dataTypeDataGridViewTextBoxColumn.DataPropertyName = "DataType";
            this.dataTypeDataGridViewTextBoxColumn.HeaderText = "DataType";
            this.dataTypeDataGridViewTextBoxColumn.MinimumWidth = 25;
            this.dataTypeDataGridViewTextBoxColumn.Name = "dataTypeDataGridViewTextBoxColumn";
            this.dataTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.dataTypeDataGridViewTextBoxColumn.Width = 79;
            // 
            // characterMaximumLengthDataGridViewTextBoxColumn
            // 
            this.characterMaximumLengthDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.characterMaximumLengthDataGridViewTextBoxColumn.DataPropertyName = "CharacterMaximumLength";
            this.characterMaximumLengthDataGridViewTextBoxColumn.HeaderText = "CharacterMaximumLength";
            this.characterMaximumLengthDataGridViewTextBoxColumn.MinimumWidth = 25;
            this.characterMaximumLengthDataGridViewTextBoxColumn.Name = "characterMaximumLengthDataGridViewTextBoxColumn";
            this.characterMaximumLengthDataGridViewTextBoxColumn.ReadOnly = true;
            this.characterMaximumLengthDataGridViewTextBoxColumn.Width = 155;
            // 
            // sQLColumnStructureBindingSource
            // 
            this.sQLColumnStructureBindingSource.DataSource = typeof(DatabaseImport.Models.SQLColumnStructure);
            // 
            // panelColumnStructure
            // 
            this.panelColumnStructure.AllowDrop = true;
            this.panelColumnStructure.Controls.Add(this.btnOpenDatagridForm);
            this.panelColumnStructure.Controls.Add(this.btnReturnDatagrid);
            this.panelColumnStructure.Controls.Add(this.dgvTableStructure);
            this.panelColumnStructure.Location = new System.Drawing.Point(12, 318);
            this.panelColumnStructure.Name = "panelColumnStructure";
            this.panelColumnStructure.Padding = new System.Windows.Forms.Padding(0, 0, 0, 50);
            this.panelColumnStructure.Size = new System.Drawing.Size(339, 165);
            this.panelColumnStructure.TabIndex = 9;
            // 
            // btnOpenDatagridForm
            // 
            this.btnOpenDatagridForm.Location = new System.Drawing.Point(69, 131);
            this.btnOpenDatagridForm.Name = "btnOpenDatagridForm";
            this.btnOpenDatagridForm.Size = new System.Drawing.Size(129, 25);
            this.btnOpenDatagridForm.TabIndex = 11;
            this.btnOpenDatagridForm.Text = "Open in new window";
            this.btnOpenDatagridForm.UseVisualStyleBackColor = true;
            this.btnOpenDatagridForm.Click += new System.EventHandler(this.btnOpenDatagridForm_Click);
            // 
            // btnReturnDatagrid
            // 
            this.btnReturnDatagrid.Enabled = false;
            this.btnReturnDatagrid.Location = new System.Drawing.Point(204, 131);
            this.btnReturnDatagrid.Name = "btnReturnDatagrid";
            this.btnReturnDatagrid.Size = new System.Drawing.Size(129, 25);
            this.btnReturnDatagrid.TabIndex = 10;
            this.btnReturnDatagrid.Text = "Return datagrid";
            this.btnReturnDatagrid.UseVisualStyleBackColor = true;
            this.btnReturnDatagrid.Click += new System.EventHandler(this.btnReturnDatagrid_Click);
            // 
            // btnImportSettings
            // 
            this.btnImportSettings.Enabled = false;
            this.btnImportSettings.Location = new System.Drawing.Point(6, 511);
            this.btnImportSettings.Name = "btnImportSettings";
            this.btnImportSettings.Size = new System.Drawing.Size(146, 25);
            this.btnImportSettings.TabIndex = 10;
            this.btnImportSettings.Text = "Import Settings";
            this.btnImportSettings.UseVisualStyleBackColor = true;
            this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 546);
            this.Controls.Add(this.btnImportSettings);
            this.Controls.Add(this.panelColumnStructure);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database file importer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableStructure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sQLColumnStructureBindingSource)).EndInit();
            this.panelColumnStructure.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkWindowsAuthentication;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbDatabases;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnListDatabases;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbTables;
        private System.Windows.Forms.DataGridView dgvTableStructure;
        private System.Windows.Forms.BindingSource sQLColumnStructureBindingSource;
        private System.Windows.Forms.Panel panelColumnStructure;
        private System.Windows.Forms.Button btnOpenDatagridForm;
        private System.Windows.Forms.Button btnReturnDatagrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ordinalPositionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDefaultDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isNullableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn characterMaximumLengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btnImportSettings;
    }
}

