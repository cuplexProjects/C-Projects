namespace FileSystemCore.Models
{
    /// <summary>
    /// StartupConfig Model
    /// </summary>
    public class StartupConfigModel
    {
        /// <summary>
        /// Gets or sets the base path requested.
        /// </summary>
        /// <value>
        /// The base path requested.
        /// </value>
        public string BasePathRequested { get; set; }

        /// <summary>
        /// Gets or sets the executing path.
        /// </summary>
        /// <value>
        /// The executing path.
        /// </value>
        public string ExecutingPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [simulated workflow].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [simulated workflow]; otherwise, <c>false</c>.
        /// </value>
        public bool SimulatedWorkflow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [invalid startup arguments].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [invalid startup arguments]; otherwise, <c>false</c>.
        /// </value>
        public bool InvalidStartupArguments { get; set; }

        /// <summary>
        /// Gets or sets the parse error messsage.
        /// </summary>
        /// <value>
        /// The parse error messsage.
        /// </value>
        public string ParseErrorMesssage { get; set; }
    }
}
