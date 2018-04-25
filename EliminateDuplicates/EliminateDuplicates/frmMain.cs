using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Delegates;
using DeleteDuplicateFiles.Managers;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Resources;
using DeleteDuplicateFiles.Resources.LanguageFiles;
using DeleteDuplicateFiles.Services;
using DeleteDuplicateFiles.WorkFlows;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using Serilog;

namespace DeleteDuplicateFiles
{
    public partial class FrmMain : Form
    {
        private readonly DuplicateFileWorkflowController _duplicateFileFinder;
        private readonly SearchProfileManager _searchProfileManager;
        private List<DuplicateFileModel> _duplicateFiles;
        private List<ScanFolderModel> _scanFolders;
        private FrmSettings _settingsForm;
        private readonly AppSettingsManager _appAppSettingsService;
        private int _hashComputeCount;
        private readonly ILifetimeScope _scope;

        public FrmMain(SearchProfileManager searchProfileManager, AppSettingsManager appAppSettingsService, SearchProfileManager searchProfileService, ILifetimeScope scope, DuplicateFileWorkflowController duplicateFileFinder)
        {
            _searchProfileManager = searchProfileManager;
            _appAppSettingsService = appAppSettingsService;
            _appAppSettingsService.LoadSettings();
            _scope = scope;
            _duplicateFileFinder = duplicateFileFinder;

            if (DesignMode)
                return;

            InitializeComponent();


            _searchProfileManager.CreateNewProfile("New Profile");

            _scanFolders = _searchProfileManager.SearchProfile.ScanFolderList;
            _duplicateFileFinder.OnProgressUpdate += duplicateFileFinder_OnProgressUpdate;
            _duplicateFileFinder.OnSearchComplete += duplicateFileFinder_OnSearchComplete;
            _duplicateFileFinder.OnSearchBegin += _duplicateFileFinder_OnSearchBegin;

            _duplicateFileFinder.OnBeginNewFileHash += _duplicateFileFinder_OnBeginNewFileHash;
            _duplicateFileFinder.OnCompleteFileHash += _duplicateFileFinder_OnCompleteFileHash;
            _duplicateFileFinder.OnCompletedRemovalOfDeletedFiles += OnCompletedRemovalOfDeletedFiles;

            LoadProfileSettings();
            _duplicateFiles = null;
            InitLoadAndSaveDialogs();
        }

        private void _duplicateFileFinder_OnCompleteFileHash(object sender, FileHashEventArgs e)
        {
            lock (this)
            {
                _hashComputeCount--;
            }

            Invoke(new FileHashEventHandler(UpdateFileHashStatus), sender, e);
        }

        private void _duplicateFileFinder_OnBeginNewFileHash(object sender, FileHashEventArgs e)
        {
            lock (this)
            {
                _hashComputeCount++;
            }

            Invoke(new FileHashEventHandler(UpdateFileHashStatus), sender, e);
        }

        private void UpdateFileHashStatus(object sender, FileHashEventArgs e)
        {
            lblFileHashesRunning.Text = Math.Max(0, _hashComputeCount).ToString();
            if (e.FileName == null)
                lblFileHashInfo.Text = "";
            else
                lblFileHashInfo.Text = GeneralConverters.GetFileNameFromPath(e.FileName) + " - " + GeneralConverters.FormatFileSizeToString(e.FileSize);
        }

        private ApplicationSettingsModel SettingsModel => _appAppSettingsService.ApplicationSettings;

        private void InitLoadAndSaveDialogs()
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = PropertyValues.frmMain_InitLoadAndSaveDialogs_OpenFileDialogFilter;
            if (_searchProfileManager.SearchProfile != null)
                saveFileDialog1.FileName = _searchProfileManager.SearchProfile.ProfileName + ".dsp";

            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = PropertyValues.frmMain_InitLoadAndSaveDialogs_OpenFileDialogFilter;
        }

