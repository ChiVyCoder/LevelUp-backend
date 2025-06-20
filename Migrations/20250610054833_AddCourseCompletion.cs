using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUp.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_InternshipCompanies_InternshipCompanyCompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId1",
                table: "InternshipRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Course_CourseId",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Course_CourseId1",
                table: "UserCourses");

            migrationBuilder.DropTable(
                name: "InternshipCompanies");

            migrationBuilder.DropIndex(
                name: "IX_UserCourses_CourseId1",
                table: "UserCourses");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_InternshipCompanyCompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_InternshipRegistrations_UserId1",
                table: "InternshipRegistrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "UserCourses");

            migrationBuilder.DropColumn(
                name: "InternshipCompanyCompanyId",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "InternshipRegistrations");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "EnrollmentCount",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Courses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Courses_CourseId",
                table: "UserCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Courses_CourseId",
                table: "UserCourses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.AddColumn<int>(
                name: "CourseId1",
                table: "UserCourses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternshipCompanyCompanyId",
                table: "InternshipRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "InternshipRegistrations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Course",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Course",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnrollmentCount",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Course",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseId");

            migrationBuilder.CreateTable(
                name: "InternshipCompanies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipCompanies", x => x.CompanyId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_CourseId1",
                table: "UserCourses",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_InternshipCompanyCompanyId",
                table: "InternshipRegistrations",
                column: "InternshipCompanyCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipRegistrations_UserId1",
                table: "InternshipRegistrations",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_InternshipCompanies_InternshipCompanyCompanyId",
                table: "InternshipRegistrations",
                column: "InternshipCompanyCompanyId",
                principalTable: "InternshipCompanies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipRegistrations_Users_UserId1",
                table: "InternshipRegistrations",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Course_CourseId",
                table: "UserCourses",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Course_CourseId1",
                table: "UserCourses",
                column: "CourseId1",
                principalTable: "Course",
                principalColumn: "CourseId");
        }
    }
}
