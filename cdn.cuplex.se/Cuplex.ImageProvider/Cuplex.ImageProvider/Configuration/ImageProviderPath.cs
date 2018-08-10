using System;
using System.Configuration;
using System.Web.Hosting;

namespace Cuplex.ImageProvider.Configuration
{
	/// <inheritdoc />
	/// <summary>
	///     Class ImageProviderPath.
	/// </summary>
	/// <seealso cref="T:System.Configuration.ConfigurationElement" />
	public class ImageProviderPath : ConfigurationElement
    {
        private string _localPath;
        private string _originalPath;

	    /// <summary>
	    ///     Gets the expiration. Format: [d.]hh:mm[:ss[.ff]].
	    /// </summary>
	    /// <value>The expiration.</value>
	    [ConfigurationProperty("expiration", IsRequired = true)]
        public TimeSpan Expiration => (TimeSpan) this["expiration"];

	    /// <summary>
	    ///     Gets the local path.
	    /// </summary>
	    /// <value>The local path.</value>
	    [ConfigurationProperty("localPath", IsRequired = true)]
        public string LocalPath
        {
            get
            {
                if (_localPath == null)
                {
                    _localPath = (string) this["localPath"];
                    if (_localPath.StartsWith("~")) _localPath = HostingEnvironment.MapPath(_localPath);
                }

                return _localPath;
            }
        }

	    /// <summary>
	    ///     Gets the original path.
	    /// </summary>
	    /// <value>The original path.</value>
	    [ConfigurationProperty("originalPath", IsRequired = true)]
        public string OriginalPath
        {
            get
            {
                if (_originalPath == null)
                {
                    _originalPath = (string) this["originalPath"];
                    if (_originalPath.StartsWith("~")) _originalPath = HostingEnvironment.MapPath(_originalPath);
                }

                return _originalPath;
            }
        }

	    /// <summary>
	    ///     Gets the route prefix.
	    /// </summary>
	    /// <value>The route prefix.</value>
	    [ConfigurationProperty("routePrefix", IsRequired = true, IsKey = true)]
        public string RoutePrefix => (string) this["routePrefix"];
    }
}