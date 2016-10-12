﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using GeneralToolkitLib.Log;
using ImageView.Models;

namespace ImageView.Services
{
    public enum ProgressStatusEnum
    {
        Started,
        Stopping,
        Running,
        Canceled,
        Complete
    }

    public class ImageLoaderService
    {
        public delegate void ProgressUpdateEventHandler(object sender, ProgressEventArgs e);

        private const string ImageSearchPatterb = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";
        private static ImageLoaderService _instance;
        private readonly Regex _fileNameRegExp;
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly object _threadLock;
        private readonly WindowsIdentity _winId;
        private int _filesLoaded;
        private string _imageBaseDir;
        private List<ImageReferenceElement> _imageReferenceList;
        private int _progressInterval;
        private bool _runWorkerThread;
        private int _tickCount;
        private int _totalNumberOfFiles;
        private Thread _workerThread;

        private ImageLoaderService()
        {
            _fileNameRegExp = new Regex(ImageSearchPatterb, RegexOptions.IgnoreCase);
            _threadLock = new object();
            _progressInterval = 100;
            _randomNumberGenerator = RandomNumberGenerator.Create();

            _winId = WindowsIdentity.GetCurrent();
        }

        public static ImageLoaderService Instance
        {
            get { return _instance ?? (_instance = new ImageLoaderService()); }
        }

        public int ProgressInterval
        {
            get { return _progressInterval; }
            set
            {
                if (value >= 10 && value <= 2500)
                    _progressInterval = value;
            }
        }

        public List<ImageReferenceElement> ImageReferenceList
        {
            get
            {
                if (IsRunningImport)
                    return null;

                return _imageReferenceList;
            }
        }

        public bool IsRunningImport { get; private set; }

        public event ProgressUpdateEventHandler OnProgressUpdate;
        public event ProgressUpdateEventHandler OnImportComplete;
        public event EventHandler OnImageWasDeleted;

        private void DoImageImport()
        {
            try
            {
                lock (_threadLock)
                {
                    _totalNumberOfFiles = GetNumberOfFilesMatchingRegexp(_imageBaseDir);
                    _tickCount = Environment.TickCount;
                    _imageReferenceList = GetAllImagesRecursive(_imageBaseDir);
                    IsRunningImport = false;
                }
                if (OnImportComplete != null)
                    OnImportComplete.Invoke(this,
                        new ProgressEventArgs(ProgressStatusEnum.Complete, _imageReferenceList.Count,
                            _totalNumberOfFiles));
            }
            catch (Exception ex)
            {
                LogWriter.LogError("class ImageLoaderService.DoImageImport()\n" + ex.Message, ex);
                IsRunningImport = false;
            }
        }

