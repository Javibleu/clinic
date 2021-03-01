using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class ChangeOwnBybyContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_AspNetUsers_OwnById",
                table: "Clinics");

            migrationBuilder.RenameColumn(
                name: "OwnById",
                table: "Clinics",
                newName: "ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_Clinics_OwnById",
                table: "Clinics",
                newName: "IX_Clinics_ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_AspNetUsers_ContactId",
                table: "Clinics",
                column: "ContactId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clinics_AspNetUsers_ContactId",
                table: "Clinics");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                table: "Clinics",
                newName: "OwnById");

            migrationBuilder.RenameIndex(
                name: "IX_Clinics_ContactId",
                table: "Clinics",
                newName: "IX_Clinics_OwnById");

            migrationBuilder.AddForeignKey(
                name: "FK_Clinics_AspNetUsers_OwnById",
                table: "Clinics",
                column: "OwnById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
