using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption.Licence;
using GeneralToolkitLib.Encryption.Licence.StaticData;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage.Memory;
using SecureMemo.DataModels;
using SecureMemo.Forms;
using SecureMemo.InputForms;
using SecureMemo.Properties;
using SecureMemo.Services;
using SecureMemo.Storage;
using SecureMemo.TextSearchModels;
using SecureMemo.UserControls;
using SecureMemo.Utility;

namespace SecureMemo
{
    public partial class FormMain : Form
    {
        private const string PwdKey = "SecureMemo";
        private const string LicenceFilename = "licence.txt";
        private readonly ApplicationState _applicationState;
        private readonly AppSettingsService _appSettingsService;
        private readonly LicenceService _licenceService;
        private readonly MemoStorageService _memoStorageService;
        private readonly PasswordStorage _passwordStorage;
        private TabDropData _dropData;
        private FormFind _formFind;
        private TabPageDataCollection _tabPageDataCollection;
        private TabSearchEngine _tabSearchEngine;
        private int _tabPageClickIndex = -1;
        private bool _isResizingWindow;
        private readonly ILifetimeScope _scope;

        public FormMain(AppSettingsService appSettingsService, MemoStorageService memoStorageService, PasswordStorage passwordStorage, ILifetimeScope scope)
        {
            if (DesignMode)
                return;

            _appSettingsService = appSettingsService;
            _memoStorageService = memoStorageService;
            _passwordStorage = passwordStorage;
            _scope = scope;

            _applicationState = new ApplicationState();
            _tabPageDataCollection = TabPageDataCollection.CreateNewPageDataCollection(_appSettingsService.Settings.DefaultEmptyTabPages);
            _licenceService = LicenceService.Instance;
            InitializeComponent();
        }

        private string AssemblyTitle
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute) attributes[0];
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeTabControls();
                _appSettingsService.LoadSettings();
                InitFormSettings();
                LoadLicenceFile();
                _licenceService.Init(SerialNumbersSettings.ProtectedApp.SecureMemo);

