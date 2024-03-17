using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.FluentAPIs
{
    public class ArtworkConfiguration : IEntityTypeConfiguration<Artwork>
    {
        public void Configure(EntityTypeBuilder<Artwork> builder)
        {
            builder.HasKey(t => t.ArtworkId);
            builder.Property(t => t.Title)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(t => t.OriginalArtUrl)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(t => t.WatermarkedArtUrl)
                .HasMaxLength(255);
            builder.Property(t => t.Description)
                .HasMaxLength(1000);
            builder.Property(t => t.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");
            builder.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(19, 4);
            builder.Property(t => t.LikeCount)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(t => t.DislikeCount)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(t => t.CommentCount)
                .IsRequired()
                .HasDefaultValue(0);
            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnType("nvarchar(10)");
            builder.HasMany(t => t.OrderDetails)
                .WithOne(e => e.Artwork)
                .HasForeignKey(x => x.ArtworkId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(t => t.Likes)
                .WithOne(e => e.Artwork)
                .HasForeignKey(x => x.ArtworkId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(t => t.Comments)
                .WithOne(e => e.Artwork)
                .HasForeignKey(x => x.ArtworkId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(t => t.Reports)
                .WithOne(e => e.Artwork)
                .HasForeignKey(x => x.ArtworkId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}