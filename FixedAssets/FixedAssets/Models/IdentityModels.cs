using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FixedAssets.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Asset> T_Assets { get; set; }
        public DbSet<AssetLocation> T_AssetLocations { get; set; }
        public DbSet<AssetType> T_AssetTypes { get; set; }
        public DbSet<DepreciationType> T_DepreciationTypes { get; set; }
        public DbSet<DepreciationCharge> T_DepreciationCharges { get; set; }
        public DbSet<ChangeInValue> T_ChangeInValues { get; set; }
        public DbSet<ReasonForChanging> T_ReasonForChangings { get; set; }
        public DbSet<EndOfLifeDisposal> T_EndOfLifeDisposals { get; set; }
        
        //public DbSet<IDbDictionary> T_IDbDictionary { get; set; }
    }


}