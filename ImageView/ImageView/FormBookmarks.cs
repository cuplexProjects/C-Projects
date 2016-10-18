using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Log;
using ImageView.DataBinding;
using ImageView.DataContracts;
using ImageView.Events;
using ImageView.InputForms;
using ImageView.Managers;
using ImageView.Properties;
using ImageView.Services;

namespace ImageView
{
    public partial class FormBookmarks : Form
    {
        private const int CUSTOM_CONTENT_HEIGHT = 4;
        private TreeViewDataContext _treeViewDataContext;
        private readonly Color gridViewGradientBackgroundColorStart = ColorTranslator.FromHtml("#b2e1ff");
        private readonly Color gridViewGradientBackgroundColorStop = ColorTranslator.FromHtml("#66b6fc");
        private readonly Color GridViewSelectionBorderColor = ColorTranslator.FromHtml("#7da2ce");
        private readonly StringBuilder logStringBuilder;
        private readonly List<string> volumeInfoArray;
        private int brokenLinks;
        private string defaultDrive = "c:\\";
        private int fixedLinks;

        public FormBookmarks()
        {
            InitializeComponent();
            logStringBuilder = new StringBuilder();
            volumeInfoArray = new List<string>();
           

            

            try
            {
                var driveInfoArray = DriveInfo.GetDrives();
                foreach (DriveInfo driveInfo in driveInfoArray.Where(driveInfo => driveInfo.IsReady))
                {
                    volumeInfoArray.Add(driveInfo.Name);
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("FormBookmarks() Constructor", ex);
            }
        }

        private void FormBookmarks_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            BookmarkService bookmarkService = ServiceLocator.GetBookmarkService();
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            bookmarksDataGridView.RowPrePaint += bookmarksDataGridView_RowPrePaint;
            bookmarksDataGridView.RowPostPaint += bookmarksDataGridView_RowPostPaint;
            bookmarkManager.OnBookmarksUpdate += Instance_OnBookmarksUpdate;
            bookmarksTree.AfterSelect += BookmarksTree_AfterSelect;
            InitBookmarksDataGridViev();
            _treeViewDataContext = new TreeViewDataContext(bookmarksTree, bookmarkService.BookmarkManager.RootFolder);

            if (ApplicationSettingsService.Instance.Settings.PasswordProtectBookmarks)
                using (var formgetPassword = new FormGetPassword
                {
                    PasswordDerivedString = ApplicationSettingsService.Instance.Settings.PasswordDerivedString
                })
                {
                    if (formgetPassword.ShowDialog() == DialogResult.OK)
                    {
                        if (!formgetPassword.PasswordVerified)
                        {
                            MessageBox.Show(this, Resources.Invalid_password_);
                            Close();
                            return;
                        }

                        if (bookmarkService.OpenBookmarks(formgetPassword.PasswordString))
                            initBookmarksDataSource();
                        else
                        {
                            MessageBox.Show(Resources.failed_to_decode_file_);
                            Close();
                        }
                    }
                    else
                        Close();
                }
            else
            {
                if (!bookmarkService.BookmarkManager.LoadedFromFile)
                {
                    bookmarkService.OpenBookmarks();
                }
                
                initBookmarksDataSource();
            }
               
        }

        private void BookmarksTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ReLoadBookmarks();
        }

        private void Instance_OnBookmarksUpdate(object sender, BookmarkUpdatedEventArgs e)
        {
            ReLoadBookmarks();
        }

        private void ReLoadBookmarks()
        {
            TreeNode selectedNode = bookmarksTree.SelectedNode;
            var selectedBookmarkfolder = selectedNode.Tag as BookmarkFolder;

            if (selectedBookmarkfolder != null)
            {
                bookmarksDataGridView.DataSource = selectedBookmarkfolder.Bookmarks.OrderBy(x => x.SortOrder).ToList();
                bookmarksDataGridView.Refresh();
            }
        }

        private void AlterTreeViewState(TreeViewFolderStateChange stateChange, BookmarkFolder folder)
        {
            if (stateChange == TreeViewFolderStateChange.FolderAdded)
                _treeViewDataContext.ExpandNode(folder);
            else
            {
                _treeViewDataContext.ExpandNode(bookmarksTree.TopNode.Tag as BookmarkFolder);
            }
        }

        private void initBookmarksDataSource()
        {
            _treeViewDataContext = new TreeViewDataContext(bookmarksTree, ServiceLocator.GetBookmarkService().BookmarkManager.RootFolder);
            _treeViewDataContext.BindData();
        }

