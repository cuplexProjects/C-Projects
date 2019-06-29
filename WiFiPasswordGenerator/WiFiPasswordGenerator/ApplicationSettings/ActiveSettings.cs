using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Documents;
using GeneralToolkitLib.Utility;

namespace WiFiPasswordGenerator.ApplicationSettings
{
    [Serializable]    
    public class ActiveSettings
    {
        [Setting]
        public List<string> AutocompleteList { get; set; }

        /// <summary>
        ///  Application current config constructor using default values
        /// </summary>
        public ActiveSettings()
        {
            // Set default settings
            PasswordType = PasswordTypes.StandardMixedChars;
            QR_CodeLevel = QR_CodeLevels.M;
            PasswordLength = 32;
            AutocompleteList= new List<string>();
        }

        /// <summary>
        /// Type of characters generated
        /// </summary>
        public PasswordTypes PasswordType { get; set; }
        /// <summary>
        /// Error correction level (L;M;Q;H) = 7,15,25,30%
        /// </summary>
        public QR_CodeLevels QR_CodeLevel { get; set; }
        /// <summary>
        /// 1-500
        /// </summary>
        public int PasswordLength { get; set; }
    }
}