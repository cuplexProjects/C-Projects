namespace HTML_Encode
{
    partial class Form1
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
            this.txtPasteBin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHtmlEncodedResult = new System.Windows.Forms.TextBox();
            this.radioSingleExpression = new System.Windows.Forms.RadioButton();
            this.radioEncodeDocument = new System.Windows.Forms.RadioButton();
            this.chkUseClipboard = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPasteBin
            // 
            this.txtPasteBin.Location = new System.Drawing.Point(12, 48);
            this.txtPasteBin.Multiline = true;
            this.txtPasteBin.Name = "txtPasteBin";
            this.txtPasteBin.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPasteBin.Size = new System.Drawing.Size(415, 175);
            this.txtPasteBin.TabIndex = 0;
            this.txtPasteBin.TextChanged += new System.EventHandler(this.txtPasteBin_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Paste your text here and the output will be placed in clipboard memory.";
            // 
            // txtHtmlEncodedResult
            // 
            this.txtHtmlEncodedResult.Location = new System.Drawing.Point(12, 229);
            this.txtHtmlEncodedResult.Multiline = true;
            this.txtHtmlEncodedResult.Name = "txtHtmlEncodedResult";
            this.txtHtmlEncodedResult.ReadOnly = true;
            this.txtHtmlEncodedResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHtmlEncodedResult.Size = new System.Drawing.Size(415, 175);
            this.txtHtmlEncodedResult.TabIndex = 2;
            // 
            // radioSingleExpression
            // 
            this.radioSingleExpression.AutoSize = true;
            this.radioSingleExpression.Checked = true;
            this.radioSingleExpression.Location = new System.Drawing.Point(15, 25);
            this.radioSingleExpression.Name = "radioSingleExpression";
            this.radioSingleExpression.Size = new System.Drawing.Size(84, 17);
            this.radioSingleExpression.TabIndex = 3;
            this.radioSingleExpression.TabStop = true;
            this.radioSingleExpression.Text = "Enoce string";
            this.radioSingleExpression.UseVisualStyleBackColor = true;
            // 
            // radioEncodeDocument
            // 
            this.radioEncodeDocument.AutoSize = true;
            this.radioEncodeDocument.Location = new System.Drawing.Point(105, 25);
            this.radioEncodeDocument.Name = "radioEncodeDocument";
            this.radioEncodeDocument.Size = new System.Drawing.Size(112, 17);
            this.radioEncodeDocument.TabIndex = 4;
            this.radioEncodeDocument.Text = "Encode document";
            this.radioEncodeDocument.UseVisualStyleBackColor = true;
            // 
            // chkUseClipboard
            // 
            this.chkUseClipboard.AutoSize = true;
            this.chkUseClipboard.Location = new System.Drawing.Point(223, 25);
            this.chkUseClipboard.Name = "chkUseClipboard";
            this.chkUseClipboard.Size = new System.Drawing.Size(124, 17);
            this.chkUseClipboard.TabIndex = 5;
            this.chkUseClipboard.Text = "Use clipboard output";
            this.chkUseClipboard.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(357, 9);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 28);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 415);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkUseClipboard);
            this.Controls.Add(this.radioEncodeDocument);
            this.Controls.Add(this.radioSingleExpression);
            this.Controls.Add(this.txtHtmlEncodedResult);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPasteBin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HTML Encoder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPasteBin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHtmlEncodedResult;
        private System.Windows.Forms.RadioButton radioSingleExpression;
        private System.Windows.Forms.RadioButton radioEncodeDocument;
        private System.Windows.Forms.CheckBox chkUseClipboard;
        private System.Windows.Forms.Button btnClear;
    }
}

