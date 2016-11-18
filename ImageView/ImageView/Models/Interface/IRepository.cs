using System.Drawing;

namespace ImageView.Models.Interface
{
    public interface IRepository
    {
        bool IsLocked { get; }
        Image ReadImage(int position, int length);
        FileEntry WriteImage(Image img);
        void SaveToDisk();
        void CloseStream();
        bool LockDatabase();
        void UnlockDatabase();
    }
}