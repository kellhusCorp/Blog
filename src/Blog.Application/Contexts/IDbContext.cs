using Blog.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Contexts
{
    public interface IDbContext
    {
        public DbSet<Post> Posts { get; set; }
        
        public DbSet<PostFile> PostFiles { get; set; }
        
        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<Image> Images { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        
        public DbSet<TagAssignment> TagAssignments { get; set; }
        
        public DbSet<Page> Pages { get; set; }
        
        public DbSet<Project> Projects { get; set; }
    }
}