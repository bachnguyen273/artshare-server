using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.FluentAPIs
{
    public class WatermarkConfiguration : IEntityTypeConfiguration<Watermark>
    {
        public void Configure(EntityTypeBuilder<Watermark> builder)
        {
            builder.HasKey(t => t.WatermarkId);
            builder.Property(t => t.WatermarkUrl)
                .HasMaxLength(255)
                .IsRequired();
            builder.HasMany(t => t.Artworks)
                .WithOne(e => e.Watermark)
                .HasForeignKey(x => x.WatermarkId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}