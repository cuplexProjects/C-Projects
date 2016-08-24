namespace WiFiPasswordGenerator
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
                _innerPen.Dispose();
                _outerPen.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabelLastQRPath = new System.Windows.Forms.LinkLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPasswordTypesDescr = new System.Windows.Forms.TextBox();
            this.flowLayoutOutputType = new System.Windows.Forms.FlowLayoutPanel();
            this.rbDefault = new System.Windows.Forms.RadioButton();
            this.rbAlphaNumberic = new System.Windows.Forms.RadioButton();
            this.rbNumeric = new System.Windows.Forms.RadioButton();
            this.rbBase64 = new System.Windows.Forms.RadioButton();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.txtPasswordLength = new System.Windows.Forms.TextBox();
            this.lblPasswordLength = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutQRSettings = new System.Windows.Forms.FlowLayoutPanel();
            this.rbLevel0 = new System.Windows.Forms.RadioButton();
            this.rbLevel1 = new System.Windows.Forms.RadioButton();
            this.rbLevel2 = new System.Windows.Forms.RadioButton();
            this.rbLevel3 = new System.Windows.Forms.RadioButton();
            this.txtECCDescription = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.contextMenuQRImage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyImgInStringEncoding = new System.Windows.Forms.ToolStripMenuItem();
            this.grpBoxQRCode = new System.Windows.Forms.GroupBox();
            this.PicBoxQRCode = new System.Windows.Forms.PictureBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.txtGeneratedPassword = new System.Windows.Forms.TextBox();
            this.contextMenuGeneratedPassword = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTextFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.grpImgRes = new System.Windows.Forms.GroupBox();
            this.pnlUserDefinedRes = new System.Windows.Forms.Panel();
            this.txtUserDefinedQRHeight = new System.Windows.Forms.TextBox();
            this.txtUserDefinedQRWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbUserDefined = new System.Windows.Forms.RadioButton();
            this.rbDefaultRes = new System.Windows.Forms.RadioButton();
            this.btnSaveQRCode = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuClipboardIO = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemCBImport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImportPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemImportBase64ImgData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCBExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExportQRImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExportPwdStr = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.generateQRCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipPasswordLength = new System.Windows.Forms.ToolTip(this.components);
            this.generateQRCodeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flowLayoutOutputType.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutQRSettings.SuspendLayout();
            this.contextMenuQRImage.SuspendLayout();
            this.grpBoxQRCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxQRCode)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.contextMenuGeneratedPassword.SuspendLayout();
            this.grpImgRes.SuspendLayout();
            this.pnlUserDefinedRes.SuspendLayout();
            this.contextMenuClipboardIO.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.linkLabelLastQRPath);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.txtPasswordLength);
            this.groupBox1.Controls.Add(this.lblPasswordLength);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(685, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // linkLabelLastQRPath
            // 
            this.linkLabelLastQRPath.AutoSize = true;
            this.linkLabelLastQRPath.Location = new System.Drawing.Point(196, 172);
            this.linkLabelLastQRPath.Name = "linkLabelLastQRPath";
            this.linkLabelLastQRPath.Size = new System.Drawing.Size(83, 13);
            this.linkLabelLastQRPath.TabIndex = 7;
            this.linkLabelLastQRPath.TabStop = true;
            this.linkLabelLastQRPath.Text = "http://127.0.0.1";
            this.linkLabelLastQRPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLastQRPath_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPasswordTypesDescr);
            this.groupBox3.Controls.Add(this.flowLayoutOutputType);
            this.groupBox3.Location = new System.Drawing.Point(303, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(375, 140);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Password Types";
            // 
            // txtPasswordTypesDescr
            // 
            this.txtPasswordTypesDescr.BackColor = System.Drawing.SystemColors.Control;
            this.txtPasswordTypesDescr.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPasswordTypesDescr.Location = new System.Drawing.Point(137, 19);
            this.txtPasswordTypesDescr.Multiline = true;
            this.txtPasswordTypesDescr.Name = "txtPasswordTypesDescr";
            this.txtPasswordTypesDescr.ReadOnly = true;
            this.txtPasswordTypesDescr.Size = new System.Drawing.Size(225, 103);
            this.txtPasswordTypesDescr.TabIndex = 7;
            this.txtPasswordTypesDescr.Text = "Default settings uses alpha numeric characters, UC, LC and spcecial characters li" +
    "ke !+-@#?$";
            // 
            // flowLayoutOutputType
            // 
            this.flowLayoutOutputType.Controls.Add(this.rbDefault);
            this.flowLayoutOutputType.Controls.Add(this.rbAlphaNumberic);
            this.flowLayoutOutputType.Controls.Add(this.rbNumeric);
            this.flowLayoutOutputType.Controls.Add(this.rbBase64);
            this.flowLayoutOutputType.Controls.Add(this.rbHex);
            this.flowLayoutOutputType.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutOutputType.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutOutputType.Name = "flowLayoutOutputType";
            this.flowLayoutOutputType.Size = new System.Drawing.Size(125, 115);
            this.flowLayoutOutputType.TabIndex = 5;
            // 
            // rbDefault
            // 
            this.rbDefault.AutoSize = true;
            this.rbDefault.Checked = true;
            this.rbDefault.Location = new System.Drawing.Point(3, 3);
            this.rbDefault.Name = "rbDefault";
            this.rbDefault.Size = new System.Drawing.Size(59, 17);
            this.rbDefault.TabIndex = 0;
            this.rbDefault.TabStop = true;
            this.rbDefault.Tag = "0";
            this.rbDefault.Text = "Default";
            this.rbDefault.UseVisualStyleBackColor = true;
            this.rbDefault.Click += new System.EventHandler(this.rbPasswordType_Click);
            // 
            // rbAlphaNumberic
            // 
            this.rbAlphaNumberic.AutoSize = true;
            this.rbAlphaNumberic.Location = new System.Drawing.Point(3, 26);
            this.rbAlphaNumberic.Name = "rbAlphaNumberic";
            this.rbAlphaNumberic.Size = new System.Drawing.Size(100, 17);
            this.rbAlphaNumberic.TabIndex = 1;
            this.rbAlphaNumberic.Tag = "1";
            this.rbAlphaNumberic.Text = "Alpha Numberic";
            this.rbAlphaNumberic.UseVisualStyleBackColor = true;
            this.rbAlphaNumberic.Click += new System.EventHandler(this.rbPasswordType_Click);
            // 
            // rbNumeric
            // 
            this.rbNumeric.AutoSize = true;
            this.rbNumeric.Location = new System.Drawing.Point(3, 49);
            this.rbNumeric.Name = "rbNumeric";
            this.rbNumeric.Size = new System.Drawing.Size(64, 17);
            this.rbNumeric.TabIndex = 2;
            this.rbNumeric.Tag = "2";
            this.rbNumeric.Text = "Numeric";
            this.rbNumeric.UseVisualStyleBackColor = true;
            this.rbNumeric.Click += new System.EventHandler(this.rbPasswordType_Click);
            // 
            // rbBase64
            // 
            this.rbBase64.AutoSize = true;
            this.rbBase64.Location = new System.Drawing.Point(3, 72);
            this.rbBase64.Name = "rbBase64";
            this.rbBase64.Size = new System.Drawing.Size(61, 17);
            this.rbBase64.TabIndex = 3;
            this.rbBase64.Tag = "3";
            this.rbBase64.Text = "Base64";
            this.rbBase64.UseVisualStyleBackColor = true;
            this.rbBase64.Click += new System.EventHandler(this.rbPasswordType_Click);
            // 
            // rbHex
            // 
            this.rbHex.AutoSize = true;
            this.rbHex.Location = new System.Drawing.Point(3, 95);
            this.rbHex.Name = "rbHex";
            this.rbHex.Size = new System.Drawing.Size(44, 17);
            this.rbHex.TabIndex = 4;
            this.rbHex.Tag = "4";
            this.rbHex.Text = "Hex";
            this.rbHex.UseVisualStyleBackColor = true;
            this.rbHex.Click += new System.EventHandler(this.rbPasswordType_Click);
            // 
            // txtPasswordLength
            // 
            this.txtPasswordLength.Location = new System.Drawing.Point(119, 165);
            this.txtPasswordLength.MaxLength = 6;
            this.txtPasswordLength.Name = "txtPasswordLength";
            this.txtPasswordLength.Size = new System.Drawing.Size(40, 20);
            this.txtPasswordLength.TabIndex = 4;
            this.txtPasswordLength.Text = "63";
            this.toolTipPasswordLength.SetToolTip(this.txtPasswordLength, "The password length can maximum be 500 characters.");
            this.txtPasswordLength.TextChanged += new System.EventHandler(this.txtPasswordLength_TextChanged);
            this.txtPasswordLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPasswordLength_KeyPress);
            this.txtPasswordLength.Validating += new System.ComponentModel.CancelEventHandler(this.txtPasswordLength_Validating);
            this.txtPasswordLength.Validated += new System.EventHandler(this.txtPasswordLength_Validated);
            // 
            // lblPasswordLength
            // 
            this.lblPasswordLength.AutoSize = true;
            this.lblPasswordLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasswordLength.Location = new System.Drawing.Point(9, 168);
            this.lblPasswordLength.Name = "lblPasswordLength";
            this.lblPasswordLength.Size = new System.Drawing.Size(104, 13);
            this.lblPasswordLength.TabIndex = 3;
            this.lblPasswordLength.Text = "PasswordLength:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flowLayoutQRSettings);
            this.groupBox2.Controls.Add(this.txtECCDescription);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 140);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "QR Code ECC Level";
            // 
            // flowLayoutQRSettings
            // 
            this.flowLayoutQRSettings.Controls.Add(this.rbLevel0);
            this.flowLayoutQRSettings.Controls.Add(this.rbLevel1);
            this.flowLayoutQRSettings.Controls.Add(this.rbLevel2);
            this.flowLayoutQRSettings.Controls.Add(this.rbLevel3);
            this.flowLayoutQRSettings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutQRSettings.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutQRSettings.Name = "flowLayoutQRSettings";
            this.flowLayoutQRSettings.Size = new System.Drawing.Size(45, 115);
            this.flowLayoutQRSettings.TabIndex = 1;
            // 
            // rbLevel0
            // 
            this.rbLevel0.AutoSize = true;
            this.rbLevel0.Location = new System.Drawing.Point(3, 3);
            this.rbLevel0.Name = "rbLevel0";
            this.rbLevel0.Size = new System.Drawing.Size(31, 17);
            this.rbLevel0.TabIndex = 0;
            this.rbLevel0.Text = "L";
            this.rbLevel0.UseVisualStyleBackColor = true;
            this.rbLevel0.Click += new System.EventHandler(this.rbQRCodeLevel_Click);
            // 
            // rbLevel1
            // 
            this.rbLevel1.AutoSize = true;
            this.rbLevel1.Checked = true;
            this.rbLevel1.Location = new System.Drawing.Point(3, 26);
            this.rbLevel1.Name = "rbLevel1";
            this.rbLevel1.Size = new System.Drawing.Size(34, 17);
            this.rbLevel1.TabIndex = 1;
            this.rbLevel1.TabStop = true;
            this.rbLevel1.Text = "M";
            this.rbLevel1.UseVisualStyleBackColor = true;
            this.rbLevel1.Click += new System.EventHandler(this.rbQRCodeLevel_Click);
            // 
            // rbLevel2
            // 
            this.rbLevel2.AutoSize = true;
            this.rbLevel2.Location = new System.Drawing.Point(3, 49);
            this.rbLevel2.Name = "rbLevel2";
            this.rbLevel2.Size = new System.Drawing.Size(33, 17);
            this.rbLevel2.TabIndex = 2;
            this.rbLevel2.Text = "Q";
            this.rbLevel2.UseVisualStyleBackColor = true;
            this.rbLevel2.Click += new System.EventHandler(this.rbQRCodeLevel_Click);
            // 
            // rbLevel3
            // 
            this.rbLevel3.AutoSize = true;
            this.rbLevel3.Location = new System.Drawing.Point(3, 72);
            this.rbLevel3.Name = "rbLevel3";
            this.rbLevel3.Size = new System.Drawing.Size(33, 17);
            this.rbLevel3.TabIndex = 3;
            this.rbLevel3.Text = "H";
            this.rbLevel3.UseVisualStyleBackColor = true;
            this.rbLevel3.Click += new System.EventHandler(this.rbQRCodeLevel_Click);
            // 
            // txtECCDescription
            // 
            this.txtECCDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txtECCDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtECCDescription.Location = new System.Drawing.Point(59, 19);
            this.txtECCDescription.Multiline = true;
            this.txtECCDescription.Name = "txtECCDescription";
            this.txtECCDescription.ReadOnly = true;
            this.txtECCDescription.Size = new System.Drawing.Size(225, 103);
            this.txtECCDescription.TabIndex = 4;
            this.txtECCDescription.Text = "Level L - 7% of codewords can be restored\r\nLevel M - 15% of codewords can be rest" +
    "ored\r\nLevel Q - 25% of codewords can be restored\r\nLevel H - 30% of codewords can" +
    " be restored";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(570, 434);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(120, 26);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate Password";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // contextMenuQRImage
            // 
            this.contextMenuQRImage.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuQRImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuItemCopy,
            this.toolStripMenuItemCopyImgInStringEncoding});
            this.contextMenuQRImage.Name = "Edit";
            this.contextMenuQRImage.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuQRImage.Size = new System.Drawing.Size(336, 80);
            this.contextMenuQRImage.Text = "Edit";
            // 
            // contextMenuItemCopy
            // 
            this.contextMenuItemCopy.Image = global::WiFiPasswordGenerator.Properties.Resources._32image_x_generic1;
            this.contextMenuItemCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.contextMenuItemCopy.Name = "contextMenuItemCopy";
            this.contextMenuItemCopy.Size = new System.Drawing.Size(335, 38);
            this.contextMenuItemCopy.Text = "Copy Image to Clipboard";
            this.contextMenuItemCopy.Click += new System.EventHandler(this.contextMenuItemCopy_Click);
            // 
            // toolStripMenuItemCopyImgInStringEncoding
            // 
            this.toolStripMenuItemCopyImgInStringEncoding.Image = global::WiFiPasswordGenerator.Properties.Resources._32Documents_Library1;
            this.toolStripMenuItemCopyImgInStringEncoding.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripMenuItemCopyImgInStringEncoding.Name = "toolStripMenuItemCopyImgInStringEncoding";
            this.toolStripMenuItemCopyImgInStringEncoding.Size = new System.Drawing.Size(335, 38);
            this.toolStripMenuItemCopyImgInStringEncoding.Text = "Copy Png Base64 encoded Image to Clipboard";
            this.toolStripMenuItemCopyImgInStringEncoding.Click += new System.EventHandler(this.toolStripMenuItemCopyImgInStringEncoding_Click);
            // 
            // grpBoxQRCode
            // 
            this.grpBoxQRCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxQRCode.ContextMenuStrip = this.contextMenuQRImage;
            this.grpBoxQRCode.Controls.Add(this.PicBoxQRCode);
            this.grpBoxQRCode.Location = new System.Drawing.Point(7, 267);
            this.grpBoxQRCode.Name = "grpBoxQRCode";
            this.grpBoxQRCode.Padding = new System.Windows.Forms.Padding(5, 5, 3, 3);
            this.grpBoxQRCode.Size = new System.Drawing.Size(557, 196);
            this.grpBoxQRCode.TabIndex = 5;
            this.grpBoxQRCode.TabStop = false;
            this.grpBoxQRCode.Text = "QR Code";
            this.grpBoxQRCode.Resize += new System.EventHandler(this.grpBoxQRCode_Resize);
            // 
            // PicBoxQRCode
            // 
            this.PicBoxQRCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.PicBoxQRCode.BackColor = System.Drawing.SystemColors.Window;
            this.PicBoxQRCode.ContextMenuStrip = this.contextMenuQRImage;
            this.PicBoxQRCode.Location = new System.Drawing.Point(174, 10);
            this.PicBoxQRCode.Name = "PicBoxQRCode";
            this.PicBoxQRCode.Size = new System.Drawing.Size(182, 180);
            this.PicBoxQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBoxQRCode.TabIndex = 3;
            this.PicBoxQRCode.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.ContextMenuStrip = this.contextMenuClipboardIO;
            this.pnlMain.Controls.Add(this.txtGeneratedPassword);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.grpImgRes);
            this.pnlMain.Controls.Add(this.btnSaveQRCode);
            this.pnlMain.Controls.Add(this.groupBox1);
            this.pnlMain.Controls.Add(this.grpBoxQRCode);
            this.pnlMain.Controls.Add(this.btnGenerate);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(3, 3);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(4);
            this.pnlMain.Size = new System.Drawing.Size(698, 467);
            this.pnlMain.TabIndex = 6;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            // 
            // txtGeneratedPassword
            // 
            this.txtGeneratedPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGeneratedPassword.ContextMenuStrip = this.contextMenuGeneratedPassword;
            this.txtGeneratedPassword.Location = new System.Drawing.Point(153, 217);
            this.txtGeneratedPassword.Multiline = true;
            this.txtGeneratedPassword.Name = "txtGeneratedPassword";
            this.txtGeneratedPassword.ReadOnly = true;
            this.txtGeneratedPassword.Size = new System.Drawing.Size(539, 44);
            this.txtGeneratedPassword.TabIndex = 9;
            // 
            // contextMenuGeneratedPassword
            // 
            this.contextMenuGeneratedPassword.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuGeneratedPassword.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem,
            this.setTextFromClipboardToolStripMenuItem,
            this.toolStripSeparator2,
            this.generateQRCodeToolStripMenuItem1});
            this.contextMenuGeneratedPassword.Name = "contextMenuGeneratedPassword";
            this.contextMenuGeneratedPassword.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuGeneratedPassword.Size = new System.Drawing.Size(320, 124);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Image = global::WiFiPasswordGenerator.Properties.Resources._32text_x_nfo1;
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(319, 38);
            this.copyToClipboardToolStripMenuItem.Text = "Copy Generated Password To Clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // setTextFromClipboardToolStripMenuItem
            // 
            this.setTextFromClipboardToolStripMenuItem.Image = global::WiFiPasswordGenerator.Properties.Resources._32Paste1;
            this.setTextFromClipboardToolStripMenuItem.Name = "setTextFromClipboardToolStripMenuItem";
            this.setTextFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(319, 38);
            this.setTextFromClipboardToolStripMenuItem.Text = "Paste Text To The Generated Password Field";
            this.setTextFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.setTextFromClipboardToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Generated Password:";
            // 
            // grpImgRes
            // 
            this.grpImgRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpImgRes.Controls.Add(this.pnlUserDefinedRes);
            this.grpImgRes.Controls.Add(this.rbUserDefined);
            this.grpImgRes.Controls.Add(this.rbDefaultRes);
            this.grpImgRes.Location = new System.Drawing.Point(570, 267);
            this.grpImgRes.Name = "grpImgRes";
            this.grpImgRes.Size = new System.Drawing.Size(125, 129);
            this.grpImgRes.TabIndex = 7;
            this.grpImgRes.TabStop = false;
            this.grpImgRes.Text = "Image Resolution";
            // 
            // pnlUserDefinedRes
            // 
            this.pnlUserDefinedRes.Controls.Add(this.txtUserDefinedQRHeight);
            this.pnlUserDefinedRes.Controls.Add(this.txtUserDefinedQRWidth);
            this.pnlUserDefinedRes.Controls.Add(this.label2);
            this.pnlUserDefinedRes.Controls.Add(this.label1);
            this.pnlUserDefinedRes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUserDefinedRes.Enabled = false;
            this.pnlUserDefinedRes.Location = new System.Drawing.Point(3, 63);
            this.pnlUserDefinedRes.Name = "pnlUserDefinedRes";
            this.pnlUserDefinedRes.Size = new System.Drawing.Size(119, 63);
            this.pnlUserDefinedRes.TabIndex = 8;
            // 
            // txtUserDefinedQRHeight
            // 
            this.txtUserDefinedQRHeight.Location = new System.Drawing.Point(47, 33);
            this.txtUserDefinedQRHeight.MaxLength = 6;
            this.txtUserDefinedQRHeight.Name = "txtUserDefinedQRHeight";
            this.txtUserDefinedQRHeight.Size = new System.Drawing.Size(69, 20);
            this.txtUserDefinedQRHeight.TabIndex = 9;
            this.txtUserDefinedQRHeight.Text = "500";
            this.txtUserDefinedQRHeight.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserDefinedQRHeight_Validating);
            this.txtUserDefinedQRHeight.Validated += new System.EventHandler(this.txtUserDefinedQRHeight_Validated);
            // 
            // txtUserDefinedQRWidth
            // 
            this.txtUserDefinedQRWidth.Location = new System.Drawing.Point(47, 8);
            this.txtUserDefinedQRWidth.MaxLength = 6;
            this.txtUserDefinedQRWidth.Name = "txtUserDefinedQRWidth";
            this.txtUserDefinedQRWidth.Size = new System.Drawing.Size(69, 20);
            this.txtUserDefinedQRWidth.TabIndex = 8;
            this.txtUserDefinedQRWidth.Text = "500";
            this.txtUserDefinedQRWidth.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserDefinedQRWidth_Validating);
            this.txtUserDefinedQRWidth.Validated += new System.EventHandler(this.txtUserDefinedQRWidth_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Height:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Width:";
            // 
            // rbUserDefined
            // 
            this.rbUserDefined.AutoSize = true;
            this.rbUserDefined.Location = new System.Drawing.Point(6, 42);
            this.rbUserDefined.Name = "rbUserDefined";
            this.rbUserDefined.Size = new System.Drawing.Size(87, 17);
            this.rbUserDefined.TabIndex = 1;
            this.rbUserDefined.Text = "User Defined";
            this.rbUserDefined.UseVisualStyleBackColor = true;
            this.rbUserDefined.CheckedChanged += new System.EventHandler(this.rbDefaultRes_CheckedChanged);
            // 
            // rbDefaultRes
            // 
            this.rbDefaultRes.AutoSize = true;
            this.rbDefaultRes.Checked = true;
            this.rbDefaultRes.Location = new System.Drawing.Point(6, 19);
            this.rbDefaultRes.Name = "rbDefaultRes";
            this.rbDefaultRes.Size = new System.Drawing.Size(59, 17);
            this.rbDefaultRes.TabIndex = 0;
            this.rbDefaultRes.TabStop = true;
            this.rbDefaultRes.Text = "Default";
            this.rbDefaultRes.UseVisualStyleBackColor = true;
            this.rbDefaultRes.CheckedChanged += new System.EventHandler(this.rbDefaultRes_CheckedChanged);
            // 
            // btnSaveQRCode
            // 
            this.btnSaveQRCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveQRCode.Location = new System.Drawing.Point(570, 402);
            this.btnSaveQRCode.Name = "btnSaveQRCode";
            this.btnSaveQRCode.Size = new System.Drawing.Size(120, 26);
            this.btnSaveQRCode.TabIndex = 6;
            this.btnSaveQRCode.Text = "Save QR Code to file";
            this.btnSaveQRCode.UseVisualStyleBackColor = true;
            this.btnSaveQRCode.Click += new System.EventHandler(this.btnSaveQRCode_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "png";
            this.saveFileDialog1.FileName = "Password_QRCode";
            this.saveFileDialog1.Filter = "png files|*.png|jpeg|*.jpg";
            // 
            // contextMenuClipboardIO
            // 
            this.contextMenuClipboardIO.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuClipboardIO.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCBImport,
            this.ToolStripMenuItemCBExport,
            this.toolStripSeparator1,
            this.generateQRCodeToolStripMenuItem});
            this.contextMenuClipboardIO.Name = "contextMenuClipboardIO";
            this.contextMenuClipboardIO.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuClipboardIO.Size = new System.Drawing.Size(188, 146);
            // 
            // ToolStripMenuItemCBImport
            // 
            this.ToolStripMenuItemCBImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemImportPassword,
            this.toolStripMenuItemImportBase64ImgData});
            this.ToolStripMenuItemCBImport.Image = global::WiFiPasswordGenerator.Properties.Resources._32edit_copy;
            this.ToolStripMenuItemCBImport.Name = "ToolStripMenuItemCBImport";
            this.ToolStripMenuItemCBImport.Size = new System.Drawing.Size(187, 38);
            this.ToolStripMenuItemCBImport.Text = "Clipboard Import";
            // 
            // toolStripMenuItemImportPassword
            // 
            this.toolStripMenuItemImportPassword.Image = global::WiFiPasswordGenerator.Properties.Resources._32text_x_nfo1;
            this.toolStripMenuItemImportPassword.Name = "toolStripMenuItemImportPassword";
            this.toolStripMenuItemImportPassword.Size = new System.Drawing.Size(220, 38);
            this.toolStripMenuItemImportPassword.Text = "Text Content";
            this.toolStripMenuItemImportPassword.Click += new System.EventHandler(this.toolStripMenuItemImportPassword_Click);
            // 
            // toolStripMenuItemImportBase64ImgData
            // 
            this.toolStripMenuItemImportBase64ImgData.Image = global::WiFiPasswordGenerator.Properties.Resources._32Documents_Library1;
            this.toolStripMenuItemImportBase64ImgData.Name = "toolStripMenuItemImportBase64ImgData";
            this.toolStripMenuItemImportBase64ImgData.Size = new System.Drawing.Size(220, 38);
            this.toolStripMenuItemImportBase64ImgData.Text = "QR Image In Text Format";
            this.toolStripMenuItemImportBase64ImgData.Click += new System.EventHandler(this.toolStripMenuItemImportBase64ImgData_Click);
            // 
            // ToolStripMenuItemCBExport
            // 
            this.ToolStripMenuItemCBExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExportQRImage,
            this.toolStripMenuItemExportPwdStr});
            this.ToolStripMenuItemCBExport.Image = global::WiFiPasswordGenerator.Properties.Resources._32edit_paste;
            this.ToolStripMenuItemCBExport.Name = "ToolStripMenuItemCBExport";
            this.ToolStripMenuItemCBExport.Size = new System.Drawing.Size(187, 38);
            this.ToolStripMenuItemCBExport.Text = "Clipboard Export";
            // 
            // toolStripMenuItemExportQRImage
            // 
            this.toolStripMenuItemExportQRImage.Image = global::WiFiPasswordGenerator.Properties.Resources.qr_code_32x32;
            this.toolStripMenuItemExportQRImage.Name = "toolStripMenuItemExportQRImage";
            this.toolStripMenuItemExportQRImage.Size = new System.Drawing.Size(197, 38);
            this.toolStripMenuItemExportQRImage.Text = "QR Code Image";
            this.toolStripMenuItemExportQRImage.ToolTipText = "QR Code Image is encoded in PNG format and the output text\r\nis a Base64 converted" +
    " byte array.\r\n";
            this.toolStripMenuItemExportQRImage.Click += new System.EventHandler(this.toolStripMenuItemExportQRImage_Click);
            // 
            // toolStripMenuItemExportPwdStr
            // 
            this.toolStripMenuItemExportPwdStr.Image = global::WiFiPasswordGenerator.Properties.Resources._32text_x_nfo1;
            this.toolStripMenuItemExportPwdStr.Name = "toolStripMenuItemExportPwdStr";
            this.toolStripMenuItemExportPwdStr.Size = new System.Drawing.Size(197, 38);
            this.toolStripMenuItemExportPwdStr.Text = "Generated Password";
            this.toolStripMenuItemExportPwdStr.Click += new System.EventHandler(this.toolStripMenuItemExportPwdStr_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // generateQRCodeToolStripMenuItem
            // 
            this.generateQRCodeToolStripMenuItem.Image = global::WiFiPasswordGenerator.Properties.Resources._32Chrisbanks2_Cold_Fusion_Hd_QR_scanner;
            this.generateQRCodeToolStripMenuItem.Name = "generateQRCodeToolStripMenuItem";
            this.generateQRCodeToolStripMenuItem.Size = new System.Drawing.Size(187, 38);
            this.generateQRCodeToolStripMenuItem.Text = "Generate QR Code";
            this.generateQRCodeToolStripMenuItem.Click += new System.EventHandler(this.generateQRCodeToolStripMenuItem_Click);
            // 
            // toolTipPasswordLength
            // 
            this.toolTipPasswordLength.Active = false;
            this.toolTipPasswordLength.IsBalloon = true;
            this.toolTipPasswordLength.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTipPasswordLength.ToolTipTitle = "Invalid password length";
            // 
            // generateQRCodeToolStripMenuItem1
            // 
            this.generateQRCodeToolStripMenuItem1.Image = global::WiFiPasswordGenerator.Properties.Resources._32Chrisbanks2_Cold_Fusion_Hd_QR_scanner;
            this.generateQRCodeToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.generateQRCodeToolStripMenuItem1.Name = "generateQRCodeToolStripMenuItem1";
            this.generateQRCodeToolStripMenuItem1.Size = new System.Drawing.Size(319, 38);
            this.generateQRCodeToolStripMenuItem1.Text = "GenerateQR Code";
            this.generateQRCodeToolStripMenuItem1.Click += new System.EventHandler(this.generateQRCodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(316, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 473);
            this.Controls.Add(this.pnlMain);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(6, 6);
            this.MinimumSize = new System.Drawing.Size(720, 512);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wifi Password generator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.flowLayoutOutputType.ResumeLayout(false);
            this.flowLayoutOutputType.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.flowLayoutQRSettings.ResumeLayout(false);
            this.flowLayoutQRSettings.PerformLayout();
            this.contextMenuQRImage.ResumeLayout(false);
            this.grpBoxQRCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxQRCode)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.contextMenuGeneratedPassword.ResumeLayout(false);
            this.grpImgRes.ResumeLayout(false);
            this.grpImgRes.PerformLayout();
            this.pnlUserDefinedRes.ResumeLayout(false);
            this.pnlUserDefinedRes.PerformLayout();
            this.contextMenuClipboardIO.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutQRSettings;
        private System.Windows.Forms.RadioButton rbLevel0;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.PictureBox PicBoxQRCode;
        private System.Windows.Forms.RadioButton rbLevel1;
        private System.Windows.Forms.RadioButton rbLevel2;
        private System.Windows.Forms.RadioButton rbLevel3;
        private System.Windows.Forms.TextBox txtPasswordLength;
        private System.Windows.Forms.Label lblPasswordLength;
        private System.Windows.Forms.TextBox txtECCDescription;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutOutputType;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtPasswordTypesDescr;
        private System.Windows.Forms.RadioButton rbDefault;
        private System.Windows.Forms.RadioButton rbAlphaNumberic;
        private System.Windows.Forms.RadioButton rbNumeric;
        private System.Windows.Forms.RadioButton rbBase64;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.GroupBox grpBoxQRCode;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnSaveQRCode;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox grpImgRes;
        private System.Windows.Forms.Panel pnlUserDefinedRes;
        private System.Windows.Forms.TextBox txtUserDefinedQRHeight;
        private System.Windows.Forms.TextBox txtUserDefinedQRWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbUserDefined;
        private System.Windows.Forms.RadioButton rbDefaultRes;
        private System.Windows.Forms.LinkLabel linkLabelLastQRPath;
        private System.Windows.Forms.ContextMenuStrip contextMenuQRImage;
        private System.Windows.Forms.ToolStripMenuItem contextMenuItemCopy;
        private System.Windows.Forms.TextBox txtGeneratedPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopyImgInStringEncoding;
        private System.Windows.Forms.ContextMenuStrip contextMenuGeneratedPassword;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuClipboardIO;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCBImport;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImportPassword;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemImportBase64ImgData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCBExport;
        private System.Windows.Forms.ToolStripMenuItem generateQRCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTextFromClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExportQRImage;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExportPwdStr;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolTip toolTipPasswordLength;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem generateQRCodeToolStripMenuItem1;
    }
}

