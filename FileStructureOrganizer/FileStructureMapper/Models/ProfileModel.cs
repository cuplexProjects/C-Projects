using System;

namespace FileStructureMapper.Models
{
    public class ProfileModel
    {
        public string ProfileName { get; set; }

        public Guid Id { get; set; }

        public string BasePath { get; set; }

        public string FullDirectoryPath { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}