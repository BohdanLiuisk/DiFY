using Dify.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dify.Core.Infrastructure.Context.EntityConfigurations;

public class CallParticipantEntityTypeConfiguration : IEntityTypeConfiguration<CallParticipant>
{
    public void Configure(EntityTypeBuilder<CallParticipant> builder)
    {
        builder.HasKey(p => p.Id);
        builder
            .HasOne(p => p.Participant)
            .WithMany()
            .HasForeignKey(p => p.ParticipantId)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}
