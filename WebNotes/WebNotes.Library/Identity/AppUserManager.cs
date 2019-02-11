using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace WebNotes.Library.Identity
{
    public class AppUserManager : UserManager<AppUser, int>
    {
        private string _systemCountryCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Flipp.Library.Identity.AppUserManager" /> class.
        /// </summary>
        /// <exception cref="System.ArgumentException">Invalid system country code. - systemCountryCode</exception>
        /// <inheritdoc />
        public AppUserManager(AppUserStore store, IDataProtectionProvider dataProtectionProvider) : base(store)
        { 
            // Configure validation logic for usernames
            UserValidator = new UserValidator<AppUser, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6
            };

            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<AppUser, int>(dataProtectionProvider.Create("ASP.NET Identity")) { TokenLifespan = TimeSpan.FromHours(30) };
            }
        }

        #region Overrides of UserManager<AppUser,int>

        /// <inheritdoc />
        /// <summary>
        /// Create a user with no password
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        /// <exception cref="T:System.Exception">Use the overload with password an countryCode.</exception>
        public override async Task<IdentityResult> CreateAsync(AppUser user)
        {
            EnsureValidCountryCode(_systemCountryCode);
            return await base.CreateAsync(user);
        }

        /// <inheritdoc />
        /// <summary>
        /// create as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public override async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            EnsureValidCountryCode(_systemCountryCode);
            return await base.CreateAsync(user, password);
        }

        /// <inheritdoc />
        /// <summary>
        /// find as an asynchronous operation.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>Task&lt;AppUser&gt;.</returns>
        public override async Task<AppUser> FindAsync(string userName, string password) => await FindAsync(userName, password, _systemCountryCode);

        /// <inheritdoc />
        /// <summary>
        /// find by email as an asynchronous operation.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>Task&lt;AppUser&gt;.</returns>
        /// <exception cref="T:System.ArgumentNullException">email</exception>
        public override async Task<AppUser> FindByEmailAsync(string email) => await FindByEmailAsync(email);

        /// <inheritdoc />
        /// <summary>
        /// find by name as an asynchronous operation.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Task&lt;AppUser&gt;.</returns>
        /// <exception cref="T:System.ArgumentNullException">userName</exception>
        public override async Task<AppUser> FindByNameAsync(string userName) => await FindByNameAsync(userName);

        /// <inheritdoc />
        /// <summary>
        /// update as an asynchronous operation.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        public override async Task<IdentityResult> UpdateAsync(AppUser user) => await UpdateAsync(user, _systemCountryCode);

        #endregion

        /// <summary>
        /// change password as an asynchronous operation.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        /// <exception cref="System.Exception">Current UserStore doesn't implement IUserPasswordStore</exception>
        /// <exception cref="System.InvalidOperationException">User not found</exception>
        public virtual async Task<IdentityResult> ChangePasswordAsync(int userId, string newPassword)
        {
            if (!(Store is IUserPasswordStore<AppUser, int> passwordStore))
            {
                return new IdentityResult("Current UserStore doesn't implement IUserPasswordStore");
            }

            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                return new IdentityResult("User not found.");
            }

            return await UpdatePassword(passwordStore, user, newPassword);
        }

        /// <summary>
        /// find an <see cref="AppUser" /> as an asynchronous operation.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="systemCountryCode">The system country code.</param>
        /// <returns>Task&lt;AppUser&gt;.</returns>
        public async Task<AppUser> FindAsync(string userName, string password, string systemCountryCode)
        {
            _systemCountryCode = systemCountryCode;
            EnsureValidCountryCode(systemCountryCode);
            userName = $"{systemCountryCode}-{userName}";

            return await base.FindAsync(userName, password);
        }

        /// <summary>
        /// find by email as an asynchronous operation.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="systemCountryCode">The system country code.</param>
        /// <returns>Task&lt;AppUser&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">email</exception>
        /// <exception cref="ArgumentNullException">email</exception>
        /// <exception cref="T:System.ArgumentNullException">email</exception>




        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="systemCountryCode">The system country code.</param>
        /// <returns>Task&lt;IdentityResult&gt;.</returns>
        /// <exception cref="ArgumentNullException">user</exception>
        public Task<IdentityResult> UpdateAsync(AppUser user, string systemCountryCode)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return base.UpdateAsync(user);
        }


        private AppUserStore AppUserStore => Store as AppUserStore;

        private void EnsureValidCountryCode(string systemCountryCode)
        {
            if (systemCountryCode == "--")
            {
                throw new ArgumentException(@"Invalid system country code.", nameof(systemCountryCode));
            }
        }
    }
}
