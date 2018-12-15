using System;
using Microsoft.EntityFrameworkCore;
using User.Api.Models;

namespace User.Api.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().ToTable("T_Users")
                        .HasKey(u => u.Id);

            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>() .Property(u => u.Value).HasMaxLength(100);
            
            modelBuilder.Entity<UserProperty>()
                .ToTable("T_UserPropertys")
                .HasKey(u => new {u.Key, u.AppUserId, u.Value});

            modelBuilder.Entity<UserTag>().Property(u=>u.Tag).HasMaxLength(100);
            
            modelBuilder.Entity<UserTag>().ToTable("T_UserTags")
                .HasKey(u => new {u.AppUserId, u.Tag});
            
            modelBuilder.Entity<BPFile>()
                .ToTable("T_BPFiles")
                .HasKey(u => new {u.Id});
            
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<AppUser> Users { get; set; }
    }
}
