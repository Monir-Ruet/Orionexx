using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orionexx.Events.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageType",
                table: "Events",
                newName: "EventType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventType",
                table: "Events",
                newName: "MessageType");
        }
    }
}
