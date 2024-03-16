using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace artshare_server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGenreArtwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkGenre");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Artworks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_GenreId",
                table: "Artworks",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Genres_GenreId",
                table: "Artworks",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Genres_GenreId",
                table: "Artworks");

            migrationBuilder.DropIndex(
                name: "IX_Artworks_GenreId",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Artworks");

            migrationBuilder.CreateTable(
                name: "ArtworkGenre",
                columns: table => new
                {
                    ArtworksArtworkId = table.Column<int>(type: "int", nullable: false),
                    GenresGenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkGenre", x => new { x.ArtworksArtworkId, x.GenresGenreId });
                    table.ForeignKey(
                        name: "FK_ArtworkGenre_Artworks_ArtworksArtworkId",
                        column: x => x.ArtworksArtworkId,
                        principalTable: "Artworks",
                        principalColumn: "ArtworkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtworkGenre_Genres_GenresGenreId",
                        column: x => x.GenresGenreId,
                        principalTable: "Genres",
                        principalColumn: "GenreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkGenre_GenresGenreId",
                table: "ArtworkGenre",
                column: "GenresGenreId");
        }
    }
}
