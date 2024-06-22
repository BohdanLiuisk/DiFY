using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dify.Entity.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialDifyEntityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "entity_structure",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    caption = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    columns = table.Column<string>(type: "json", nullable: false),
                    foreign_keys = table.Column<string>(type: "json", nullable: false),
                    indexes = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entity_structure", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entity_structure");
        }
    }
}
