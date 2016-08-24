using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SecureChat.CustomControls
{
    public partial class NetworkStatusControl : UserControl
    {
        public NetworkStatusControl()
        {
            InitializeComponent();
        }
        private ConnectionStatus _connectionStatus;
        public Image _backgroundImage = null;

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), RefreshProperties(RefreshProperties.Repaint), EditorBrowsable(EditorBrowsableState.Always)]
        public ImageList StatusImageList { get; set; }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(0)]
        public int ConnectedImageIndex { get; set; }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(0)]
        public int DisconnectedImageIndex { get; set; }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(0)]
        public int PendingImageIndex { get; set; }

        [Browsable(false)]
        public bool Connected {
            get { return _connectionStatus == ConnectionStatus.Connected; }
        }

        public void SetConnectionStatus(ConnectionStatus connStatus)
        {
            if(StatusImageList == null || StatusImageList.Images.Count < 3)
                return;

            switch (connStatus)
            {
                case ConnectionStatus.None:
                    _backgroundImage = new Bitmap(32, 32);
                    break;
                case ConnectionStatus.Connected:
                    _backgroundImage = StatusImageList.Images[ConnectedImageIndex];
                    _connectionStatus = ConnectionStatus.Connected;
                    break;
                case ConnectionStatus.Disconnected:
                    _backgroundImage = StatusImageList.Images[DisconnectedImageIndex];
                    _connectionStatus = ConnectionStatus.Disconnected;
                    break;
                case ConnectionStatus.Pending:
                    _backgroundImage = StatusImageList.Images[PendingImageIndex];
                    _connectionStatus = ConnectionStatus.Disconnected;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("connStatus");
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_backgroundImage != null)
            {
                e.Graphics.DrawImage(_backgroundImage, 0, 0, this.Width, this.Height);
            }
            base.OnPaint(e);
        }

        public enum ConnectionStatus
        {
            None,
            Connected,
            Disconnected,
            Pending,
        }
    }
}
