using System.Configuration;

namespace Cuplex.ImageProvider.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class GeneralApplicationConfig
    {
        private bool? _enableFluentImage;


        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralApplicationConfig"/> class.
        /// </summary>
        public GeneralApplicationConfig()
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable fluent image].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable fluent image]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableFluentImage
        {
            get
            {
                if (_enableFluentImage != null)
                {
                    return _enableFluentImage.Value;
                }

                bool.TryParse(ConfigurationManager.AppSettings["FluentImageEnabled"], out bool res);
                _enableFluentImage = res;

                return _enableFluentImage.Value;
            }
        }
    }
}
