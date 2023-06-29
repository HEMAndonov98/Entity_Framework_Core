using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventMi.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Event identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "The event name"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Start date and time of event"),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "End date and time of event"),
                    Place = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Place where the event is being held")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                },
                comment: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
