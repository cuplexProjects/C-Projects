using System;
using System.Drawing;
using System.Threading.Tasks;
using GeneralToolkitLib.Log;
using ImageView.Managers;
using ImageView.Models;

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

        public string BasePath => _basePath;

        public void ScanDirectory(string path)
        {
            _thumbnailManager.StartThumbnailScan(path, null);
            _thumbnailManager.SaveThumbnailDatabase();
        }

        public async void ScanDirectoryAsync(string path, IProgress<ThumbnailScanProgress> progress)
        {
            try
            {
                await Task.Run(() =>
                {
                    _thumbnailManager.StartThumbnailScan(path, progress);
                    _thumbnailManager.SaveThumbnailDatabase();
                });

            }
            catch (Exception ex)
            {
                _thumbnailManager.CloseFileHandle();
                LogWriter.LogError("Exception in ScanDirectoryAsync()", ex);
            }
        }

        public void StopThumbnailScan()
        {
            _thumbnailManager.StopThumbnailScan();
        }

        public bool SaveThumbnailDatabase()
        {
            return _thumbnailManager.SaveThumbnailDatabase();
        }

        public bool LoadThumbnailDatabase()
        {
            return _thumbnailManager.LoadThumbnailDatabase();
        }

        public int GetNumberOfCachedThumbnails()
        {
            return _thumbnailManager.GetNumberOfCachedThumbnails();
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