using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisCompetitionApi.Migrations
{
    public partial class AddedScoreField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Score",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Matches");
        }
    }
}
