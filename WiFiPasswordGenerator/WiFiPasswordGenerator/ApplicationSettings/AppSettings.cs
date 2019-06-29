using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace WiFiPasswordGenerator.ApplicationSettings
{


    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    //internal sealed class AppSettings : ApplicationSettingsBase
    //{

    //    public AppSettings()
    //    {
            
    //        // // To add event handlers for saving and changing settings, uncomment the lines below:
    //        //

    //        this.SettingChanging += this.SettingChangingEventHandler;
    //        //
    //        this.SettingsSaving += this.SettingsSavingEventHandler;
    //        //
    //    }


    //    private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
    //    {
    //        // Add code to handle the SettingChangingEvent event here.
    //    }

    //    private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
    //    {
    //        // Add code to handle the SettingsSaving event here.
    //    }

    //    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        base.OnPropertyChanged(sender, e);
    //    }

    //    protected override void OnSettingChanging(object sender, SettingChangingEventArgs e)
    //    {
    //        base.OnSettingChanging(sender, e);
    //    }

    //    protected override void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
    //    {
    //        base.OnSettingsLoaded(sender, e);
    //    }

    //    protected override void OnSettingsSaving(object sender, CancelEventArgs e)
    //    {
    //        base.OnSettingsSaving(sender, e);
    //    }

    //    public override void Save()
    //    {
           
    //    }

    //    public override object this[string propertyName]
    //    {
    //        get => base[propertyName];
    //        set => base[propertyName] = value;
    //    }

    //}
}