        private void searchProfileManager_OnProfileDataDataChanged(object sender, EventArgs e)
        {
            lblSearchProfileName.Text = _searchProfileManager.SearchProfile.ProfileName;
            UpdateMenuItemState(sender, e);
        }

        private void _duplicateFileFinder_OnSearchBegin(object sender, EventArgs e)
        {
            Invoke(new EventHandler(UpdateMenuItemState));
        }

        private void duplicateFileFinder_OnSearchComplete(object sender, EventArgs e)
        {
            Invoke(new EventHandler(duplicateFileFinder_OnSearchCompleteNativeThread), sender, e);
        }

        private void duplicateFileFinder_OnSearchCompleteNativeThread(object sender, EventArgs e)
        {
            SearchCompleted();
        }

        private void duplicateFileFinder_OnProgressUpdate(object sender, SearchProgressEventArgs e)
        {
            Invoke(new SearchProgressEventHandler(duplicateFileFinder_OnProgressUpdateNativeThread), sender, e);
        }

        private void duplicateFileFinder_OnProgressUpdateNativeThread(object sender, SearchProgressEventArgs e)
        {
            pbFileSearch.Value = e.PercentComplete;
            lblSearchStatus.Text = e.ProgressMessage;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            lbScanFolders.DataSource = _scanFolders;
            CleanFileInfoUi();

            // Append version number in window title
            Text = Properties.Resources.ProductName + @" - ver: " + Application.ProductVersion;

            UpdateMenuItemState();
        }

        private void UpdateMenuItemState(object sender, EventArgs e)
        {
            if (sender == this)
                UpdateMenuItemState();
        }

        private void UpdateMenuItemState()
        {
            searchDirectoriesToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            cancelSearchToolStripMenuItem.Enabled = _duplicateFileFinder.IsRunning;
            searchForDuplicatesToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            clearResultsToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;

            optionsToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            addDirectoryToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            removeSelectedToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            clearAllToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            setPreferredDirPrioListToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            exitToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;
            fileToolStripMenuItem.Enabled = !_duplicateFileFinder.IsRunning;

            btnMoveUp.Enabled = !_duplicateFileFinder.IsRunning;
            btnMoveDown.Enabled = !_duplicateFileFinder.IsRunning;
            btnAdd.Enabled = !_duplicateFileFinder.IsRunning;
            btnRemove.Enabled = !_duplicateFileFinder.IsRunning;

            btnSearch.Enabled = lbScanFolders.Items.Count > 0;

            saveAsToolStripMenuItem.Enabled = _searchProfileManager.HasUnsavedProfile || _searchProfileManager.NewProfileCreated;
            saveToolStripMenuItem.Enabled = (_searchProfileManager.HasUnsavedProfile || _searchProfileManager.NewProfileCreated) && !_searchProfileManager.EmptyProfile;

            btnSearch.Text = _duplicateFileFinder.IsRunning ? "Cancel" : "Search";
            openLastProfileMenuItem.Enabled = !string.IsNullOrEmpty(SettingsModel.LastProfileFilePath);
        }

        private void LoadProfileSettings()
        {
            var profile = _searchProfileManager.SearchProfile;
            lbScanFolders.DataSource = null;
            if (profile.ScanFolderList != null)
            {
                _scanFolders = _searchProfileManager.SearchProfile.ScanFolderList;
                lbScanFolders.DataSource = _scanFolders;
            }

            lbScanFolders.Refresh();
            txtFilenameFilter.Text = profile.FileNameFilter;
            lblSearchProfileName.Text = _searchProfileManager.SearchProfile.ProfileName;

            UpdateMenuItemState();
        }

        private void SetProfileSettings()
        {
            var profile = _searchProfileManager.SearchProfile;
            if (lbScanFolders.DataSource is ObservableCollection<ScanFolderModel> observableCollection) profile.ScanFolderList = observableCollection.ToList();
            profile.FileNameFilter = txtFilenameFilter.Text;
        }

