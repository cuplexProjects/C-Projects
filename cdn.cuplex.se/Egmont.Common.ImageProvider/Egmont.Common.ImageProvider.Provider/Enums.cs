namespace Cuplex.Common.ImageProvider
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
	///     Enum för att hantera vilken typ av Teknik som ska användas vid automatisk bildskalning
	/// </summary>
	public enum Teknik
	{
		/// <summary>
		///     Force. Will stretch image to desired size.
		/// </summary>
		Tvinga,

		/// <summary>
		///     Max size. Will not fill nor stretch. Will not force larger than source.
		/// </summary>
		Max,

		/// <summary>
		///     Middle of the picture with cutoff.
		/// </summary>
		Utsnitt,

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