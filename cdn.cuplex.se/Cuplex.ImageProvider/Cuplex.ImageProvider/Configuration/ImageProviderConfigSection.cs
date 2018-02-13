using System.Configuration;

namespace Cuplex.ImageProvider.Configuration
{
	/// <summary>
	/// Class ImageProviderConfigSection.
	/// </summary>
	/// <seealso cref="System.Configuration.ConfigurationSection" />
	public class ImageProviderConfigSection : ConfigurationSection
	{
		/// <summary>
		/// Gets or sets the members.
		/// </summary>
		/// <value>The members.</value>
		[ConfigurationProperty("paths", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
		public PathCollection Members
		{
			get { return base["paths"] as PathCollection; }
			set { base["paths"] = value; }
		}
	}
}