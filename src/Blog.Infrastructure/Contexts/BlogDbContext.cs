using System.Reflection;
using Blog.Application.Contexts;
using Blog.Domain;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Infrastructure.Contexts;

public class BlogDbContext : IdentityDbContext<User>, IMigratoryContext, IDbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options)
    {
        
    }
    
    static BlogDbContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    public DbSet<Project> Projects { get; set; }
    
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    public DbSet<Post> Posts { get; set; }
    
    public DbSet<PostFile> PostFiles { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Image> Images { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<TagAssignment> TagAssignments { get; set; }
    
    public DbSet<Page> Pages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}