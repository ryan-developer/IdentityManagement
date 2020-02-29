using IdentityManagement.Persistence.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityManagement.Persistence.Identity
{
    public class TenantUserDbContext : IdentityDbContext<TenantUserEntity>
    {
        public TenantUserDbContext(DbContextOptions<TenantUserDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<TenantUserEntity>()
                .HasMany(d => d.Claims)
                .WithOne()
                .HasForeignKey("UserId");

            builder
                .Entity<TenantUserEntity>()
                .HasMany(d => d.Roles)
                .WithOne()
                .HasForeignKey("UserId");
        }
    }
}
