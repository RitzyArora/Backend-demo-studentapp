using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentCrudAppWithEFCoreCodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class IndexAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectName",
                table: "Subjects",
                column: "SubjectName");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentMarks",
                table: "Students",
                column: "StudentMarks");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentMarks_StudentAge",
                table: "Students",
                columns: new[] { "StudentMarks", "StudentAge" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentName",
                table: "Students",
                column: "StudentName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_SubjectName",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentMarks",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentMarks_StudentAge",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentName",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Addresses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Addresses",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
