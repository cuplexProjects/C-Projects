using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DatabaseImport.Delegates;
using DatabaseImport.Models;

namespace DatabaseImport.Forms
{
    public partial class frmImportSettings : Form
    {
        private readonly List<SQLColumnStructure> sqlColumnStructures;
        private readonly ImportSettingsContainer importSettingsCallback;
        private List<DatabaseColumnCoupling> columnCouplings;

        public frmImportSettings(ImportSettingsContainer settingsCallback, List<SQLColumnStructure> columnStructures)
        {
            sqlColumnStructures = columnStructures;
            importSettingsCallback = settingsCallback;
            InitializeComponent();
        }

        private void frmImportSettings_Load(object sender, EventArgs e)
        {
            columnCouplings = new List<DatabaseColumnCoupling>();
            foreach (SQLColumnStructure columnStructure in sqlColumnStructures)
            {
                if (!columnStructure.ReadOnly)
                {
                    columnCouplings.Add(new DatabaseColumnCoupling {Name = columnStructure.ColumnName, Value = ""});
                }
            }

            dgvImportDataCoupling.DataSource = columnCouplings;
        }

        private void frmImportSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            importSettingsCallback.Invoke(columnCouplings);
        }
    }
}