        private void InitBookmarksDataGridViev()
        {
            // Set a cell padding to provide space for the top of the focus 
            // rectangle and for the content that spans multiple columns. 
            var newPadding = new Padding(0, 1, 0, CUSTOM_CONTENT_HEIGHT);
            bookmarksDataGridView.RowTemplate.DefaultCellStyle.Padding = newPadding;

            // Set the selection background color to transparent so 
            // the cell won't paint over the custom selection background.
            bookmarksDataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.Transparent;

            // Set the row height to accommodate the content that 
            // spans multiple columns.
            bookmarksDataGridView.RowTemplate.Height += CUSTOM_CONTENT_HEIGHT;

            // Initialize other DataGridView properties.
            bookmarksDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            bookmarksDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // Paints the content that spans multiple columns and the focus rectangle.
        private void bookmarksDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush forebrush = null;
            try
            {
                // Determine the foreground color.
                forebrush = (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected
                    ? new SolidBrush(e.InheritedRowStyle.SelectionForeColor)
                    : new SolidBrush(e.InheritedRowStyle.ForeColor);
            }
            finally
            {
                forebrush?.Dispose();
            }
        }

        private void bookmarksDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                // Do not automatically paint the focus rectangle.
                e.PaintParts &= ~DataGridViewPaintParts.Focus;

                // Determine whether the cell should be painted
                // with the custom selection background.
                if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                {
                    // Calculate the bounds of the row.
                    var rowBounds = new Rectangle(0, e.RowBounds.Top,
                        bookmarksDataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) -
                        bookmarksDataGridView.HorizontalScrollingOffset + 1,
                        e.RowBounds.Height);

                    // Paint the custom selection background.
                    using (
                        Brush backbrush = new LinearGradientBrush(rowBounds, gridViewGradientBackgroundColorStart,
                            gridViewGradientBackgroundColorStop, LinearGradientMode.Vertical))
                    {
                        e.Graphics.FillRectangle(backbrush, rowBounds);
                        var p = new Pen(backbrush, 1) {Color = GridViewSelectionBorderColor};
                        e.Graphics.DrawRectangle(p, rowBounds);
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Exception in bookmarksDataGridView_RowPrePaint()", ex);
            }
        }

