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
        private readonly BookmarkService _bookmarkService;
        private ImageReferenceElement _imageReference;
        private readonly ApplicationSettingsService _applicationSettingsService;
        public FormAddBookmark(BookmarkManager bookmarkManager, BookmarkService bookmarkService, ApplicationSettingsService applicationSettingsService)
        {
            InitializeComponent();

            _bookmarkManager = bookmarkManager;
            _bookmarkService = bookmarkService;
            _applicationSettingsService = applicationSettingsService;
        }

        public void Init(Point startupPosition, ImageReferenceElement imageReference)
        {
            SetDesktopLocation(startupPosition.X, startupPosition.Y);
            _imageReference = imageReference;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int csDropshadow = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= csDropshadow;
                return cp;
            }
        }

        private void FormAddBookmark_Load(object sender, EventArgs e)
        {

            if (!_bookmarkManager.LoadedFromFile && !_applicationSettingsService.Settings.PasswordProtectBookmarks)
            {
                _bookmarkService.OpenBookmarks();
            }

            if (_imageReference == null)
            {
                Close();
                return;
            }

            txtBookmarkName.Text = _imageReference.FileName;
            InitFolderDropdownList();
            txtBookmarkName.Focus();
            txtBookmarkName.SelectAll();
        }

        private void InitFolderDropdownList()
        {
            BookmarkFolder rootNode = _bookmarkManager.RootFolder;
            comboBoxBookmarkFolders.Items.Clear();

            comboBoxBookmarkFolders.Items.Add(rootNode);

            foreach (BookmarkFolder folder in rootNode.BookmarkFolders)
                comboBoxBookmarkFolders.Items.Add(folder);

            if (comboBoxBookmarkFolders.Items.Count > 0)
                comboBoxBookmarkFolders.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BookmarkFolder bookmarkFolder = comboBoxBookmarkFolders.SelectedItem as BookmarkFolder ?? _bookmarkManager.RootFolder;

            _bookmarkManager.AddBookmark(bookmarkFolder.Id, txtBookmarkName.Text, _imageReference);
            _bookmarkService.SaveBookmarks();
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
            var formAddBookmarkWithNewFolder = new FormAddBookmarkWithNewFolder(_imageReference, _bookmarkService, _bookmarkManager);
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