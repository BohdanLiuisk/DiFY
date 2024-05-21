﻿// <auto-generated />
using System;
using Dify.Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dify.Entity.WebApi.Migrations
{
    [DbContext(typeof(DifyEntityContext))]
    [Migration("20240508201038_InitialDifyEntityMigration")]
    partial class InitialDifyEntityMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dify.Entity.Models.EntityDescriptor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("caption");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("code");

                    b.Property<string>("ColumnsJson")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("columns_json");

                    b.HasKey("Id")
                        .HasName("pk_entity_descriptor");

                    b.ToTable("entity_descriptor", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