        private void bookmarksDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            BookmarkService bookmarkService = ServiceLocator.GetBookmarkService();
            if (bookmarksDataGridView.SelectedRows.Count != 1) return;
            DataGridViewRow selectedRow = bookmarksDataGridView.CurrentRow;
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;

            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                LoadImageFromSelectedRow();
            }
            if (e.KeyData == Keys.Delete && selectedRow != null &&
                MessageBox.Show(this, Resources.Are_you_sure_you_want_to_delete_this_bookmark_, Resources.Delete,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var bookmark = selectedRow.DataBoundItem as Bookmark;

                if (bookmark == null) return;
                bookmarkManager.DeleteBookmark(bookmark);
                bookmarksDataGridView.DataSource =
                    bookmarkManager.RootFolder.Bookmarks.OrderBy(x => x.SortOrder).ToList();
            }
        }

        private void LoadImageFromSelectedRow()
        {
            if (bookmarksDataGridView.SelectedRows.Count != 1) return;

            DataGridViewRow selectedRow = bookmarksDataGridView.CurrentRow;
            var bookmark = selectedRow?.DataBoundItem as Bookmark;

            if (bookmark == null) return;
            try
            {
                Process.Start(bookmark.CompletePath);
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error in LoadImageFromSelectedRow()", ex);
            }
        }

        private void LoadPreviewImageFromSelectedRow()
        {
            if (bookmarksDataGridView.SelectedRows.Count != 1) return;

            DataGridViewRow selectedRow = bookmarksDataGridView.CurrentRow;
            var bookmark = selectedRow?.DataBoundItem as Bookmark;

            if (bookmark != null)
                pictureBoxPreview.ImageLocation = bookmark.CompletePath;
        }

        private void bookmarksDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            LoadPreviewImageFromSelectedRow();
        }

        private void bookmarksDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                LoadImageFromSelectedRow();
        }

        private void bookmarksDataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStripBookmarks.Show(bookmarksDataGridView, new Point(e.X, e.Y));
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bookmarksTree.SelectedNode == null)
                return;
            BookmarkService bookmarkService = ServiceLocator.GetBookmarkService();
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;

            var inputFormData = new FormInputRow.InputFormData
            {
                GroupBoxText = "Bookmark folder name",
                LabelText = "Name:",
                WindowText = "Add new bookmark folder"
            };
            var formInputRow = new FormInputRow(inputFormData);

            if (formInputRow.ShowDialog(this) == DialogResult.OK)
            {
                string folderName = formInputRow.UserInputText;
                TreeNode selectedNode = bookmarksTree.SelectedNode;
                var selectedBookmarkfolder = selectedNode.Tag as BookmarkFolder;

                if (selectedBookmarkfolder != null)
                {
                    BookmarkFolder newFolder = bookmarkManager.AddBookmarkFolder(selectedBookmarkfolder.Id, folderName);
                    AlterTreeViewState(TreeViewFolderStateChange.FolderAdded, newFolder);
                }
            }
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var treeNode = bookmarksTree.SelectedNode?.Tag as BookmarkFolder;
            BookmarkService bookmarkService = ServiceLocator.GetBookmarkService();
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;

            if (treeNode == null)
                return;

            if (
                MessageBox.Show(this, Resources.Are_you_sure_you_want_to_delete_this_folder_, Resources.Remove_folder_,
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bookmarkManager.DeleteBookmarkFolder(treeNode);
                AlterTreeViewState(TreeViewFolderStateChange.FolderRemoved, treeNode);
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRow = bookmarksDataGridView.CurrentRow;
            
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedRow = bookmarksDataGridView.CurrentRow;
            Bookmark bookmark = selectedRow?.DataBoundItem as Bookmark;
            if (bookmark == null) return;

            BookmarkService bookmarkService = ServiceLocator.GetBookmarkService();
            bookmarkService.BookmarkManager.DeleteBookmark(bookmark);
        }

        private bool VolumeExists(string volumeLabel)
        {
            if (string.IsNullOrEmpty(volumeLabel))
                return false;

            return volumeInfoArray.Any(x => x.ToLower() == volumeLabel.ToLower());
        }

        private void tryToFixBrokenLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookmarkService bookmarkService = ServiceLocator.GetBookmarkService();
            BookmarkManager bookmarkManager = bookmarkService.BookmarkManager;
            if (bookmarkManager == null)
                return;

            showLogToolStripMenuItem.Enabled = true;
            logStringBuilder.Clear();
            brokenLinks = 0;
            fixedLinks = 0;

            BookmarkFolder bookmarkFolder = bookmarkManager.RootFolder;
            UpdateBrokenLinksOnBookmarks(bookmarkFolder.Bookmarks);
            UpdateBrokenLinksOnThreeNodes(bookmarkFolder.BookmarkFolders);

            logStringBuilder.AppendLine($"Found {brokenLinks} broken linkes and fixed {fixedLinks} links.");

            if (fixedLinks > 0)
                bookmarkService.SaveBookmarks();
        }

        private void UpdateBrokenLinksOnThreeNodes(List<BookmarkFolder> bookmarkTreeNodes)
        {
            if (bookmarkTreeNodes == null)
                return;

            foreach (BookmarkFolder bookmarkTreeNode in bookmarkTreeNodes)
            {
                UpdateBrokenLinksOnBookmarks(bookmarkTreeNode.Bookmarks);
                UpdateBrokenLinksOnThreeNodes(bookmarkTreeNode.BookmarkFolders);
            }
        }

        private void UpdateBrokenLinksOnBookmarks(List<Bookmark> bookmarks)
        {
            if (bookmarks == null)
                return;

            foreach (Bookmark bookmark in bookmarks)
            {
                string volumeLabel = GeneralConverters.GetVolumeLabelFromPath(bookmark.Directory);
                string originalPath = bookmark.CompletePath;
                if (!VolumeExists(volumeLabel)) continue;
                if (!File.Exists(bookmark.CompletePath))
                {
                    brokenLinks++;
                    if (!FindFilePath(bookmark, defaultDrive)) continue;
                    logStringBuilder.AppendLine($"Fixed broken link: {originalPath} replaced to {bookmark.CompletePath}");
                    fixedLinks++;
                }
            }
        }

        private bool FindFilePath(Bookmark bookmark, string rootDirectory)
        {
            var directoryInfo = new DirectoryInfo(rootDirectory);
            FileInfo[] fileInfoArray = null;
            try
            {
                fileInfoArray = directoryInfo.GetFiles(bookmark.FileName, SearchOption.AllDirectories);
            }
            catch (Exception exception)
            {
                LogWriter.LogError("Exception in FindFilePath() rootDirectory=" + rootDirectory, exception);
            }

            FileInfo fileInfo =
                fileInfoArray?.FirstOrDefault(fi => fi.Name == bookmark.FileName && fi.Length == bookmark.Size);
            if (fileInfo == null) return false;
            bookmark.Directory = fileInfo.DirectoryName;
            bookmark.CompletePath = fileInfo.DirectoryName + "\\" + bookmark.FileName;
            bookmark.LastAccessTime = fileInfo.LastAccessTime;

            return true;
        }

        private void showLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formLogWindow = new FormLogWindow();
            formLogWindow.SetLogText(logStringBuilder.ToString());
            formLogWindow.ShowDialog(this);
        }

        private void setDefaultDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formSetDefaultDrive = new FormSetDefaultDrive();
            if (formSetDefaultDrive.ShowDialog(this) == DialogResult.OK)
                defaultDrive = formSetDefaultDrive.SelectedDrive;
        }

        private void renameFolderMenuItem_Click(object sender, EventArgs e)
        {
            var bookmarkTree = bookmarksTree.SelectedNode?.Tag as BookmarkFolder;
            if (bookmarkTree != null)
            {
            }
        }

        private enum TreeViewFolderStateChange
        {
            FolderRemoved,
            FolderAdded
        }
    }
}