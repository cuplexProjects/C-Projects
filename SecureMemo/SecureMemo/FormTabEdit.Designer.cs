namespace SecureMemo
{
    partial class FormTabEdit
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("", 0);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTabEdit));
            this.grpBoxTabs = new System.Windows.Forms.GroupBox();
            this.listViewTabs = new System.Windows.Forms.ListView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpDragAndDropOptions = new System.Windows.Forms.GroupBox();
            this.radioButtonShift = new System.Windows.Forms.RadioButton();
            this.radioButtonSwitch = new System.Windows.Forms.RadioButton();
            this.grpBoxTabs.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpDragAndDropOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxTabs
            // 
            this.grpBoxTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxTabs.Controls.Add(this.listViewTabs);
            this.grpBoxTabs.Location = new System.Drawing.Point(6, 2);
            this.grpBoxTabs.Name = "grpBoxTabs";
            this.grpBoxTabs.Size = new System.Drawing.Size(372, 319);
            this.grpBoxTabs.TabIndex = 0;
            this.grpBoxTabs.TabStop = false;
            this.grpBoxTabs.Text = "Tabs";
            // 
            // listViewTabs
            // 
            this.listViewTabs.AllowDrop = true;
            this.listViewTabs.AutoArrange = false;
            this.listViewTabs.ContextMenuStrip = this.contextMenu;
            this.listViewTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTabs.GridLines = true;
            this.listViewTabs.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewTabs.LabelEdit = true;
            this.listViewTabs.LargeImageList = this.imageList1;
            this.listViewTabs.Location = new System.Drawing.Point(3, 16);
            this.listViewTabs.MultiSelect = false;
            this.listViewTabs.Name = "listViewTabs";
            this.listViewTabs.ShowGroups = false;
            this.listViewTabs.Size = new System.Drawing.Size(366, 300);
            this.listViewTabs.SmallImageList = this.imageList1;
            this.listViewTabs.TabIndex = 0;
            this.listViewTabs.UseCompatibleStateImageBehavior = false;
            this.listViewTabs.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewTabs_AfterLabelEdit);
            this.listViewTabs.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewTabs_ItemDrag);
            this.listViewTabs.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewTabs_DragDrop);
            this.listViewTabs.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewTabs_DragEnter);
            this.listViewTabs.DragOver += new System.Windows.Forms.DragEventHandler(this.listViewTabs_DragOver);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewTabToolStripMenuItem,
            this.deleteSelectedTabToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(178, 48);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // addNewTabToolStripMenuItem
            // 
            this.addNewTabToolStripMenuItem.Name = "addNewTabToolStripMenuItem";
            this.addNewTabToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.addNewTabToolStripMenuItem.Text = "Add New Tab";
            this.addNewTabToolStripMenuItem.Click += new System.EventHandler(this.addNewTabToolStripMenuItem_Click);
            // 
            // deleteSelectedTabToolStripMenuItem
            // 
            this.deleteSelectedTabToolStripMenuItem.Name = "deleteSelectedTabToolStripMenuItem";
            this.deleteSelectedTabToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.deleteSelectedTabToolStripMenuItem.Text = "Delete Selected Tab";
            this.deleteSelectedTabToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedTabToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.ico");
            this.imageList1.Images.SetKeyName(1, "Folder.ico");
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(216, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 27);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(297, 7);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpDragAndDropOptions);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 326);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 40);
            this.panel1.TabIndex = 3;
            // 
            // grpDragAndDropOptions
            // 
            this.grpDragAndDropOptions.Controls.Add(this.radioButtonShift);
            this.grpDragAndDropOptions.Controls.Add(this.radioButtonSwitch);
            this.grpDragAndDropOptions.Location = new System.Drawing.Point(0, 0);
            this.grpDragAndDropOptions.Name = "grpDragAndDropOptions";
            this.grpDragAndDropOptions.Size = new System.Drawing.Size(140, 38);
            this.grpDragAndDropOptions.TabIndex = 5;
            this.grpDragAndDropOptions.TabStop = false;
            this.grpDragAndDropOptions.Text = "Drag And Drop Options";
            // 
            // radioButtonShift
            // 
            this.radioButtonShift.AutoSize = true;
            this.radioButtonShift.Location = new System.Drawing.Point(75, 16);
            this.radioButtonShift.Name = "radioButtonShift";
            this.radioButtonShift.Size = new System.Drawing.Size(46, 17);
            this.radioButtonShift.TabIndex = 4;
            this.radioButtonShift.Text = "Shift";
            this.radioButtonShift.UseVisualStyleBackColor = true;
            // 
            // radioButtonSwitch
            // 
            this.radioButtonSwitch.AutoSize = true;
            this.radioButtonSwitch.Checked = true;
            this.radioButtonSwitch.Location = new System.Drawing.Point(12, 16);
            this.radioButtonSwitch.Name = "radioButtonSwitch";
            this.radioButtonSwitch.Size = new System.Drawing.Size(57, 17);
            this.radioButtonSwitch.TabIndex = 3;
            this.radioButtonSwitch.TabStop = true;
            this.radioButtonSwitch.Text = "Switch";
            this.radioButtonSwitch.UseVisualStyleBackColor = true;
            // 
            // FormTabEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 366);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpBoxTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 300);
            this.Name = "FormTabEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Available tabs";
            this.Load += new System.EventHandler(this.frmTabEdit_Load);
            this.ResizeBegin += new System.EventHandler(this.FormTabEdit_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.FormTabEdit_ResizeEnd);
            this.grpBoxTabs.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grpDragAndDropOptions.ResumeLayout(false);
            this.grpDragAndDropOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxTabs;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView listViewTabs;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem addNewTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedTabToolStripMenuItem;
        private System.Windows.Forms.GroupBox grpDragAndDropOptions;
        private System.Windows.Forms.RadioButton radioButtonShift;
        private System.Windows.Forms.RadioButton radioButtonSwitch;
    }
}