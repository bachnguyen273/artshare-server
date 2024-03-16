using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.FluentAPIs
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(t => t.GenreId);
            builder.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.HasMany(t => t.Artworks)
                .WithOne(e => e.Genre)
                .HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}