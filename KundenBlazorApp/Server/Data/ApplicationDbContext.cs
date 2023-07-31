using KundenBlazorApp.Shared;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Numerics;

namespace KundenBlazorApp.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }
        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<ZiehungAuslosung> ZiehungAuslosungen { get; set; }
        public DbSet<KundeList> KundeListe { get; set; }
        public DbSet<WinKundeList> WinKundeListe { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

    }

    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                //Console.WriteLine(Enum.GetName(typeof(Role), role));
                string str = Enum.GetName(typeof(Role), role);
                if (!string.IsNullOrEmpty(str))
                {
                    builder.HasData(
                        new IdentityRole
                        {
                            Name = str,
                            NormalizedName = str.ToUpper()
                        });
                }
            }
        }
    }
}