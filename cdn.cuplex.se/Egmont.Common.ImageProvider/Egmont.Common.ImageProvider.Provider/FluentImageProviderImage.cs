using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

namespace Cuplex.Common.ImageProvider
{
	/// <summary>
	///     Class FluentImageProviderImage.
	/// </summary>
	public class FluentImageProviderImage
	{
		private readonly string _imageUrl;
		private bool _cropFromTop;

		private string _headline;
		private int? _height;

		private RotateFlipType _rotateFlipType = RotateFlipType.RotateNoneFlipNone;

		private string _special;
		private Teknik? _technique;
		private int? _width;

		public static FluentImageProviderImage Create(string imageProviderUrl)
		{
			if (string.IsNullOrWhiteSpace(imageProviderUrl))
			{
				throw new ArgumentNullException(nameof(imageProviderUrl), "Missing image URL.");
			}

			return new FluentImageProviderImage(imageProviderUrl);
		}

		private FluentImageProviderImage(string imageUrl)
		{
			_imageUrl = imageUrl;
		}

		/// <summary>
		///     Crops from top.
		/// </summary>
		/// <param name="cropFromTop">if set to <c>true</c> [crop from top].</param>
		/// <returns>FluentImageProviderImage.</returns>
		public FluentImageProviderImage CropFromTop(bool cropFromTop)
		{
			_cropFromTop = cropFromTop;
			return this;
		}

		/// <summary>
		///     Headlines the specified headline.
		/// </summary>
		/// <param name="headline">The headline.</param>
		/// <returns>FluentImageProviderImage.</returns>
		public FluentImageProviderImage Headline(string headline)
		{
			_headline = headline;
			return this;
		}

		/// <summary>
		///     Heights the specified height.
		/// </summary>
		/// <param name="height">The height.</param>
		/// <returns>FluentImageProviderImage.</returns>
		public FluentImageProviderImage Height(int height)
		{
			_height = height;
			return this;
		}

		/// <summary>
		///     Renders this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		private string Render()
		{
			var addUrl = new StringBuilder();

			if (_width.HasValue)
			{
				addUrl.Append($"b{_width.Value.ToString(CultureInfo.InvariantCulture)}");
			}

			if (_height.HasValue)
			{
				addUrl.Append($"h{_height.Value.ToString(CultureInfo.InvariantCulture)}");
			}

			if (_technique == Teknik.Utsnitt && _cropFromTop)
			{
				_technique = Teknik.Top;
			}

			switch (_technique)
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

			if (!string.IsNullOrWhiteSpace(_headline))
			{
				var builder = new StringBuilder(_headline);
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

			if (_rotateFlipType != RotateFlipType.RotateNoneFlipNone)
			{
				addUrl.Append($"Rotera({_rotateFlipType})");
			}

			if (!string.IsNullOrWhiteSpace(_special))
			{
				addUrl.Append($"Special({_special})");
			}

			var imageUrl = _imageUrl;

			if (imageUrl.StartsWith("~"))
			{
				imageUrl = _imageUrl.Substring(1);
			}

			imageUrl = $"{imageUrl.Substring(0, imageUrl.LastIndexOf('.'))}__{addUrl}{Path.GetExtension(imageUrl)}";

			return imageUrl;
		}

		#region Overrides of Object

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return Render();
		}

		#endregion

		/// <summary>
		///     Renders the MVC string.
		/// </summary>
		/// <returns>HtmlString.</returns>
		public HtmlString MvcRender()
		{
			return new HtmlString(Render());
		}

		/// <summary>
		///     Rotates the and flip.
		/// </summary>
		/// <param name="rotateFlipType">Type of the rotate flip.</param>
		/// <returns>FluentImageProviderImage.</returns>
		public FluentImageProviderImage RotateAndFlip(RotateFlipType rotateFlipType)
		{
			_rotateFlipType = rotateFlipType;
			return this;
		}

		/// <summary>
		///     Specials the specified special.
		/// </summary>
		/// <param name="special">The special.</param>
		/// <returns>FluentImageProviderImage.</returns>
		public FluentImageProviderImage Special(string special)
		{
			_special = special;
			return this;
		}

		/// <summary>
		///     Techniques the specified technique.
		/// </summary>
		/// <param name="technique">The technique.</param>
		/// <returns>FluentImageProviderImage.</returns>
		public FluentImageProviderImage Technique(Teknik technique)
		{
			_technique = technique;
			return this;
		}

		/// <summary>
		///     Widthes the specified width.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <returns>FluentImageProviderImage.</returns>
		public FluentImageProviderImage Width(int width)
		{
			_width = width;
			return this;
		}
	}
}