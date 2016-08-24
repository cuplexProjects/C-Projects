using ImageView.DataContracts;

namespace ImageView
{
    partial class FormBookmarks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBookmarks));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FolderImages = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.bookmarksTree = new System.Windows.Forms.TreeView();
            this.contextMenuStripFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.renameFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.bookmarksDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.completePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formatedFileSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookmarkBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.mainWinMenu = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tryToFixBrokenLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDefaultDriveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookmarkBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStripBookmarks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookmarkBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.bookmarkFolderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStripFolders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarksDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource1)).BeginInit();
            this.mainWinMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource)).BeginInit();
            this.contextMenuStripBookmarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkFolderBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FolderImages
            // 
            this.FolderImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FolderImages.ImageStream")));
            this.FolderImages.TransparentColor = System.Drawing.Color.Transparent;
            this.FolderImages.Images.SetKeyName(0, "folder.ico");
            this.FolderImages.Images.SetKeyName(1, "opened_folder.ico");
            this.FolderImages.Images.SetKeyName(2, "Folder_Icon.ico");
            this.FolderImages.Images.SetKeyName(3, "Folder_yellow_Icon.ico");
            this.FolderImages.Images.SetKeyName(4, "Folder_IconOpen.ico");
            this.FolderImages.Images.SetKeyName(5, "Folder_my_pictures_Icon.ico");
            this.FolderImages.Images.SetKeyName(6, "Folder-Opened-icon.ico");
            this.FolderImages.Images.SetKeyName(7, "Folder-Closed-icon.ico");
            this.FolderImages.Images.SetKeyName(8, "Documents-icon.ico");
            this.FolderImages.Images.SetKeyName(9, "Folder-icon.ico");
            this.FolderImages.Images.SetKeyName(10, "normal_folder.ico");
            this.FolderImages.Images.SetKeyName(11, "Open_Folder_Icon.ico");
            this.FolderImages.Images.SetKeyName(12, "Closed_Folder_Icon.ico");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.bookmarksDataGridView);
            this.splitContainer1.Panel2.Controls.Add(this.mainWinMenu);
            this.splitContainer1.Size = new System.Drawing.Size(734, 462);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.bookmarksTree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.pictureBoxPreview);
            this.splitContainer2.Size = new System.Drawing.Size(206, 462);
            this.splitContainer2.SplitterDistance = 225;
            this.splitContainer2.TabIndex = 0;
            // 
            // bookmarksTree
            // 
            this.bookmarksTree.AllowDrop = true;
            this.bookmarksTree.ContextMenuStrip = this.contextMenuStripFolders;
            this.bookmarksTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bookmarksTree.ImageIndex = 0;
            this.bookmarksTree.ImageList = this.FolderImages;
            this.bookmarksTree.Location = new System.Drawing.Point(0, 0);
            this.bookmarksTree.Name = "bookmarksTree";
            this.bookmarksTree.SelectedImageIndex = 1;
            this.bookmarksTree.ShowPlusMinus = false;
            this.bookmarksTree.ShowRootLines = false;
            this.bookmarksTree.Size = new System.Drawing.Size(206, 225);
            this.bookmarksTree.TabIndex = 0;
            // 
            // contextMenuStripFolders
            // 
            this.contextMenuStripFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFolderToolStripMenuItem,
            this.deleteFolderToolStripMenuItem,
            this.toolStripSeparator2,
            this.renameFolderMenuItem});
            this.contextMenuStripFolders.Name = "contextMenuStripFolders";
            this.contextMenuStripFolders.Size = new System.Drawing.Size(154, 76);
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.addFolderToolStripMenuItem.Text = "Add Folder";
            this.addFolderToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // deleteFolderToolStripMenuItem
            // 
            this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
            this.deleteFolderToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.deleteFolderToolStripMenuItem.Text = "Delete Folder";
            this.deleteFolderToolStripMenuItem.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // renameFolderMenuItem
            // 
            this.renameFolderMenuItem.Name = "renameFolderMenuItem";
            this.renameFolderMenuItem.Size = new System.Drawing.Size(153, 22);
            this.renameFolderMenuItem.Text = "Rename Folder";
            this.renameFolderMenuItem.Click += new System.EventHandler(this.renameFolderMenuItem_Click);
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.BackColor = System.Drawing.Color.White;
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(206, 233);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPreview.TabIndex = 1;
            this.pictureBoxPreview.TabStop = false;
            // 
            // bookmarksDataGridView
            // 
            this.bookmarksDataGridView.AllowDrop = true;
            this.bookmarksDataGridView.AllowUserToAddRows = false;
            this.bookmarksDataGridView.AllowUserToResizeRows = false;
            this.bookmarksDataGridView.AutoGenerateColumns = false;
            this.bookmarksDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.bookmarksDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.bookmarksDataGridView.ColumnHeadersHeight = 25;
            this.bookmarksDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.bookmarksDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.completePathDataGridViewTextBoxColumn,
            this.formatedFileSizeDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.bookmarksDataGridView.DataSource = this.bookmarkBindingSource1;
            this.bookmarksDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bookmarksDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.bookmarksDataGridView.GridColor = System.Drawing.SystemColors.ScrollBar;
            this.bookmarksDataGridView.Location = new System.Drawing.Point(0, 24);
            this.bookmarksDataGridView.MultiSelect = false;
            this.bookmarksDataGridView.Name = "bookmarksDataGridView";
            this.bookmarksDataGridView.RowHeadersVisible = false;
            this.bookmarksDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(227)))), ((int)(((byte)(252)))));
            this.bookmarksDataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.bookmarksDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.bookmarksDataGridView.ShowCellErrors = false;
            this.bookmarksDataGridView.ShowCellToolTips = false;
            this.bookmarksDataGridView.ShowEditingIcon = false;
            this.bookmarksDataGridView.ShowRowErrors = false;
            this.bookmarksDataGridView.Size = new System.Drawing.Size(524, 438);
            this.bookmarksDataGridView.StandardTab = true;
            this.bookmarksDataGridView.TabIndex = 0;
            this.bookmarksDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.bookmarksDataGridView_CellMouseDoubleClick);
            this.bookmarksDataGridView.SelectionChanged += new System.EventHandler(this.bookmarksDataGridView_SelectionChanged);
            this.bookmarksDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bookmarksDataGridView_KeyDown);
            this.bookmarksDataGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.bookmarksDataGridView_MouseUp);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "FileName";
            this.dataGridViewTextBoxColumn3.HeaderText = "FileName";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "BoookmarkName";
            this.dataGridViewTextBoxColumn1.HeaderText = "BoookmarkName";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Directory";
            this.dataGridViewTextBoxColumn2.HeaderText = "Directory";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // completePathDataGridViewTextBoxColumn
            // 
            this.completePathDataGridViewTextBoxColumn.DataPropertyName = "CompletePath";
            this.completePathDataGridViewTextBoxColumn.HeaderText = "CompletePath";
            this.completePathDataGridViewTextBoxColumn.Name = "completePathDataGridViewTextBoxColumn";
            // 
            // formatedFileSizeDataGridViewTextBoxColumn
            // 
            this.formatedFileSizeDataGridViewTextBoxColumn.DataPropertyName = "FormatedFileSize";
            this.formatedFileSizeDataGridViewTextBoxColumn.HeaderText = "FormatedFileSize";
            this.formatedFileSizeDataGridViewTextBoxColumn.Name = "formatedFileSizeDataGridViewTextBoxColumn";
            this.formatedFileSizeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "CreationTime";
            this.dataGridViewTextBoxColumn5.HeaderText = "CreationTime";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "LastWriteTime";
            this.dataGridViewTextBoxColumn6.HeaderText = "LastWriteTime";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "LastAccessTime";
            this.dataGridViewTextBoxColumn7.HeaderText = "LastAccessTime";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "FileExtention";
            this.dataGridViewTextBoxColumn8.HeaderText = "FileExtention";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // bookmarkBindingSource1
            // 
            this.bookmarkBindingSource1.DataSource = typeof(Bookmark);
            // 
            // mainWinMenu
            // 
            this.mainWinMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.mainWinMenu.Location = new System.Drawing.Point(0, 0);
            this.mainWinMenu.Name = "mainWinMenu";
            this.mainWinMenu.Size = new System.Drawing.Size(524, 24);
            this.mainWinMenu.TabIndex = 1;
            this.mainWinMenu.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tryToFixBrokenLinksToolStripMenuItem,
            this.setDefaultDriveToolStripMenuItem,
            this.toolStripSeparator1,
            this.showLogToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // tryToFixBrokenLinksToolStripMenuItem
            // 
            this.tryToFixBrokenLinksToolStripMenuItem.Name = "tryToFixBrokenLinksToolStripMenuItem";
            this.tryToFixBrokenLinksToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.tryToFixBrokenLinksToolStripMenuItem.Text = "Fix Broken Links";
            this.tryToFixBrokenLinksToolStripMenuItem.Click += new System.EventHandler(this.tryToFixBrokenLinksToolStripMenuItem_Click);
            // 
            // setDefaultDriveToolStripMenuItem
            // 
            this.setDefaultDriveToolStripMenuItem.Name = "setDefaultDriveToolStripMenuItem";
            this.setDefaultDriveToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.setDefaultDriveToolStripMenuItem.Text = "Set Default Drive";
            this.setDefaultDriveToolStripMenuItem.Click += new System.EventHandler(this.setDefaultDriveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Enabled = false;
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.showLogToolStripMenuItem.Text = "Show Log";
            this.showLogToolStripMenuItem.Click += new System.EventHandler(this.showLogToolStripMenuItem_Click);
            // 
            // contextMenuStripBookmarks
            // 
            this.contextMenuStripBookmarks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStripBookmarks.Name = "contextMenuStripBookmarks";
            this.contextMenuStripBookmarks.Size = new System.Drawing.Size(118, 70);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // bookmarkBindingSource2
            // 
            this.bookmarkBindingSource2.DataSource = typeof(Bookmark);
            // 
            // bookmarkFolderBindingSource
            // 
            this.bookmarkFolderBindingSource.DataSource = typeof(BookmarkFolder);
            // 
            // FormBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 462);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainWinMenu;
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FormBookmarks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bookmarks";
            this.Load += new System.EventHandler(this.FormBookmarks_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStripFolders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarksDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource1)).EndInit();
            this.mainWinMenu.ResumeLayout(false);
            this.mainWinMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource)).EndInit();
            this.contextMenuStripBookmarks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkFolderBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ImageList FolderImages;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView bookmarksTree;
        private System.Windows.Forms.DataGridView bookmarksDataGridView;
        private System.Windows.Forms.BindingSource bookmarkBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFolders;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFolderToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBookmarks;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MenuStrip mainWinMenu;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tryToFixBrokenLinksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.BindingSource bookmarkBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn completePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn formatedFileSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.ToolStripMenuItem setDefaultDriveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem renameFolderMenuItem;
        private System.Windows.Forms.BindingSource bookmarkBindingSource2;
        private System.Windows.Forms.BindingSource bookmarkFolderBindingSource;
    }
}