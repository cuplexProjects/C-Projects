using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Models;

namespace DeleteDuplicateFiles
{
    public partial class FrmSetPreferredMasterFileRules : Form
    {
        private readonly BindingList<PreferredDirectoryDataModel> _preferredDirectoryList;

        public FrmSetPreferredMasterFileRules()
        {
            InitializeComponent();
            _preferredDirectoryList = new BindingList<PreferredDirectoryDataModel>();
            lstPerferedDirectories.DataSource = _preferredDirectoryList;
        }

        public void SetPreferredDirectories(IEnumerable<PreferredDirectoryDataModel> preferredDirectories)
        {
            _preferredDirectoryList.Clear();
            foreach (var preferredDirectory in preferredDirectories)
            {
                _preferredDirectoryList.Add(preferredDirectory);
            }
        }

        public List<PreferredDirectoryDataModel> GetPreferredDirectories()
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
                _preferredDirectoryList.Add(new PreferredDirectoryDataModel
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
                _preferredDirectoryList.Add(new PreferredDirectoryDataModel {Path = path, SortOrder = sortOrder});
                lstPerferedDirectories.SelectedItem = _preferredDirectoryList[sortOrder];
                txtEditPath.Text = "";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectoryDataModel directoryDataModel = lstPerferedDirectories.SelectedItem as PreferredDirectoryDataModel;
                if (directoryDataModel != null)
                {
                    _preferredDirectoryList.Remove(directoryDataModel);
                    UpdateSortOrderForListItems();
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (_preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectoryDataModel directoryDataModel = lstPerferedDirectories.SelectedItem as PreferredDirectoryDataModel;
                if (directoryDataModel != _preferredDirectoryList.Last())
                {
                    _preferredDirectoryList[directoryDataModel.SortOrder] = _preferredDirectoryList[directoryDataModel.SortOrder + 1];
                    _preferredDirectoryList[directoryDataModel.SortOrder + 1] = directoryDataModel;

                    UpdateSortOrderForListItems();
                    lstPerferedDirectories.SelectedItem = directoryDataModel;
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (_preferredDirectoryList.Count > 0 && lstPerferedDirectories.SelectedItem != null)
            {
                PreferredDirectoryDataModel directoryDataModel = lstPerferedDirectories.SelectedItem as PreferredDirectoryDataModel;
                if (directoryDataModel != _preferredDirectoryList.First())
                {
                    _preferredDirectoryList[directoryDataModel.SortOrder] = _preferredDirectoryList[directoryDataModel.SortOrder - 1];
                    _preferredDirectoryList[directoryDataModel.SortOrder - 1] = directoryDataModel;

                    UpdateSortOrderForListItems();
                    lstPerferedDirectories.SelectedItem = directoryDataModel;
                }
            }
        }
    }
}