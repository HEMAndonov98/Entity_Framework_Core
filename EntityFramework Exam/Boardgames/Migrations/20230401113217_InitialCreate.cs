using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boardgames.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boardgame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    YearPublished = table.Column<int>(type: "int", nullable: false),
                    CategoryType = table.Column<int>(type: "int", nullable: false),
                    Mechanics = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CreatorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boardgame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boardgame_Creator_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Creator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardgameSeller",
                columns: table => new
                {
                    BoardgameId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardgameSeller", x => new { x.BoardgameId, x.SellerId });
                    table.ForeignKey(
                        name: "FK_BoardgameSeller_Boardgame_BoardgameId",
                        column: x => x.BoardgameId,
                        principalTable: "Boardgame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardgameSeller_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boardgame_CreatorId",
                table: "Boardgame",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardgameSeller_SellerId",
                table: "BoardgameSeller",
                column: "SellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardgameSeller");

            migrationBuilder.DropTable(
                name: "Boardgame");

            migrationBuilder.DropTable(
                name: "Seller");

            migrationBuilder.DropTable(
                name: "Creator");
        }
    }
}
