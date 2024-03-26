using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.FluentAPIs
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(t => t.OrderId);
            builder.Property(t => t.CreateDate)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");
            builder.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(19, 4);
        }
    }
}