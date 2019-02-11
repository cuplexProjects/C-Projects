using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebNotes.Library.Identity
{
    public class AppUserStore : UserStore<AppUser, AppRole, int, AppLogin, AppUserRole, AppClaim>
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor which takes a db context and wires up the stores with default instances using the context
        /// </summary>
        public AppUserStore(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// Fins a user by email as an asynchronous operation.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="systemId">The system identifier.</param>
        /// <returns>Task&lt;AppUser&gt;.</returns>
        public async Task<AppUser> FindByEmailAsync(string email, int systemId) => await GetUserAggregateAsync(u => u.Email == email);

        /// <summary>
        /// Find a user by username as an asynchronous operation.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="systemId">The system identifier.</param>
        /// <returns>Task&lt;AppUser&gt;.</returns>
        public async Task<AppUser> FindByNameAsync(string username, int systemId) => await GetUserAggregateAsync(u => u.UserName == username);
    }
}
