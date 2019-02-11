using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OauthClient.Models
{
    /// <summary>
    /// MediaconectProfile ViewModel
    /// </summary>
    public class MediaconectProfileViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is authenticated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string MiddleName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the email addres.
        /// </summary>
        /// <value>
        /// The email addres.
        /// </value>
        public string EmailAddres { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the customer number.
        /// </summary>
        /// <value>
        /// The customer number.
        /// </value>
        public string CustomerNumber { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public string UniqueId { get; set; }
    }
}