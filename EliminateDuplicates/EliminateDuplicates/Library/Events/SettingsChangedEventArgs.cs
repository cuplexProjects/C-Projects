using System;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.Encryption.Licence.StaticData;

namespace DeleteDuplicateFiles.Library.Events
{
    public delegate void SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e);


    public class SettingsChangedEventArgs : EventArgs
    {
        private readonly ApplicationSettingsModel _originalModel;
        private readonly ApplicationSettingsModel _modifiedlModel;

     

        public SettingsChangedEventArgs(ApplicationSettingsModel originalModel, ApplicationSettingsModel modifiedlModel)
        {
            _originalModel = originalModel;
            _modifiedlModel = modifiedlModel;
        }

        private SettingsChangedEventArgs()
        {
        }

        public ApplicationSettingsModel OriginalModel
        {
            get { return _originalModel; }
        }

        public ApplicationSettingsModel ModifiedlModel
        {
            get { return _modifiedlModel; }
        }
    }
}