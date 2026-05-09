using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.ste.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class NextCreate_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StandingsTableID",
                table: "Standings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Standings_StandingsTableID",
                table: "Standings",
                column: "StandingsTableID");

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_StandingsTables_StandingsTableID",
                table: "Standings",
                column: "StandingsTableID",
                principalTable: "StandingsTables",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standings_StandingsTables_StandingsTableID",
                table: "Standings");

            migrationBuilder.DropIndex(
                name: "IX_Standings_StandingsTableID",
                table: "Standings");

            migrationBuilder.DropColumn(
                name: "StandingsTableID",
                table: "Standings");
        }
    }
}
