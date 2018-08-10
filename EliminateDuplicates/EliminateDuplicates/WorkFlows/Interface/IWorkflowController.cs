using System.Collections.Generic;
using System.Threading.Tasks;
using DeleteDuplicateFiles.Models;

namespace DeleteDuplicateFiles.WorkFlows.Interface
{
    public interface IWorkflowController
    {
        IEnumerable<ScanFolderModel> GetDuplicateFiles(ISearchSettings settings);

        Task<bool> StartDuplicateSearchAsync(SearchProfileModel searchProfile);
    }
}
