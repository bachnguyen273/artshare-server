using artshare_server.Core.Enums;
using artshare_server.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace artshare_server.Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(t => t.AccountId);
            builder.Property(t => t.Email)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(t => t.PasswordHash)
                .HasMaxLength(72)
                .IsRequired();
            builder.Property(t => t.AvatarUrl)
                .HasMaxLength(255);
            builder.Property(t => t.UserName)
                .HasMaxLength(36)
                .IsRequired();
            builder.Property(t => t.FullName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(t => t.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();
            builder.Property(t => t.JoinDate)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");
            builder.Property(t => t.Role)
                .IsRequired()
                .HasColumnType("nvarchar(10)");
            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnType("nvarchar(10)")
                .HasDefaultValue(AccountStatus.Active);
            builder.HasMany(t => t.Artworks)
                .WithOne(e => e.Creator)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(t => t.Orders)
                .WithOne(e => e.Customer)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(t => t.Likes)
                .WithOne(e => e.Account)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(t => t.Comments)
                .WithOne(e => e.Commenter)
                .HasForeignKey(x => x.CommenterId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(t => t.Reports)
                .WithOne(e => e.Account)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}