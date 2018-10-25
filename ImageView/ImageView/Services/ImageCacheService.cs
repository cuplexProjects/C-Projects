using System;
using System.Drawing;
using ImageViewer.Models;
using ImageViewer.Repositories;
using ImageViewer.Utility;
using JetBrains.Annotations;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class ImageCacheService : ServiceBase, IDisposable
    {
        private readonly ImageCacheRepository _imageCacheRepository;
        private static readonly object CacheLock = new object();
        public const long DefaultCacheSize = 16777216;
        public const long MinCacheSize = 5242880;
        public const long MaxCacheSize = 268435456;

        public ImageCacheService(ApplicationSettingsService applicationSettingsService, ImageCacheRepository imageCacheRepository)
        {
            _imageCacheRepository = imageCacheRepository;
            applicationSettingsService.OnSettingsSaved += _applicationSettingsService_OnSettingsChanged;
        }

        private void _applicationSettingsService_OnSettingsChanged(object sender, EventArgs e)
        {
            if (sender is ApplicationSettingsService appSettingsService)
            {
                CacheSize = appSettingsService.Settings.ImageCacheSize;
            }
        }


        public int CachedImages => _imageCacheRepository.CachedImages;


        public long CacheSize
        {
            get => _imageCacheRepository.CacheSize;
            set
            {
                _imageCacheRepository.SetCacheSize(value, CacheTruncatePriority.RemoveOldest);
            }
        }

        public CachedImage GetCachedImage(string fileName)
        {
            lock (CacheLock)
            {
                return _imageCacheRepository.GetImageFromCache(fileName);
            }
        }

        public Image GetImageFromCache(string fileName)
        {
            lock (CacheLock)
            {
                var image = _imageCacheRepository.GetImageFromCache(fileName).GetImage(ImageProcessHelper.GetImageFromByteArray);
                return image;
            }
        }

        public void Dispose()
        {
            _imageCacheRepository?.Dispose();
        }

        public enum CacheTruncatePriority
        {
            RemoveOldest,
            RemoveLargest,
        }
    }
}