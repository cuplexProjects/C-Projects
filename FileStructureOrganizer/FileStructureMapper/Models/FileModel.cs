using System;

namespace FileStructureMapper.Models
{
    public class FileModel
    {
        public string Name { get; set; }

        public long FileSize { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModifies { get; set; }

        public DateTime LastAccessed { get; set; }

        public FolderModel ParentFolder { get; set; }
    }
}