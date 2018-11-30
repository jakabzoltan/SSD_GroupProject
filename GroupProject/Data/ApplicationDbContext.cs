using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;

namespace GroupProject.Data
{
    public class GroupProjectContext : IdentityDbContext<ApplicationUser>
    {
        public GroupProjectContext(DbContextOptions<GroupProjectContext> options)
            : base(options)
        {
        }
        public DbSet<CreditCard> CreditCard { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<GroupProject.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
