namespace Cuplex.ImageProvider
{
    /// <summary>
    ///     Enum Rotation
    /// </summary>
    public enum Rotation
    {
        /// <summary>
        ///     The error
        /// </summary>
        Error = -1,

        /// <summary>
        ///     The unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     The correct
        /// </summary>
        Correct = 1,

        /// <summary>
        ///     The right
        /// </summary>
        Right = 6,

        /// <summary>
        ///     The left
        /// </summary>
        Left = 8
    }


    /// <summary>
    /// Type of image processing being used
    /// </summary>
    public enum ScalingType
    {
        /// <summary>
        ///     Force. Will stretch image to desired size.
        /// </summary>
        Force,

        /// <summary>
        ///     Max size. Will not fill nor stretch. Will not force larger than source.
        /// </summary>
        Max,

        /// <summary>
        ///     Middle of the picture with cutoff.
        /// </summary>
        CutOut,

        /// <summary>
        ///     Top of image. Best for person images with face in top.
        /// </summary>
        Top,

        /// <summary>
        ///     Place the image in center, and fill with white around if nessesary.
        /// </summary>
        Center
    }
}