using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Winku.DatabaseFolder
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Followers> Followers { get; set; }
        public DbSet<Comments> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Like>()
                        .HasOne(l => l.Post)
                        .WithMany(p => p.Likes)
                        .HasForeignKey(l => l.PostId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.ApplicationUser)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Comments>()
                       .HasOne(l => l.Post)
                       .WithMany(p => p.Comments)
                       .HasForeignKey(l => l.PostId)
                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comments>()
                .HasOne(l => l.ApplicationUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Followers>()
                   .HasOne(m => m.ApplicationUser1)
                   .WithMany(t => t.Followers1)
                   .HasForeignKey(m => m.FollowerId)
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Followers>()
                        .HasOne(m => m.ApplicationUser2)
                        .WithMany(t => t.Followers2)
                        .HasForeignKey(m => m.FollowingId)
                       .OnDelete(DeleteBehavior.Restrict);

        }
    }
}