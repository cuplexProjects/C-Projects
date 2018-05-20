namespace FileStructureOrganizer
{
    partial class FormProfile
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
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Matching Episode media");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Erisodes Folders", new System.Windows.Forms.TreeNode[] {
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Season []", new System.Windows.Forms.TreeNode[] {
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Series Names", new System.Windows.Forms.TreeNode[] {
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("TV-Series", new System.Windows.Forms.TreeNode[] {
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Incorrectly Named Series");
            this.groupBoxTVSeries = new System.Windows.Forms.GroupBox();
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxTvSeries = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.txtBasePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxTVSeries.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTVSeries
            // 
            this.groupBoxTVSeries.Controls.Add(this.txtProfileName);
            this.groupBoxTVSeries.Controls.Add(this.label4);
            this.groupBoxTVSeries.Controls.Add(this.listBoxTvSeries);
            this.groupBoxTVSeries.Controls.Add(this.label3);
            this.groupBoxTVSeries.Controls.Add(this.label2);
            this.groupBoxTVSeries.Controls.Add(this.treeView1);
            this.groupBoxTVSeries.Controls.Add(this.txtBasePath);
            this.groupBoxTVSeries.Controls.Add(this.label1);
            this.groupBoxTVSeries.Controls.Add(this.btnBrowse);
            this.groupBoxTVSeries.Location = new System.Drawing.Point(12, 12);
            this.groupBoxTVSeries.Name = "groupBoxTVSeries";
            this.groupBoxTVSeries.Size = new System.Drawing.Size(380, 565);
            this.groupBoxTVSeries.TabIndex = 2;
            this.groupBoxTVSeries.TabStop = false;
            this.groupBoxTVSeries.Text = "TV-Series Profile Settings";
            // 
            // txtProfileName
            // 
            this.txtProfileName.Location = new System.Drawing.Point(79, 26);
            this.txtProfileName.MaxLength = 256;
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(200, 20);
            this.txtProfileName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Profile Name";
            // 
            // listBoxTvSeries
            // 
            this.listBoxTvSeries.FormattingEnabled = true;
            this.listBoxTvSeries.Location = new System.Drawing.Point(13, 303);
            this.listBoxTvSeries.Name = "listBoxTvSeries";
            this.listBoxTvSeries.Size = new System.Drawing.Size(354, 251);
            this.listBoxTvSeries.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 286);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Known system series names";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 103);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Final Folder Structure";
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(9, 120);
            this.treeView1.Name = "treeView1";
            treeNode7.Name = "Node7";
            treeNode7.Text = "Matching Episode media";
            treeNode7.ToolTipText = "Must contain a supported media file, other files to like subs, covers etc.";
            treeNode8.Name = "Node6";
            treeNode8.Text = "Erisodes Folders";
            treeNode8.ToolTipText = "Short Format, just the name and not quality, distributor etc";
            treeNode9.Name = "Node5";
            treeNode9.Text = "Season []";
            treeNode10.Name = "NodeSeriesNames";
            treeNode10.Text = "Series Names";
            treeNode10.ToolTipText = "Names in short format";
            treeNode11.Name = "TV_Series";
            treeNode11.Text = "TV-Series";
            treeNode12.Name = "NodeNoneMaching";
            treeNode12.Text = "Incorrectly Named Series";
            treeNode12.ToolTipText = "Optional, output folder for items that could not be matched";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12});
            this.treeView1.Size = new System.Drawing.Size(359, 155);
            this.treeView1.TabIndex = 3;
            // 
            // txtBasePath
            // 
            this.txtBasePath.Location = new System.Drawing.Point(79, 57);
            this.txtBasePath.MaxLength = 256;
            this.txtBasePath.Name = "txtBasePath";
            this.txtBasePath.Size = new System.Drawing.Size(289, 20);
            this.txtBasePath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "BasePath:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(343, 83);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(25, 20);
            this.btnBrowse.TabIndex = 0;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(292, 583);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(186, 583);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 617);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBoxTVSeries);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MinimizeBox = false;
            this.Name = "FormProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profile";
            this.Load += new System.EventHandler(this.FormProfile_Load);
            this.groupBoxTVSeries.ResumeLayout(false);
            this.groupBoxTVSeries.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTVSeries;
        private System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxTvSeries;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox txtBasePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}