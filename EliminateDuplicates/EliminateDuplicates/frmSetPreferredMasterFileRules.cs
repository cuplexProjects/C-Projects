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
    public partial class frmSetPreferredMasterFileRules : Form
    {
        private readonly BindingList<PreferredDirectory> preferredDirectoryList;

        public frmSetPreferredMasterFileRules()
        {
            InitializeComponent();
            preferredDirectoryList = new BindingList<PreferredDirectory>();
            lstPerferedDirectories.DataSource = preferredDirectoryList;
        }

        public void SetPreferredDirectories(IEnumerable<PreferredDirectory> preferredDirectories)
        {
            preferredDirectoryList.Clear();
            foreach (var preferredDirectory in preferredDirectories)
            {
                preferredDirectoryList.Add(preferredDirectory);
            }
        }

        public List<PreferredDirectory> GetPreferredDirectories()
        {
            return preferredDirectoryList.ToList();
        }

        private void UpdateSortOrderForListItems()
        {
            for (int i = 0; i < preferredDirectoryList.Count; i++)
            {
                preferredDirectoryList[i].SortOrder = i;
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
                preferredDirectoryList.Add(new PreferredDirectory
                {
                    Path = folderBrowserDialog1.SelectedPath,
                    SortOrder = preferredDirectoryList.Count
                });
                lstPerferedDirectories.SelectedItem = preferredDirectoryList[preferredDirectoryList.Count - 1];
                txtEditPath.Text = "";
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string path = txtEditPath.Text;
            if (Directory.Exists(path))
            {
                if (preferredDirectoryList.Any(pd => pd.Path == path))
                {
                    MessageBox.Show("Can not add already existing directory");
                    return;
                }
                int sortOrder = preferredDirectoryList.Count;
                preferredDirectoryList.Add(new PreferredDirectory {Path = path, SortOrder = sortOrder});
                lstPerferedDirectories.SelectedItem = preferredDirectoryList[sortOrder];
                txtEditPath.Text = "";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectory directory = lstPerferedDirectories.SelectedItem as PreferredDirectory;
                if (directory != null)
                {
                    preferredDirectoryList.Remove(directory);
                    UpdateSortOrderForListItems();
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectory directory = lstPerferedDirectories.SelectedItem as PreferredDirectory;
                if (directory != preferredDirectoryList.Last())
                {
                    preferredDirectoryList[directory.SortOrder] = preferredDirectoryList[directory.SortOrder + 1];
                    preferredDirectoryList[directory.SortOrder + 1] = directory;

                    UpdateSortOrderForListItems();
                    lstPerferedDirectories.SelectedItem = directory;
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectory directory = lstPerferedDirectories.SelectedItem as PreferredDirectory;
                if (directory != preferredDirectoryList.First())
                {
                    preferredDirectoryList[directory.SortOrder] = preferredDirectoryList[directory.SortOrder - 1];
                    preferredDirectoryList[directory.SortOrder - 1] = directory;

                    UpdateSortOrderForListItems();
                    lstPerferedDirectories.SelectedItem = directory;
                }
            }
        }
    }
}