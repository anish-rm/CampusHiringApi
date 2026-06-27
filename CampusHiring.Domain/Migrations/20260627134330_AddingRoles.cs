using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CampusHiring.Api.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "58da3b22-a0f4-4f53-8c98-bb4aabd69b60", "762167d2-790c-4392-a64a-f641e54a1f0e", "Admin", "ADMIN" },
                    { "67b715fe-d9bf-4f50-b0c0-37599ba4fb6b", "1587469f-1e01-4c94-9e72-32ec1aa0018d", "Student", "STUDENT" },
                    { "964ab596-c9ba-4397-8378-42d8bc792af2", "82833d51-a7ba-4c5f-bb44-a2e923c3e6b3", "Interviewer", "INTERVIEWER" },
                    { "f90eaaa0-013e-4b3b-90c3-79f155289704", "33595ae8-4367-4d22-bf3b-a2f3a06583c6", "College Admin", "COLLEGE ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58da3b22-a0f4-4f53-8c98-bb4aabd69b60");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "67b715fe-d9bf-4f50-b0c0-37599ba4fb6b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "964ab596-c9ba-4397-8378-42d8bc792af2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f90eaaa0-013e-4b3b-90c3-79f155289704");
        }
    }
}
