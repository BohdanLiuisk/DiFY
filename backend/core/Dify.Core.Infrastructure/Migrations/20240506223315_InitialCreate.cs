using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dify.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: false),
                    last_name = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: false),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    online = table.Column<bool>(type: "boolean", nullable: false),
                    connection_id = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: true),
                    modified_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_users_created_by_id1",
                        column: x => x.created_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_users_users_modified_by_id",
                        column: x => x.modified_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "calls",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    dropped_by_id = table.Column<int>(type: "integer", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    duration = table.Column<double>(type: "double precision", nullable: false),
                    created_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    modified_by_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_calls", x => x.id);
                    table.ForeignKey(
                        name: "fk_calls_users_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_calls_users_dropped_by_id",
                        column: x => x.dropped_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_calls_users_modified_by_id",
                        column: x => x.modified_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "call_participants",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    call_id = table.Column<Guid>(type: "uuid", nullable: false),
                    participant_id = table.Column<int>(type: "integer", nullable: false),
                    joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    stream_id = table.Column<string>(type: "text", nullable: true),
                    peer_id = table.Column<string>(type: "text", nullable: true),
                    connection_id = table.Column<string>(type: "text", nullable: true),
                    direction = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_call_participants", x => x.id);
                    table.ForeignKey(
                        name: "fk_call_participants_calls_call_id",
                        column: x => x.call_id,
                        principalTable: "calls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_call_participants_users_participant_id",
                        column: x => x.participant_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_call_participants_call_id",
                table: "call_participants",
                column: "call_id");

            migrationBuilder.CreateIndex(
                name: "ix_call_participants_participant_id",
                table: "call_participants",
                column: "participant_id");

            migrationBuilder.CreateIndex(
                name: "ix_calls_created_by_id",
                table: "calls",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_calls_dropped_by_id",
                table: "calls",
                column: "dropped_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_calls_modified_by_id",
                table: "calls",
                column: "modified_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_created_by_id",
                table: "users",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_modified_by_id",
                table: "users",
                column: "modified_by_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "call_participants");

            migrationBuilder.DropTable(
                name: "calls");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
