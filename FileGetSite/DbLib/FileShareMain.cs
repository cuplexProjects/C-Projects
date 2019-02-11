using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FileGetDbLib.Linq;

namespace FileGetDbLib
{
    public class FileShareMain
    {
        private const int VALID_TO_SEC = 86400;

        public string CreateLocalFileLink(string path, bool isDirectory)
        {
            if (!isDirectory && !System.IO.File.Exists(path))
                throw new Exception("invalid file path");
            else if (isDirectory && !System.IO.Directory.Exists(path))
                throw new Exception("invalid file path");

            try
            {
                string id = IdGenerator.GenerateFileId();
                using (var context = FgContext.Create())
                {
                    FileShare fs = new FileShare();
                    fs.FilePath = path;
                    fs.FileShareId = id;
                    fs.IsDirectory = isDirectory;
                    fs.ValidUntil = DateTime.Now.AddSeconds(VALID_TO_SEC);

                    context.FileShares.InsertOnSubmit(fs);
                    context.SubmitChanges();

                    return id;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public FileShare GetLocalFilePath(string link)
        {
            try
            {
                using (var context = FgContext.Create())
                {
                    FileShare fs = context.FileShares.Where(x => x.FileShareId == link && x.ValidUntil >= DateTime.Now).SingleOrDefault();
                    if (fs == null)
                        return null;
                    else
                        return fs; 
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
