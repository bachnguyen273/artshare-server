using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.FluentAPIs
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(t => t.CommentId);
            builder.Property(t => t.Content)
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(t => t.PostDate)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");
        }
    }
}