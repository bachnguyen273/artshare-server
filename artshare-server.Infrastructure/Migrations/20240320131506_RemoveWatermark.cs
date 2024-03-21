using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace artshare_server.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWatermark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Watermarks_WatermarkId",
                table: "Artworks");

            migrationBuilder.DropTable(
                name: "Watermarks");

            migrationBuilder.DropIndex(
                name: "IX_Artworks_WatermarkId",
                table: "Artworks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Watermarks",
                columns: table => new
                {
                    WatermarkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<int>(type: "int", nullable: true),
                    WatermarkUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watermarks", x => x.WatermarkId);
                    table.ForeignKey(
                        name: "FK_Watermarks_Accounts_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_WatermarkId",
                table: "Artworks",
                column: "WatermarkId");

            migrationBuilder.CreateIndex(
                name: "IX_Watermarks_CreatorId",
                table: "Watermarks",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Watermarks_WatermarkId",
                table: "Artworks",
                column: "WatermarkId",
                principalTable: "Watermarks",
                principalColumn: "WatermarkId");
        }
    }
}
