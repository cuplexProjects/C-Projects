namespace SetTimeAndDate
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
            this.btnSetDate = new System.Windows.Forms.Button();
            this.btnResetDate = new System.Windows.Forms.Button();
            this.dtPickerTarget = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentDate = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.syncTimer = new System.Windows.Forms.Timer(this.components);
            this.txtTargetTime = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetDate
            // 
            this.btnSetDate.Location = new System.Drawing.Point(228, 111);
            this.btnSetDate.Name = "btnSetDate";
            this.btnSetDate.Size = new System.Drawing.Size(77, 26);
            this.btnSetDate.TabIndex = 0;
            this.btnSetDate.Text = "Set Date";
            this.btnSetDate.UseVisualStyleBackColor = true;
            this.btnSetDate.Click += new System.EventHandler(this.btnSetDate_Click);
            // 
            // btnResetDate
            // 
            this.btnResetDate.Location = new System.Drawing.Point(311, 111);
            this.btnResetDate.Name = "btnResetDate";
            this.btnResetDate.Size = new System.Drawing.Size(77, 26);
            this.btnResetDate.TabIndex = 2;
            this.btnResetDate.Text = "Reset Date";
            this.btnResetDate.UseVisualStyleBackColor = true;
            this.btnResetDate.Click += new System.EventHandler(this.btnResetDate_Click);
            // 
            // dtPickerTarget
            // 
            this.dtPickerTarget.Location = new System.Drawing.Point(6, 19);
            this.dtPickerTarget.Name = "dtPickerTarget";
            this.dtPickerTarget.Size = new System.Drawing.Size(158, 20);
            this.dtPickerTarget.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCurrentTime);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblCurrentDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 90);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current date and time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date:";
            // 
            // lblCurrentDate
            // 
            this.lblCurrentDate.AutoSize = true;
            this.lblCurrentDate.Location = new System.Drawing.Point(54, 28);
            this.lblCurrentDate.Name = "lblCurrentDate";
            this.lblCurrentDate.Size = new System.Drawing.Size(35, 13);
            this.lblCurrentDate.TabIndex = 1;
            this.lblCurrentDate.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Time:";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(54, 57);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(35, 13);
            this.lblCurrentTime.TabIndex = 3;
            this.lblCurrentTime.Text = "label4";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTargetTime);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dtPickerTarget);
            this.groupBox2.Location = new System.Drawing.Point(203, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 90);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target date and time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Time:";
            // 
            // syncTimer
            // 
            this.syncTimer.Tick += new System.EventHandler(this.syncTimer_Tick);
            // 
            // txtTargetTime
            // 
            this.txtTargetTime.Location = new System.Drawing.Point(36, 50);
            this.txtTargetTime.Mask = "00:00:00";
            this.txtTargetTime.Name = "txtTargetTime";
            this.txtTargetTime.Size = new System.Drawing.Size(57, 20);
            this.txtTargetTime.TabIndex = 5;
            this.txtTargetTime.Text = "000000";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 143);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnResetDate);
            this.Controls.Add(this.btnSetDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Date set";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSetDate;
        private System.Windows.Forms.Button btnResetDate;
        private System.Windows.Forms.DateTimePicker dtPickerTarget;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCurrentDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer syncTimer;
        private System.Windows.Forms.MaskedTextBox txtTargetTime;
    }
}

