using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebNotes.Library.Helpers;

namespace WebNotes.Library.Identity
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppLogin, AppUserRole, AppClaim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        public AppDbContext()
            : base(ConnectionStringHelper.WebNotes)
        {
            Database.SetInitializer(new IdentityCustomInitializer());
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>AppDbContext.</returns>
        public static AppDbContext Create() => new AppDbContext();

        /// <summary>
        /// Maps table names, and sets up relationships between the various user entities
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().ToTable("Account");
            modelBuilder.Entity<AppUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AppRole>().ToTable("Role");
            modelBuilder.Entity<AppClaim>().ToTable("UserClaim");
            modelBuilder.Entity<AppLogin>().ToTable("UserLogin");
            modelBuilder.Entity<AppUserRole>().ToTable("UserRole");

            // Override some column mappings that do not match our default
            modelBuilder.Entity<AppUser>().Property(r => r.Id).HasColumnName("IDAccount");
            modelBuilder.Entity<AppUser>().Property(r => r.Email).HasColumnName("EmailAddress");
            modelBuilder.Entity<AppUser>().Property(r => r.PasswordHash).HasColumnName("Password");

            modelBuilder.Entity<AppClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AppRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
