using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Cuplex.ImageProvider.Configuration
{
	/// <summary>
	///     Class PathCollection.
	/// </summary>
	/// <seealso cref="System.Configuration.ConfigurationElementCollection" />
	/// <seealso>
	///     <cref>System.Collections.Generic.IEnumerable{Egmont.Common.ImageProvider.Provider.Configuration.ImageProviderPath}</cref>
	/// </seealso>
	public class PathCollection : ConfigurationElementCollection, IEnumerable<ImageProviderPath>
    {
	    /// <summary>
	    ///     Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection" />.
	    /// </summary>
	    /// <value>The type of the collection.</value>
	    public override ConfigurationElementCollectionType CollectionType =>
            ConfigurationElementCollectionType.BasicMap;

	    /// <summary>
	    ///     Gets the name used to identify this collection of elements in the configuration file when overridden in a derived
	    ///     class.
	    /// </summary>
	    /// <value>The name of the element.</value>
	    protected override string ElementName => "path";

	    /// <summary>
	    ///     Gets the <see cref="ImageProviderPath" /> at the specified index.
	    /// </summary>
	    /// <param name="index">The index.</param>
	    /// <returns>ImageProviderPath.</returns>
	    public ImageProviderPath this[int index] => BaseGet(index) as ImageProviderPath;

	    /// <summary>
	    ///     Gets the <see cref="ImageProviderPath" /> with the specified key.
	    /// </summary>
	    /// <param name="key">The key.</param>
	    /// <returns>ImageProviderPath.</returns>
	    public new ImageProviderPath this[string key] => BaseGet(key) as ImageProviderPath;

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the
        ///     collection.
        /// </returns>
        IEnumerator<ImageProviderPath> IEnumerable<ImageProviderPath>.GetEnumerator()
        {
            return this.OfType<ImageProviderPath>().GetEnumerator();
        }

	    /// <summary>When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.</summary>
	    /// <returns>A newly created <see cref="T:System.Configuration.ConfigurationElement" />.</returns>
	    /// <summary>
	    ///     When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
	    /// </summary>
	    /// <returns>A newly created <see cref="T:System.Configuration.ConfigurationElement" />.</returns>
	    protected override ConfigurationElement CreateNewElement()
        {
            return new ImageProviderPath();
        }

	    /// <summary>
	    ///     Gets the element key for a specified configuration element when overridden in a derived class.
	    /// </summary>
	    /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
	    /// <returns>
	    ///     An <see cref="T:System.Object" /> that acts as the key for the specified
	    ///     <see cref="T:System.Configuration.ConfigurationElement" />.
	    /// </returns>
	    protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ImageProviderPath) element).RoutePrefix;
        }

	    /// <summary>
	    ///     Indicates whether the specified <see cref="T:System.Configuration.ConfigurationElement" /> exists in the
	    ///     <see cref="T:System.Configuration.ConfigurationElementCollection" />.
	    /// </summary>
	    /// <param name="elementName">The name of the element to verify.</param>
	    /// <returns>true if the element exists in the collection; otherwise, false. The default is false.</returns>
	    protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrEmpty(elementName) && elementName == ElementName;
        }
    }
}