                Text = AssemblyTitle + " - v" + Assembly.GetExecutingAssembly().GetName().Version;
                UpdateApplicationState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.FormMain__Error_loading_application_settings, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (!_isResizingWindow)
            {
                _appSettingsService.Settings.MainWindowWith = ClientRectangle.Width;
                _appSettingsService.Settings.MainWindowHeight = ClientRectangle.Height;
            }
        }

        private void tabPageControl_TabTextDataChanged(object sender, EventArgs e)
        {
            if (_applicationState.Initializing)
                return;

            _tabPageDataCollection.TabPageDictionary[tabControlNotepad.SelectedIndex].TabPageText = GetTextInTabControl(tabControlNotepad.SelectedIndex);

            _applicationState.TabTextDataChanged = true;
            UpdateApplicationState();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.FormOwnerClosing) && !PromptExit())
            {
                e.Cancel = true;
                return;
            }

            _appSettingsService.SaveSettings();
        }

        private void tabControlNotepad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_applicationState.Initializing)
                return;

            _applicationState.TabIndexChanged = true;
            _tabPageDataCollection.ActiveTabIndex = tabControlNotepad.SelectedIndex;

            UpdateApplicationState();
        }

        private void saveToSharedFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_appSettingsService.Settings.UseSharedSyncFolder)
            {
                MessageBox.Show(Resources.FormMain_saveToSharedFolderMenuDisabled, Resources.FormMain__Could_not_save, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(_appSettingsService.Settings.SyncFolderPath) || !Directory.Exists(_appSettingsService.Settings.SyncFolderPath))
            {
                MessageBox.Show(Resources.FormMain_saveToSharedFoldrMenuInvalidPath, Resources.FormMain__Could_not_save, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            var formSetPassword = new FormSetPassword();
            if (formSetPassword.ShowDialog(this) != DialogResult.OK)
                return;

            string password = formSetPassword.VerifiedPassword;


            _tabPageDataCollection.ActiveTabIndex = tabControlNotepad.SelectedIndex;
            bool result = _memoStorageService.SaveTabPageCollection(_tabPageDataCollection, password, true);

            if (result)
                MessageBox.Show("Database successfully saved to sync folder", Resources.FormMain__Database_Saved, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Failed to save database to sync folder path", Resources.FormMain__Could_not_save, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RestoreDatabaseFromSyncPathMenuItem_Click(object sender, EventArgs e)
        {
            if (_applicationState.DatabaseExists &&
                MessageBox.Show("Are you sure that you want to replace the existing database?\nIt is recommended that you perform a backup before replacing the existing database!",
                    "Replace database from sync folder",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            var formGetPassword = new FormGetPassword {UsePasswordValidation = false};
            if (formGetPassword.ShowDialog(this) != DialogResult.OK)
                return;

            string password = formGetPassword.PasswordString;

            _tabPageDataCollection = null;
            _applicationState.DatabaseLoaded = false;
            _appSettingsService.LoadSettings();
            InitFormSettings();

            RestoreSyncDataResult result = _memoStorageService.RestoreBackupFromSyncFolder(password);
            if (result.Successful)
            {
                MessageBox.Show("Database and app settings restored from sync folder.", "Restore complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _applicationState.DatabaseExists = true;
                UpdateApplicationState();
            }
            else
                MessageBox.Show("Could not restore sync folder content: " + result.ErrorText, "Error restoring data", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CheckFormUpdatesMenuItem_Click(object sender, EventArgs e)
        {
            var applicationUpdate = new ApplicationUpdate();
            applicationUpdate.ShowDialog(this);
        }

        private void tabControlNotepad_DoubleClick(object sender, EventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl != null)
            {
            }
        }

        private void tabControlNotepad_DragDrop(object sender, DragEventArgs e)
        {
            var tabDropData = e.Data.GetData(typeof(TabDropData)) as TabDropData;

            if (tabDropData == null)
                return;

            InitializeTabControls();
        }

        private void tabControlNotepad_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabDropData)))
                e.Effect = e.AllowedEffect;
        }

        private void tabControlNotepad_DragOver(object sender, DragEventArgs e)
        {
            var p = new Point(e.X, e.Y);
            p = tabControlNotepad.PointToClient(p);
            var tabDropData = e.Data.GetData(typeof(TabDropData)) as TabDropData;

            if (tabDropData == null)
                return;

            int sourceIndex = tabDropData.SourceIndex;

            for (int i = 0; i < tabControlNotepad.TabCount; i++)
            {
                if (sourceIndex == i)
                    continue;

                Rectangle tabRectangle = tabControlNotepad.GetTabRect(i);
                if (!tabRectangle.Contains(p)) continue;
                SwapTabs(sourceIndex, i);
                tabDropData.SourceIndex = i;
                e.Data.SetData(typeof(TabDropData), tabDropData);
                break;
            }
        }

        private void tabControlNotepad_MouseDown(object sender, MouseEventArgs e)
        {
            if (tabControlNotepad.SelectedIndex < 0)
                return;

            int i = tabControlNotepad.SelectedIndex;
            _dropData = new TabDropData {SourceIndex = i, DoingDragDrop = false, InitialPosition = e.Location};
        }

        private void tabControlNotepad_MouseUp(object sender, MouseEventArgs e)
        {
            _dropData = null;
        }

        private void tabControlNotepad_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dropData == null || e.Button != MouseButtons.Left) return;
            if (_dropData.DoingDragDrop || !_dropData.CheckValidHorizontalDistance(e.X)) return;
            _dropData.DoingDragDrop = true;
            DoDragDrop(_dropData, DragDropEffects.Move);
        }

        private void SwapTabs(int sourceIndex, int destinationIndex)
        {
            TabPageData tabData = _tabPageDataCollection.TabPageDictionary[sourceIndex];
            _tabPageDataCollection.TabPageDictionary[sourceIndex] = _tabPageDataCollection.TabPageDictionary[destinationIndex];
            _tabPageDataCollection.TabPageDictionary[destinationIndex] = tabData;

            _tabPageDataCollection.TabPageDictionary[sourceIndex].PageIndex = sourceIndex;
            _tabPageDataCollection.TabPageDictionary[destinationIndex].PageIndex = destinationIndex;

            _tabPageDataCollection.ActiveTabIndex = destinationIndex;
        }

        private class ApplicationState
        {
            public bool DatabaseLoaded { get; set; }
            public bool DatabaseExists { get; set; }
            public bool TabTextDataChanged { get; set; }
            public bool TabIndexChanged { get; set; }
            public bool TabPageAddOrRemove { get; set; }
            public bool UniqueIdMissingFromExistingTabPage { get; set; }
            public bool Initializing { get; set; }
        }

        private class TabDropData
        {
            private const int MinDistance = 10;
            public int SourceIndex { get; set; }
            public Point InitialPosition { get; set; }
            public bool DoingDragDrop { get; set; }

            public bool CheckValidHorizontalDistance(int x)
            {
                return x < InitialPosition.X - MinDistance || x > InitialPosition.X + MinDistance;
            }
        }

        #region Private Methods

        private void InitializeTabControls()
        {
            _applicationState.Initializing = true;
            if (!_memoStorageService.DatabaseExists())
            {
                _applicationState.DatabaseLoaded = false;
                _applicationState.DatabaseExists = false;
            }
            else
                _applicationState.DatabaseExists = true;

            // Create tab pages
            tabControlNotepad.TabPages.Clear();
            foreach (int tabPageIndex in _tabPageDataCollection.TabPageDictionary.Keys)
            {
                TabPageData tabPageData = _tabPageDataCollection.TabPageDictionary[tabPageIndex];
                var tabPageControl = new MemoTabPageControl("MemoTabPageControl", tabPageIndex) {Dock = DockStyle.Fill};
                var tabPage = new TabPage(tabPageData.TabPageLabel);
                tabPageControl.TabTextDataChanged += tabPageControl_TabTextDataChanged;

                tabPage.Controls.Add(tabPageControl);
                var richTextBox = ControlHelper.GetChildControlByName(tabPageControl, tabPageControl.TabPageControlTextboxName) as RichTextBox;

                if (richTextBox != null)
                {
                    SecureMemoFontSettings fontSettings = _appSettingsService.Settings.FontSettings;
                    richTextBox.Font = new Font(fontSettings.FontFamily, fontSettings.FontSize, fontSettings.Style);
                    richTextBox.Text = tabPageData.TabPageText;
                    richTextBox.ContextMenuStrip = contextMenuTextArea;
                    richTextBox.SelectionChanged += RichTextBox_SelectionChanged;
                }

                tabControlNotepad.TabPages.Add(tabPage);
            }

            tabControlNotepad.SelectedIndex = _tabPageDataCollection.ActiveTabIndex;
            _applicationState.Initializing = false;
            UpdateApplicationState();
        }

        private void RichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            var richTextBox = sender as RichTextBox;

            if (richTextBox == null) return;
            if (_tabSearchEngine != null && !_tabSearchEngine.SelectionSetByCode)
                _tabSearchEngine.ResetSearchState(_tabPageDataCollection.ActiveTabIndex, richTextBox.SelectionStart, richTextBox.SelectionLength);
        }

        private void SaveFormDataToModel()
        {
            _tabPageDataCollection.ActiveTabIndex = tabControlNotepad.SelectedIndex;
            _memoStorageService.SaveTabPageCollection(_tabPageDataCollection, _passwordStorage.Get(PwdKey));
            _applicationState.TabIndexChanged = false;
            _applicationState.TabTextDataChanged = false;

            UpdateApplicationState();
        }

        private string GetTextInTabControl(int tabIndex)
        {
            TabPage tabPage = tabControlNotepad.TabPages[tabIndex];
            var memoTabPageControl = tabPage.Controls[0] as MemoTabPageControl;
            if (memoTabPageControl != null)
            {
                var richTextBox = ControlHelper.GetChildControlByName(memoTabPageControl, memoTabPageControl.TabPageControlTextboxName) as RichTextBox;

                return richTextBox?.Text;
            }
            return null;
        }

        private void UpdateApplicationState()
        {
            openDatabaseToolStripMenuItem.Enabled = _applicationState.DatabaseExists;
            saveToSharedFolderToolStripMenuItem.Enabled = _applicationState.DatabaseLoaded;

            if (_applicationState.DatabaseLoaded)
            {
                saveToolStripMenuItem.Enabled = _applicationState.TabIndexChanged || _applicationState.TabTextDataChanged || _applicationState.TabPageAddOrRemove;

                if (_applicationState.UniqueIdMissingFromExistingTabPage)
                {
                    SaveFormDataToModel();
                    _applicationState.UniqueIdMissingFromExistingTabPage = false;
                }
            }
            else
                saveToolStripMenuItem.Enabled = false;

            tabsToolStripMenuItem.Enabled = _applicationState.DatabaseLoaded;
            changePasswordToolStripMenuItem.Enabled = _applicationState.DatabaseLoaded;

            BackupDatabasetoolStripMenuItem.Enabled = _applicationState.DatabaseExists;
            RestoreDatabasetoolStripMenuItem.Enabled = _applicationState.DatabaseExists;

            if (TopMost != _appSettingsService.Settings.AlwaysOntop)
                TopMost = _appSettingsService.Settings.AlwaysOntop;

            RestoreDatabaseFromSyncPathMenuItem.Enabled = _appSettingsService.Settings.UseSharedSyncFolder && !string.IsNullOrWhiteSpace(_appSettingsService.Settings.SyncFolderPath);
        }

        private void InitFormSettings()
        {
            _isResizingWindow = true;
            var screenSize = Screen.PrimaryScreen.Bounds;
            TopMost = _appSettingsService.Settings.AlwaysOntop;
            Width = GetSafeParameter(_appSettingsService.Settings.MainWindowWith, this.MinimumSize.Width, screenSize.Width);
            Height = GetSafeParameter(_appSettingsService.Settings.MainWindowHeight, MinimumSize.Height, screenSize.Height);

            // Center form
            Left = screenSize.Width / 2 - Width / 2;
            Top = screenSize.Height / 2 - Height / 2;
            _isResizingWindow = false;
        }

        private int GetSafeParameter(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }

            return value;
        }

        private bool PromptExit()
        {
            if (!_applicationState.DatabaseLoaded || !_applicationState.TabTextDataChanged) return true;
            return MessageBox.Show(this, "Are you sure you want to exit without saving?", "Exit without save?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
        }

        private void LoadLicenceFile()
        {
            try
            {
                if (File.Exists(LicenceFilename))
                    _licenceService.LoadLicenceFromFile(LicenceFilename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.FormMain__ErrorText);
            }
        }

        private Point GetCenterLocationForChildForm(Control childForm)
        {
            int x = Width/2 - childForm.Width/2 + Left;
            int y = Height/2 - childForm.Height/2 + Top;
            Rectangle activeScreenArea = Screen.GetBounds(new Point(x, y));

            if (y + childForm.Height > activeScreenArea.Bottom)
                y = Top - childForm.Height - 5;

            if (x + childForm.Width > activeScreenArea.Right)
                x = activeScreenArea.Right - childForm.Width - 5;

            return new Point(x, y);
        }

        #endregion

        #region Menu stip methods

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptExit()) return;
            _appSettingsService.SaveSettings();
            Application.Exit();
        }

        private void createNewDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(this, "Do you want to create a new Memo database? Doing so will overwrite any existing stored database under this account.", "Create new database?",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) !=
                DialogResult.OK)
                return;

            var frmsetPassword = new FormSetPassword {Text = "Choose password"};
            if (frmsetPassword.ShowDialog(this) != DialogResult.OK)
                return;

            string password = frmsetPassword.VerifiedPassword;

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show(this, "Password can not be empty!", Resources.FormMain__ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _passwordStorage.Set(PwdKey, password);
            _tabPageDataCollection = TabPageDataCollection.CreateNewPageDataCollection(_appSettingsService.Settings.DefaultEmptyTabPages);
            _memoStorageService.SaveTabPageCollection(_tabPageDataCollection, password);

            _appSettingsService.Settings.PasswordDerivedString =
                GeneralConverters.GeneratePasswordDerivedString(_appSettingsService.Settings.ApplicationSaltValue + password + _appSettingsService.Settings.ApplicationSaltValue);

            _appSettingsService.SaveSettings();
            InitializeTabControls();
            _applicationState.DatabaseLoaded = true;
            _applicationState.DatabaseExists = true;
            UpdateApplicationState();
        }

        private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formGetPassword = new FormGetPassword
            {
                PasswordSalt = _appSettingsService.Settings.ApplicationSaltValue,
                PasswordDerivedString = _appSettingsService.Settings.PasswordDerivedString
            };

            if (formGetPassword.ShowDialog(this) != DialogResult.OK) return;
            string password = formGetPassword.PasswordString;
            _passwordStorage.Set(PwdKey, password);

            TabPageDataCollection tabPageCollection;
            try
            {
                tabPageCollection = _memoStorageService.LoadTabPageCollection(password);

                // Make sure that every tabPageData has a unique Id
                bool uniqueIdCreated = tabPageCollection.TabPageDictionary.Values.Aggregate(false, (current, tabPageData) => current | tabPageData.GenerateUniqueIdIfNoneExists());

                if (uniqueIdCreated)
                    _applicationState.UniqueIdMissingFromExistingTabPage = true;

                if (tabPageCollection.TabPageDictionary.Count == 0)
                    tabPageCollection = TabPageDataCollection.CreateNewPageDataCollection(_appSettingsService.Settings.DefaultEmptyTabPages);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to load database, please verify that you entered the correct password. " + ex.Message, "Failed to load database", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            _tabPageDataCollection = tabPageCollection;
            InitializeTabControls();
            _applicationState.DatabaseLoaded = true;
            _applicationState.TabIndexChanged = false;
            _applicationState.TabPageAddOrRemove = false;
            _applicationState.TabTextDataChanged = false;
            UpdateApplicationState();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFormDataToModel();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmsetPassword = new FormSetPassword {Text = "Choose a new password"};
            if (frmsetPassword.ShowDialog(this) != DialogResult.OK) return;
            string password = frmsetPassword.VerifiedPassword;
            _passwordStorage.Set(PwdKey, password);
            _memoStorageService.SaveTabPageCollection(_tabPageDataCollection, password);

            _appSettingsService.Settings.PasswordDerivedString =
                GeneralConverters.GeneratePasswordDerivedString(_appSettingsService.Settings.ApplicationSaltValue + password + _appSettingsService.Settings.ApplicationSaltValue);

            _appSettingsService.SaveSettings();
            UpdateApplicationState();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new FormAbout();
            aboutForm.ShowDialog(this);
        }

        private void appendNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int tabPageIndex = _tabPageDataCollection.TabPageDictionary.Max(p => p.Key) + 1;
            var tabPageData = new TabPageData {PageIndex = tabPageIndex, TabPageLabel = "Page" + (tabPageIndex + 1)};
            tabPageData.GenerateUniqueIdIfNoneExists();
            _tabPageDataCollection.TabPageDictionary.Add(tabPageIndex, tabPageData);
            _applicationState.TabPageAddOrRemove = true;
            _tabPageDataCollection.ActiveTabIndex = tabPageData.PageIndex;
            InitializeTabControls();
        }

        private void tabWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmtabEdit = new FormTabEdit(_tabPageDataCollection);
            if (frmtabEdit.ShowDialog(this) != DialogResult.OK) return;
            if (frmtabEdit.TabDataChanged)
                _applicationState.TabPageAddOrRemove = true;

            InitializeTabControls();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmSettings = _scope.Resolve<FormSettings>();
            if (frmSettings.ShowDialog(this) == DialogResult.OK)
            {
                _appSettingsService.SaveSettings();
                InitializeTabControls();
            }
        }

        private void BackupDatabasetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _memoStorageService.MakeBackup();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.FormMain_Failed_to_backup_database, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(this, Resources.FormMain_Backup_completed_successfully, Resources.FormMain_Failed_to_backup_database, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RestoreDatabasetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmSelectBackup = _scope.Resolve<FormRestoreBackup>();
            frmSelectBackup.ShowDialog(this);
        }

        private void fileManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmfileManager = new FormFileManager();
            frmfileManager.ShowDialog(this);
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set Tab page object referrence to the serarch Service
            if (_formFind != null && !_formFind.IsDisposed)
            {
                _formFind.Show();
                _formFind.Location = GetCenterLocationForChildForm(_formFind);
                _formFind.Activate();
                return;
            }

            _formFind = new FormFind();
            _formFind.OnSearch += _formFind_OnSearch;
            _formFind.OnFormClose += _formFind_OnFormClose;
            _formFind.Show(this);
            _formFind.Location = GetCenterLocationForChildForm(_formFind);
        }

        private void _formFind_OnFormClose(object sender, EventArgs e)
        {
            if (_formFind == null) return;
            _formFind.OnSearch -= _formFind_OnSearch;
            _formFind.OnFormClose -= _formFind_OnFormClose;

            _formFind = null;
            _tabSearchEngine = null;
        }

        private void _formFind_OnSearch(object sender, TextSearchEventArgs eventArgs)
        {
            if (_tabSearchEngine == null)
                _tabSearchEngine = new TabSearchEngine(_tabPageDataCollection);

            try
            {
                var searchProperties = new TextSearchProperties
                {
                    CaseSensitive = eventArgs.SearchProperties.CaseSensitive,
                    LoopSearch = eventArgs.SearchProperties.LoopSearch,
                    SearchAllTabs = eventArgs.SearchProperties.SearchAllTabs,
                    SearchDirection = eventArgs.SearchProperties.SearchDirection,
                    SearchText = eventArgs.SearchProperties.SearchText
                };

                TextSearchResult searchResult = _tabSearchEngine.GetTextSearchResult(searchProperties);

                if (searchResult.SearchTextFound)
                {
                    _tabSearchEngine.SelectionSetByCode = true;
                    if (searchResult.TabIndex != _tabPageDataCollection.ActiveTabIndex && searchProperties.SearchAllTabs)
                    {
                        _tabPageDataCollection.ActiveTabIndex = searchResult.TabIndex;
                        tabControlNotepad.SelectedIndex = searchResult.TabIndex;
                    }

                    Focus();
                    RichTextBox textBox = GetRichTextBoxInActiveTab();
                    textBox.SelectionStart = searchResult.StartPos;
                    textBox.SelectionLength = searchResult.Length;
                    textBox.Focus();
                    _tabSearchEngine.SelectionSetByCode = false;
                }
                else
                {
                    MessageBox.Show(Resources.FormMain__Search_string_not_found, Resources.FormMain__Not_found, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RichTextBox textBox = GetRichTextBoxInActiveTab();
                    _tabSearchEngine.ResetSearchState(_tabPageDataCollection.ActiveTabIndex, textBox.SelectionStart, textBox.SelectionLength);
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Unhandled exception when calling _formFind_OnSearch()", ex);
                MessageBox.Show(ex.Message, Resources.FormMain__ErrorText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_tabPageDataCollection.TabPageDictionary.ContainsKey(_tabPageDataCollection.ActiveTabIndex))
                return;

            TabPageData tabPageData = _tabPageDataCollection.TabPageDictionary[_tabPageDataCollection.ActiveTabIndex];
            RichTextBox richTextBox = GetRichTextBoxInActiveTab();

            if (richTextBox != null && richTextBox.CanUndo)
                richTextBox.Undo();
            if (richTextBox != null) tabPageData.TabPageText = richTextBox.Text;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_tabPageDataCollection.TabPageDictionary.ContainsKey(_tabPageDataCollection.ActiveTabIndex))
                return;

            TabPageData tabPageData = _tabPageDataCollection.TabPageDictionary[_tabPageDataCollection.ActiveTabIndex];
            RichTextBox richTextBox = GetRichTextBoxInActiveTab();

            if (!string.IsNullOrWhiteSpace(richTextBox?.SelectedText))
            {
                richTextBox.Cut();
                tabPageData.TabPageText = richTextBox.Text;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_tabPageDataCollection.TabPageDictionary.ContainsKey(_tabPageDataCollection.ActiveTabIndex))
                return;

            RichTextBox richTextBox = GetRichTextBoxInActiveTab();
            if (!string.IsNullOrWhiteSpace(richTextBox?.SelectedText))
                richTextBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_tabPageDataCollection.TabPageDictionary.ContainsKey(_tabPageDataCollection.ActiveTabIndex))
                return;

            if (!Clipboard.ContainsText())
                return;

            TabPageData tabPageData = _tabPageDataCollection.TabPageDictionary[_tabPageDataCollection.ActiveTabIndex];
            RichTextBox richTextBox = GetRichTextBoxInActiveTab();

            if (richTextBox == null) return;
            richTextBox.Paste();
            tabPageData.TabPageText = richTextBox.Text;
        }

        private RichTextBox GetRichTextBoxInActiveTab()
        {
            var tabPageControl = tabControlNotepad.TabPages[_tabPageDataCollection.ActiveTabIndex].Controls[0] as MemoTabPageControl;

            if (tabPageControl == null)
                return null;

            var richTextBox = ControlHelper.GetChildControlByName(tabPageControl, tabPageControl.TabPageControlTextboxName) as RichTextBox;
            return richTextBox;
        }

        #endregion

        private void tabControlNotepad_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var p = new Point(e.X, e.Y);
            _tabPageClickIndex = -1;

            for (int i = 0; i < tabControlNotepad.TabCount; i++)
            {
                Rectangle tabRectangle = tabControlNotepad.GetTabRect(i);
                if (!tabRectangle.Contains(p)) continue;
                _tabPageClickIndex = i;
                break;
            }
            contextMenuEditTabPage.Show(tabControlNotepad, e.Location);
        }

        private void renameTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_tabPageClickIndex < 0 || _tabPageClickIndex > tabControlNotepad.TabCount)
            {
                LogWriter.LogMessage($"Rename tab page could not find selected index: {_tabPageClickIndex}", LogWriter.LogLevel.Warning);
                return;
            }
            var tabPage = tabControlNotepad.TabPages[_tabPageClickIndex];

            var renameTabControl = new RenameTabPageControl {TabPageName = tabPage.Text};
            var renameTabForm = FormFactory.CreateFormFromUserControl(renameTabControl);

            if (renameTabForm.ShowDialog(this) == DialogResult.OK)
            {
                string tabPageName = renameTabControl.TabPageName;
                _tabPageDataCollection.TabPageDictionary[_tabPageClickIndex].TabPageLabel = tabPageName;
                tabPage.Text = tabPageName;
                _applicationState.TabTextDataChanged = true;
            }
        }

        private void deleteTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_tabPageClickIndex < 0 || _tabPageClickIndex > tabControlNotepad.TabCount)
            {
                LogWriter.LogMessage($"Delete tab page could not find selected index: {_tabPageClickIndex}", LogWriter.LogLevel.Warning);
                return;
            }

            if (_tabPageDataCollection.TabPageDictionary.Count == 1)
            {
                MessageBox.Show(this, "You must have atleast one tab page active", "Could not delete tab page", MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show(this, "Are you sure you want to delete this tab page?", "Confirm delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (_tabPageDataCollection.ActiveTabIndex >= _tabPageDataCollection.TabPageDictionary.Count)
                {
                    _tabPageDataCollection.ActiveTabIndex--;
                }
                _tabPageDataCollection.TabPageDictionary.Remove(_tabPageClickIndex);
                _applicationState.TabPageAddOrRemove = true;
                InitializeTabControls();
            }
        }
    }
}