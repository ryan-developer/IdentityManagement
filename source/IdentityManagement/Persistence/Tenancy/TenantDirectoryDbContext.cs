using System.Collections.Generic;
using IdentityManagement.Persistence.Tenancy.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityManagement.Persistence.Tenancy
{
    public class TenantDirectoryDbContext : DbContext
    {
        public TenantDirectoryDbContext(DbContextOptions<TenantDirectoryDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public virtual DbSet<TenantEntity> Tenants { get; set; }
    }
}