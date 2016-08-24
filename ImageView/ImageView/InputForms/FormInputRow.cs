using System;
using System.Windows.Forms;

namespace ImageView.InputForms
{
    public partial class FormInputRow : Form
    {
        private readonly InputFormData _inputFormData;

        public FormInputRow(InputFormData inputFormData)
        {
            _inputFormData = inputFormData;
            InitializeComponent();
        }

        public string UserInputText { get; set; }

        private void FormInputRowData_Load(object sender, EventArgs e)
        {
            Text = _inputFormData.WindowText;
            groupBoxMain.Text = _inputFormData.GroupBoxText;
            labelInput.Text = _inputFormData.LabelText;
        }

        private void FormInputRow_Shown(object sender, EventArgs e)
        {
            txtInput.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UserInputText = txtInput.Text;
            Close();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public sealed class InputFormData
        {
            public string WindowText { get; set; }
            public string GroupBoxText { get; set; }
            public string LabelText { get; set; }
        }
    }
}