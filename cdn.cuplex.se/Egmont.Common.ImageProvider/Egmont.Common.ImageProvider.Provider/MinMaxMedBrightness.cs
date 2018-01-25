using System;

namespace Cuplex.Common.ImageProvider
{
	/// <summary>
	/// Class MinMaxMedBrightness.
	/// </summary>
	internal class MinMaxMedBrightness
	{
		/// <summary>
		/// Gets or sets the maximum.
		/// </summary>
		/// <value>The maximum.</value>
		internal float Max { get; set; }

		/// <summary>
		/// Gets or sets the minimum.
		/// </summary>
		/// <value>The minimum.</value>
		internal float Min { get; set; }

		/// <summary>
		/// Gets or sets the med.
		/// </summary>
		/// <value>The med.</value>
		internal float Med { get; set; }

		/// <summary>
		/// Gets the luminans avvikelse.
		/// </summary>
		/// <value>The luminans avvikelse.</value>
		internal float LuminansAvvikelse => (Max - Min);

		/// <summary>
		/// Returnerar en poäng för hur bra hörnet lämpar sig för att
		/// skriva texten i.
		/// </summary>
		/// <value>The poäng.</value>
		internal float Poäng
		{
			get
			{
				// Anger luminansavvikelsen som ett flyttal mellan 0 och 1
				// där högre tal innebär mindre avvikelse = positivt för att skriva text
				// 0 = Negativt, luminansen varierar väldigt mycket.
				// 1 = Positivt. Luminansen är helt stabil och varierar inte alls
				var positivLuminansAvvikelse = 1f - (Max - Min);

				// Anger extremiteten för medelluminansen.
				// 0 = Negativt, luminansen är gråaktig
				// 1 = Positivt, luminansen är antingen helt svart, eller helt vit
				var extremMedelLuminans = Math.Abs(Med - 0.5f) * 2f;

				// Lägg på "Bias"
				positivLuminansAvvikelse += 1;
				extremMedelLuminans += 1;

				// Experimentera med viktning
				positivLuminansAvvikelse = positivLuminansAvvikelse * 5;    // Hur viktigt det är med jämn luminans
				extremMedelLuminans = extremMedelLuminans * 3;              // Hur viktigt det är med hög kontrast (mkt svart eller vitt)

				// Returnerad poäng är en sammanräkning av hur positivt det är
				// att skriva text i hörnet.
				return positivLuminansAvvikelse + extremMedelLuminans;
			}
		}
	}
}