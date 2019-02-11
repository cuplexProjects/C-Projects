namespace CloseAllDemoApps
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnTerminate = new System.Windows.Forms.Button();
            this.ProcessListbox = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBarCloseApps = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSplitButtonLog = new System.Windows.Forms.ToolStripSplitButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlProcessList = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.toolStripProgressBarCLoseApps = new System.Windows.Forms.ToolStripProgressBar();
            this.sysTrayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.txtProcessDescriptionFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.pnlProcessList.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTerminate
            // 
            this.btnTerminate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTerminate.Location = new System.Drawing.Point(301, 5);
            this.btnTerminate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTerminate.Name = "btnTerminate";
            this.btnTerminate.Size = new System.Drawing.Size(170, 27);
            this.btnTerminate.TabIndex = 0;
            this.btnTerminate.Text = "Close Selected Processes";
            this.btnTerminate.UseVisualStyleBackColor = true;
            this.btnTerminate.Click += new System.EventHandler(this.btnTerminate_Click);
            // 
            // ProcessListbox
            // 
            this.ProcessListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProcessListbox.FormattingEnabled = true;
            this.ProcessListbox.ItemHeight = 20;
            this.ProcessListbox.Location = new System.Drawing.Point(0, 0);
            this.ProcessListbox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 4);
            this.ProcessListbox.Name = "ProcessListbox";
            this.ProcessListbox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ProcessListbox.Size = new System.Drawing.Size(462, 267);
            this.ProcessListbox.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(188, 5);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(107, 27);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(2, 358);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(478, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel1.Text = "Status:";
            // 
            // ProgressBarCloseApps
            // 
            this.ProgressBarCloseApps.Name = "ProgressBarCloseApps";
            this.ProgressBarCloseApps.Size = new System.Drawing.Size(250, 16);
            // 
            // toolStripSplitButtonLog
            // 
            this.toolStripSplitButtonLog.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSplitButtonLog.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.toolStripSplitButtonLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonLog.DropDownButtonWidth = 0;
            this.toolStripSplitButtonLog.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonLog.Image")));
            this.toolStripSplitButtonLog.ImageTransparentColor = System.Drawing.SystemColors.Control;
            this.toolStripSplitButtonLog.Name = "toolStripSplitButtonLog";
            this.toolStripSplitButtonLog.Size = new System.Drawing.Size(61, 20);
            this.toolStripSplitButtonLog.Text = "Show log";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProcessDescriptionFilter);
            this.groupBox1.Controls.Add(this.pnlProcessList);
            this.groupBox1.Controls.Add(this.pnlFooter);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(2, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.groupBox1.Size = new System.Drawing.Size(478, 358);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // pnlProcessList
            // 
            this.pnlProcessList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProcessList.Controls.Add(this.ProcessListbox);
            this.pnlProcessList.Location = new System.Drawing.Point(9, 52);
            this.pnlProcessList.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.pnlProcessList.Name = "pnlProcessList";
            this.pnlProcessList.Size = new System.Drawing.Size(462, 267);
            this.pnlProcessList.TabIndex = 5;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnSettings);
            this.pnlFooter.Controls.Add(this.btnTerminate);
            this.pnlFooter.Controls.Add(this.btnRefresh);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(2, 322);
            this.pnlFooter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(474, 36);
            this.pnlFooter.TabIndex = 0;
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettings.Location = new System.Drawing.Point(7, 4);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 27);
            this.btnSettings.TabIndex = 5;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // toolStripProgressBarCLoseApps
            // 
            this.toolStripProgressBarCLoseApps.Name = "toolStripProgressBarCLoseApps";
            this.toolStripProgressBarCLoseApps.Size = new System.Drawing.Size(250, 16);
            // 
            // sysTrayNotifyIcon
            // 
            this.sysTrayNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("sysTrayNotifyIcon.Icon")));
            this.sysTrayNotifyIcon.Text = "Close all demo apps";
            this.sysTrayNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sysTrayNotifyIcon_MouseDoubleClick);
            // 
            // txtProcessDescriptionFilter
            // 
            this.txtProcessDescriptionFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessDescriptionFilter.Location = new System.Drawing.Point(187, 17);
            this.txtProcessDescriptionFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProcessDescriptionFilter.MaxLength = 512;
            this.txtProcessDescriptionFilter.Name = "txtProcessDescriptionFilter";
            this.txtProcessDescriptionFilter.Size = new System.Drawing.Size(284, 27);
            this.txtProcessDescriptionFilter.TabIndex = 5;
            this.txtProcessDescriptionFilter.Text = "Toyota.TMHE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(5, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Process Description filter:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 380);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 425);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Close all running demo apps";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlProcessList.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTerminate;
        private System.Windows.Forms.ListBox ProcessListbox;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar ProgressBarCloseApps;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonLog;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarCLoseApps;
        private System.Windows.Forms.Panel pnlProcessList;
        private System.Windows.Forms.NotifyIcon sysTrayNotifyIcon;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.TextBox txtProcessDescriptionFilter;
        private System.Windows.Forms.Label label1;
    }
}

