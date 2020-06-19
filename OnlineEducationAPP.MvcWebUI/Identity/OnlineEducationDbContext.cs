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
        public DbSet<Event> Events { get; set; }
        public DbSet<Message> Messages { get; set; }

        
        public OnlineEducationDbContext(DbContextOptions<OnlineEducationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //builder.Entity<ApplicationUser>()
            //    .HasMany(a => a.Stream)
            //    .WithOne(b => b.User)
            //    .HasForeignKey(s =>s.UserId);

            builder.Entity<Stream>()
                .HasIndex(t => t.UserId)
                .IsUnique(false);

            builder.Entity<ApplicationUser>(b => {
                b.HasMany(x => x.Roles)
                .WithOne()
                .HasForeignKey(u => u.UserId)
                .IsRequired();
            });
            builder.Entity<Message>()
                .HasIndex(t => t.ReceiverId)
                .IsUnique(false);

            builder.Entity<Message>()
                .HasOne(a => a.ReceiverUser)
                .WithMany(c => c.ReceiverMessages)
                .HasForeignKey(t => t.ReceiverId)
                .HasPrincipalKey(t=>t.Id);

            builder.Entity<Message>()
                .HasOne(a => a.SenderUser)
                .WithMany(c => c.SenderMessages)
                .HasForeignKey(t => t.SenderId)
                .HasPrincipalKey(t =>t.Id);


        }
    }
}
