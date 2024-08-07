using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Persistence.Configurations
{
    public class PageConfiguration : IEntityTypeConfiguration<Domain.Page>
    {
        public void Configure(EntityTypeBuilder<Domain.Page> builder)
        {
            builder.HasIndex(x => new { x.Name })
                .IsUnique();

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Body)
                .IsRequired();

            builder.Property(x => x.ShortTitle)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.ToTable("Pages");
        }
    }
}