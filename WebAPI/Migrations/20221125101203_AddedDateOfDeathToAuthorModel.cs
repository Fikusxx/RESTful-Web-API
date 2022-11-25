using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class AddedDateOfDeathToAuthorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("6cdc6e75-f079-42d9-aa5c-d1f30a3294d6"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("8faa5545-c980-4676-badd-da43cf676483"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("b5e87b55-e4d2-422a-9afd-db48fd9f5811"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeath",
                table: "Authors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateOfBirth", "DateOfDeath", "FirstName", "LastName", "MainCategory" },
                values: new object[] { new Guid("36756456-1967-46e3-9d36-69aa469d5ed4"), new DateTime(2001, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bill", "Gates", "Rum" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateOfBirth", "DateOfDeath", "FirstName", "LastName", "MainCategory" },
                values: new object[] { new Guid("a07b3d12-7057-4307-9c36-e56e612da99b"), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tom", "Cruise", "Ships" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateOfBirth", "DateOfDeath", "FirstName", "LastName", "MainCategory" },
                values: new object[] { new Guid("c1b2116c-3e38-4d82-bd93-d67913a465fc"), new DateTime(1991, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Billy", "Herrington", "Singing" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("36756456-1967-46e3-9d36-69aa469d5ed4"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("a07b3d12-7057-4307-9c36-e56e612da99b"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("c1b2116c-3e38-4d82-bd93-d67913a465fc"));

            migrationBuilder.DropColumn(
                name: "DateOfDeath",
                table: "Authors");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName", "MainCategory" },
                values: new object[] { new Guid("6cdc6e75-f079-42d9-aa5c-d1f30a3294d6"), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tom", "Cruise", "Ships" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName", "MainCategory" },
                values: new object[] { new Guid("8faa5545-c980-4676-badd-da43cf676483"), new DateTime(1991, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Billy", "Herrington", "Singing" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName", "MainCategory" },
                values: new object[] { new Guid("b5e87b55-e4d2-422a-9afd-db48fd9f5811"), new DateTime(2001, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bill", "Gates", "Rum" });
        }
    }
}
