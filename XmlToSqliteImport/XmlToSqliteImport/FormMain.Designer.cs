namespace XmlToSqliteImport
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewXML = new System.Windows.Forms.DataGridView();
            this.btnImportXML = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewXML)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(646, 621);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(136, 34);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Import Data";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dataGridViewXML);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(775, 298);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // dataGridViewXML
            // 
            this.dataGridViewXML.AllowUserToAddRows = false;
            this.dataGridViewXML.AllowUserToDeleteRows = false;
            this.dataGridViewXML.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewXML.Location = new System.Drawing.Point(5, 18);
            this.dataGridViewXML.Name = "dataGridViewXML";
            this.dataGridViewXML.ReadOnly = true;
            this.dataGridViewXML.Size = new System.Drawing.Size(765, 275);
            this.dataGridViewXML.TabIndex = 0;
            // 
            // btnImportXML
            // 
            this.btnImportXML.Location = new System.Drawing.Point(646, 316);
            this.btnImportXML.Name = "btnImportXML";
            this.btnImportXML.Size = new System.Drawing.Size(136, 34);
            this.btnImportXML.TabIndex = 2;
            this.btnImportXML.Text = "Import xml";
            this.btnImportXML.UseVisualStyleBackColor = true;
            this.btnImportXML.Click += new System.EventHandler(this.btnImportXML_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 678);
            this.Controls.Add(this.btnImportXML);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConnect);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import XML sms data to sqlite db";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewXML)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridViewXML;
        private System.Windows.Forms.Button btnImportXML;
    }
}

