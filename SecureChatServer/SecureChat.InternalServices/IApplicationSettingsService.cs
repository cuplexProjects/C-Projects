using SecureChat.Common.ConfigManagement;

namespace SecureChat.InternalServices
{
    public interface IApplicationSettingsService
    {
        IServerConfiguration GetServerConfiguration();
        void SaveServerConfiguration(IServerConfiguration serverConfiguration);
    }
}