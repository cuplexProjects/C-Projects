using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ImageView.Models;
using JetBrains.Annotations;

namespace ImageView.Services
{
    [UsedImplicitly]
    public sealed class ImageCacheService : ServiceBase, IDisposable
    {
        private const long DefaultCacheSize = 16777216;
        public const long MinCacheSize = 5242880;
        public const long MaxCacheSize = 268435456;
        private Dictionary<string, CachedImage> _cachedImages;
        private long _cacheSize;

        public ImageCacheService(ApplicationSettingsService applicationSettingsService)
        {
            long cacheSizeFromSetings = applicationSettingsService.Settings.ImageCacheSize;
            _cacheSize = DefaultCacheSize;

            _cachedImages = new Dictionary<string, CachedImage>();

            if (cacheSizeFromSetings >= MinCacheSize && cacheSizeFromSetings <= MaxCacheSize)
            {
                CacheSize = cacheSizeFromSetings;
            }
            else
            {
                applicationSettingsService.Settings.ImageCacheSize = CacheSize;
                applicationSettingsService.SaveSettings();
            }

            applicationSettingsService.OnSettingsSaved += _applicationSettingsService_OnSettingsChanged;
        }

        private void _applicationSettingsService_OnSettingsChanged(object sender, EventArgs e)
        {
            if (sender is ApplicationSettingsService appSettingsService)
            {
                CacheSize = appSettingsService.Settings.ImageCacheSize;
            }

        }

        public long CacheSize
        {
            get => _cacheSize;
            set
            {
                if (value < MinCacheSize)
                    throw new ArgumentOutOfRangeException($"Cache size must be atleast {MinCacheSize} bytes.");
                if (value > MaxCacheSize)
                    throw new ArgumentOutOfRangeException($"Cache size can not be higher than {MaxCacheSize} bytes.");

                _cacheSize = value;
                TruncateCache();
            }
        }

        public int CachedItems => _cachedImages.Count;

        public void Dispose()
        {
            _cachedImages = null;
        }

        private void TruncateCache()
        {
            while (_cachedImages.Count > 0 && _cachedImages.Sum(img => img.Value.Size) > _cacheSize)
            {
                string oldestImageFilename =
                    _cachedImages.OrderBy(img => img.Value.CreatedDate).Select(img => img.Value.Filename).First();

                _cachedImages.Remove(oldestImageFilename);
            }
        }

        public long GetCacheUsage()
        {
            return _cachedImages.Sum(img => img.Value.Size);
        }

        public Image GetImage(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName)) return _cachedImages[fileName].ImageObject;
            var cachedImage = new CachedImage(fileName);
            cachedImage.LoadImage();
            _cachedImages.Add(fileName, cachedImage);

            Image image = _cachedImages[fileName].ImageObject;
            TruncateCache();
            return image;
        }
    }
}