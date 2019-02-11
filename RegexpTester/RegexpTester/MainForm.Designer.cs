namespace RegexpTester
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtRegularExpression = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReplace = new System.Windows.Forms.Button();
            this.chkGlobal = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAutoMatch = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkOntop = new System.Windows.Forms.CheckBox();
            this.btnMatch = new System.Windows.Forms.Button();
            this.txtReplacement = new System.Windows.Forms.TextBox();
            this.chkMultiline = new System.Windows.Forms.CheckBox();
            this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSilentError = new System.Windows.Forms.Label();
            this.lblMatchCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSubjectString = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRegularExpression
            // 
            this.txtRegularExpression.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRegularExpression.Location = new System.Drawing.Point(9, 19);
            this.txtRegularExpression.Multiline = true;
            this.txtRegularExpression.Name = "txtRegularExpression";
            this.txtRegularExpression.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRegularExpression.Size = new System.Drawing.Size(375, 120);
            this.txtRegularExpression.TabIndex = 1;
            this.txtRegularExpression.TextChanged += new System.EventHandler(this.txtRegularExpression_TextChanged);
            this.txtRegularExpression.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtRegularExpression.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Replacement:";
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(409, 19);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(69, 24);
            this.btnReplace.TabIndex = 3;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // chkGlobal
            // 
            this.chkGlobal.AutoSize = true;
            this.chkGlobal.Checked = true;
            this.chkGlobal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGlobal.Location = new System.Drawing.Point(10, 145);
            this.chkGlobal.Name = "chkGlobal";
            this.chkGlobal.Size = new System.Drawing.Size(60, 17);
            this.chkGlobal.TabIndex = 8;
            this.chkGlobal.Text = "Global";
            this.chkGlobal.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAutoMatch);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.chkOntop);
            this.groupBox1.Controls.Add(this.btnMatch);
            this.groupBox1.Controls.Add(this.txtReplacement);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chkMultiline);
            this.groupBox1.Controls.Add(this.btnReplace);
            this.groupBox1.Controls.Add(this.chkIgnoreCase);
            this.groupBox1.Controls.Add(this.txtRegularExpression);
            this.groupBox1.Controls.Add(this.chkGlobal);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 226);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Regular Expression";
            // 
            // chkAutoMatch
            // 
            this.chkAutoMatch.AutoSize = true;
            this.chkAutoMatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoMatch.Location = new System.Drawing.Point(398, 122);
            this.chkAutoMatch.Name = "chkAutoMatch";
            this.chkAutoMatch.Size = new System.Drawing.Size(90, 17);
            this.chkAutoMatch.TabIndex = 6;
            this.chkAutoMatch.Text = "Auto match";
            this.chkAutoMatch.UseVisualStyleBackColor = true;
            this.chkAutoMatch.CheckedChanged += new System.EventHandler(this.chkAutoMatch_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(409, 79);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(69, 24);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chkOntop
            // 
            this.chkOntop.AutoSize = true;
            this.chkOntop.Location = new System.Drawing.Point(398, 145);
            this.chkOntop.Name = "chkOntop";
            this.chkOntop.Size = new System.Drawing.Size(98, 17);
            this.chkOntop.TabIndex = 7;
            this.chkOntop.Text = "Always Ontop";
            this.chkOntop.UseVisualStyleBackColor = true;
            this.chkOntop.CheckedChanged += new System.EventHandler(this.chkOntop_CheckedChanged);
            // 
            // btnMatch
            // 
            this.btnMatch.Location = new System.Drawing.Point(409, 49);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.Size = new System.Drawing.Size(69, 24);
            this.btnMatch.TabIndex = 4;
            this.btnMatch.Text = "Match";
            this.btnMatch.UseVisualStyleBackColor = true;
            this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click);
            // 
            // txtReplacement
            // 
            this.txtReplacement.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReplacement.Location = new System.Drawing.Point(10, 190);
            this.txtReplacement.MaxLength = 50;
            this.txtReplacement.Name = "txtReplacement";
            this.txtReplacement.Size = new System.Drawing.Size(375, 23);
            this.txtReplacement.TabIndex = 2;
            this.txtReplacement.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtReplacement.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // chkMultiline
            // 
            this.chkMultiline.AutoSize = true;
            this.chkMultiline.Checked = true;
            this.chkMultiline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMultiline.Location = new System.Drawing.Point(158, 145);
            this.chkMultiline.Name = "chkMultiline";
            this.chkMultiline.Size = new System.Drawing.Size(72, 17);
            this.chkMultiline.TabIndex = 10;
            this.chkMultiline.Text = "Multiline";
            this.chkMultiline.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreCase
            // 
            this.chkIgnoreCase.AutoSize = true;
            this.chkIgnoreCase.Location = new System.Drawing.Point(72, 145);
            this.chkIgnoreCase.Name = "chkIgnoreCase";
            this.chkIgnoreCase.Size = new System.Drawing.Size(84, 17);
            this.chkIgnoreCase.TabIndex = 9;
            this.chkIgnoreCase.Text = "IgnoreCase";
            this.chkIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblSilentError);
            this.groupBox2.Controls.Add(this.lblMatchCount);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtResult);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtSubjectString);
            this.groupBox2.Location = new System.Drawing.Point(12, 244);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(500, 299);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Result";
            // 
            // lblSilentError
            // 
            this.lblSilentError.ForeColor = System.Drawing.Color.Maroon;
            this.lblSilentError.Location = new System.Drawing.Point(391, 54);
            this.lblSilentError.Name = "lblSilentError";
            this.lblSilentError.Size = new System.Drawing.Size(103, 239);
            this.lblSilentError.TabIndex = 8;
            this.lblSilentError.Text = "[EMPTY]";
            // 
            // lblMatchCount
            // 
            this.lblMatchCount.AutoSize = true;
            this.lblMatchCount.Location = new System.Drawing.Point(459, 32);
            this.lblMatchCount.Name = "lblMatchCount";
            this.lblMatchCount.Size = new System.Drawing.Size(13, 13);
            this.lblMatchCount.TabIndex = 7;
            this.lblMatchCount.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(391, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Match count:";
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.White;
            this.txtResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(6, 168);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(375, 125);
            this.txtResult.TabIndex = 2;
            this.txtResult.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Result:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Subject string:";
            // 
            // txtSubjectString
            // 
            this.txtSubjectString.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtSubjectString.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSubjectString.Location = new System.Drawing.Point(10, 29);
            this.txtSubjectString.Multiline = true;
            this.txtSubjectString.Name = "txtSubjectString";
            this.txtSubjectString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSubjectString.Size = new System.Drawing.Size(375, 120);
            this.txtSubjectString.TabIndex = 1;
            this.txtSubjectString.TextChanged += new System.EventHandler(this.txtSubjectString_TextChanged);
            this.txtSubjectString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyDown);
            this.txtSubjectString.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBox_KeyUp);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 552);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Regular expression tester - Made by Martin Dahl 2013";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtRegularExpression;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.CheckBox chkGlobal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnMatch;
        private System.Windows.Forms.TextBox txtReplacement;
        private System.Windows.Forms.CheckBox chkMultiline;
        private System.Windows.Forms.CheckBox chkIgnoreCase;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkOntop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSubjectString;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RichTextBox txtResult;
        private System.Windows.Forms.Label lblMatchCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAutoMatch;
        private System.Windows.Forms.Label lblSilentError;
    }
}

