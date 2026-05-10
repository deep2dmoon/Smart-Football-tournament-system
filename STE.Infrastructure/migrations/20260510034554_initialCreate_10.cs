using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.ste.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class initialCreate_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standings_StandingsTables_StandingsTableID",
                table: "Standings");

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_StandingsTables_StandingsTableID",
                table: "Standings",
                column: "StandingsTableID",
                principalTable: "StandingsTables",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standings_StandingsTables_StandingsTableID",
                table: "Standings");

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_StandingsTables_StandingsTableID",
                table: "Standings",
                column: "StandingsTableID",
                principalTable: "StandingsTables",
                principalColumn: "ID");
        }
    }
}
