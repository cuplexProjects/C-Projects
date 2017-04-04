using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            root = new FileSystemDirectory {DirectoryList = fileSystemDrive.DirectoryList, FileList = fileSystemDrive.RootFileList};
            driveLetter = fileSystemDrive.DriveLetter.Replace("\\", "");
        }

        public async Task<List<TreeSearchResult>> Search(string searchString, bool isRegex, bool ignoreCaseSensitiveMatch)
        {
            List<TreeSearchResult> searchRes = null;
            ignoreCase = ignoreCaseSensitiveMatch;

            if (isRegex)
            {
                regularExpression = ignoreCaseSensitiveMatch ? new Regex(searchString, RegexOptions.IgnoreCase) : new Regex(searchString);
            }
            else
                regularExpression = null;

            searchRes = await _search(searchString, root, driveLetter);
            return searchRes;
        }

        private async Task<List<TreeSearchResult>> _search(string searchString, FileSystemDirectory rootDirectory, string currentPath)
        {
            var searchRes = new List<TreeSearchResult>();

            if (rootDirectory.FileList != null)
            {
                foreach (FileSystemFile fileSystemFile in rootDirectory.FileList)
                {
                    if (regularExpression != null)
                    {
                        if (regularExpression.IsMatch(fileSystemFile.Name))
                            searchRes.Add(new TreeSearchResult(fileSystemFile, currentPath));
                    }
                    else if (ignoreCase && fileSystemFile.Name.Equals(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                             (!ignoreCase && fileSystemFile.Name.Equals(searchString)))
                        searchRes.Add(new TreeSearchResult(fileSystemFile, currentPath));
                }
            }

            if (rootDirectory.DirectoryList != null)
            {
                foreach (FileSystemDirectory fsd in rootDirectory.DirectoryList)
                {
                    var recursiveResult = await _search(searchString, fsd, currentPath + "\\" + fsd.Name);
                    if (recursiveResult.Count > 0)
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
            file = fileSystemFile;
            this.path = path;
        }

        public FileSystemFile file { get; }
        public string path { get; }

        public override string ToString()
        {
            return path + "\\" + file.Name;
        }
    }
}