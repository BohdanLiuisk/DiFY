using Dify.Entity.Structure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dify.Entity.Context;

public class EntityStructureTypeConfig : IEntityTypeConfiguration<EntityStructure>
{
    public void Configure(EntityTypeBuilder<EntityStructure> builder) {
        builder.ToTable("entity_structure");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.Caption)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.ColumnsJson)
            .HasColumnType("json")
            .HasColumnName("columns")
            .IsRequired();
        builder.Property(u => u.ForeignKeysJson)
            .HasColumnType("json")
            .HasColumnName("foreign_keys");
        builder.Property(u => u.IndexesJson)
            .HasColumnType("json")
            .HasColumnName("indexes");
        builder.Ignore(u => u.Columns);
        builder.Ignore(u => u.ForeignKeys);
        builder.Ignore(u => u.Indexes);
    }
}
