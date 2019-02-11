using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SearchForDuplicates
{
    public partial class FormFileInformation : Form
    {
        private List<FileEntryExtended> _fileEntryList = null;
        private bool leftMouseDown = false;
        private Point mouseDownPoint;
        public FormFileInformation()
        {            
            InitializeComponent();            
        }
        private void FileInformation_Load(object sender, EventArgs e)
        {
            if (FileEntryList==null)
                return;
            for (int i=0; i<FileEntryList.Count; i++)
                lstFiles.Items.Add(FileEntryList[i].FileName);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected void grpFileInformation_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                this.Cursor = Cursors.NoMove2D;
                mouseDownPoint = new Point(e.X, e.Y);                
                leftMouseDown = true;                
            }
            
        }
        protected void grpFileInformation_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseDown)
            {
                this.Left = MousePosition.X - mouseDownPoint.X;
                this.Top = MousePosition.Y - mouseDownPoint.Y;
            }            
        }
        protected void grpFileInformation_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                leftMouseDown = false;
                this.Cursor = Cursors.Arrow;
            }
        }
        public List<FileEntryExtended> FileEntryList
        {
            get { return _fileEntryList; }
            set { _fileEntryList = value; }
        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFileInformation(lstFiles.SelectedIndex);
        }
        private void updateFileInformation(int index)
        {
            if (FileEntryList!=null && FileEntryList.Count>index)
            {
                FileEntryExtended entry = FileEntryList[index];
                if (!entry.IsUpToDate)
                    entry.Update();
                txtFileName.Text = entry.FileName;
                txtFilePath.Text = entry.FilePath;
                txtCreatedDate.Text = entry.CreationTime.ToString("yyyy-MM-dd - hh:mm:ss");
                txtModifiedDate.Text = entry.LastWriteTime.ToString("yyyy-MM-dd - hh:mm:ss");
                txtFileSize.Text = entry.FileSizeStr;

                if ((entry.Attributes & FileAttributes.Compressed)>0)
                    chkCompressed.CheckState = CheckState.Checked;
                else
                    chkCompressed.CheckState = CheckState.Unchecked;

                if ((entry.Attributes & FileAttributes.Hidden)>0)
                    chkHidden.CheckState = CheckState.Checked;
                else
                    chkHidden.CheckState = CheckState.Unchecked;

                if ((entry.Attributes & FileAttributes.System)>0)
                    chkSystem.CheckState = CheckState.Checked;
                else
                    chkSystem.CheckState = CheckState.Unchecked;

                if ((entry.Attributes & FileAttributes.ReadOnly)>0)
                    chkWriteProtected.CheckState = CheckState.Checked;
                else
                    chkWriteProtected.CheckState = CheckState.Unchecked;
            }
        }
    }
}