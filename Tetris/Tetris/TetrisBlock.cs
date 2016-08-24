using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class TetrisBlock : ITetrisBlock
    {
        private readonly TetrisObjects.Blocks _blockType;

        public TetrisBlock(TetrisObjects.Blocks blockType)
        {
            _blockType = blockType;
        }

        public TetrisObjects.BlockRotations Rotation { get; }
        public TetrisObjects.Blocks BlockType { get; }

        public void RotateCW()
        {
            switch (_blockType)
            {
                case TetrisObjects.Blocks.None:
                    break;
                case TetrisObjects.Blocks.I:
                    break;
                case TetrisObjects.Blocks.J:
                    break;
                case TetrisObjects.Blocks.K:
                    break;
                case TetrisObjects.Blocks.L:
                    break;
                case TetrisObjects.Blocks.M:
                    break;
                case TetrisObjects.Blocks.N:
                    break;
                case TetrisObjects.Blocks.O:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RotateCCW()
        {
            switch (_blockType)
            {
                case TetrisObjects.Blocks.None:
                    break;
                case TetrisObjects.Blocks.I:
                    break;
                case TetrisObjects.Blocks.J:
                    break;
                case TetrisObjects.Blocks.K:
                    break;
                case TetrisObjects.Blocks.L:
                    break;
                case TetrisObjects.Blocks.M:
                    break;
                case TetrisObjects.Blocks.N:
                    break;
                case TetrisObjects.Blocks.O:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int TopPos { get; set; }
        public int LeftPos { get; set; }
    }

}
