namespace CloseAllDemoApps
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkAlwaysOntop = new System.Windows.Forms.CheckBox();
            this.chkMinimizeToSystemTray = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblProcessDescriptionFilter = new System.Windows.Forms.Label();
            this.txtProcessDescriptionFilter = new System.Windows.Forms.TextBox();
            this.btnRestoreDefault = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCloseAllGlobalShortcut = new System.Windows.Forms.TextBox();
            this.lblGlobalShortcut = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(297, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkAlwaysOntop
            // 
            this.chkAlwaysOntop.AutoSize = true;
            this.chkAlwaysOntop.Location = new System.Drawing.Point(15, 19);
            this.chkAlwaysOntop.Name = "chkAlwaysOntop";
            this.chkAlwaysOntop.Size = new System.Drawing.Size(91, 17);
            this.chkAlwaysOntop.TabIndex = 1;
            this.chkAlwaysOntop.Text = "Always Ontop";
            this.chkAlwaysOntop.UseVisualStyleBackColor = true;
            // 
            // chkMinimizeToSystemTray
            // 
            this.chkMinimizeToSystemTray.AutoSize = true;
            this.chkMinimizeToSystemTray.Location = new System.Drawing.Point(15, 42);
            this.chkMinimizeToSystemTray.Name = "chkMinimizeToSystemTray";
            this.chkMinimizeToSystemTray.Size = new System.Drawing.Size(133, 17);
            this.chkMinimizeToSystemTray.TabIndex = 2;
            this.chkMinimizeToSystemTray.Text = "Minimize to system tray";
            this.chkMinimizeToSystemTray.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(216, 200);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblProcessDescriptionFilter
            // 
            this.lblProcessDescriptionFilter.AutoSize = true;
            this.lblProcessDescriptionFilter.Location = new System.Drawing.Point(12, 68);
            this.lblProcessDescriptionFilter.Name = "lblProcessDescriptionFilter";
            this.lblProcessDescriptionFilter.Size = new System.Drawing.Size(126, 13);
            this.lblProcessDescriptionFilter.TabIndex = 4;
            this.lblProcessDescriptionFilter.Text = "Process Description filter:";
            // 
            // txtProcessDescriptionFilter
            // 
            this.txtProcessDescriptionFilter.Location = new System.Drawing.Point(15, 84);
            this.txtProcessDescriptionFilter.MaxLength = 200;
            this.txtProcessDescriptionFilter.Name = "txtProcessDescriptionFilter";
            this.txtProcessDescriptionFilter.Size = new System.Drawing.Size(275, 20);
            this.txtProcessDescriptionFilter.TabIndex = 5;
            // 
            // btnRestoreDefault
            // 
            this.btnRestoreDefault.Location = new System.Drawing.Point(12, 200);
            this.btnRestoreDefault.Name = "btnRestoreDefault";
            this.btnRestoreDefault.Size = new System.Drawing.Size(133, 25);
            this.btnRestoreDefault.TabIndex = 6;
            this.btnRestoreDefault.Text = "Restore default settings";
            this.btnRestoreDefault.UseVisualStyleBackColor = true;
            this.btnRestoreDefault.Click += new System.EventHandler(this.btnRestoreDefault_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCloseAllGlobalShortcut);
            this.groupBox1.Controls.Add(this.lblGlobalShortcut);
            this.groupBox1.Controls.Add(this.chkAlwaysOntop);
            this.groupBox1.Controls.Add(this.chkMinimizeToSystemTray);
            this.groupBox1.Controls.Add(this.txtProcessDescriptionFilter);
            this.groupBox1.Controls.Add(this.lblProcessDescriptionFilter);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 182);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // txtCloseAllGlobalShortcut
            // 
            this.txtCloseAllGlobalShortcut.Location = new System.Drawing.Point(15, 123);
            this.txtCloseAllGlobalShortcut.MaxLength = 200;
            this.txtCloseAllGlobalShortcut.Name = "txtCloseAllGlobalShortcut";
            this.txtCloseAllGlobalShortcut.Size = new System.Drawing.Size(200, 20);
            this.txtCloseAllGlobalShortcut.TabIndex = 7;
            this.txtCloseAllGlobalShortcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCloseAllGlobalShortcut_KeyPress);
            this.txtCloseAllGlobalShortcut.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCloseAllGlobalShortcut_KeyUp);
            // 
            // lblGlobalShortcut
            // 
            this.lblGlobalShortcut.AutoSize = true;
            this.lblGlobalShortcut.Location = new System.Drawing.Point(12, 107);
            this.lblGlobalShortcut.Name = "lblGlobalShortcut";
            this.lblGlobalShortcut.Size = new System.Drawing.Size(177, 13);
            this.lblGlobalShortcut.TabIndex = 6;
            this.lblGlobalShortcut.Text = "Close All demo apps global shortcut:";
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 237);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRestoreDefault);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkAlwaysOntop;
        private System.Windows.Forms.CheckBox chkMinimizeToSystemTray;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblProcessDescriptionFilter;
        private System.Windows.Forms.TextBox txtProcessDescriptionFilter;
        private System.Windows.Forms.Button btnRestoreDefault;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCloseAllGlobalShortcut;
        private System.Windows.Forms.Label lblGlobalShortcut;
    }
}