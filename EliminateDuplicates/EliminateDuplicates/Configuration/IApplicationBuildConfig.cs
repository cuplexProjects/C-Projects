using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteDuplicateFiles.Configuration
{
    public interface IApplicationBuildConfig
    {
        /// <summary>
        /// Applications the log file path.
        /// </summary>
        /// <param name="rollingFile">if set to <c>true</c> [rolling file].</param>
        /// <returns></returns>
        string ApplicationLogFilePath(bool rollingFile);
        /// <summary>
        /// Gets the user data path.
        /// </summary>
        /// <value>
        /// The user data path.
        /// </value>
        string UserDataPath { get; }
        /// <summary>
        /// Gets a value indicating whether [debug mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [debug mode]; otherwise, <c>false</c>.
        /// </value>
        bool DebugMode { get; }
    }
}
