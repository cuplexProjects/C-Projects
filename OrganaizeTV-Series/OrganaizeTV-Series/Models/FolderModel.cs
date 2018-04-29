using System;
using System.Collections.Generic;

namespace OrganaizeTV_Series.Models
{
    public class FolderModel
    {
        public FolderModel()
        {
            InstanceGuid = Guid.NewGuid();
        }

        public FolderModel ParentFolder { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public IEnumerable<FolderModel> SubFolders { get; set; }

        public IEnumerable<FileModel> Files { get; set; }

        public Guid InstanceGuid { get; }
    }
}