namespace PriceCalculator
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
            this.grpExchange = new System.Windows.Forms.GroupBox();
            this.txtSekUsdRate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBitcoinPrice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCommision = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPriceInUSD = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPriceInSEK = new System.Windows.Forms.TextBox();
            this.txtPriceInBTC = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkTopmost = new System.Windows.Forms.CheckBox();
            this.txtNumberOfItems = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.grpExtraParameters = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkAutofocusPrice = new System.Windows.Forms.CheckBox();
            this.grpExchange.SuspendLayout();
            this.grpResult.SuspendLayout();
            this.grpExtraParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpExchange
            // 
            this.grpExchange.Controls.Add(this.txtSekUsdRate);
            this.grpExchange.Controls.Add(this.label2);
            this.grpExchange.Controls.Add(this.txtBitcoinPrice);
            this.grpExchange.Controls.Add(this.label1);
            this.grpExchange.Location = new System.Drawing.Point(12, 12);
            this.grpExchange.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpExchange.Name = "grpExchange";
            this.grpExchange.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpExchange.Size = new System.Drawing.Size(235, 100);
            this.grpExchange.TabIndex = 0;
            this.grpExchange.TabStop = false;
            this.grpExchange.Text = "Exchange rates";
            // 
            // txtSekUsdRate
            // 
            this.txtSekUsdRate.Location = new System.Drawing.Point(133, 60);
            this.txtSekUsdRate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSekUsdRate.MaxLength = 10;
            this.txtSekUsdRate.Name = "txtSekUsdRate";
            this.txtSekUsdRate.Size = new System.Drawing.Size(80, 25);
            this.txtSekUsdRate.TabIndex = 1;
            this.txtSekUsdRate.Text = "7,00";
            this.txtSekUsdRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "SEK/USD:";
            // 
            // txtBitcoinPrice
            // 
            this.txtBitcoinPrice.Location = new System.Drawing.Point(133, 27);
            this.txtBitcoinPrice.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBitcoinPrice.MaxLength = 10;
            this.txtBitcoinPrice.Name = "txtBitcoinPrice";
            this.txtBitcoinPrice.Size = new System.Drawing.Size(80, 25);
            this.txtBitcoinPrice.TabIndex = 0;
            this.txtBitcoinPrice.Text = "100,00";
            this.txtBitcoinPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bitcoin Price USD:";
            // 
            // txtCommision
            // 
            this.txtCommision.Location = new System.Drawing.Point(144, 27);
            this.txtCommision.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCommision.MaxLength = 8;
            this.txtCommision.Name = "txtCommision";
            this.txtCommision.Size = new System.Drawing.Size(57, 25);
            this.txtCommision.TabIndex = 2;
            this.txtCommision.Text = "0";
            this.txtCommision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Commision (%):";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(353, 224);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 30);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // grpResult
            // 
            this.grpResult.Controls.Add(this.label8);
            this.grpResult.Controls.Add(this.txtPriceInUSD);
            this.grpResult.Controls.Add(this.label6);
            this.grpResult.Controls.Add(this.txtPriceInSEK);
            this.grpResult.Location = new System.Drawing.Point(253, 12);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(175, 100);
            this.grpResult.TabIndex = 2;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "Result";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(126, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "USD";
            // 
            // txtPriceInUSD
            // 
            this.txtPriceInUSD.Location = new System.Drawing.Point(20, 27);
            this.txtPriceInUSD.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPriceInUSD.MaxLength = 10;
            this.txtPriceInUSD.Name = "txtPriceInUSD";
            this.txtPriceInUSD.ReadOnly = true;
            this.txtPriceInUSD.Size = new System.Drawing.Size(100, 25);
            this.txtPriceInUSD.TabIndex = 13;
            this.txtPriceInUSD.TabStop = false;
            this.txtPriceInUSD.Text = "0,00";
            this.txtPriceInUSD.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(126, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "SEK";
            // 
            // txtPriceInSEK
            // 
            this.txtPriceInSEK.Location = new System.Drawing.Point(20, 60);
            this.txtPriceInSEK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPriceInSEK.MaxLength = 10;
            this.txtPriceInSEK.Name = "txtPriceInSEK";
            this.txtPriceInSEK.ReadOnly = true;
            this.txtPriceInSEK.Size = new System.Drawing.Size(100, 25);
            this.txtPriceInSEK.TabIndex = 11;
            this.txtPriceInSEK.TabStop = false;
            this.txtPriceInSEK.Text = "0,00";
            this.txtPriceInSEK.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPriceInBTC
            // 
            this.txtPriceInBTC.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPriceInBTC.Location = new System.Drawing.Point(261, 48);
            this.txtPriceInBTC.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPriceInBTC.MaxLength = 10;
            this.txtPriceInBTC.Name = "txtPriceInBTC";
            this.txtPriceInBTC.Size = new System.Drawing.Size(100, 25);
            this.txtPriceInBTC.TabIndex = 0;
            this.txtPriceInBTC.Text = "0,00";
            this.txtPriceInBTC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(367, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "BTC";
            // 
            // chkTopmost
            // 
            this.chkTopmost.AutoSize = true;
            this.chkTopmost.Location = new System.Drawing.Point(12, 224);
            this.chkTopmost.Name = "chkTopmost";
            this.chkTopmost.Size = new System.Drawing.Size(84, 21);
            this.chkTopmost.TabIndex = 5;
            this.chkTopmost.Text = "Top Most";
            this.chkTopmost.UseVisualStyleBackColor = true;
            this.chkTopmost.CheckedChanged += new System.EventHandler(this.chkTopmost_CheckedChanged);
            // 
            // txtNumberOfItems
            // 
            this.txtNumberOfItems.Location = new System.Drawing.Point(144, 60);
            this.txtNumberOfItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNumberOfItems.MaxLength = 8;
            this.txtNumberOfItems.Name = "txtNumberOfItems";
            this.txtNumberOfItems.Size = new System.Drawing.Size(57, 25);
            this.txtNumberOfItems.TabIndex = 1;
            this.txtNumberOfItems.Text = "1";
            this.txtNumberOfItems.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Number Of Items:";
            // 
            // grpExtraParameters
            // 
            this.grpExtraParameters.Controls.Add(this.label7);
            this.grpExtraParameters.Controls.Add(this.txtCommision);
            this.grpExtraParameters.Controls.Add(this.label3);
            this.grpExtraParameters.Controls.Add(this.txtPriceInBTC);
            this.grpExtraParameters.Controls.Add(this.txtNumberOfItems);
            this.grpExtraParameters.Controls.Add(this.label5);
            this.grpExtraParameters.Controls.Add(this.label4);
            this.grpExtraParameters.Location = new System.Drawing.Point(12, 118);
            this.grpExtraParameters.Name = "grpExtraParameters";
            this.grpExtraParameters.Size = new System.Drawing.Size(416, 100);
            this.grpExtraParameters.TabIndex = 8;
            this.grpExtraParameters.TabStop = false;
            this.grpExtraParameters.Text = "Additional Parameters";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(258, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Item price:";
            // 
            // chkAutofocusPrice
            // 
            this.chkAutofocusPrice.AutoSize = true;
            this.chkAutofocusPrice.Location = new System.Drawing.Point(102, 224);
            this.chkAutofocusPrice.Name = "chkAutofocusPrice";
            this.chkAutofocusPrice.Size = new System.Drawing.Size(117, 21);
            this.chkAutofocusPrice.TabIndex = 9;
            this.chkAutofocusPrice.Text = "Autofocus Price";
            this.chkAutofocusPrice.UseVisualStyleBackColor = true;
            this.chkAutofocusPrice.CheckedChanged += new System.EventHandler(this.chkAutofocusPrice_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 264);
            this.Controls.Add(this.chkAutofocusPrice);
            this.Controls.Add(this.grpExtraParameters);
            this.Controls.Add(this.chkTopmost);
            this.Controls.Add(this.grpResult);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.grpExchange);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bitcoin Price Calculator";
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.Deactivate += new System.EventHandler(this.frmMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.grpExchange.ResumeLayout(false);
            this.grpExchange.PerformLayout();
            this.grpResult.ResumeLayout(false);
            this.grpResult.PerformLayout();
            this.grpExtraParameters.ResumeLayout(false);
            this.grpExtraParameters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpExchange;
        private System.Windows.Forms.TextBox txtBitcoinPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtSekUsdRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCommision;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPriceInBTC;
        private System.Windows.Forms.CheckBox chkTopmost;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPriceInUSD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPriceInSEK;
        private System.Windows.Forms.TextBox txtNumberOfItems;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grpExtraParameters;
        private System.Windows.Forms.CheckBox chkAutofocusPrice;
        private System.Windows.Forms.Label label7;
    }
}

