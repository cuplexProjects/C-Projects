namespace TestPrime
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
            this.txtBase64Input = new System.Windows.Forms.TextBox();
            this.txtPrimeNumberOutput = new System.Windows.Forms.TextBox();
            this.btnTestConvert = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblInputSize = new System.Windows.Forms.Label();
            this.lblOutputTextLength = new System.Windows.Forms.Label();
            this.lblOutLength = new System.Windows.Forms.Label();
            this.lblInLength = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBase64Input
            // 
            this.txtBase64Input.Location = new System.Drawing.Point(12, 12);
            this.txtBase64Input.Multiline = true;
            this.txtBase64Input.Name = "txtBase64Input";
            this.txtBase64Input.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBase64Input.Size = new System.Drawing.Size(660, 200);
            this.txtBase64Input.TabIndex = 0;
            // 
            // txtPrimeNumberOutput
            // 
            this.txtPrimeNumberOutput.Location = new System.Drawing.Point(12, 218);
            this.txtPrimeNumberOutput.Multiline = true;
            this.txtPrimeNumberOutput.Name = "txtPrimeNumberOutput";
            this.txtPrimeNumberOutput.ReadOnly = true;
            this.txtPrimeNumberOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrimeNumberOutput.Size = new System.Drawing.Size(660, 196);
            this.txtPrimeNumberOutput.TabIndex = 1;
            // 
            // btnTestConvert
            // 
            this.btnTestConvert.Location = new System.Drawing.Point(532, 420);
            this.btnTestConvert.Name = "btnTestConvert";
            this.btnTestConvert.Size = new System.Drawing.Size(140, 30);
            this.btnTestConvert.TabIndex = 2;
            this.btnTestConvert.Text = "Test to convert";
            this.btnTestConvert.UseVisualStyleBackColor = true;
            this.btnTestConvert.Click += new System.EventHandler(this.btnTestConvert_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(437, 420);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(89, 30);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblInputSize
            // 
            this.lblInputSize.AutoSize = true;
            this.lblInputSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInputSize.Location = new System.Drawing.Point(12, 417);
            this.lblInputSize.Name = "lblInputSize";
            this.lblInputSize.Size = new System.Drawing.Size(83, 13);
            this.lblInputSize.TabIndex = 4;
            this.lblInputSize.Text = "Input Length:";
            // 
            // lblOutputTextLength
            // 
            this.lblOutputTextLength.AutoSize = true;
            this.lblOutputTextLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputTextLength.Location = new System.Drawing.Point(12, 437);
            this.lblOutputTextLength.Name = "lblOutputTextLength";
            this.lblOutputTextLength.Size = new System.Drawing.Size(92, 13);
            this.lblOutputTextLength.TabIndex = 5;
            this.lblOutputTextLength.Text = "Output Length:";
            // 
            // lblOutLength
            // 
            this.lblOutLength.AutoSize = true;
            this.lblOutLength.Location = new System.Drawing.Point(101, 437);
            this.lblOutLength.Name = "lblOutLength";
            this.lblOutLength.Size = new System.Drawing.Size(13, 13);
            this.lblOutLength.TabIndex = 6;
            this.lblOutLength.Text = "0";
            // 
            // lblInLength
            // 
            this.lblInLength.AutoSize = true;
            this.lblInLength.Location = new System.Drawing.Point(101, 417);
            this.lblInLength.Name = "lblInLength";
            this.lblInLength.Size = new System.Drawing.Size(13, 13);
            this.lblInLength.TabIndex = 7;
            this.lblInLength.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.lblInLength);
            this.Controls.Add(this.lblOutLength);
            this.Controls.Add(this.lblOutputTextLength);
            this.Controls.Add(this.lblInputSize);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnTestConvert);
            this.Controls.Add(this.txtPrimeNumberOutput);
            this.Controls.Add(this.txtBase64Input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test Large Prime";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBase64Input;
        private System.Windows.Forms.TextBox txtPrimeNumberOutput;
        private System.Windows.Forms.Button btnTestConvert;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblInputSize;
        private System.Windows.Forms.Label lblOutputTextLength;
        private System.Windows.Forms.Label lblOutLength;
        private System.Windows.Forms.Label lblInLength;
    }
}

