namespace SecureMemo
{
    partial class FormFind
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFind));
            this.btnFind = new System.Windows.Forms.Button();
            this.txtFindText = new System.Windows.Forms.TextBox();
            this.lblFindText = new System.Windows.Forms.Label();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkSearchAllTabs = new System.Windows.Forms.CheckBox();
            this.grpDirection = new System.Windows.Forms.GroupBox();
            this.rdButtonDown = new System.Windows.Forms.RadioButton();
            this.rdButtonUp = new System.Windows.Forms.RadioButton();
            this.chkLoopSearch = new System.Windows.Forms.CheckBox();
            this.grpDirection.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(347, 8);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 25);
            this.btnFind.TabIndex = 1;
            this.btnFind.Text = "Find Next";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtFindText
            // 
            this.txtFindText.Location = new System.Drawing.Point(74, 11);
            this.txtFindText.MaxLength = 256;
            this.txtFindText.Name = "txtFindText";
            this.txtFindText.Size = new System.Drawing.Size(250, 20);
            this.txtFindText.TabIndex = 0;
            this.txtFindText.TextChanged += new System.EventHandler(this.txtFindText_TextChanged);
            this.txtFindText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFindText_KeyUp);
            // 
            // lblFindText
            // 
            this.lblFindText.AutoSize = true;
            this.lblFindText.Location = new System.Drawing.Point(12, 14);
            this.lblFindText.Name = "lblFindText";
            this.lblFindText.Size = new System.Drawing.Size(56, 13);
            this.lblFindText.TabIndex = 2;
            this.lblFindText.Text = "Find what:";
            // 
            // chkCaseSensitive
            // 
            this.chkCaseSensitive.AutoSize = true;
            this.chkCaseSensitive.Location = new System.Drawing.Point(14, 47);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(96, 17);
            this.chkCaseSensitive.TabIndex = 3;
            this.chkCaseSensitive.Text = "Case Sensitive";
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            this.chkCaseSensitive.CheckedChanged += new System.EventHandler(this.chkCaseSensitive_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 39);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkSearchAllTabs
            // 
            this.chkSearchAllTabs.AutoSize = true;
            this.chkSearchAllTabs.Checked = true;
            this.chkSearchAllTabs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSearchAllTabs.Location = new System.Drawing.Point(14, 70);
            this.chkSearchAllTabs.Name = "chkSearchAllTabs";
            this.chkSearchAllTabs.Size = new System.Drawing.Size(97, 17);
            this.chkSearchAllTabs.TabIndex = 4;
            this.chkSearchAllTabs.Text = "Find in all Tabs";
            this.chkSearchAllTabs.UseVisualStyleBackColor = true;
            this.chkSearchAllTabs.CheckedChanged += new System.EventHandler(this.chkSearchAllTabs_CheckedChanged);
            // 
            // grpDirection
            // 
            this.grpDirection.Controls.Add(this.rdButtonDown);
            this.grpDirection.Controls.Add(this.rdButtonUp);
            this.grpDirection.Location = new System.Drawing.Point(249, 42);
            this.grpDirection.Name = "grpDirection";
            this.grpDirection.Size = new System.Drawing.Size(75, 68);
            this.grpDirection.TabIndex = 5;
            this.grpDirection.TabStop = false;
            this.grpDirection.Text = "Direction";
            // 
            // rdButtonDown
            // 
            this.rdButtonDown.AutoSize = true;
            this.rdButtonDown.Checked = true;
            this.rdButtonDown.Location = new System.Drawing.Point(6, 42);
            this.rdButtonDown.Name = "rdButtonDown";
            this.rdButtonDown.Size = new System.Drawing.Size(53, 17);
            this.rdButtonDown.TabIndex = 1;
            this.rdButtonDown.TabStop = true;
            this.rdButtonDown.Text = "Down";
            this.rdButtonDown.UseVisualStyleBackColor = true;
            this.rdButtonDown.CheckedChanged += new System.EventHandler(this.rdButtonDirection_CheckedChanged);
            // 
            // rdButtonUp
            // 
            this.rdButtonUp.AutoSize = true;
            this.rdButtonUp.Location = new System.Drawing.Point(6, 19);
            this.rdButtonUp.Name = "rdButtonUp";
            this.rdButtonUp.Size = new System.Drawing.Size(39, 17);
            this.rdButtonUp.TabIndex = 0;
            this.rdButtonUp.Text = "Up";
            this.rdButtonUp.UseVisualStyleBackColor = true;
            this.rdButtonUp.CheckedChanged += new System.EventHandler(this.rdButtonDirection_CheckedChanged);
            // 
            // chkLoopSearch
            // 
            this.chkLoopSearch.AutoSize = true;
            this.chkLoopSearch.Checked = true;
            this.chkLoopSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoopSearch.Enabled = false;
            this.chkLoopSearch.Location = new System.Drawing.Point(14, 93);
            this.chkLoopSearch.Name = "chkLoopSearch";
            this.chkLoopSearch.Size = new System.Drawing.Size(85, 17);
            this.chkLoopSearch.TabIndex = 6;
            this.chkLoopSearch.Text = "Loop search";
            this.chkLoopSearch.UseVisualStyleBackColor = true;
            // 
            // FormFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 115);
            this.Controls.Add(this.chkLoopSearch);
            this.Controls.Add(this.grpDirection);
            this.Controls.Add(this.chkSearchAllTabs);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkCaseSensitive);
            this.Controls.Add(this.lblFindText);
            this.Controls.Add(this.txtFindText);
            this.Controls.Add(this.btnFind);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFind";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find";
            this.Activated += new System.EventHandler(this.FormFind_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormFind_FormClosed);
            this.Load += new System.EventHandler(this.FormFind_Load);
            this.Enter += new System.EventHandler(this.FormFind_Enter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormFind_KeyUp);
            this.grpDirection.ResumeLayout(false);
            this.grpDirection.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtFindText;
        private System.Windows.Forms.Label lblFindText;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkSearchAllTabs;
        private System.Windows.Forms.GroupBox grpDirection;
        private System.Windows.Forms.RadioButton rdButtonDown;
        private System.Windows.Forms.RadioButton rdButtonUp;
        private System.Windows.Forms.CheckBox chkLoopSearch;
    }
}