using System.Collections.Generic;
using DeleteDuplicateFiles.Models;

namespace DeleteDuplicateFiles.WorkFlows.Interface
{
    public interface IWorkflowController
    {
        IEnumerable<ScanFolderModel> GetDuplicateFiles(ISearchSettings settings);
    }
}
