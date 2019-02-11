using System.ComponentModel.DataAnnotations;

namespace OauthClient.Models
{
    /// <summary>
    /// Account ViewModel
    /// </summary>
    public class AccountViewModel
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [Display(Name = "Access Token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        [Display(Name = "Refresh Token")]
        public string RefreshToken { get; set; }
        /// <summary>
        /// Gets or sets the mesage.
        /// </summary>
        /// <value>
        /// The mesage.
        /// </value>
        [Display(Name = "Information")]
        public string Mesage { get; set; }
    }
}