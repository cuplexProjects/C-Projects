using System;
using System.Windows.Forms;
using ImageView.DataContracts;
using ImageView.Models;
using ImageView.Services;
using ImageView.Utility;

namespace ImageView
{
    public partial class FormAddBookmarkWithNewFolder : Form
    {
        private readonly ImageReferenceElement _imageReference;
        private readonly BookmarkFolder rootBookmarkFolder;

        public FormAddBookmarkWithNewFolder(ImageReferenceElement imageReference)
        {
            _imageReference = imageReference;
            rootBookmarkFolder = ServiceLocator.GetBookmarkService().BookmarkManager.RootFolder;
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private void FormAddBookmarkWithNewFolder_Load(object sender, EventArgs e)
        {
            var rootBookmarksFolders = rootBookmarkFolder.BookmarkFolders;
            var rootNode = new TreeNode("Bookmarks");
            bookmarksTree.Nodes.Add(rootNode);
            foreach (BookmarkFolder bookmarkFolder in rootBookmarksFolders)
            {
                rootNode.Nodes.Add(new TreeNode(bookmarkFolder.Name));
            }

            txtBookmarkName.Text = _imageReference.FileName;
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
        }

        private void mainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, IntPtr.Zero);
            }
        }
    }
}