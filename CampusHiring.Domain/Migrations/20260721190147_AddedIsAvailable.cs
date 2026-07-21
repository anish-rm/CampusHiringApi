using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusHiring.Api.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsAvailable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "InterviewerAvailabilities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "InterviewerAvailabilities");
        }
    }
}
