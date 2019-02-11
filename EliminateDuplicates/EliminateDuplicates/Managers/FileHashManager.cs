using DeleteDuplicateFiles.Repositories;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.Managers
{
    [UsedImplicitly]
    public class FileHashManager : ManagerBase
    {
        private readonly FileHashRepository _hashRepository;

        public FileHashManager(FileHashRepository hashRepository)
        {
            _hashRepository = hashRepository;
        }

        public void RemoveDeletedFilesFromDataBase()
        {
        }
    }
}
