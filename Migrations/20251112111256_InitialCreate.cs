using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketSys.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "created_at", "email", "full_name", "password", "role" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 12, 11, 12, 54, 511, DateTimeKind.Utc).AddTicks(7505), "admin@test.com", "Admin User", "$2a$11$NaPC3t5xq7SyhPW4vKXgUeR8kDIp9/wgYkfbrG8FsNMkSVFZ09gOm", "Admin" },
                    { 2, new DateTime(2025, 11, 12, 11, 12, 54, 649, DateTimeKind.Utc).AddTicks(7973), "customer@test.com", "John Doe", "$2a$11$qMe4fXblZCoXvyzDJwtzn.QzE.AM7zGyO1gmzP1jWOjDFFPcDmlRS", "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "created_at", "description", "priority", "status", "title", "updated_at", "user_id" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 12, 11, 12, 54, 649, DateTimeKind.Utc).AddTicks(8858), "Cannot login to my account", "High", "Open", "Login issue", null, 2 },
                    { 2, new DateTime(2025, 11, 12, 11, 12, 54, 649, DateTimeKind.Utc).AddTicks(8860), "Please add dark mode", "Low", "InProgress", "Feature request", null, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_user_id",
                table: "Tickets",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
