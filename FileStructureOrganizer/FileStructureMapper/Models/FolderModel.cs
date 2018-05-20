using System;
using System.Collections.Generic;

namespace FileStructureMapper.Models
{
    public class FolderModel
    {
        public string Name { get; set; }

        public bool IsRootFolder { get; set; }

        public DateTime CreateDate { get; set; }

        public FolderModel ParentFolder { get; set; }

        public IEnumerable<FileModel> Files { get; set; }

        public IEnumerable<FolderModel> SubFolders { get; set; }
    }
}