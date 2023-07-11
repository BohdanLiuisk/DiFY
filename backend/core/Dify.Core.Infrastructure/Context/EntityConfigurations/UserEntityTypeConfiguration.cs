using Dify.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dify.Core.Infrastructure.Context.EntityConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Login)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(125);
        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(125);
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(250);
        builder.HasIndex(u => u.Login)
            .IsUnique();
        builder.Property(u => u.Password)
            .IsRequired();
        builder.Property(u => u.Email)
            .IsRequired();
        builder.HasOne(u => u.CreatedBy)
            .WithMany()
            .HasForeignKey(u => u.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .IsRequired(false);
        builder.HasOne(u => u.ModifiedBy)
            .WithMany()
            .HasForeignKey(u => u.ModifiedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .IsRequired(false);
    }
}
