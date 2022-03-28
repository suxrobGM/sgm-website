using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGM.Domain.Entities.BlogEntities;
using SGM.Domain.Entities.UserEntities;

namespace SGM.EntityFramework.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
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
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Initial Catalog=SGM_BlogDB; Trusted_Connection=True")
                .UseLazyLoadingProxies();
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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

        builder.Entity<Tag>(entity =>
        {
            entity.HasMany(m => m.Blogs)
                .WithMany(m => m.Tags);
        });
    }
}
