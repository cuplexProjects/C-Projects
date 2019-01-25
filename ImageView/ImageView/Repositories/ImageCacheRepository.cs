using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using GeneralToolkitLib.Converters;
using ImageProcessor;
using ImageViewer.Models;
using ImageViewer.Services;
using JetBrains.Annotations;

namespace ImageViewer.Repositories
{
    [UsedImplicitly]
    public class ImageCacheRepository : RepositoryBase, IDisposable
    {
        private readonly Dictionary<string, CachedImage> _cachedImages;
        private readonly ImageFactory _imageFactory;
        private long _cacheSize;
        private long _maxCacheSize;
        private bool _cacheSizeIsValid = false;


        public ImageCacheRepository()
        {
            _cachedImages = new Dictionary<string, CachedImage>();
            _imageFactory = new ImageFactory();
            _maxCacheSize = ImageCacheService.DefaultCacheSize;
        }

        public long CacheSize
        {
            get
            {
                if (!_cacheSizeIsValid)
                {
                    _cacheSize = _cachedImages.Select(x => x.Value.Size).Sum();
                    _cacheSizeIsValid = true;
                }

                return _cacheSize;
            }
        }

        public int CachedImages => _cachedImages.Count;

        public CachedImage GetImageFromCache(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                return _cachedImages[fileName];
            }

            var imageModel = CreateCachedImageModel(fileName);

            return imageModel;
        }

        private CachedImage CreateCachedImageModel(string fileName)
        {
            var imageModel = new CachedImage();
            imageModel.SetImage(ImageConverter, fileName);
            _cachedImages.Add(fileName, imageModel);
            _cacheSizeIsValid = false;
            return imageModel;
        }

        private byte[] ImageConverter(string fileName)
        {
            _imageFactory.Load(fileName);
            using (var ms = new MemoryStream())
            {
                _imageFactory.Image.Save(ms, ImageFormat.Jpeg);
                ms.Flush();
                return ms.ToArray();
            }
        }

        public void Dispose()
        {
            _imageFactory?.Dispose();
        }

        public void SetCacheSize(long cacheSize, ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            if (cacheSize < ImageCacheService.MinCacheSize)
            {
                throw new ArgumentException($"Cache size can not be lower then {GeneralConverters.FileSizeToStringFormater.ConvertFileSizeToString(ImageCacheService.MinCacheSize, 0)}");
            }

            if (cacheSize > ImageCacheService.MaxCacheSize)
            {
                throw new ArgumentException($"Cache size can not be higher then {GeneralConverters.FileSizeToStringFormater.ConvertFileSizeToString(ImageCacheService.MaxCacheSize, 0)}");
            }

            if (cacheSize < CacheSize)
            {
                TruncateCache(truncatePriority);
            }

            _maxCacheSize = cacheSize;

        }
        private void TruncateCache(ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            _cacheSizeIsValid = false;
            long truncatedSize = Convert.ToInt64((double)_maxCacheSize * 0.75d);

            if (truncatePriority == ImageCacheService.CacheTruncatePriority.RemoveOldest)
            {
                while (_cachedImages.Count > 0 && CacheSize > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.AddedToCacheTime).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                    _cacheSizeIsValid = false;
                }
            }
            else
            {
                while (_cachedImages.Count > 0 && CacheSize > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.Size).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                    _cacheSizeIsValid = false;
                }
            }
        }
    }
}