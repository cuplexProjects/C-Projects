using System;
using System.Windows.Forms;
using DatabaseImport.Delegates;

namespace DatabaseImport.Forms
{
    public partial class frmDataGrid : Form
    {
        private readonly DataGridView dgvTableStructure;
        private readonly DataGridViewContainer dataGridViewContainerCallback;
        public frmDataGrid(DataGridViewContainer dgvCallback, DataGridView dgv)
        {
            InitializeComponent();
            dataGridViewContainerCallback = dgvCallback;
            dgvTableStructure = dgv;
        }

        private void frmDataGrid_Load(object sender, EventArgs e)
        {
            this.panelDgvContainer.Controls.Clear();
            this.panelDgvContainer.Controls.Add(dgvTableStructure);
        }

        private void frmDataGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataGridViewContainerCallback.Invoke(dgvTableStructure);
        }
    }
}
