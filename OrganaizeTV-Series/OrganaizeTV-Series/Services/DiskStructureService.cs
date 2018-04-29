using OrganaizeTV_Series.Configuration;

namespace OrganaizeTV_Series.Services
{
    public class DiskStructureService : ServiceBase
    {
        private readonly AppSettingsManager _appSettings;
        private readonly string _basePath;

        public DiskStructureService(AppSettingsManager appSettings, Models.InputArgumentsModel argumentInputs)
        {
            _basePath = argumentInputs.BasePath;
            _appSettings = appSettings;
        }

        public void StartWorkflow()
        {
        }
    }
}
