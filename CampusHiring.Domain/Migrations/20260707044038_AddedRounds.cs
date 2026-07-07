using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusHiring.Api.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedRounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "AssessmentTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Round",
                table: "AssessmentTypes");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "Assessments");
        }
    }
}
