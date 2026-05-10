using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.ste.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class initialCreate_9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Groups_GroupID",
                table: "Matches");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Groups_GroupID",
                table: "Matches",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Groups_GroupID",
                table: "Matches");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Groups_GroupID",
                table: "Matches",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupID");
        }
    }
}
