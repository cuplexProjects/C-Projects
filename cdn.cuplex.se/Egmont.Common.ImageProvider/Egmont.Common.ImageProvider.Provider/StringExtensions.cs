namespace Cuplex.Common.ImageProvider
{
	/// <summary>
	/// Class StringExtensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Ases the fluent image provider image.
		/// </summary>
		/// <param name="imageProviderUrl">The image provider URL.</param>
		/// <returns>FluentImageProviderImage.</returns>
		public static FluentImageProviderImage AsFluentImageProviderImage(this string imageProviderUrl)
		{
			return FluentImageProviderImage.Create(imageProviderUrl);
		}
	}
}