using System;
using System.Windows.Forms;

namespace SearchForDuplicates
{
    public partial class FormLogWindow : Form
    {
        
        public FormLogWindow()
        {
            InitializeComponent();
        }

        private void LogWindow_Load(object sender, EventArgs e)
        {
            LoadLogs();
        }
        public void LoadLogs()
        {
        
        }

        private void lstLogs_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}