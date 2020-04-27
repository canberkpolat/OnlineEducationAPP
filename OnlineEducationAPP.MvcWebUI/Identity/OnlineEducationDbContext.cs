using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineEducationAPP.MvcWebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationAPP.MvcWebUI.Identity
{
    public class OnlineEducationDbContext : IdentityDbContext<ApplicationUser,IdentityRole ,string>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Stream> Streams { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public OnlineEducationDbContext(DbContextOptions<OnlineEducationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Entity<ApplicationUser>()
                .HasMany(a => a.Stream)
                .WithOne(b => b.User);

            builder.Entity<Stream>()
                .HasIndex(t => t.UserId)
                .IsUnique(false);
        }
    }
}
