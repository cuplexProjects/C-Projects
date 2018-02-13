using System;

namespace Cuplex.ImageProvider
{
	/// <summary>
	/// Class BrightnessCornerInfo.
	/// </summary>
	internal class BrightnessCornerInfo
	{
		/// <summary>
		/// To vä
		/// </summary>
		public MinMaxMedBrightness ToVä = new MinMaxMedBrightness();
		/// <summary>
		/// To hö
		/// </summary>
		public MinMaxMedBrightness ToHö = new MinMaxMedBrightness();
		/// <summary>
		/// The bo vä
		/// </summary>
		public MinMaxMedBrightness BoVä = new MinMaxMedBrightness();
		/// <summary>
		/// The bo hö
		/// </summary>
		public MinMaxMedBrightness BoHö = new MinMaxMedBrightness();

		/// <summary>
		/// Gets the maximum medium brightness.
		/// </summary>
		/// <value>The maximum medium brightness.</value>
		public float MaxMediumBrightness
		{
			get
			{
				var maxMediumBrightness = Math.Max(ToVä.Med, ToHö.Med);
				maxMediumBrightness = Math.Max(maxMediumBrightness, BoVä.Med);
				maxMediumBrightness = Math.Max(maxMediumBrightness, BoHö.Med);

				return maxMediumBrightness;
			}
		}

		/// <summary>
		/// Gets the minimum medium brightness.
		/// </summary>
		/// <value>The minimum medium brightness.</value>
		public float MinMediumBrightness
		{
			get
			{
				var minMediumBrightness = Math.Min(ToVä.Med, ToHö.Med);
				minMediumBrightness = Math.Min(minMediumBrightness, BoVä.Med);
				minMediumBrightness = Math.Min(minMediumBrightness, BoHö.Med);
				return minMediumBrightness;
			}
		}

		/// <summary>
		/// Gets the maximum poäng.
		/// </summary>
		/// <value>The maximum poäng.</value>
		public float MaxPoäng
		{
			get
			{
				var maxPoäng = Math.Max(ToVä.Poäng, ToHö.Poäng);
				maxPoäng = Math.Max(maxPoäng, BoVä.Poäng);
				maxPoäng = Math.Max(maxPoäng, BoHö.Poäng);
				return maxPoäng;
			}
		}

		/// <summary>
		/// Gets the maximum poäng HÖRN.
		/// </summary>
		/// <value>The maximum poäng HÖRN.</value>
		public int MaxPoängHörn
		{
			get
			{
				var maxPoäng = MaxPoäng;
				var preferredCorner = 0;

				if (maxPoäng == ToHö.Poäng) preferredCorner = 2;
				if (maxPoäng == ToVä.Poäng) preferredCorner = 1;
				if (maxPoäng == BoHö.Poäng) preferredCorner = 4;
				if (maxPoäng == BoVä.Poäng) preferredCorner = 3;

				/*throw new Exception("Fel i MaxPoängHörn this.tohö.poäng: " + this.ToHö.Poäng.ToString() 
					+ " " + this.ToVä.Poäng.ToString() 
					+ " " + this.BoHö.Poäng.ToString()
					+ " " + this.BoVä.Poäng.ToString()
					+ " maxPoäng: " + maxPoäng.ToString()
					+ " hörn: " + PreferredCorner.ToString());*/

				return preferredCorner;
			}
		}

		/// <summary>
		/// Gets the minimum medium brightness corner.
		/// </summary>
		/// <value>The minimum medium brightness corner.</value>
		public int MinMediumBrightnessCorner
		{
			get
			{
				var minMediumBrightness = MinMediumBrightness;
				var preferredCorner = 0;

				if (minMediumBrightness == ToHö.Min) preferredCorner = 2;
				if (minMediumBrightness == ToVä.Min) preferredCorner = 1;
				if (minMediumBrightness == BoHö.Min) preferredCorner = 4;
				if (minMediumBrightness == BoVä.Min) preferredCorner = 3;

				return preferredCorner;
			}
		}

		/// <summary>
		/// Gets the maximum medium brightness corner.
		/// </summary>
		/// <value>The maximum medium brightness corner.</value>
		public int MaxMediumBrightnessCorner
		{
			get
			{
				var maxMediumBrightness = MaxMediumBrightness;
				var preferredCorner = 0;

				if (maxMediumBrightness == ToHö.Max) preferredCorner = 2;
				if (maxMediumBrightness == ToVä.Max) preferredCorner = 1;
				if (maxMediumBrightness == BoHö.Max) preferredCorner = 4;
				if (maxMediumBrightness == BoVä.Max) preferredCorner = 3;

				return preferredCorner;
			}
		}
	}
}