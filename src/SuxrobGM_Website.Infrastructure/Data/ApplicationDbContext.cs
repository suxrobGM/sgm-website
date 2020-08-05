using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuxrobGM_Website.Core.Entities.BlogEntities;
using SuxrobGM_Website.Core.Entities.UserEntities;

namespace SuxrobGM_Website.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, UserRole, string>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {               
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectsV14;Database=SGM_BlogDB; AttachDbFilename=C:\\Users\\suxrobgm\\Databases\\SGM_BlogDB.mdf; Trusted_Connection=True; MultipleActiveResultSets=true")
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Change standard identity table names
            builder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<UserRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });

            builder.Entity<Blog>(entity =>
            {
                entity.HasMany(m => m.Comments)
                    .WithOne(m => m.Blog);

            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasMany(m => m.Replies)
                    .WithOne(m => m.ParentComment);
            });

            builder.Entity<BlogTag>(entity =>
            {
                entity.HasKey(k => new {k.BlogId, k.TagId});

                entity.HasOne(m => m.Blog)
                    .WithMany(m => m.BlogTags)
                    .HasForeignKey(m => m.BlogId);

                entity.HasOne(m => m.Tag)
                    .WithMany(m => m.BlogTags)
                    .HasForeignKey(m => m.TagId);
            });
        }
    }
}
