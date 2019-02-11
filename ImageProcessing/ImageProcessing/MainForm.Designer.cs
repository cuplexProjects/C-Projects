namespace ImageProcessing
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbExtend = new System.Windows.Forms.RadioButton();
            this.rbWrap = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.lblInputImageWidth = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInputImageHeight = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.txtInputImagePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControlFilterSettings = new System.Windows.Forms.TabControl();
            this.tabPageGammaCorrection = new System.Windows.Forms.TabPage();
            this.sliderGamma = new System.Windows.Forms.TrackBar();
            this.txtGammaCorrection = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageBoxBlur = new System.Windows.Forms.TabPage();
            this.sliderSmooth = new System.Windows.Forms.TrackBar();
            this.txtSmoothAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPageSharpen = new System.Windows.Forms.TabPage();
            this.sliderSharpen = new System.Windows.Forms.TrackBar();
            this.txtSharpenAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageGausianBlur = new System.Windows.Forms.TabPage();
            this.txtGaussianBlurPixels = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.sliderGaussianBlur = new System.Windows.Forms.TrackBar();
            this.label12 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbGamma = new System.Windows.Forms.RadioButton();
            this.rbSmooth = new System.Windows.Forms.RadioButton();
            this.rbSharpen = new System.Windows.Forms.RadioButton();
            this.rbEdgeDetection = new System.Windows.Forms.RadioButton();
            this.rbGaussianBlur = new System.Windows.Forms.RadioButton();
            this.rbEmboss = new System.Windows.Forms.RadioButton();
            this.rbMosaic = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBoxOutput = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnApplyFilter = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnSaveImageToFile = new System.Windows.Forms.Button();
            this.progressBarImageProcess = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControlFilterSettings.SuspendLayout();
            this.tabPageGammaCorrection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderGamma)).BeginInit();
            this.tabPageBoxBlur.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSmooth)).BeginInit();
            this.tabPageSharpen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSharpen)).BeginInit();
            this.tabPageGausianBlur.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderGaussianBlur)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutput)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.btnSelectImage);
            this.groupBox1.Controls.Add(this.txtInputImagePath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 154);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Image Selection";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.flowLayoutPanel3);
            this.groupBox6.Location = new System.Drawing.Point(10, 99);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(248, 46);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Edge Handling";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.rbExtend);
            this.flowLayoutPanel3.Controls.Add(this.rbWrap);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(242, 27);
            this.flowLayoutPanel3.TabIndex = 6;
            // 
            // rbExtend
            // 
            this.rbExtend.AutoSize = true;
            this.rbExtend.Checked = true;
            this.rbExtend.Location = new System.Drawing.Point(3, 3);
            this.rbExtend.Name = "rbExtend";
            this.rbExtend.Size = new System.Drawing.Size(58, 17);
            this.rbExtend.TabIndex = 4;
            this.rbExtend.TabStop = true;
            this.rbExtend.Text = "Extend";
            this.rbExtend.UseVisualStyleBackColor = true;
            // 
            // rbWrap
            // 
            this.rbWrap.AutoSize = true;
            this.rbWrap.Location = new System.Drawing.Point(67, 3);
            this.rbWrap.Name = "rbWrap";
            this.rbWrap.Size = new System.Drawing.Size(51, 17);
            this.rbWrap.TabIndex = 5;
            this.rbWrap.Text = "Wrap";
            this.rbWrap.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.flowLayoutPanel2);
            this.groupBox5.Location = new System.Drawing.Point(6, 51);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(251, 45);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Image Size";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label9);
            this.flowLayoutPanel2.Controls.Add(this.lblInputImageWidth);
            this.flowLayoutPanel2.Controls.Add(this.label4);
            this.flowLayoutPanel2.Controls.Add(this.lblInputImageHeight);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(245, 26);
            this.flowLayoutPanel2.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Width:";
            // 
            // lblInputImageWidth
            // 
            this.lblInputImageWidth.AutoSize = true;
            this.lblInputImageWidth.Location = new System.Drawing.Point(47, 0);
            this.lblInputImageWidth.Name = "lblInputImageWidth";
            this.lblInputImageWidth.Size = new System.Drawing.Size(39, 13);
            this.lblInputImageWidth.TabIndex = 4;
            this.lblInputImageWidth.Text = "100 px";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(92, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Height:";
            // 
            // lblInputImageHeight
            // 
            this.lblInputImageHeight.AutoSize = true;
            this.lblInputImageHeight.Location = new System.Drawing.Point(139, 0);
            this.lblInputImageHeight.Name = "lblInputImageHeight";
            this.lblInputImageHeight.Size = new System.Drawing.Size(39, 13);
            this.lblInputImageHeight.TabIndex = 2;
            this.lblInputImageHeight.Text = "100 px";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(290, 120);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(85, 25);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSelectImage
            // 
            this.btnSelectImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectImage.Location = new System.Drawing.Point(381, 120);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(85, 25);
            this.btnSelectImage.TabIndex = 0;
            this.btnSelectImage.Text = "Select Image";
            this.btnSelectImage.UseVisualStyleBackColor = true;
            this.btnSelectImage.Click += new System.EventHandler(this.btnSelectImage_Click);
            // 
            // txtInputImagePath
            // 
            this.txtInputImagePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputImagePath.Location = new System.Drawing.Point(78, 25);
            this.txtInputImagePath.Name = "txtInputImagePath";
            this.txtInputImagePath.ReadOnly = true;
            this.txtInputImagePath.Size = new System.Drawing.Size(388, 20);
            this.txtInputImagePath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input Image:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tabControlFilterSettings);
            this.groupBox2.Controls.Add(this.flowLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(6, 172);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 196);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image Filters";
            // 
            // tabControlFilterSettings
            // 
            this.tabControlFilterSettings.Controls.Add(this.tabPageGammaCorrection);
            this.tabControlFilterSettings.Controls.Add(this.tabPageBoxBlur);
            this.tabControlFilterSettings.Controls.Add(this.tabPageSharpen);
            this.tabControlFilterSettings.Controls.Add(this.tabPageGausianBlur);
            this.tabControlFilterSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControlFilterSettings.Location = new System.Drawing.Point(3, 78);
            this.tabControlFilterSettings.Name = "tabControlFilterSettings";
            this.tabControlFilterSettings.SelectedIndex = 0;
            this.tabControlFilterSettings.Size = new System.Drawing.Size(469, 115);
            this.tabControlFilterSettings.TabIndex = 1;
            // 
            // tabPageGammaCorrection
            // 
            this.tabPageGammaCorrection.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageGammaCorrection.Controls.Add(this.sliderGamma);
            this.tabPageGammaCorrection.Controls.Add(this.txtGammaCorrection);
            this.tabPageGammaCorrection.Controls.Add(this.label5);
            this.tabPageGammaCorrection.Location = new System.Drawing.Point(4, 22);
            this.tabPageGammaCorrection.Name = "tabPageGammaCorrection";
            this.tabPageGammaCorrection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGammaCorrection.Size = new System.Drawing.Size(461, 89);
            this.tabPageGammaCorrection.TabIndex = 0;
            this.tabPageGammaCorrection.Text = "Gamma Correction";
            // 
            // sliderGamma
            // 
            this.sliderGamma.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sliderGamma.LargeChange = 10;
            this.sliderGamma.Location = new System.Drawing.Point(3, 41);
            this.sliderGamma.Maximum = 100;
            this.sliderGamma.Minimum = 10;
            this.sliderGamma.Name = "sliderGamma";
            this.sliderGamma.Size = new System.Drawing.Size(455, 45);
            this.sliderGamma.TabIndex = 4;
            this.sliderGamma.Value = 50;
            this.sliderGamma.Scroll += new System.EventHandler(this.sliderGamma_Scroll);
            // 
            // txtGammaCorrection
            // 
            this.txtGammaCorrection.Location = new System.Drawing.Point(58, 10);
            this.txtGammaCorrection.Name = "txtGammaCorrection";
            this.txtGammaCorrection.ReadOnly = true;
            this.txtGammaCorrection.Size = new System.Drawing.Size(45, 20);
            this.txtGammaCorrection.TabIndex = 3;
            this.txtGammaCorrection.Text = "1,0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Amount:";
            // 
            // tabPageBoxBlur
            // 
            this.tabPageBoxBlur.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageBoxBlur.Controls.Add(this.sliderSmooth);
            this.tabPageBoxBlur.Controls.Add(this.txtSmoothAmount);
            this.tabPageBoxBlur.Controls.Add(this.label3);
            this.tabPageBoxBlur.Location = new System.Drawing.Point(4, 22);
            this.tabPageBoxBlur.Name = "tabPageBoxBlur";
            this.tabPageBoxBlur.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBoxBlur.Size = new System.Drawing.Size(461, 89);
            this.tabPageBoxBlur.TabIndex = 4;
            this.tabPageBoxBlur.Text = "BoxBlur";
            // 
            // sliderSmooth
            // 
            this.sliderSmooth.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sliderSmooth.LargeChange = 20;
            this.sliderSmooth.Location = new System.Drawing.Point(3, 41);
            this.sliderSmooth.Maximum = 100;
            this.sliderSmooth.Name = "sliderSmooth";
            this.sliderSmooth.Size = new System.Drawing.Size(455, 45);
            this.sliderSmooth.SmallChange = 10;
            this.sliderSmooth.TabIndex = 10;
            this.sliderSmooth.Value = 10;
            this.sliderSmooth.Scroll += new System.EventHandler(this.sliderSmooth_Scroll);
            // 
            // txtSmoothAmount
            // 
            this.txtSmoothAmount.Location = new System.Drawing.Point(58, 10);
            this.txtSmoothAmount.Name = "txtSmoothAmount";
            this.txtSmoothAmount.ReadOnly = true;
            this.txtSmoothAmount.Size = new System.Drawing.Size(45, 20);
            this.txtSmoothAmount.TabIndex = 9;
            this.txtSmoothAmount.Text = "1,0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Amount:";
            // 
            // tabPageSharpen
            // 
            this.tabPageSharpen.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageSharpen.Controls.Add(this.sliderSharpen);
            this.tabPageSharpen.Controls.Add(this.txtSharpenAmount);
            this.tabPageSharpen.Controls.Add(this.label2);
            this.tabPageSharpen.Location = new System.Drawing.Point(4, 22);
            this.tabPageSharpen.Name = "tabPageSharpen";
            this.tabPageSharpen.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSharpen.Size = new System.Drawing.Size(461, 89);
            this.tabPageSharpen.TabIndex = 2;
            this.tabPageSharpen.Text = "Sharpen";
            // 
            // sliderSharpen
            // 
            this.sliderSharpen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sliderSharpen.LargeChange = 20;
            this.sliderSharpen.Location = new System.Drawing.Point(3, 41);
            this.sliderSharpen.Maximum = 100;
            this.sliderSharpen.Name = "sliderSharpen";
            this.sliderSharpen.Size = new System.Drawing.Size(455, 45);
            this.sliderSharpen.SmallChange = 10;
            this.sliderSharpen.TabIndex = 7;
            this.sliderSharpen.Value = 10;
            this.sliderSharpen.Scroll += new System.EventHandler(this.sliderSharpen_Scroll);
            // 
            // txtSharpenAmount
            // 
            this.txtSharpenAmount.Location = new System.Drawing.Point(58, 10);
            this.txtSharpenAmount.Name = "txtSharpenAmount";
            this.txtSharpenAmount.ReadOnly = true;
            this.txtSharpenAmount.Size = new System.Drawing.Size(45, 20);
            this.txtSharpenAmount.TabIndex = 6;
            this.txtSharpenAmount.Text = "1,0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Amount:";
            // 
            // tabPageGausianBlur
            // 
            this.tabPageGausianBlur.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPageGausianBlur.Controls.Add(this.txtGaussianBlurPixels);
            this.tabPageGausianBlur.Controls.Add(this.label13);
            this.tabPageGausianBlur.Controls.Add(this.sliderGaussianBlur);
            this.tabPageGausianBlur.Controls.Add(this.label12);
            this.tabPageGausianBlur.Location = new System.Drawing.Point(4, 22);
            this.tabPageGausianBlur.Name = "tabPageGausianBlur";
            this.tabPageGausianBlur.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGausianBlur.Size = new System.Drawing.Size(461, 89);
            this.tabPageGausianBlur.TabIndex = 3;
            this.tabPageGausianBlur.Text = " Gaussian Blur";
            // 
            // txtGaussianBlurPixels
            // 
            this.txtGaussianBlurPixels.Location = new System.Drawing.Point(82, 10);
            this.txtGaussianBlurPixels.Name = "txtGaussianBlurPixels";
            this.txtGaussianBlurPixels.ReadOnly = true;
            this.txtGaussianBlurPixels.Size = new System.Drawing.Size(45, 20);
            this.txtGaussianBlurPixels.TabIndex = 10;
            this.txtGaussianBlurPixels.Text = "1,0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(133, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Pixels";
            // 
            // sliderGaussianBlur
            // 
            this.sliderGaussianBlur.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sliderGaussianBlur.LargeChange = 20;
            this.sliderGaussianBlur.Location = new System.Drawing.Point(3, 41);
            this.sliderGaussianBlur.Maximum = 100;
            this.sliderGaussianBlur.Minimum = 1;
            this.sliderGaussianBlur.Name = "sliderGaussianBlur";
            this.sliderGaussianBlur.Size = new System.Drawing.Size(455, 45);
            this.sliderGaussianBlur.SmallChange = 10;
            this.sliderGaussianBlur.TabIndex = 2;
            this.sliderGaussianBlur.Value = 10;
            this.sliderGaussianBlur.Scroll += new System.EventHandler(this.sliderGaussianBlur_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Gaussian Blur:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.rbGamma);
            this.flowLayoutPanel1.Controls.Add(this.rbSmooth);
            this.flowLayoutPanel1.Controls.Add(this.rbSharpen);
            this.flowLayoutPanel1.Controls.Add(this.rbEdgeDetection);
            this.flowLayoutPanel1.Controls.Add(this.rbGaussianBlur);
            this.flowLayoutPanel1.Controls.Add(this.rbEmboss);
            this.flowLayoutPanel1.Controls.Add(this.rbMosaic);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(469, 177);
            this.flowLayoutPanel1.TabIndex = 0;
            this.flowLayoutPanel1.TabStop = true;
            // 
            // rbGamma
            // 
            this.rbGamma.AutoSize = true;
            this.rbGamma.Checked = true;
            this.rbGamma.Location = new System.Drawing.Point(3, 3);
            this.rbGamma.Name = "rbGamma";
            this.rbGamma.Size = new System.Drawing.Size(61, 17);
            this.rbGamma.TabIndex = 0;
            this.rbGamma.TabStop = true;
            this.rbGamma.Tag = "1";
            this.rbGamma.Text = "Gamma";
            this.rbGamma.UseVisualStyleBackColor = true;
            this.rbGamma.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // rbSmooth
            // 
            this.rbSmooth.AutoSize = true;
            this.rbSmooth.Location = new System.Drawing.Point(70, 3);
            this.rbSmooth.Name = "rbSmooth";
            this.rbSmooth.Size = new System.Drawing.Size(43, 17);
            this.rbSmooth.TabIndex = 1;
            this.rbSmooth.Tag = "2";
            this.rbSmooth.Text = "Blur";
            this.rbSmooth.UseVisualStyleBackColor = true;
            this.rbSmooth.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // rbSharpen
            // 
            this.rbSharpen.AutoSize = true;
            this.rbSharpen.Location = new System.Drawing.Point(119, 3);
            this.rbSharpen.Name = "rbSharpen";
            this.rbSharpen.Size = new System.Drawing.Size(65, 17);
            this.rbSharpen.TabIndex = 2;
            this.rbSharpen.Tag = "3";
            this.rbSharpen.Text = "Sharpen";
            this.rbSharpen.UseVisualStyleBackColor = true;
            this.rbSharpen.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // rbEdgeDetection
            // 
            this.rbEdgeDetection.AutoSize = true;
            this.rbEdgeDetection.Location = new System.Drawing.Point(190, 3);
            this.rbEdgeDetection.Name = "rbEdgeDetection";
            this.rbEdgeDetection.Size = new System.Drawing.Size(99, 17);
            this.rbEdgeDetection.TabIndex = 3;
            this.rbEdgeDetection.TabStop = true;
            this.rbEdgeDetection.Tag = "4";
            this.rbEdgeDetection.Text = "Edge Detection";
            this.rbEdgeDetection.UseVisualStyleBackColor = true;
            this.rbEdgeDetection.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // rbGaussianBlur
            // 
            this.rbGaussianBlur.AutoSize = true;
            this.rbGaussianBlur.Location = new System.Drawing.Point(295, 3);
            this.rbGaussianBlur.Name = "rbGaussianBlur";
            this.rbGaussianBlur.Size = new System.Drawing.Size(90, 17);
            this.rbGaussianBlur.TabIndex = 4;
            this.rbGaussianBlur.Tag = "5";
            this.rbGaussianBlur.Text = "Gaussian Blur";
            this.rbGaussianBlur.UseVisualStyleBackColor = true;
            this.rbGaussianBlur.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // rbEmboss
            // 
            this.rbEmboss.AutoSize = true;
            this.rbEmboss.Location = new System.Drawing.Point(391, 3);
            this.rbEmboss.Name = "rbEmboss";
            this.rbEmboss.Size = new System.Drawing.Size(62, 17);
            this.rbEmboss.TabIndex = 5;
            this.rbEmboss.Tag = "6";
            this.rbEmboss.Text = "Emboss";
            this.rbEmboss.UseVisualStyleBackColor = true;
            this.rbEmboss.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // rbMosaic
            // 
            this.rbMosaic.AutoSize = true;
            this.rbMosaic.Location = new System.Drawing.Point(3, 26);
            this.rbMosaic.Name = "rbMosaic";
            this.rbMosaic.Size = new System.Drawing.Size(62, 17);
            this.rbMosaic.TabIndex = 6;
            this.rbMosaic.TabStop = true;
            this.rbMosaic.Tag = "7";
            this.rbMosaic.Text = " Mosaic";
            this.rbMosaic.UseVisualStyleBackColor = true;
            this.rbMosaic.CheckedChanged += new System.EventHandler(this.filterRadioButton_CheckedChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBoxOutput
            // 
            this.pictureBoxOutput.BackColor = System.Drawing.Color.White;
            this.pictureBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOutput.Location = new System.Drawing.Point(3, 16);
            this.pictureBoxOutput.Name = "pictureBoxOutput";
            this.pictureBoxOutput.Size = new System.Drawing.Size(469, 157);
            this.pictureBoxOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxOutput.TabIndex = 2;
            this.pictureBoxOutput.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.pictureBoxOutput);
            this.groupBox3.Location = new System.Drawing.Point(6, 374);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(475, 176);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output Image";
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyFilter.Location = new System.Drawing.Point(355, 52);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(125, 30);
            this.btnApplyFilter.TabIndex = 0;
            this.btnApplyFilter.Text = "Apply Filter";
            this.btnApplyFilter.UseVisualStyleBackColor = true;
            this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click);
            // 
            // btnSaveImageToFile
            // 
            this.btnSaveImageToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveImageToFile.Location = new System.Drawing.Point(224, 52);
            this.btnSaveImageToFile.Name = "btnSaveImageToFile";
            this.btnSaveImageToFile.Size = new System.Drawing.Size(125, 30);
            this.btnSaveImageToFile.TabIndex = 1;
            this.btnSaveImageToFile.Text = "Save Image";
            this.btnSaveImageToFile.UseVisualStyleBackColor = true;
            this.btnSaveImageToFile.Click += new System.EventHandler(this.btnSaveImageToFile_Click);
            // 
            // progressBarImageProcess
            // 
            this.progressBarImageProcess.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBarImageProcess.Location = new System.Drawing.Point(5, 18);
            this.progressBarImageProcess.Name = "progressBarImageProcess";
            this.progressBarImageProcess.Size = new System.Drawing.Size(474, 25);
            this.progressBarImageProcess.TabIndex = 6;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.progressBarImageProcess);
            this.groupBox4.Controls.Add(this.btnApplyFilter);
            this.groupBox4.Controls.Add(this.btnSaveImageToFile);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.Location = new System.Drawing.Point(0, 556);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox4.Size = new System.Drawing.Size(484, 90);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Process Image";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 646);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(500, 685);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Processing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControlFilterSettings.ResumeLayout(false);
            this.tabPageGammaCorrection.ResumeLayout(false);
            this.tabPageGammaCorrection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderGamma)).EndInit();
            this.tabPageBoxBlur.ResumeLayout(false);
            this.tabPageBoxBlur.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSmooth)).EndInit();
            this.tabPageSharpen.ResumeLayout(false);
            this.tabPageSharpen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSharpen)).EndInit();
            this.tabPageGausianBlur.ResumeLayout(false);
            this.tabPageGausianBlur.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderGaussianBlur)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOutput)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSelectImage;
        private System.Windows.Forms.TextBox txtInputImagePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rbGaussianBlur;
        private System.Windows.Forms.RadioButton rbSmooth;
        private System.Windows.Forms.PictureBox pictureBoxOutput;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnApplyFilter;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnSaveImageToFile;
        private System.Windows.Forms.ProgressBar progressBarImageProcess;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblInputImageWidth;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblInputImageHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbSharpen;
        private System.Windows.Forms.RadioButton rbGamma;
        private System.Windows.Forms.TabControl tabControlFilterSettings;
        private System.Windows.Forms.TabPage tabPageGammaCorrection;
        private System.Windows.Forms.TrackBar sliderGamma;
        private System.Windows.Forms.TextBox txtGammaCorrection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPageBoxBlur;
        private System.Windows.Forms.TrackBar sliderSmooth;
        private System.Windows.Forms.TextBox txtSmoothAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPageSharpen;
        private System.Windows.Forms.TrackBar sliderSharpen;
        private System.Windows.Forms.TextBox txtSharpenAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPageGausianBlur;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TrackBar sliderGaussianBlur;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton rbEmboss;
        private System.Windows.Forms.RadioButton rbMosaic;
        private System.Windows.Forms.RadioButton rbEdgeDetection;
        private System.Windows.Forms.TextBox txtGaussianBlurPixels;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.RadioButton rbExtend;
        private System.Windows.Forms.RadioButton rbWrap;
    }
}

