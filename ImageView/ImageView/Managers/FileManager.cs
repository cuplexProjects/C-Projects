using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using GeneralToolkitLib.Converters;
using ImageView.Configuration;
using ImageView.DataContracts;
using ImageView.Models;
using JetBrains.Annotations;
using Serilog;

namespace ImageView.Managers
{
    public class FileManager : ManagerBase, IDisposable
    {
        private readonly Dictionary<string, bool> _directoryAccessDictionary;
        private readonly string _fileName;
        private FileStream _fileStream;
        private const string TemporaryDatabaseFilename = "temp.ibd";
        private const string DatabaseImgDataFilename = "thumbs.ibd";

        [UsedImplicitly]
        public FileManager()
        {
            _fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseImgDataFilename);
            _directoryAccessDictionary = new Dictionary<string, bool>();
        }

        public FileManager(string fileName)
        {
            _fileName = fileName;
            _directoryAccessDictionary = new Dictionary<string, bool>();
        }

        public bool IsLocked { get; private set; }

        public void Dispose()
        {
            CloseStream();
        }

        public Image ReadImage(int position, int length)
        {
            if (_fileStream == null)
                _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            _fileStream.Position = position;
            var buffer = new byte[length];
            _fileStream.Read(buffer, 0, length);

            var ms = new MemoryStream(buffer);
            Image img = Image.FromStream(ms);


            return img;
        }

        public FileEntry WriteImage(Image img)
        {
            if (_fileStream == null)
                _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            _fileStream.Position = _fileStream.Length;
            long position = _fileStream.Position;
            img.Save(_fileStream, ImageFormat.Jpeg);

            var fileEntry = new FileEntry
            {
                Position = position,
                Length = Convert.ToInt32(_fileStream.Position - position)
            };

            return fileEntry;
        }

        public void RecreateDatabase(IEnumerable<ThumbnailEntry> thumbnailEntries)
        {
            if (_fileStream == null)
                _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            else
            {
                SaveToDisk();
            }

            string tempFileName = GeneralConverters.GetDirectoryNameFromPath(_fileName) + TemporaryDatabaseFilename;

            FileStream temporaryDatabaseFile = null;
            try
            {
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);

                temporaryDatabaseFile = File.OpenWrite(tempFileName);
                foreach (ThumbnailEntry entry in thumbnailEntries)
                {
                    var buffer = new byte[entry.Length];
                    _fileStream.Position = entry.FilePosition;
                    _fileStream.Read(buffer, 0, entry.Length);

                    entry.FilePosition = temporaryDatabaseFile.Position;
                    temporaryDatabaseFile.Write(buffer, 0, entry.Length);
                }

                CloseStream();
                temporaryDatabaseFile.Close();
                temporaryDatabaseFile = null;
                File.Delete(_fileName);
                File.Move(tempFileName, _fileName);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error in RecreateDatabase()");
            }
            finally
            {
                temporaryDatabaseFile?.Close();
                _fileStream?.Close();
                _fileStream = null;
            }
        }

        private void SaveToDisk()
        {
            _fileStream?.Flush(true);
        }

        public void CloseStream()
        {
            if (_fileStream != null)
            {
                _fileStream.Flush(true);
                _fileStream.Close();
                _fileStream = null;
            }
        }

        /// <summary>
        ///     Verifies that the file does excist and that the physical file has not been written to after the thumbnail was
        ///     created.
        ///     Assumes access to the directory
        /// </summary>
        /// <param name="thumbnailEntry"></param>
        /// <returns>True if the thumbnail is up to date and the original file exists</returns>
        public static bool IsUpToDate(ThumbnailEntry thumbnailEntry)
        {
            var fileInfo = new FileInfo(thumbnailEntry.Directory + thumbnailEntry.FileName);
            return fileInfo.Exists && fileInfo.LastWriteTime == thumbnailEntry.SourceImageDate;
        }

        public void ClearDirectoryAccessCache()
        {
            _directoryAccessDictionary.Clear();
        }

        public bool HasAccessToDirectory(string directory)
        {
            if (_directoryAccessDictionary.ContainsKey(directory))
                return _directoryAccessDictionary[directory];

            string volume = GeneralConverters.GetVolumeLabelFromPath(directory);
            var drives = DriveInfo.GetDrives().ToList();

            if (drives.Any(d => d.IsReady && d.Name.Equals(volume, StringComparison.CurrentCultureIgnoreCase)))
            {
                try
                {
                    var directoryInfo = new DirectoryInfo(directory);
                    directoryInfo.EnumerateFiles();
                    _directoryAccessDictionary.Add(directory, true);
                    return true;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _directoryAccessDictionary.Add(directory, false);
            return false;
        }

        public bool LockDatabase()
        {
            if (IsLocked)
                return false;

            IsLocked = true;
            return true;
        }

        public void UnlockDatabase()
        {
            IsLocked = false;
        }
    }
}
