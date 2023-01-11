using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.Domain;

namespace MyBlogOnCore.DataSource.Contexts;

public class BlogDbContext : IdentityDbContext<User>, IMigratoryContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options)
    {
        
    }
    
    static BlogDbContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }
    
    public virtual DbSet<BlogFile> Files { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagAssignment> TagAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        builder.Entity<TagAssignment>()
            .HasKey(m => new { m.BlogId, m.TagId });

        builder.Entity<Blog>()
            .HasIndex(m => new { m.PermanentLink })
            .IsUnique(true);

        builder.Entity<Tag>()
            .HasIndex(m => new { m.Name })
            .IsUnique(true);

        base.OnModelCreating(builder);
    }
}