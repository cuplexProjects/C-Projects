using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebNotes.Library.Identity
{
    public class AppUser : IdentityUser<int, AppLogin, AppUserRole, AppClaim>
    {

        /// <summary>
        /// Gets or sets the account status.
        /// </summary>
        /// <value>The account status.</value>
        public int AccountStatus { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }



        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode { get; set; }
        
        /// <summary>
        /// Gets or sets the final expire.
        /// </summary>
        /// <value>The final expire.</value>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime? FinalExpire { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [accept information].
        /// </summary>
        /// <value><c>null</c> if [accept information] contains no value, <c>true</c> if [accept information]; otherwise, <c>false</c>.</value>
        public bool? AcceptInformation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account has been cancelled.
        /// </summary>
        /// <value><c>null</c> if [account cancelled] contains no value, <c>true</c> if [account cancelled]; otherwise, <c>false</c>.</value>
        public bool AccountCancelled { get; set; }

        /// <summary>
        /// Gets or sets the exp date.
        /// </summary>
        /// <value>The exp date.</value>
        public DateTime ExpDate { get; set; }


        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        /// <value>The profiles.</value>
        public IEnumerable<Profile> Profiles { get; set; }


        /// <summary>
        /// generate user identity as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns>Task&lt;ClaimsIdentity&gt;.</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser, int> manager)
        {
            // Logger.Log("SimpleUser:GenerateUserIdentityAsync ()");
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        /// <summary>
        /// Generates the user identity.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <returns>ClaimsIdentity.</returns>
        public ClaimsIdentity GenerateUserIdentity(UserManager<AppUser, int> manager)
        {
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => $"Id={Id} Username={UserName} PasswordHash={PasswordHash ?? string.Empty}";
    }
}
