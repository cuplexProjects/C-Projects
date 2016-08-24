using static Tetris.TetrisObjects;

namespace Tetris
{
    public interface ITetrisBlock
    {

        BlockRotations Rotation { get; }
        Blocks BlockType { get; }
        void RotateCW();
        void RotateCCW();
        int TopPos { get; set; }
        int LeftPos { get; set; }
    }
}
