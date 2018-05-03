using System;

namespace FileStructureMapper.Models
{
    public class TVSeriesFolder
    {
        private readonly FolderModel _folder;

        public TVSeriesFolder(FolderModel folder)
        {
            _folder = folder ?? throw new ArgumentNullException(nameof(folder), "folder Object can not be null");
        }

        public void Analyze()
        {

        }



        public bool CorrectlyNamed { get; private set; }

        public bool GloballyIdentified { get; private set; }
    }
}