using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deck",
                columns: table => new
                {
                    DeckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deck", x => x.DeckId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActiveColour = table.Column<int>(type: "int", nullable: false),
                    ActiveValue = table.Column<int>(type: "int", nullable: false),
                    IsPlayed = table.Column<bool>(type: "bit", nullable: false),
                    DeckDbDTODeckId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.ForeignKey(
                        name: "FK_Cards_Deck_DeckDbDTODeckId",
                        column: x => x.DeckDbDTODeckId,
                        principalTable: "Deck",
                        principalColumn: "DeckId");
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeckDbDTODeckId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerNameId);
                    table.ForeignKey(
                        name: "FK_Players_Deck_DeckDbDTODeckId",
                        column: x => x.DeckDbDTODeckId,
                        principalTable: "Deck",
                        principalColumn: "DeckId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckDbDTODeckId",
                table: "Cards",
                column: "DeckDbDTODeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_DeckDbDTODeckId",
                table: "Players",
                column: "DeckDbDTODeckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Deck");
        }
    }
}
