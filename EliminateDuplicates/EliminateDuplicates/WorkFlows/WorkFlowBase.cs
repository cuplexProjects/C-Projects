namespace DeleteDuplicateFiles.WorkFlows
{
    /// <summary>
    /// WorkFlowBase base properties
    /// </summary>
    public abstract class WorkFlowBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is started.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is started; otherwise, <c>false</c>.
        /// </value>
        public bool IsStarted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompleted { get; set; }
    }
}
