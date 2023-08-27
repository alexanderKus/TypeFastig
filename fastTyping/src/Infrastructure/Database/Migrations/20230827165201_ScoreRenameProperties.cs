using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ScoreRenameProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Scores");

            migrationBuilder.RenameColumn(
                name: "Precision",
                table: "Scores",
                newName: "Accuracy");

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Scores",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Scores");

            migrationBuilder.RenameColumn(
                name: "Accuracy",
                table: "Scores",
                newName: "Precision");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "Scores",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
