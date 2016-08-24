using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileSystemImage.FileSystem;

namespace FileSystemImage.FileTree
{
    public class TreeSearch
    {
        private readonly string driveLetter;
        private readonly FileSystemDirectory root;
        private bool ignoreCase;
        private Regex regularExpression;

        public TreeSearch(FileSystemDrive fileSystemDrive)
        {
            this.root = new FileSystemDirectory {DirectoryList = fileSystemDrive.DirectoryList, FileList = fileSystemDrive.RootFileList};
            this.driveLetter = fileSystemDrive.DriveLetter.Replace("\\", "");
        }

        public List<TreeSearchResult> Search(string searchString, bool isRegex, bool ignoreCaseSensitiveMatch)
        {
            List<TreeSearchResult> searchRes = null;
            this.ignoreCase = ignoreCaseSensitiveMatch;

            if(isRegex)
            {
                this.regularExpression = ignoreCaseSensitiveMatch ? new Regex(searchString, RegexOptions.IgnoreCase) : new Regex(searchString);
            }
            else
                this.regularExpression = null;

            searchRes = this._search(searchString, this.root, this.driveLetter);
            return searchRes;
        }

        private List<TreeSearchResult> _search(string searchString, FileSystemDirectory rootDirectory, string currentPath)
        {
            List<TreeSearchResult> searchRes = new List<TreeSearchResult>();

            if(rootDirectory.FileList != null)
            {
                foreach (FileSystemFile fileSystemFile in rootDirectory.FileList)
                {
                    if(this.regularExpression != null)
                    {
                        if(this.regularExpression.IsMatch(fileSystemFile.Name))
                            searchRes.Add(new TreeSearchResult(fileSystemFile, currentPath));
                    }
                    else if(this.ignoreCase && fileSystemFile.Name.Equals(searchString, StringComparison.CurrentCultureIgnoreCase) || (!this.ignoreCase && fileSystemFile.Name.Equals(searchString)))
                        searchRes.Add(new TreeSearchResult(fileSystemFile, currentPath));
                }
            }

            if(rootDirectory.DirectoryList != null)
            {
                foreach (FileSystemDirectory fsd in rootDirectory.DirectoryList)
                {
                    List<TreeSearchResult> recursiveResult = this._search(searchString, fsd, currentPath + "\\" + fsd.Name);
                    if(recursiveResult.Count > 0)
                        searchRes.AddRange(recursiveResult);
                }
            }

            return searchRes;
        }
    }

    public class TreeSearchResult
    {
        public TreeSearchResult(FileSystemFile fileSystemFile, string path)
        {
            this.file = fileSystemFile;
            this.path = path;
        }

        public FileSystemFile file { get; private set; }
        public string path { get; private set; }

        public override string ToString()
        {
            return this.path + "\\" + this.file.Name;
        }
    }
}