using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ImageView.Models;

namespace ImageView.Services
{
    public class ImageCacheService : IDisposable
    {
        private const int DefaultCacheSize = 16777216;
        public const int MinCacheSize = 4194304;
        public const int MaxCacheSize = 268435456;
        private static ImageCacheService _instance;
        private Dictionary<string, CachedImage> _cachedImages;
        private int _cacheSize;

        private ImageCacheService()
        {
            int cacheSizeFromSetings = ApplicationSettingsService.Instance.Settings.ImageCacheSize;
            _cacheSize = DefaultCacheSize;
            _cachedImages = new Dictionary<string, CachedImage>();

            if (cacheSizeFromSetings >= MinCacheSize && cacheSizeFromSetings <= MaxCacheSize)
            {
                CacheSize = cacheSizeFromSetings;
            }
            else
            {
                ApplicationSettingsService.Instance.Settings.ImageCacheSize = CacheSize;
                ApplicationSettingsService.Instance.SaveSettings();
            }
        }

        public static ImageCacheService Instance => _instance ?? (_instance = new ImageCacheService());

        public int CacheSize
        {
            get { return _cacheSize; }
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
            _instance = null;
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