using System;
using System.Drawing;
using System.Windows.Forms;
using ImageView.DataContracts;
using ImageView.Managers;
using ImageView.Models;
using ImageView.Services;
using ImageView.Utility;

namespace ImageView
{
    public partial class FormAddBookmark : Form
    {
        private readonly BookmarkManager _bookmarkManager;
        private readonly ImageReferenceElement _imageReference;

        public FormAddBookmark(Point startupPosition, ImageReferenceElement imageReference)
        {
            InitializeComponent();
            SetDesktopLocation(startupPosition.X, startupPosition.Y);
            _imageReference = imageReference;
            _bookmarkManager = BookmarkService.Instance.BookmarkManager;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                var cp = base.CreateParams;
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
            var rootNode = _bookmarkManager.RootFolder;
            comboBoxBookmarkFolders.Items.Clear();

            foreach (var folder in rootNode.BookmarkFolders)
                comboBoxBookmarkFolders.Items.Add(folder);

            if (comboBoxBookmarkFolders.Items.Count > 0)
                comboBoxBookmarkFolders.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var bookmarkFolder = comboBoxBookmarkFolders.SelectedItem as BookmarkFolder ?? _bookmarkManager.RootFolder;
            _bookmarkManager.AddBookmark(bookmarkFolder.ParentFolderId, txtBookmarkName.Text, _imageReference);

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
                _bookmarkManager.AddBookmark(_bookmarkManager.RootFolder.Id, txtBookmarkName.Text, _imageReference);
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