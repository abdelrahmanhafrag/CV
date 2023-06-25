using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATSWebAppV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class cvData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "CV",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HobbiesAndInterests",
                table: "CV",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkExperience",
                table: "CV",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Education",
                table: "CV");

            migrationBuilder.DropColumn(
                name: "HobbiesAndInterests",
                table: "CV");

            migrationBuilder.DropColumn(
                name: "WorkExperience",
                table: "CV");
        }
    }
}
