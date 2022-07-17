using System;
using DiFY.Modules.Social.Domain.Calling;
using DiFY.Modules.Social.Domain.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiFY.Modules.Social.Infrastructure.Domain.Calling;

public class CallEntityTypeConfiguration : IEntityTypeConfiguration<Call>
{
    public void Configure(EntityTypeBuilder<Call> builder)
    {
        builder.Ignore(c => c.Participants);
        builder.ToTable("Calls", "social");
        builder.HasKey(c => c.Id);
        builder.Property<MemberId>("_initiatorId").HasColumnName("InitiatorId");
        builder.Property<DateTime>("_startDate").HasColumnName("StartDate");
        builder.Property<DateTime?>("_endDate").HasColumnName("EndDate");
        builder.OwnsOne<Duration>("_duration", b =>
        {
            b.Property(d => d.Value).HasColumnName("Duration");
        });
        builder.OwnsMany<CallParticipant>("_participants", b =>
        {
            b.WithOwner().HasForeignKey("CallId");
            b.ToTable("CallParticipants", "social");
            b.HasKey(p => p.Id);
            b.Property<MemberId>("ParticipantId").HasColumnName("ParticipantId");
            b.Property<CallId>("CallId").HasColumnName("CallId");
            b.Property<DateTime>("_joinDate").HasColumnName("JoinOn");
        });
    }
}