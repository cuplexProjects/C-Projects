using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public class TetrisGameEngine : GameEngine
    {
        public int columns = 10;
        public int rows = 22;

        // In pixels
        public const int SizeOfBlockMatrixUnit = 10;
        private List<ITetrisBlock> blocks;

        public TetrisGameEngine()
        {
            blocks= new List<ITetrisBlock>();
        }

        public void DropBlock()
        {
            
        }

        public override void NextMovement()
        {
            throw new NotImplementedException();
        }

        public override void MoveLeft()
        {
            throw new NotImplementedException();
        }

        public override void MoveRight()
        {
            throw new NotImplementedException();
        }

        public override void RotateCW()
        {
            throw new NotImplementedException();
        }

        public override void RotateCCW()
        {
            throw new NotImplementedException();
        }

        public override void OnPaint(PaintEventArgs e)
        {
            
        }


    }

}
