using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OdapApi.Migrations
{
    public partial class SeparateDateTimeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateAndTime",
                table: "DevicesData",
                newName: "Date");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "DevicesData",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "DevicesData");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DevicesData",
                newName: "DateAndTime");
        }
    }
}
