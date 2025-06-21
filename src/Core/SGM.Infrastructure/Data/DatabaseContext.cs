using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGM.Domain.Entities.Blogs;
using SGM.Domain.Entities.Users;
using SGM.Infrastructure.Helpers;

namespace SGM.Infrastructure.Data;

public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly string _connectionString;

    public DatabaseContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            DbContextHelpers.ConfigureSqlServer(_connectionString, options);
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
