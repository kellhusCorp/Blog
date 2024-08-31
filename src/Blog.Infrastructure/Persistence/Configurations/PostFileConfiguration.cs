using Blog.Domain;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.Configurations
{
    public class PostFileConfiguration : IEntityTypeConfiguration<PostFile>
    {
        public void Configure(EntityTypeBuilder<PostFile> builder)
        {
            builder.ToTable("PostFiles");
        }
    }
}