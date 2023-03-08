using System;
using Microsoft.EntityFrameworkCore;
using DiFY.Modules.Social.Domain.Membership;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiFY.Modules.Social.Infrastructure.Domain.Membership;

public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members", "social");
        builder.HasKey(x => x.Id);
        builder.Property<string>("_login").HasColumnName("Login");
        builder.Property<string>("_email").HasColumnName("Email");
        builder.Property<string>("_firstName").HasColumnName("FirstName");
        builder.Property<string>("_lastName").HasColumnName("LastName");
        builder.Property<string>("_name").HasColumnName("Name");
        builder.Property<DateTime>("_createdOn").HasColumnName("CreatedOn");
        builder.Property<string>("_avatarUrl").HasColumnName("AvatarUrl");
    }
}