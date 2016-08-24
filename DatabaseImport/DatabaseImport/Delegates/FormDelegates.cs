using System.Collections.Generic;
using System.Windows.Forms;
using DatabaseImport.Models;

namespace DatabaseImport.Delegates
{
    public delegate void DataGridViewContainer(DataGridView dgv);
    public delegate void ImportSettingsContainer(List<DatabaseColumnCoupling> columnCouplings);
    public delegate void ProgressCallback(int percentComplete);
}
