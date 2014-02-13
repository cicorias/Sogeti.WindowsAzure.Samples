using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebForms.Cloud.Multiple.Models
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<IssuingAuthorityKey> IssuingAuthorityKeys { get; set; }

        public DbSet<SignupToken> SignupTokens { get; set; }
    }
}