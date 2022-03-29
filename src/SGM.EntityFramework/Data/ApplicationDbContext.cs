using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SGM.EntityFramework.Data;

public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly string connectionString;

    public DatabaseContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            DbContextHelpers.ConfigureSqlServer(connectionString, options);
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
