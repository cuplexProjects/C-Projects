using System;
using System.Windows.Forms;

namespace ImageView.UserControls
{
    public partial class ThumbnailSettings : UserControl
    {
        public ThumbnailSettings()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Form parentForm = ParentForm;
            if (parentForm == null)
                return;

            parentForm.DialogResult = DialogResult.OK;
            parentForm.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form parentForm = ParentForm;
            if (parentForm == null)
                return;

            parentForm.DialogResult = DialogResult.Cancel;
            parentForm.Close();
        }
    }
}
