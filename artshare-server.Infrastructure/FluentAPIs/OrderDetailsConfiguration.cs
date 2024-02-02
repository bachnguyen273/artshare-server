using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.FluentAPIs
{
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.HasKey(t => new { t.OrderId, t.ArtworkId });
            builder.Property(t => t.UnitPrice)
                .IsRequired()
                .HasPrecision(19, 4);
        }
    }
}