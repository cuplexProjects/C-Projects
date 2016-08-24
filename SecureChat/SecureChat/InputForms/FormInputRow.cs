using System;
using System.Windows.Forms;

namespace SecureChat.InputForms
{
    public partial class FormInputRow : Form
    {
        private readonly InputFormData _inputFormData;
        public string UserInputText { get; set; }

        public FormInputRow(InputFormData inputFormData)
        {
            _inputFormData = inputFormData;
            InitializeComponent();
        }

        private void FormInputRowData_Load(object sender, EventArgs e)
        {
            this.Text = _inputFormData.WindowText;
            this.groupBoxMain.Text = _inputFormData.GroupBoxText;
            this.labelInput.Text = _inputFormData.LabelText;
        }

        private void FormInputRow_Shown(object sender, EventArgs e)
        {
            txtInput.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UserInputText = txtInput.Text;
            this.Close();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public sealed class InputFormData
        {
            public string WindowText { get; set; }
            public string GroupBoxText { get; set; }
            public string LabelText { get; set; }
        }
    }
}