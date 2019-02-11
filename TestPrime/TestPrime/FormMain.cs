using System;
using System.Numerics;
using System.Windows.Forms;

namespace TestPrime
{
    public partial class FormMain : Form
    {
        //Just basic number converter for now but will implement Integer factorisation later...
        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnTestConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBase64Input.Text))
                return;

            try
            {
                byte[] b64Bytes = Convert.FromBase64String(txtBase64Input.Text);
                lblInLength.Text = b64Bytes.Length * 8 + " bit";
                BigInteger bigInteger = new BigInteger(b64Bytes);
                txtPrimeNumberOutput.Text = bigInteger.ToString();
                lblOutLength.Text = txtPrimeNumberOutput.Text.Length + " Digit prime number";
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPrimeNumberOutput.Text = "";
            txtBase64Input.Text = "";

            lblInLength.Text = "0";
            lblOutLength.Text = "0";
        }
    }
}
