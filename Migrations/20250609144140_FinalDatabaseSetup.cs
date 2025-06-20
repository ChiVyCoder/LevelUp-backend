using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUp.Migrations
{
    /// <inheritdoc />
    public partial class FinalDatabaseSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_InternshipCompanies_CompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerRegistrations_Users_UserId",
                table: "VolunteerRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerRegistrations_VolunteerOrganization_VolunteerOrganizationId",
                table: "VolunteerRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerRegistrations_Volunteers_VolunteerID",
                table: "VolunteerRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_CompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_UserId",
                table: "InternshipRegistrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VolunteerRegistrations",
                table: "VolunteerRegistrations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.RenameTable(
                name: "VolunteerRegistrations",
                newName: "VolunteerRegistration");

            migrationBuilder.RenameColumn(
                name: "RegisteredAt",
                table: "InternshipRegistrations",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "InternshipId",
                table: "InternshipRegistrations",
                newName: "JobID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerRegistrations_VolunteerOrganizationId",
                table: "VolunteerRegistration",
                newName: "IX_VolunteerRegistration_VolunteerOrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerRegistrations_VolunteerID",
                table: "VolunteerRegistration",
                newName: "IX_VolunteerRegistration_VolunteerID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerRegistrations_UserId",
                table: "VolunteerRegistration",
                newName: "IX_VolunteerRegistration_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "InternshipRegistrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "InternshipRegistrations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "InternshipRegistrations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "InternshipRegistrations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InternshipCompanyCompanyId",
                table: "InternshipRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "InternshipRegistrations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "InternshipRegistrations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "InternshipRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VolunteerRegistration",
                table: "VolunteerRegistration",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VolunteerApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VolunteerID = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VolunteerApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VolunteerApplications_Volunteers_VolunteerID",
                        column: x => x.VolunteerID,
                        principalTable: "Volunteers",
                        principalColumn: "VolunteerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_InternshipCompanyCompanyId",
                table: "InternshipRegistrations",
                column: "InternshipCompanyCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_JobID",
                table: "InternshipRegistrations",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_UserId_JobID",
                table: "InternshipRegistrations",
                columns: new[] { "UserId", "JobID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_UserId1",
                table: "InternshipRegistrations",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerApplications_UserId",
                table: "VolunteerApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerApplications_VolunteerID",
                table: "VolunteerApplications",
                column: "VolunteerID");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_InternshipCompanies_InternshipCompanyCompanyId",
                table: "InternshipRegistrations",
                column: "InternshipCompanyCompanyId",
                principalTable: "InternshipCompanies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_Jobs_JobID",
                table: "InternshipRegistrations",
                column: "JobID",
                principalTable: "Jobs",
                principalColumn: "JobID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId",
                table: "InternshipRegistrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId1",
                table: "InternshipRegistrations",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerRegistration_Users_UserId",
                table: "VolunteerRegistration",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerRegistration_VolunteerOrganization_VolunteerOrganizationId",
                table: "VolunteerRegistration",
                column: "VolunteerOrganizationId",
                principalTable: "VolunteerOrganization",
                principalColumn: "VolunteerOrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerRegistration_Volunteers_VolunteerID",
                table: "VolunteerRegistration",
                column: "VolunteerID",
                principalTable: "Volunteers",
                principalColumn: "VolunteerID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_InternshipCompanies_InternshipCompanyCompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_Jobs_JobID",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId1",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerRegistration_Users_UserId",
                table: "VolunteerRegistration");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerRegistration_VolunteerOrganization_VolunteerOrganizationId",
                table: "VolunteerRegistration");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerRegistration_Volunteers_VolunteerID",
                table: "VolunteerRegistration");

            migrationBuilder.DropTable(
                name: "VolunteerApplications");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_InternshipCompanyCompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_JobID",
                table: "InternshipRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_UserId_JobID",
                table: "InternshipRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_UserId1",
                table: "InternshipRegistrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VolunteerRegistration",
                table: "VolunteerRegistration");

            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "InternshipCompanyCompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "InternshipRegistrations");

            migrationBuilder.RenameTable(
                name: "VolunteerRegistration",
                newName: "VolunteerRegistrations");

            migrationBuilder.RenameColumn(
                name: "JobID",
                table: "InternshipRegistrations",
                newName: "InternshipId");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "InternshipRegistrations",
                newName: "RegisteredAt");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerRegistration_VolunteerOrganizationId",
                table: "VolunteerRegistrations",
                newName: "IX_VolunteerRegistrations_VolunteerOrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerRegistration_VolunteerID",
                table: "VolunteerRegistrations",
                newName: "IX_VolunteerRegistrations_VolunteerID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerRegistration_UserId",
                table: "VolunteerRegistrations",
                newName: "IX_VolunteerRegistrations_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "InternshipRegistrations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VolunteerRegistrations",
                table: "VolunteerRegistrations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_CompanyId",
                table: "InternshipRegistrations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_UserId",
                table: "InternshipRegistrations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_InternshipCompanies_CompanyId",
                table: "InternshipRegistrations",
                column: "CompanyId",
                principalTable: "InternshipCompanies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId",
                table: "InternshipRegistrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerRegistrations_Users_UserId",
                table: "VolunteerRegistrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerRegistrations_VolunteerOrganization_VolunteerOrganizationId",
                table: "VolunteerRegistrations",
                column: "VolunteerOrganizationId",
                principalTable: "VolunteerOrganization",
                principalColumn: "VolunteerOrganizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerRegistrations_Volunteers_VolunteerID",
                table: "VolunteerRegistrations",
                column: "VolunteerID",
                principalTable: "Volunteers",
                principalColumn: "VolunteerID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
