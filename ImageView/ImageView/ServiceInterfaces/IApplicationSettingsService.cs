using System;
using ImageView.DataContracts;

namespace ImageView.ServiceInterfaces
{
    public interface IApplicationSettingsService
    {
        ImageViewApplicationSettings Settings { get; }
        event EventHandler OnSettingsChanged;
        event EventHandler OnRegistryAccessDenied;
        bool LoadSettings();
        void SaveSettings();
    }
}
