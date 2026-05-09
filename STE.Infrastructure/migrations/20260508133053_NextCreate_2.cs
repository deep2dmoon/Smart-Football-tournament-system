using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.ste.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class NextCreate_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TableID",
                table: "Standings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standings_TableID",
                table: "Standings",
                column: "TableID");

            migrationBuilder.AddForeignKey(
                name: "FK_Standings_Tables_TableID",
                table: "Standings",
                column: "TableID",
                principalTable: "Tables",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standings_Tables_TableID",
                table: "Standings");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_Standings_TableID",
                table: "Standings");

            migrationBuilder.DropColumn(
                name: "TableID",
                table: "Standings");
        }
    }
}
