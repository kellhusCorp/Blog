using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Domain.Tag>
    {
        public void Configure(EntityTypeBuilder<Domain.Tag> builder)
        {
            builder.HasIndex(m => new { m.Name })
                .IsUnique();

            builder.ToTable("PostTags");
        }
    }
}