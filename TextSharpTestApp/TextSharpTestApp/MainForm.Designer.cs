namespace TextSharpTestApp
{
    partial class MainForm
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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtText1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGeneratePdf = new System.Windows.Forms.Button();
            this.txtText2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtText3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.txtFont = new System.Windows.Forms.TextBox();
            this.btnSetNewFont = new System.Windows.Forms.Button();
            this.pictureBoxBackground = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.pictureBoxBackground);
            this.groupBox1.Controls.Add(this.btnSetNewFont);
            this.groupBox1.Controls.Add(this.txtFont);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtText3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtText2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnGeneratePdf);
            this.groupBox1.Controls.Add(this.txtText1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(558, 349);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PDF Properties";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Controls.Add(this.txtFilename);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 367);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(558, 74);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Save";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filename:";
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(9, 38);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(487, 22);
            this.txtFilename.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(502, 38);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(50, 22);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "..";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtText1
            // 
            this.txtText1.Location = new System.Drawing.Point(9, 54);
            this.txtText1.Name = "txtText1";
            this.txtText1.Size = new System.Drawing.Size(543, 22);
            this.txtText1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Text 1:";
            // 
            // btnGeneratePdf
            // 
            this.btnGeneratePdf.Location = new System.Drawing.Point(415, 306);
            this.btnGeneratePdf.Name = "btnGeneratePdf";
            this.btnGeneratePdf.Size = new System.Drawing.Size(137, 26);
            this.btnGeneratePdf.TabIndex = 5;
            this.btnGeneratePdf.Text = "Generate PDF";
            this.btnGeneratePdf.UseVisualStyleBackColor = true;
            this.btnGeneratePdf.Click += new System.EventHandler(this.btnGeneratePdf_Click);
            // 
            // txtText2
            // 
            this.txtText2.Location = new System.Drawing.Point(9, 99);
            this.txtText2.Name = "txtText2";
            this.txtText2.Size = new System.Drawing.Size(543, 22);
            this.txtText2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Text 2:";
            // 
            // txtText3
            // 
            this.txtText3.Location = new System.Drawing.Point(9, 144);
            this.txtText3.Name = "txtText3";
            this.txtText3.Size = new System.Drawing.Size(543, 22);
            this.txtText3.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Text 3:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Font:";
            // 
            // txtFont
            // 
            this.txtFont.Location = new System.Drawing.Point(9, 200);
            this.txtFont.Name = "txtFont";
            this.txtFont.Size = new System.Drawing.Size(543, 22);
            this.txtFont.TabIndex = 3;
            // 
            // btnSetNewFont
            // 
            this.btnSetNewFont.Location = new System.Drawing.Point(415, 274);
            this.btnSetNewFont.Name = "btnSetNewFont";
            this.btnSetNewFont.Size = new System.Drawing.Size(137, 26);
            this.btnSetNewFont.TabIndex = 4;
            this.btnSetNewFont.Text = "Set New Font";
            this.btnSetNewFont.UseVisualStyleBackColor = true;
            this.btnSetNewFont.Click += new System.EventHandler(this.btnSetNewFont_Click);
            // 
            // pictureBoxBackground
            // 
            this.pictureBoxBackground.Image = global::TextSharpTestApp.Resource1.demo;
            this.pictureBoxBackground.Location = new System.Drawing.Point(9, 274);
            this.pictureBoxBackground.Name = "pictureBoxBackground";
            this.pictureBoxBackground.Size = new System.Drawing.Size(141, 57);
            this.pictureBoxBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBackground.TabIndex = 12;
            this.pictureBoxBackground.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Background Image:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 453);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create PDF";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackground)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnGeneratePdf;
        private System.Windows.Forms.TextBox txtText1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtText3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtText2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSetNewFont;
        private System.Windows.Forms.TextBox txtFont;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBoxBackground;
    }
}

