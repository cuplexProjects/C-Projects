using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageView.Services;

namespace ImageView
{
    public partial class FormThumbnailSettings : Form
    {
        private readonly ThumbnailService _thumbnailService;
        public FormThumbnailSettings(ThumbnailService thumbnailService)
        {
            _thumbnailService = thumbnailService;
            InitializeComponent();
        }

        private void FormThumbnailSettings_Load(object sender, EventArgs e)
        {

        }
    }
}
