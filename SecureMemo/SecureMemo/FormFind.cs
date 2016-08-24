using System;
using System.Windows.Forms;
using SecureMemo.TextSearchModels;

namespace SecureMemo
{
    public partial class FormFind : Form
    {
        private bool _searchInAllTabs;
        private TextSearchProperties _textSearchProperties;

        public FormFind()
        {
            InitializeComponent();
        }

        public event TextSearchEvents.TextSearchEventHandler OnSearch;
        public event EventHandler OnFormClose;

        private void FormFind_Load(object sender, EventArgs e)
        {
            _textSearchProperties = new TextSearchProperties();
            UpdateControlState();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormFind_Enter(object sender, EventArgs e)
        {
        }

        private void FormFind_Activated(object sender, EventArgs e)
        {
            txtFindText.Focus();
        }

        private void txtFindText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                PerformSearch();
        }

        private void PerformSearch()
        {
            if (txtFindText.Text.Length > 0)
            {
                _textSearchProperties.SearchText = txtFindText.Text;
                UpdateControlState();
                OnSearch?.Invoke(this, new TextSearchEventArgs(_searchInAllTabs, _textSearchProperties));
            }
        }

        private void FormFind_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void UpdateControlState()
        {
            _searchInAllTabs = chkSearchAllTabs.Checked;
            chkLoopSearch.Enabled = !chkSearchAllTabs.Checked;
            _textSearchProperties.CaseSensitive = chkCaseSensitive.Checked;
            _textSearchProperties.SearchDirection = rdButtonUp.Checked ? TextSearchEvents.SearchDirection.Up : TextSearchEvents.SearchDirection.Down;
            _textSearchProperties.SearchAllTabs = chkSearchAllTabs.Checked;
            Text = _searchInAllTabs ? "Find in all Tabs" : "Find";
        }

        private void chkSearchAllTabs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlState();
        }

        private void txtFindText_TextChanged(object sender, EventArgs e)
        {
            UpdateControlState();
        }

        private void rdButtonDirection_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlState();
        }

        private void chkCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            _textSearchProperties.CaseSensitive = chkCaseSensitive.Checked;
        }

        private void FormFind_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnFormClose?.Invoke(this, new EventArgs());
        }
    }
}