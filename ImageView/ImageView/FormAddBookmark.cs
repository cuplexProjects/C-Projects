using System;
using System.Drawing;
using System.Windows.Forms;
using ImageView.DataContracts;
using ImageView.Models.Implementation;
using ImageView.Services;
using ImageView.Utility;

namespace ImageView
{
    public partial class FormAddBookmark : Form
    {
        private readonly ImageReferenceElement _imageReference;
        private readonly BookmarkFolderImplementation _rootFolder;

        public FormAddBookmark(Point startupPosition, ImageReferenceElement imageReference)
        {
            InitializeComponent();
            SetDesktopLocation(startupPosition.X, startupPosition.Y);
            _imageReference = imageReference;
            _rootFolder = BookmarkService.Instance.BookmarksContainer.RootFolderImplementation;
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

        private void FormAddBookmark_Load(object sender, EventArgs e)
        {
            txtBookmarkName.Text = _imageReference.FileName;
            InitFolderDropdownList();
            txtBookmarkName.Focus();
            txtBookmarkName.SelectAll();
        }

        private void InitFolderDropdownList()
        {
            BookmarkFolder rootNode = _rootFolder;
            comboBoxBookmarkFolders.Items.Clear();

            comboBoxBookmarkFolders.Items.Add(rootNode);

            foreach (BookmarkFolder folder in rootNode.BookmarkFolders)
                comboBoxBookmarkFolders.Items.Add(folder);

            comboBoxBookmarkFolders.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var bookmarkTreeNode = comboBoxBookmarkFolders.SelectedItem as BookmarkFolder;

            if (bookmarkTreeNode != null)
            {
                var parentFolder = bookmarkTreeNode.ParentFolder as BookmarkFolderBase;

                if (parentFolder == null)
                    _rootFolder.AddBookmark(txtBookmarkName.Text, _imageReference);
                else
                {
                    parentFolder.AddBookmark(txtBookmarkName.Text, _imageReference);
                }
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, IntPtr.Zero);
            }
        }

        private void btnCreateFolder_Click(object sender, EventArgs e)
        {
            var formAddBookmarkWithNewFolder = new FormAddBookmarkWithNewFolder(_imageReference);
            formAddBookmarkWithNewFolder.Show();
            Close();
        }

        private void txtBookmarkName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _rootFolder.AddBookmark(txtBookmarkName.Text, _imageReference);
                e.Handled = true;
                Close();
            }
        }

        private void FormAddBookmark_Shown(object sender, EventArgs e)
        {
            txtBookmarkName.Focus();
            txtBookmarkName.SelectAll();
        }
    }
}