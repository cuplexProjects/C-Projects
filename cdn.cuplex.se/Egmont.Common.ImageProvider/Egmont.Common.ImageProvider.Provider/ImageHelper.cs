using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;

namespace Cuplex.Common.ImageProvider
{
	/// <summary>
	/// Class ImageHelper.
	/// </summary>
	public static class ImageHelper
	{
		private const string BaseUrl = "/ImageProvider/";

		/// <summary>
		/// Builds the image handler URL.
		/// </summary>
		/// <param name="imageUrl">The image URL.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="technique">The technique.</param>
		/// <param name="cropFromTop">if set to <c>true</c> [crop from top].</param>
		/// <param name="headline">The headline.</param>
		/// <param name="rotateFlip">The rotate flip.</param>
		/// <param name="special">The special.</param>
		/// <returns>System.String.</returns>
		public static string BuildImageHandlerUrl(string imageUrl, int width, int height, Teknik technique = Teknik.Max, bool cropFromTop = false, string headline = "", RotateFlipType rotateFlip = RotateFlipType.RotateNoneFlipNone, string special = "")
		{
			var addUrl = new StringBuilder();

			if (width > 0)
			{
				addUrl.Append($"b{width}");
			}

			if (height > 0)
			{
				addUrl.Append($"h{height}");
			}

			if (technique == Teknik.Utsnitt && cropFromTop)
			{
				technique = Teknik.Top;
			}

			switch (technique)
			{
				case Teknik.Top:
					addUrl.Append("f"); // Face - the top
					break;
				case Teknik.Max:
					addUrl.Append("m");
					break;
				case Teknik.Tvinga:
					addUrl.Append("t");
					break;
				case Teknik.Utsnitt:
					addUrl.Append("u");
					break;
			}

			if (!string.IsNullOrWhiteSpace(headline))
			{
				var builder = new StringBuilder(headline);
				builder.Replace("?", "escquestionmark")
					   .Replace(":", "esccolon")
					   .Replace("<", "esclt")
					   .Replace(">", "escgt")
					   .Replace("*", "escstar")
					   .Replace("+", "escplus")
					   .Replace("_", "escunderscore")
					   .Replace(".", "escperiod")
					   .Replace(" ", "_");
				var encoded = HttpContext.Current.Server.UrlEncode(builder.ToString());
				addUrl.Append($"R-{encoded}--");
			}

			if (rotateFlip != RotateFlipType.RotateNoneFlipNone)
			{
				addUrl.Append($"Rotera({rotateFlip})");
			}

			if (!string.IsNullOrWhiteSpace(special))
			{
				addUrl.Append($"Special({special})");
			}

			if (imageUrl.StartsWith("~"))
			{
				imageUrl = imageUrl.Substring(1);
			}

			if (!string.IsNullOrWhiteSpace(imageUrl) && imageUrl.Contains("."))
			{
				imageUrl = $"{imageUrl.Substring(0, imageUrl.LastIndexOf('.'))}__{addUrl}{Path.GetExtension(imageUrl)}";

				if (imageUrl.IndexOf(BaseUrl, StringComparison.InvariantCultureIgnoreCase) == -1)
				{
					imageUrl = Path.Combine(BaseUrl, imageUrl);
				}
			}
			else
			{
				imageUrl = Path.Combine(BaseUrl, $"__{addUrl}.jpg");
			}

			return imageUrl;
		}
	}
}