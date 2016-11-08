using System.Collections.Generic;
using System.IO;
using GeneralToolkitLib.Converters;
using ImageView.Services;

namespace ImageView.Models
{
    public class ImageReferenceCollection
    {
        private readonly List<int> _randomImagePosList;
        private int _imageListPointer;

        public ImageReferenceCollection(List<int> randomImagePosList)
        {
            _randomImagePosList = randomImagePosList;
        }

        public ImageReferenceElement CurrentImage { get; private set; }

        private int ImageListPointer
        {
            get { return _imageListPointer; }
            set
            {
                if (value < 0)
                    _imageListPointer = _randomImagePosList.Count - 1;
                else if (value >= _randomImagePosList.Count)
                    _imageListPointer = 0;
                else
                    _imageListPointer = value;
            }
        }

        public ImageReferenceElement GetNextImage()
        {
            ImageListPointer = ImageListPointer + 1;
            CurrentImage = ImageLoaderService.Instance.ImageReferenceList[_randomImagePosList[ImageListPointer]];
            return CurrentImage;
        }

        public ImageReferenceElement GetPreviousImage()
        {
            ImageListPointer = ImageListPointer - 1;
            CurrentImage = ImageLoaderService.Instance.ImageReferenceList[_randomImagePosList[ImageListPointer]];
            return CurrentImage;
        }

        public ImageReferenceElement SetCurrentImage(string fileName)
        {
            ImageReferenceElement imageReferenceElement = new ImageReferenceElement();
            FileInfo fi=new FileInfo(fileName);
            imageReferenceElement.Size = fi.Length;
            imageReferenceElement.CompletePath = fileName;
            imageReferenceElement.CreationTime = fi.CreationTime;
            imageReferenceElement.Directory = GeneralConverters.GetDirectoryNameFromPath(fileName);
            imageReferenceElement.LastAccessTime = fi.LastAccessTime;
            imageReferenceElement.LastWriteTime = fi.LastWriteTime;
            imageReferenceElement.FileName = GeneralConverters.GetFileNameFromPath(fileName);

            CurrentImage = imageReferenceElement;

            if (_randomImagePosList.Count == 0)
                _randomImagePosList.Add(0);

            return imageReferenceElement;
        }
    }
}