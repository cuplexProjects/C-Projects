using System;
using System.Drawing;
using System.Threading.Tasks;
using ImageView.Managers;

namespace ImageView.Services
{
    public class ThumbnailService : IDisposable
    {
        private readonly string _basePath;
        private readonly ThumbnailManager _thumbnailManager;

        public ThumbnailService(string databaseDirectory)
        {
            _basePath = databaseDirectory;
            _thumbnailManager = ThumbnailManager.CreateNew(_basePath);
        }

        public void ScanDirectory(string path)
        {
            _thumbnailManager.StartThumbnailScan(path);
            _thumbnailManager.SaveThumbnailDatabase();
        }

        public async void ScanDirectoryAsync(string path)
        {
            await Task.Run(() =>
            {
                _thumbnailManager.StartThumbnailScan(path);
                _thumbnailManager.SaveThumbnailDatabase();
            });
        }

        public Image GetThumbnail(string filename)
        {
            return _thumbnailManager.GetThumbnail(filename);
        }

        public void Dispose()
        {
            _thumbnailManager.Dispose();
        }
    }
}