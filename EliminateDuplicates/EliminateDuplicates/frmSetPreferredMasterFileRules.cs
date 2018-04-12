#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DeleteDuplicateFiles.Models;

#endregion

namespace DeleteDuplicateFiles
{
    public partial class FrmSetPreferredMasterFileRules : Form
    {
        private readonly BindingList<PreferredDirectory> _preferredDirectoryList;

        public FrmSetPreferredMasterFileRules()
        {
            InitializeComponent();
            _preferredDirectoryList = new BindingList<PreferredDirectory>();
            lstPerferedDirectories.DataSource = _preferredDirectoryList;
        }

        public void SetPreferredDirectories(IEnumerable<PreferredDirectory> preferredDirectories)
        {
            _preferredDirectoryList.Clear();
            foreach (var preferredDirectory in preferredDirectories)
            {
                _preferredDirectoryList.Add(preferredDirectory);
            }
        }

        public List<PreferredDirectory> GetPreferredDirectories()
        {
            return _preferredDirectoryList.ToList();
        }

        private void UpdateSortOrderForListItems()
        {
            for (int i = 0; i < _preferredDirectoryList.Count; i++)
            {
                _preferredDirectoryList[i].SortOrder = i;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _preferredDirectoryList.Add(new PreferredDirectory
                {
                    Path = folderBrowserDialog1.SelectedPath,
                    SortOrder = _preferredDirectoryList.Count
                });
                lstPerferedDirectories.SelectedItem = _preferredDirectoryList[_preferredDirectoryList.Count - 1];
                txtEditPath.Text = "";
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string path = txtEditPath.Text;
            if (Directory.Exists(path))
            {
                if (_preferredDirectoryList.Any(pd => pd.Path == path))
                {
                    MessageBox.Show("Can not add already existing directory");
                    return;
                }
                int sortOrder = _preferredDirectoryList.Count;
                _preferredDirectoryList.Add(new PreferredDirectory {Path = path, SortOrder = sortOrder});
                lstPerferedDirectories.SelectedItem = _preferredDirectoryList[sortOrder];
                txtEditPath.Text = "";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectory directory = lstPerferedDirectories.SelectedItem as PreferredDirectory;
                if (directory != null)
                {
                    _preferredDirectoryList.Remove(directory);
                    UpdateSortOrderForListItems();
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (_preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectory directory = lstPerferedDirectories.SelectedItem as PreferredDirectory;
                if (directory != _preferredDirectoryList.Last())
                {
                    _preferredDirectoryList[directory.SortOrder] = _preferredDirectoryList[directory.SortOrder + 1];
                    _preferredDirectoryList[directory.SortOrder + 1] = directory;

                    UpdateSortOrderForListItems();
                    lstPerferedDirectories.SelectedItem = directory;
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (_preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectory directory = lstPerferedDirectories.SelectedItem as PreferredDirectory;
                if (directory != _preferredDirectoryList.First())
                {
                    _preferredDirectoryList[directory.SortOrder] = _preferredDirectoryList[directory.SortOrder - 1];
                    _preferredDirectoryList[directory.SortOrder - 1] = directory;

                    UpdateSortOrderForListItems();
                    lstPerferedDirectories.SelectedItem = directory;
                }
            }
        }
    }
}