namespace SecureMemo.UserControls
{
    partial class MemoTabPageControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.contentPanel = new System.Windows.Forms.Panel();
            this.tabRichTextBox = new System.Windows.Forms.RichTextBox();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.tabRichTextBox);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(400, 300);
            this.contentPanel.TabIndex = 0;
            // 
            // tabRichTextBox
            // 
            this.tabRichTextBox.AcceptsTab = true;
            this.tabRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabRichTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabRichTextBox.HideSelection = false;
            this.tabRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.tabRichTextBox.Name = "tabRichTextBox";
            this.tabRichTextBox.Size = new System.Drawing.Size(400, 300);
            this.tabRichTextBox.TabIndex = 0;
            this.tabRichTextBox.Text = "";
            this.tabRichTextBox.SelectionChanged += new System.EventHandler(this.tabRichTextBox_SelectionChanged);
            this.tabRichTextBox.TextChanged += new System.EventHandler(this.tabRichTextBox_TextChanged);
            // 
            // MemoTabPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.contentPanel);
            this.Name = "MemoTabPageControl";
            this.Size = new System.Drawing.Size(400, 300);
            this.contentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.RichTextBox tabRichTextBox;
    }
}
