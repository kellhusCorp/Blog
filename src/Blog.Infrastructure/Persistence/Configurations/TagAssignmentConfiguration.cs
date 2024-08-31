using Blog.Domain;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.Configurations
{
    public class TagAssignmentConfiguration : IEntityTypeConfiguration<TagAssignment>
    {
        public void Configure(EntityTypeBuilder<TagAssignment> builder)
        {
            builder.HasKey(m => new { m.BlogId, m.TagId });
            builder.ToTable("PostTagAssignments");
        }
    }
}