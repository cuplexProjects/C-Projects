using System;
using System.Drawing;
using System.Threading.Tasks;
using ImageView.Configuration;
using ImageView.Managers;
using ImageView.Models;
using Serilog;

namespace ImageView.Services
{
    public class ThumbnailService : ServiceBase, IDisposable
    {
        private readonly ThumbnailManager _thumbnailManager;

        public ThumbnailService(ThumbnailManager thumbnailManager)
        {
            _thumbnailManager = thumbnailManager;
            string databaseDirectory = ApplicationBuildConfig.UserDataPath;
            BasePath = databaseDirectory;
        }

        public string BasePath { get; private set; }

        public void Dispose()
        {
            _thumbnailManager.Dispose();
        }

        public void ScanDirectory(string path, bool scanSubdirectories)
        {
            _thumbnailManager.StartThumbnailScan(path, null, scanSubdirectories);
            _thumbnailManager.SaveThumbnailDatabase();
        }

        public async void ScanDirectoryAsync(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            try
            {
                await Task.Run(() =>
                {
                    _thumbnailManager.StartThumbnailScan(path, progress, scanSubdirectories);
                    _thumbnailManager.SaveThumbnailDatabase();
                });
            }
            catch (Exception ex)
            {
                _thumbnailManager.CloseFileHandle();
                Log.Error(ex, "Exception in ScanDirectoryAsync()");
            }
        }

        public void StopThumbnailScan()
        {
            _thumbnailManager.StopThumbnailScan();
        }

        public void OptimizeDatabase()
        {
            _thumbnailManager.OptimizeDatabase();
        }

        public async void OptimizeDatabaseAsync()
        {
            try
            {
                await Task.Run(() => { _thumbnailManager.OptimizeDatabase(); });
            }
            catch (Exception exception)
            {
                _thumbnailManager.CloseFileHandle();
                Log.Error(exception, "Error in OptimizeDatabaseAsync()");
            }
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
    }
}