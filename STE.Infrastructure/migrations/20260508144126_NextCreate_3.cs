using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.ste.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class NextCreate_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GroupID",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GroupID",
                table: "Matches",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Groups_GroupID",
                table: "Matches",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "GroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Groups_GroupID",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_GroupID",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "Matches");
        }
    }
}
