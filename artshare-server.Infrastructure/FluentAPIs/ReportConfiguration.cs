using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.FluentAPIs
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(t => t.ReportId);
            builder.Property(t => t.Content)
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(t => t.Category)
                .IsRequired()
                .HasColumnType("nvarchar(50)");
            builder.Property(t => t.ReportDate)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");
            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnType("nvarchar(10)");
        }
    }
}