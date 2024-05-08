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
                name: "entity_descriptor",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    caption = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    columns_json = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entity_descriptor", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entity_descriptor");
        }
    }
}
