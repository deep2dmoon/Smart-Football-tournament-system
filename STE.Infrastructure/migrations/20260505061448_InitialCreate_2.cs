using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.ste.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TournamentID",
                table: "Teams",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TournamentMode",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TournamentID",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TournamentID",
                table: "Teams",
                column: "TournamentID");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TournamentID",
                table: "Groups",
                column: "TournamentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Tournaments_TournamentID",
                table: "Groups",
                column: "TournamentID",
                principalTable: "Tournaments",
                principalColumn: "TournamentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Tournaments_TournamentID",
                table: "Teams",
                column: "TournamentID",
                principalTable: "Tournaments",
                principalColumn: "TournamentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Tournaments_TournamentID",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Tournaments_TournamentID",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Teams_TournamentID",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TournamentID",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TournamentID",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TournamentMode",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TournamentID",
                table: "Groups");
        }
    }
}
