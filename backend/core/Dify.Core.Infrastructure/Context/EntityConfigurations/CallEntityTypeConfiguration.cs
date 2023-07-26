using Dify.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dify.Core.Infrastructure.Context.EntityConfigurations;

public class CallEntityTypeConfiguration : IEntityTypeConfiguration<Call>
{
    public void Configure(EntityTypeBuilder<Call> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasOne(u => u.CreatedBy)
            .WithMany()
            .HasForeignKey(u => u.CreatedById)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .IsRequired();
        builder.HasOne(u => u.DroppedBy)
            .WithMany()
            .HasForeignKey(u => u.DroppedById)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(u => u.ModifiedBy)
            .WithMany()
            .HasForeignKey(u => u.ModifiedById)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder
            .HasMany(c => c.Participants)
            .WithOne(p => p.Call)
            .HasForeignKey(p => p.CallId);
    }
}
