using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using FileSystemImage.FileSystem;
using FileSystemImage.FileTree;

namespace FileSystemImage
{
    public partial class frmSearch : Form
    {
        private FileSystemDrive fileSystemDrive;

        public frmSearch()
        {
            this.InitializeComponent();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
        }

        public void SetFileSystemDrive(FileSystemDrive fsd)
        {
            this.fileSystemDrive = fsd;
        }

        private void frmSearch_Resize(object sender, EventArgs e)
        {
            const int padding = 20;
            const int innerPadding = 6;
            this.grpSearch.Width = Width - padding*2;
            this.lstResults.Width = this.grpSearch.Width;
            this.lstResults.Height = Height - this.lstResults.Top - this.statusStrip1.Height - padding*2;

            this.txtSearch.Width = this.txtSearch.Parent.Width - innerPadding*2;
            this.btnSearch.Left = this.btnSearch.Parent.Width - this.btnSearch.Width - innerPadding;
            this.chkRegexp.Left = this.chkRegexp.Parent.Width - this.chkRegexp.Width - this.btnSearch.Width - innerPadding*2;
            this.chkIgnoreCase.Left = this.chkRegexp.Left;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.PerformSearch();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                this.PerformSearch();
        }

        private void PerformSearch()
        {
            if(this.txtSearch.Text == "")
                return;

            TreeSearch searchAlg = new TreeSearch(this.fileSystemDrive);
            bool truncate = false;

            try
            {
                List<TreeSearchResult> searchRes = searchAlg.Search(this.txtSearch.Text, this.chkRegexp.Checked, this.chkIgnoreCase.Checked);
                this.toolStripSearchResCount.Text = searchRes.Count.ToString();

                if(searchRes.Count > 5000)
                {
                    truncate = true;
                    searchRes = searchRes.Take(5000).ToList();
                }

                this.lstResults.Items.Clear();
                foreach (TreeSearchResult res in searchRes)
                    this.lstResults.Items.Add(res);

                if(truncate)
                    MessageBox.Show("The Search result has been truncated to 5000 results");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lstResults_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                TreeSearchResult item = this.lstResults.SelectedItem as TreeSearchResult;
                if(item != null)
                {
                    try
                    {
                        string p = item.path + "\\" + item.file.Name;
                        string args = string.Format("/e, /select, \"{0}\"", p);

                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = "explorer";
                        info.Arguments = args;
                        Process.Start(info);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}