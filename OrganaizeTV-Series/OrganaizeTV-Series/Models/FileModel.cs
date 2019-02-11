using System;

namespace OrganaizeTV_Series.Models
{
    public class FileModel
    {
        public string FileName { get; set; }

        public string OriginalFullPath { get; set; }

        public long FileSize { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastModified { get; set; }

        public Guid ParentFolderId { get; set; }
    }
}