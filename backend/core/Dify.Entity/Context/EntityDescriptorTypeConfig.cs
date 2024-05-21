using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dify.Entity.Context;

public class EntityDescriptorTypeConfig : IEntityTypeConfiguration<EntityDescriptor>
{
    public void Configure(EntityTypeBuilder<EntityDescriptor> builder) {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Code)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.Caption)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.ColumnsJson)
            .IsRequired();
    }
}