        private int GetNumberOfFilesMatchingRegexp(string basePath)
        {
            var noFiles = 0;
            try
            {
                if (!_runWorkerThread)
                    return 0;
                var currentDirectory = new DirectoryInfo(basePath);
                if (UserHasReadAccessToDirectory(currentDirectory))
                {
                    noFiles += currentDirectory.GetFiles().Count(f => _fileNameRegExp.IsMatch(f.Name));
                    noFiles +=
                        currentDirectory.GetDirectories()
                            .Sum(directory => GetNumberOfFilesMatchingRegexp(directory.FullName));
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogError(
                    "class ImageLoaderService.GetNumberOfFilesMatchingRegexp(string basePath)\n" + ex.Message, ex);
            }

            return noFiles;
        }

        private bool UserHasReadAccessToDirectory(DirectoryInfo directoryInfo)
        {
            var dSecurity = directoryInfo.GetAccessControl();
            var authorizarionRuleCollecion = dSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (FileSystemAccessRule fsAccessRules  in authorizarionRuleCollecion)
            {
                if (_winId.UserClaims.Any(c => c.Value == fsAccessRules.IdentityReference.Value) &&
                    fsAccessRules.FileSystemRights.HasFlag(FileSystemRights.ListDirectory) &&
                    fsAccessRules.AccessControlType == AccessControlType.Allow)
                    return true;
            }
            return false;
        }

        public List<ImageReferenceElement> GetAllImagesRecursive(string baseDir)
        {
            var imageReferenceList = new List<ImageReferenceElement>();

            if (!_runWorkerThread)
                return imageReferenceList;

            var currentDirectory = new DirectoryInfo(baseDir);

            if (!UserHasReadAccessToDirectory(currentDirectory))
                return imageReferenceList;

            var fileInfoArray = currentDirectory.GetFiles();
            foreach (var fileInfo in fileInfoArray)
            {
                if (_fileNameRegExp.IsMatch(fileInfo.Name))
                    imageReferenceList.Add(new ImageReferenceElement
                    {
                        FileName = fileInfo.Name,
                        Directory = baseDir,
                        CompletePath = fileInfo.FullName,
                        CreationTime = fileInfo.CreationTime,
                        LastWriteTime = fileInfo.LastWriteTime,
                        LastAccessTime = fileInfo.LastWriteTime,
                        Size = fileInfo.Length
                    });
            }

            _filesLoaded += imageReferenceList.Count;
            if (OnProgressUpdate != null && Environment.TickCount > _tickCount + _progressInterval)
            {
                _tickCount = Environment.TickCount;
                OnProgressUpdate.Invoke(this,
                    new ProgressEventArgs(ProgressStatusEnum.Running, _filesLoaded, _totalNumberOfFiles));
            }

            foreach (var directory in currentDirectory.GetDirectories())
                imageReferenceList.AddRange(GetAllImagesRecursive(directory.FullName));

            return imageReferenceList;
        }

        public bool StartImageImport(string path)
        {
            if (!IsRunningImport)
            {
                _imageBaseDir = path;
                _workerThread = new Thread(DoImageImport);
                _runWorkerThread = true;
                IsRunningImport = true;
                _filesLoaded = 0;
                _workerThread.Start();
                return true;
            }
            return false;
        }

        public void StopImport()
        {
            _runWorkerThread = false;
        }

        internal bool PermanentlyRemoveFile(ImageReferenceElement imgRefElement)
        {
            var removedItems = 0;
            try
            {
                File.Delete(imgRefElement.CompletePath);
                removedItems = _imageReferenceList.RemoveAll(i => i == imgRefElement);
                OnImageWasDeleted?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                LogWriter.LogError("class ImageLoaderService.PermanentlyRemoveFile()\n" + ex.Message, ex);
            }
            return removedItems > 0;
        }

        private List<int> GetRandomImagePositionList()
        {
            var randomImagePosList = new List<int>();
            var randomData = new byte[ImageReferenceList.Count*4];
            _randomNumberGenerator.GetBytes(randomData);
            var randomDataPointer = 0;
            var candidates = new List<int>();

            for (var i = 0; i < ImageReferenceList.Count; i++)
                candidates.Add(i);

            while (candidates.Count > 0)
            {
                var index = Math.Abs(BitConverter.ToInt32(randomData, randomDataPointer))%candidates.Count;
                randomDataPointer += 4;

                randomImagePosList.Add(candidates[index]);
                candidates.RemoveAt(index);
            }

            return randomImagePosList;
        }

        public ImageReferenceCollection GenerateImageReferenceCollection(bool randomizeImageCollection)
        {
            List<int> randomImagePosList;
            if (randomizeImageCollection)
                randomImagePosList = GetRandomImagePositionList();
            else
            {
                randomImagePosList = new List<int>();
                for (var i = 0; i < ImageReferenceList.Count; i++)
                    randomImagePosList.Add(i);
            }

            var imageReferenceCollection = new ImageReferenceCollection(randomImagePosList);
            return imageReferenceCollection;
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(ProgressStatusEnum progressStatus, int imagesLoaded, int totalNumberOfFiles)
        {
            ProgressStatus = progressStatus;
            ImagesLoaded = imagesLoaded;

            if (totalNumberOfFiles > 0)
                CompletionRate = imagesLoaded/(double) totalNumberOfFiles;
        }

        public ProgressStatusEnum ProgressStatus { get; private set; }
        public int ImagesLoaded { get; private set; }
        public double CompletionRate { get; set; }
    }
}