using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations
{
    public class PostFileConfiguration : IEntityTypeConfiguration<PostFile>
    {
        public void Configure(EntityTypeBuilder<PostFile> builder)
        {
            builder.ToTable("PostFiles");
        }
    }
}