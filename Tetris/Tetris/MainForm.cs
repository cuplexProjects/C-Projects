using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
        }

        private TetrisGameEngine tetrisGameEngine;
        private void MainForm_Load(object sender, EventArgs e)
        {
            tetrisGameEngine = new TetrisGameEngine();
            tetrisGameEngine.Start();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    tetrisGameEngine.MoveLeft();
                    break;
                case Keys.Right:
                    tetrisGameEngine.MoveRight();
                    break;
                case Keys.Up:
                    tetrisGameEngine.RotateCW();
                    break;
                case Keys.Down:
                    tetrisGameEngine.RotateCCW();
                    break;
                case Keys.Space:
                    tetrisGameEngine.DropBlock();
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            tetrisGameEngine.OnPaint(e);
        }
    }
}
