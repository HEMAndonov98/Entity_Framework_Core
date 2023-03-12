using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P01_StudentSystem.Data.Migrations
{
    public partial class AddedContentToHomeworkAndFixedCourseIdinResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Courses_CourseID",
                table: "Resources");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "Resources",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_CourseID",
                table: "Resources",
                newName: "IX_Resources_CourseId");

            migrationBuilder.AddColumn<string>(
                name: "ContentFilePath",
                table: "Homeworks",
                type: "VARCHAR(260)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Courses_CourseId",
                table: "Resources",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Courses_CourseId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ContentFilePath",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Resources",
                newName: "CourseID");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_CourseId",
                table: "Resources",
                newName: "IX_Resources_CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Courses_CourseID",
                table: "Resources",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
