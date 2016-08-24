using System.Windows.Forms;

namespace ImageView
{
    public partial class FormLogWindow : Form
    {
        public FormLogWindow()
        {
            InitializeComponent();
        }

        public void SetLogText(string text)
        {
            logTextBox.Text = text;
        }
    }
}