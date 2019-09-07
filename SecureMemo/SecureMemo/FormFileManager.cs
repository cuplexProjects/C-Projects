using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SecureMemo.FileStorageEvents;
using SecureMemo.FileStorageModels;
using SecureMemo.Services;
using SecureMemo.Utility;
using Serilog;

namespace SecureMemo
{
    public partial class FormFileManager : Form
    {
        private readonly string _fileStorageDBPath;
        private readonly FileStorageService _fileStorageService;
        private StorageFileSystem _storageFileSystem;

        public FormFileManager()
        {
            InitializeComponent();
            _fileStorageService = FileStorageService.Service;
            _storageFileSystem = _fileStorageService.FileSystem;
            _fileStorageDBPath = ConfigSpecificSettings.GetSettingsFolderPath(false);
            treeViewFolders.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            treeViewFolders.DrawNode += treeViewFolders_DrawNode;
        }

        private void treeViewFolders_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var nodeRectangle = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

            if ((e.State & TreeNodeStates.Selected) > 0)
            {
                Brush borderBrushOuter = new SolidBrush(Color.FromArgb(125, 162, 206));
                Brush borderBrushInner = new SolidBrush(Color.FromArgb(235, 244, 253));
                Brush fillBrush = new SolidBrush(Color.FromArgb(217, 232, 252));

                e.Graphics.FillRoundedRectangle(borderBrushOuter, nodeRectangle.X, nodeRectangle.Y, nodeRectangle.Width, nodeRectangle.Height, 2);
                e.Graphics.FillRoundedRectangle(borderBrushInner, Deflate(nodeRectangle, 1), 2);
                e.Graphics.FillRoundedRectangle(fillBrush, Deflate(nodeRectangle, 3), 2);
            }

            Font nodeFont = Font;
            Image image = FileSystemIcons.Images[0];
            e.Graphics.DrawImage(image, nodeRectangle.X + 5, nodeRectangle.Y + 3);
            e.Graphics.DrawString(e.Node.Text, nodeFont, new SolidBrush(Color.FromArgb(5, 5, 5)), image.Width + 5 + e.Bounds.X, e.Bounds.Y + e.Bounds.Height/4);
        }

        private Rectangle Deflate(Rectangle r, int pixels)
        {
            r.X = r.X + pixels;
            r.Y = r.Y + pixels;
            r.Height = r.Height - pixels*2;
            r.Width = r.Width - pixels*2;

            return r;
        }

        private void FormFileManager_Load(object sender, EventArgs e)
        {
            InitFolderTreeViewData();
            _storageFileSystem.DirectoryStructureChanged += _storageFileSystem_DirectoryStructureChanged;
            _storageFileSystem.FileStructureChanged += _storageFileSystem_FileStructureChanged;
        }

        private void _storageFileSystem_FileStructureChanged(object sender, StorageFileSystemEventArgs e)
        {
        }

        private void _storageFileSystem_DirectoryStructureChanged(object sender, StorageDirectorySystemEventArgs e)
        {
            StorageDirectory storageDirectory;
            TreeNode[] foundNodes;

            switch (e.DirectoryEventType)
            {
                case StorageFileSystemEventTypes.Created:
                    storageDirectory = _storageFileSystem.GetDirectory(e.DirectoryId);
                    var treeNode = new TreeNode(storageDirectory.DirectoryName) {Name = storageDirectory.Id.ToString()};
                    treeViewFolders.Nodes.Add(treeNode);
                    break;
                case StorageFileSystemEventTypes.Deleted:
                    foundNodes = treeViewFolders.Nodes.Find(e.DirectoryId.ToString(), true);
                    if (foundNodes.Length > 0)
                        treeViewFolders.Nodes.Remove(foundNodes[0]);

                    break;
                case StorageFileSystemEventTypes.Renamed:
                    storageDirectory = _storageFileSystem.GetDirectory(e.DirectoryId);
                    foundNodes = treeViewFolders.Nodes.Find(e.DirectoryId.ToString(), false);
                    if (foundNodes.Length > 0)
                        foundNodes[0].Text = storageDirectory.DirectoryName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _fileStorageService.Load(_fileStorageDBPath);
                _storageFileSystem = _fileStorageService.FileSystem;
            }
            catch (Exception ex)
            {
                Log.Error(ex,"Error trying to open database");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _fileStorageService.Save(_fileStorageDBPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error trying to save database");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InitFolderTreeViewData()
        {
            treeViewFolders.Nodes.Clear();
            StorageDirectory rootDirectory = _storageFileSystem.GetRootDirectory();
            foreach (StorageDirectory directory in _storageFileSystem.GetDirectories(rootDirectory.Id))
            {
                var treeNode = new TreeNode(directory.DirectoryName) {Name = directory.Id.ToString()};
                treeViewFolders.Nodes.Add(treeNode);
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StorageDirectory baseDirectory;
            TreeNode selectedNode = treeViewFolders.SelectedNode;
            if (selectedNode == null)
                baseDirectory = _storageFileSystem.GetRootDirectory();
            else
                baseDirectory = _storageFileSystem.GetSelectedDir(treeViewFolders.SelectedNode.Name);

            _storageFileSystem.CreateDirectory(baseDirectory, "New");
        }

        private void treeViewFolders_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!_storageFileSystem.IsValidDirectoryName(e.Label))
            {
                MessageBox.Show(this, "Invalid directoryname", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.CancelEdit = true;
                return;
            }

            int directoryId = int.Parse(e.Node.Name);
            _storageFileSystem.RenameDirectory(directoryId, e.Label);
        }

        private void treeViewFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewFolders.Refresh();
        }

        private void contextMenuStripFolders_Opening(object sender, CancelEventArgs e)
        {
            renameToolStripMenuItem.Visible = treeViewFolders.SelectedNode != null;
            deleteToolStripMenuItem.Visible = treeViewFolders.SelectedNode != null;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewFolders.SelectedNode.BeginEdit();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int directoryId = int.Parse(treeViewFolders.SelectedNode.Name);
            _storageFileSystem.DeleteDirectory(directoryId);
        }
    }
}