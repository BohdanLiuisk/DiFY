using DiFY.Modules.Social.Domain.FriendshipRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DiFY.Modules.Social.Infrastructure.Domain.FriendshipRequests
{
    internal class FriendshipRequestEntityTypeConfiguration : IEntityTypeConfiguration<FriendshipRequest>
    {
        public void Configure(EntityTypeBuilder<FriendshipRequest> builder)
        {
            builder.ToTable("FriendshipRequest", "social");
            builder.HasKey(x => x.Id);
            builder.Property<RequesterId>("_requesterId").HasColumnName("RequesterId");
            builder.Property<AddresseeId>("_addresseeId").HasColumnName("AddresseeId");
            builder.Property<DateTime>("_createdOn").HasColumnName("CreatedOn");
            builder.Property<DateTime?>("_confirmedOn").HasColumnName("ConfirmedOn");
            builder.Property<DateTime?>("_rejectedOn").HasColumnName("RejectedOn");
        }
    }
}