        private void CleanFileInfoUi()
        {
            lblDirectoryInfo.Text = "";
            lblFileNameInfo.Text = "";
            lblFileSizeInfo.Text = "";
            lblCreationTimeInfo.Text = "";
            lblLastWriteTimeInfo.Text = "";
            lblSearchStatus.Text = "";
        }

        private void SearchCompleted()
        {
            lblSearchStatus.Text = "";
            lblFileHashesRunning.Text = "0";
            pbFileSearch.Value = 100;

            CleanFileInfoUi();
            UpdateMenuItemState();

            if (_duplicateFiles != null)
            {
                //Order by FullPath instead of file size
                _duplicateFiles.Sort((file, duplicateFile) => string.Compare(duplicateFile.FullName, file.FullName, StringComparison.Ordinal));

                lbResults.DataSource = _duplicateFiles;
                btnDeleteFiles.Enabled = _duplicateFiles.Count > 0;
                lblSearchResults.Text = _duplicateFiles.Count.ToString();
            }

            GC.Collect();
        }

        private void btnDeleteFiles_Click(object sender, EventArgs e)
        {
            if (lbResults.SelectedItems.Count > 0)
            {
                var duplicateFileDeleteList = (from object selectedItem in lbResults.SelectedItems select selectedItem as DuplicateFileModel).ToList();
                var noFilesToDelete = duplicateFileDeleteList.Sum(x => x.DuplicateFiles.Count);

                if (MessageBox.Show(string.Format(Generics.AreYouSureYouWantToDelete, noFilesToDelete), Generics.ConfirmDelete, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;


                foreach (var duplicateMasterFile in duplicateFileDeleteList)
                {
                    try
                    {
                        var duplicateFilesToDelete = duplicateMasterFile.DuplicateFiles;

                        switch (SettingsModel.DeletionMode)
                        {
                            case ApplicationSettingsModel.DeletionModes.Permanent:
                                foreach (var duplicateFileToDelete in duplicateFilesToDelete)
                                    File.Delete(duplicateFileToDelete.FullName);
                                break;
                            default:
                                foreach (var duplicateFileToDelete in duplicateFilesToDelete)
                                    FileOperationAPIWrapper.MoveToRecycleBin(duplicateFileToDelete.FullName);
                                break;
                        }

                        _duplicateFiles.Remove(duplicateMasterFile);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Resources.LanguageFiles.Generics.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                lbResults.DataSource = null;
                lbResults.DataSource = _duplicateFiles;
                lblSearchResults.Text = _duplicateFiles.Count.ToString();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_duplicateFileFinder.IsRunning)
            {
                CancelSearch();
                btnSearch.Enabled = false;
                return;
            }

            SearchForDuplicateFiles();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenBrowseDialogAndAddSearshDirectory();
        }

        private void lbResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbResults.SelectedItems.Count == 1)
            {
                if (!(lbResults.SelectedItems[0] is DuplicateFileModel duplicateFile)) return;
                lbDuplicateFiles.DataSource = duplicateFile.DuplicateFiles;
                txtMasterFilename.Text = duplicateFile.FullName;
            }
            else
            {
                lbDuplicateFiles.DataSource = null;
                txtMasterFilename.Text = "";
            }
        }

        private void lbDuplicateFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbDuplicateFiles.SelectedItem != null)
            {
                if (!(lbDuplicateFiles.SelectedItem is DuplicateFileModel duplicateFile)) return;
                lblDirectoryInfo.Text = Path.GetDirectoryName(duplicateFile.FullName);
                lblFileNameInfo.Text = duplicateFile.Name;
                lblFileSizeInfo.Text = GeneralConverters.FileSizeToStringFormater.ConvertFileSizeToString(duplicateFile.FileSize);
                lblCreationTimeInfo.Text = duplicateFile.CreationTime.ToString("yyyy-MM-dd - HH:mm:ss");
                lblLastWriteTimeInfo.Text = duplicateFile.LastWriteTime.ToString("yyyy-MM-dd - HH:mm:ss");
            }
            else
                CleanFileInfoUi();
        }

