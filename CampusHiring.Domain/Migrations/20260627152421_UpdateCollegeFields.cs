using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusHiring.Api.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCollegeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "Companies",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Colleges",
                newName: "Email");

            migrationBuilder.AlterColumn<long>(
                name: "Phone",
                table: "Companies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Phone",
                table: "Colleges",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Companies",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Colleges",
                newName: "email");

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "Companies",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Phone",
                table: "Colleges",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
