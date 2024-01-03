using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dify.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCallDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "CallParticipants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "CallParticipants");
        }
    }
}
