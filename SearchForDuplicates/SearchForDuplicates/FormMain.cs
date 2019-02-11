using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SearchForDuplicates
{
    public partial class FormMain : Form
    {
        private FormChecksum formChecksum;
        private FormAbout formAbout;

        public FormMain()
        {
            InitializeComponent();
        }

        #region Form Events

        private void frmMain_Load(object sender, EventArgs e)
        {

           
        }

        private void frmMain_Disposed(object sender, EventArgs e)
        {

        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        #region ButtonEvents


        private void btnAbout_Click(object sender, EventArgs e)
        {
            if(formAbout == null || formAbout.IsDisposed)
            {
                formAbout=new FormAbout();
                formAbout.Show(this);
            }
            else
            {
                formAbout.Focus();
            }
        }

        private void btnChecksumUtil_Click(object sender, EventArgs e)
        {
            if (formChecksum == null || formChecksum.IsDisposed)
            {
                formChecksum = new FormChecksum();
                formChecksum.Show(this);
            }
            else
            {
                formChecksum.Focus();
            } 

        }
        private void btnStop_Click(object sender, EventArgs e)
        {

        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK && ValidateDirectory(folderBrowserDialog1.SelectedPath))                
                    lstDirectories.Items.Add(folderBrowserDialog1.SelectedPath);
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (lstDirectories.Items.Count == 0)
            {
                MessageBox.Show("Please enter atleast one directory to search.", "Search for duplicates");
                return;
            }
            if (chkFilenameFilter.Checked && txtFilenameFilter.Text.Length == 0)
            {
                MessageBox.Show("Please enter atleast one filetype or disable the checkbox.", "Search for duplicates");
                return;
            }
            
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            lstDuplicates.Items.Clear();
            lstFileLocations.Items.Clear();
            lblDuplicateCount.Text = "";
            
        }
        private void btnClearDirectories_Click(object sender, EventArgs e)
        {
            lstDirectories.Items.Clear();
        }

        private void btnRemoveDirectory_Click(object sender, EventArgs e)
        {
            if (lstDirectories.SelectedItem != null)
                lstDirectories.Items.Remove(lstDirectories.SelectedItem);
        }
        #endregion
        
        private void lstDuplicates_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public void lstDirectories_DragDrop(object sender, DragEventArgs e)
        {

        }
        public void lstDirectories_DragEnter(object sender, DragEventArgs e)
        {

        }
        private void chkFilenameFilter_CheckedChanged(object sender, EventArgs e)
        {
            txtFilenameFilter.Enabled = chkFilenameFilter.Checked;
        }
        private void lstFileLocations_MouseUp(object sender, MouseEventArgs e)
        {
            if (lstFileLocations.SelectedItem == null)
                return;
            if (e.Button == MouseButtons.Right)
            {
                Point pos = new Point(e.X, e.Y);
                //popupMenuFileLocations.Show(lstFileLocations, pos);
            }
        }
        
        public void lstDuplicates_MouseHover(object sender, EventArgs e)
        {

        }
        #endregion
        


        //Validates that the path is a directory and that it is unique.
        //Used when adding new directories to lstDirectories.
        private bool ValidateDirectory(string path)
        {

        }

        private void chkAllwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkAllwaysOnTop.Checked;
        }
    }
}