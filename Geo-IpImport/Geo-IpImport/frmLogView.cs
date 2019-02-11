using System;
using System.Text;
using System.Windows.Forms;

namespace Geo_IpImport
{
    public partial class frmLogView : Form
    {
        private readonly StringBuilder sbEventLogs = new StringBuilder();
        public frmLogView()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogView_Load(object sender, EventArgs e)
        {
            
        }

        public void AppendLogText(string logText)
        {
            sbEventLogs.Append(logText + "\r\n");
            txtEventLog.Text = sbEventLogs.ToString();
        }

        public void ClearLogText()
        {
            sbEventLogs.Clear();
            txtEventLog.Text = sbEventLogs.ToString();
        }
    }
}
