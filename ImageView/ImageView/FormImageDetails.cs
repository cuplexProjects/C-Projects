using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageView
{
    public partial class FormImageDetails : Form
    {
        private readonly string _filename;
        public FormImageDetails(string filename)
        {
            _filename = filename;
            InitializeComponent();
        }

        private void FormImageDetails_Load(object sender, EventArgs e)
        {
            ImgInfoGroupBox.Text = $"Image information about {_filename}";

        }
    }
}