        private void txtMasterFilename_Click(object sender, EventArgs e)
        {
            txtMasterFilename.SelectAll();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && _duplicateFileFinder.IsRunning)
            {
                MessageBox.Show(PropertyValues.FormClosing_PleaseCancelSearchFirst);
                e.Cancel = true;
            }
            else if (_duplicateFileFinder.IsRunning)
            {
                _duplicateFileFinder.StopSearching();

                //Wait for search to stop
                while (_duplicateFileFinder.IsRunning)
                {
                    Thread.Sleep(100);
                }
            }

            
            _appAppSettingsService.SaveSettings();
            
        }

        private void OnCompletedRemovalOfDeletedFiles(object sender, FileHashRemovalEventArgs e)
        {
            if (Visible)
                Invoke(new RemoveDeletedHashDbItemsEventHandler(OnCompletedRemovalOfDeletedFilesNativeThread), sender, e);
        }

        private void OnCompletedRemovalOfDeletedFilesNativeThread(object sender, FileHashRemovalEventArgs e)
        {
            if (_settingsForm != null && _settingsForm.Visible)
                _settingsForm.EnableOptimizeDbButton();

            MessageBox.Show(string.Format(PropertyValues.frmMain_RemovedXItemsFromHashDb, e.ItemsRemoved), Generics.RemovalCompleted, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool AddScanFolderToList(string path)
        {
            var listItem = new ScanFolderModel
            {
                FullPath = path
            };
            var selectedIndex = lbScanFolders.SelectedIndex;
            var sortOrder = 0;

            if (_scanFolders.Count > 0)
                sortOrder = _scanFolders.Max(x => x.SortOrder) + 1;

            listItem.SortOrder = sortOrder;
            if (_scanFolders.Any(scanFolderListItem => scanFolderListItem.Contains(listItem)))
                return false;

            _scanFolders.Add(listItem);
            _scanFolders.Sort();
            lbScanFolders.DataSource = null;
            lbScanFolders.DataSource = _scanFolders;
            lbScanFolders.Refresh();

            Thread.Sleep(1);

            if (selectedIndex > 0)
                lbScanFolders.SelectedIndex = selectedIndex;

            _searchProfileManager.SearchProfile.ScanFolderListUpdated();
            return true;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedFolderFromList();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lbScanFolders.SelectedItems.Count != 1 || lbScanFolders.Items.Count <= 1) return;
            var listItem = lbScanFolders.SelectedItems[0] as ScanFolderModel;

            if (listItem != null && listItem.SortOrder == 0)
                return;

            var aboveListItem = _scanFolders.FirstOrDefault(x => listItem != null && x.SortOrder == listItem.SortOrder - 1);

            if (aboveListItem == null || listItem == null) return;
            var sortOrder = listItem.SortOrder;
            listItem.SortOrder = aboveListItem.SortOrder;
            aboveListItem.SortOrder = sortOrder;

            _scanFolders.Sort();
            lbScanFolders.DataSource = null;
            lbScanFolders.DataSource = _scanFolders;

            lbScanFolders.SelectedItem = listItem;
            _searchProfileManager.SearchProfile.ScanFolderListUpdated();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lbScanFolders.SelectedItems.Count != 1 || lbScanFolders.Items.Count <= 1) return;
            var listItem = lbScanFolders.SelectedItems[0] as ScanFolderModel;

            if (listItem != null && listItem.SortOrder == _scanFolders.Count - 1)
                return;

            var belowListItem = _scanFolders.FirstOrDefault(x => listItem != null && x.SortOrder == listItem.SortOrder + 1);

            if (belowListItem == null) return;
            if (listItem != null)
            {
                var sortOrder = listItem.SortOrder;
                listItem.SortOrder = belowListItem.SortOrder;
                belowListItem.SortOrder = sortOrder;
            }

            _scanFolders.Sort();
            lbScanFolders.DataSource = null;
            lbScanFolders.DataSource = _scanFolders;

            lbScanFolders.SelectedItem = listItem;
            _searchProfileManager.SearchProfile.ScanFolderListUpdated();
        }

        private void lbScanFolders_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileDropArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (fileDropArray.Any(path => !Directory.Exists(path)))
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                e.Effect = DragDropEffects.Copy;
            }
        }

        private void lbScanFolders_DragDrop(object sender, DragEventArgs e)
        {
            var sb = new StringBuilder();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileDropArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (fileDropArray.Any(path => !Directory.Exists(path)))
                    return;

                foreach (var path in fileDropArray)
                {
                    if (!AddScanFolderToList(path))
                        sb.AppendLine("Could not add folder: '" + path + "' because its parent or sub folder is already added");
                }
                UpdateMenuItemState();
            }
            if (sb.Length > 0)
                MessageBox.Show(sb.ToString(), PropertyValues.frmMain_SubfolderConflict, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     Handle select all items when Ctrl + a is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbResults_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control || e.KeyCode != Keys.A || lbResults.Items.Count <= 0) return;
            for (var i = 0; i < lbResults.Items.Count; i++)
                lbResults.SetSelected(i, true);
        }

        private void RemoveSelectedFolderFromList()
        {
            if (lbScanFolders.SelectedItems.Count != 1) return;

            if (!(lbScanFolders.SelectedItems[0] is ScanFolderModel listItem))
                return;

            _scanFolders.Remove(listItem);
            ReindexSearchFolderList();

            lbScanFolders.DataSource = null;
            lbScanFolders.DataSource = _scanFolders;
            _searchProfileManager.SearchProfile.ScanFolderListUpdated();
            UpdateMenuItemState();
        }

        private void ReindexSearchFolderList()
        {
            _scanFolders.Sort();

            for (var i = 0; i < _scanFolders.Count; i++)
            {
                _scanFolders[i].SortOrder = i;
            }
        }

        private void SearchForDuplicateFiles()
        {
            if (!_duplicateFileFinder.IsReady)
            {
                MessageBox.Show(Display.frmMain_SearchForDuplicateFiles_ComputeHashServUnavailable, Display.frmMain_SearchForDuplicateFiles_DatabaseIsLoading, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_scanFolders.Count == 0)
            {
                MessageBox.Show(Generics.PleaseSelectAValidDirectory);
                return;
            }

            var filenameFilter = txtFilenameFilter.Text;
            if (filenameFilter == "")
                filenameFilter = null;

            try
            {
                lbResults.DataSource = null;
                _hashComputeCount = 0;

                lblSearchResults.Text = "0";
                txtMasterFilename.Text = "";
                lbDuplicateFiles.DataSource = null;
                UpdateMenuItemState();

                var searchProfile = new SearchProfileModel(filenameFilter, _scanFolders.ToList(),new List<PreferredDirectoryDataModel> ()){IncludeSubfolders = chkIncludeSubfolders.Checked };

                _duplicateFileFinder.StartDuplicateSearch(searchProfile);
                    Invoke(new EventHandler(duplicateFileFinder_OnSearchCompleteNativeThread), this, new EventArgs());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenBrowseDialogAndAddSearshDirectory()
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            var dirPath = folderBrowserDialog1.SelectedPath;

            if (!AddScanFolderToList(dirPath))
                MessageBox.Show(Display.frmMain_AddSearshDirectory_conflicting_item, PropertyValues.frmMain_SubfolderConflict, MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                UpdateMenuItemState();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            var p = new Pen(Color.LightSteelBlue, 1f);
            e.Graphics.DrawRectangle(p, 0, 0, flowLayoutPanel1.Width - 2, flowLayoutPanel1.Height - 2);
            p = new Pen(Color.LightGray, 1f);
            e.Graphics.DrawRectangle(p, 1, 1, flowLayoutPanel1.Width - 4, flowLayoutPanel1.Height - 4);
        }

        private void frmMain_ResizeBegin(object sender, EventArgs e)
        {
            flowLayoutPanel1.Invalidate();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            flowLayoutPanel1.Invalidate();
        }

        private void frmMain_ResizeEnd(object sender, EventArgs e)
        {
            flowLayoutPanel1.Invalidate();
        }

        private void newProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _searchProfileManager.CreateNewProfile("New Profile");
            LoadProfileSettings();
        }

        private void openProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _searchProfileManager.LoadSearchProfile(openFileDialog1.FileName);
                    SettingsModel.LastProfileFilePath = openFileDialog1.FileName;
                    LoadProfileSettings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Generics.ErrorOpeningFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetProfileSettings();
                _searchProfileManager.SaveSearchProfile(_searchProfileManager.SearchProfile.FullPath);
                UpdateMenuItemState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Generics.ErrorSavingFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    _searchProfileManager.SearchProfile.FullPath = saveFileDialog1.FileName;
                    _searchProfileManager.SearchProfile.ProfileName = GeneralConverters.GetFileNameFromPath(saveFileDialog1.FileName);

                    SetProfileSettings();
                    _searchProfileManager.SaveSearchProfile(saveFileDialog1.FileName);
                    UpdateMenuItemState();
                    lblSearchProfileName.Text = _searchProfileManager.SearchProfile.ProfileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Generics.ErrorSavingFile, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Generics.AreYouSureYouWantToExit, Generics.ExitAppQuestion, MessageBoxButtons.YesNo) == DialogResult.Yes)
                Application.Exit();
        }

        private void setPreferredDirPrioListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmPreferredMasterFileRules = new FrmSetPreferredMasterFileRules();
            frmPreferredMasterFileRules.SetPreferredDirectories(_searchProfileManager.SearchProfile.PreferredDirecoryList);

            if (frmPreferredMasterFileRules.ShowDialog(this) == DialogResult.OK)
            {
                _searchProfileManager.SearchProfile.PreferredDirecoryList = frmPreferredMasterFileRules.GetPreferredDirectories();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _settingsForm = _scope.Resolve<FrmSettings>();
            _settingsForm.ShowDialog(this);
        }

        private void addDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenBrowseDialogAndAddSearshDirectory();
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSelectedFolderFromList();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Display.frmMain_clarAlSearchDirConfirmationQuesion, Generics.ClearList, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _scanFolders.Clear();
                lbScanFolders.DataSource = null;
                lbScanFolders.DataSource = _scanFolders;
                GC.Collect();
            }
        }

        private void clearResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, Display.frmMain_ClearSearchFolders_ConfirmationRequest, Generics.ClearResultsQuestion, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                CleanFileInfoUi();
                lbResults.DataSource = null;
                lbDuplicateFiles.DataSource = null;
                txtMasterFilename.Text = "";
                GC.Collect();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutDialog = new AboutBox();
            aboutDialog.ShowDialog(this);
        }

        private void searchForDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForDuplicateFiles();
        }

        private void cancelSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelSearch();
        }

        private void CancelSearch()
        {
            if (!_duplicateFileFinder.IsRunning) return;
            if (MessageBox.Show(Display.CancelSearch_Confirm_Question, Generics.Cancel_Question, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _duplicateFileFinder.StopSearching();
            UpdateMenuItemState();
            ClearSearchResults();
        }

        private void ClearSearchResults()
        {
            lbDuplicateFiles.Items.Clear();
            txtMasterFilename.Text = "";
            lbResults.Items.Clear();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openLastProfileMenuItem_Click(object sender, EventArgs e)
        {
            bool loadSearchProfileSuccessfull = _searchProfileManager.LoadSearchProfile(SettingsModel.LastProfileFilePath);

            if (!loadSearchProfileSuccessfull)
            {
                Log.Error("Failed to load search profile from: {loadedSearchProfile}", SettingsModel.LastProfileFilePath);
                MessageBox.Show(this, "Failed to load search profile, perhaps its and older format version?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            LoadProfileSettings();
        }
    }
}