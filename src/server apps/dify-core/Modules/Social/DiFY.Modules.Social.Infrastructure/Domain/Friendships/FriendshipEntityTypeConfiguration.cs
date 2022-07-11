using DiFY.Modules.Social.Domain.FriendshipRequests;
using DiFY.Modules.Social.Domain.Friendships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DiFY.Modules.Social.Infrastructure.Domain.Friendships
{
    internal class FriendshipEntityTypeConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendship", "social");
            builder.HasKey(x => x.Id);
            builder.Property<RequesterId>("_requesterId").HasColumnName("RequesterId");
            builder.Property<AddresseeId>("_addresseeId").HasColumnName("AddresseeId");
            builder.Property<DateTime>("_createdOn").HasColumnName("CreatedOn");
        }
    }
}
