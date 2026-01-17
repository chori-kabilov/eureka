using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTeacherAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "teachers");

            migrationBuilder.AddColumn<DateTime>(
                name: "birth_date",
                table: "users",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HiredAt",
                table: "teachers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "teachers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subjects",
                table: "teachers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birth_date",
                table: "users");

            migrationBuilder.DropColumn(
                name: "HiredAt",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "Subjects",
                table: "teachers");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "teachers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
