using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAPI.Migrations
{
    /// <inheritdoc />
    public partial class Payment_EnumFix_Schedule_NullableFKs_IsFreePeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjectClasses_Classes_ClassId",
                table: "TeacherSubjectClasses");

            migrationBuilder.DropIndex(
                name: "IX_TeacherSubjectClasses_ClassId",
                table: "TeacherSubjectClasses");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_TeacherId_Day_StartTime",
                table: "Schedules");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_METHOD_NOT_EMPTY",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "TeacherSubjectClasses");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_ClassId_Day_StartTime",
                table: "Schedules",
                newName: "UQ_Class_Day_StartTime");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "Schedules",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectId",
                table: "Schedules",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AddColumn<bool>(
                name: "IsFreePeriod",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payments",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Pending");

            migrationBuilder.CreateIndex(
                name: "UQ_Teacher_Day_StartTime",
                table: "Schedules",
                columns: new[] { "TeacherId", "Day", "StartTime" },
                unique: true,
                filter: "[TeacherId] IS NOT NULL AND [IsFreePeriod] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_FREEPERIOD_NO_SUBJECT",
                table: "Schedules",
                sql: "[IsFreePeriod] = 0 OR [SubjectId] IS NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_FREEPERIOD_NO_TEACHER",
                table: "Schedules",
                sql: "[IsFreePeriod] = 0 OR [TeacherId] IS NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_SUBJECT_REQUIRES_TEACHER",
                table: "Schedules",
                sql: "[IsFreePeriod] = 1 OR ([SubjectId] IS NOT NULL AND [TeacherId] IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_METHOD_VALID",
                table: "Payments",
                sql: "[Method] IN ('Cash', 'BankTransfer', 'QR')");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "UQ_Teacher_Day_StartTime",
                table: "Schedules");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_FREEPERIOD_NO_SUBJECT",
                table: "Schedules");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_FREEPERIOD_NO_TEACHER",
                table: "Schedules");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_SUBJECT_REQUIRES_TEACHER",
                table: "Schedules");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_METHOD_VALID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsFreePeriod",
                table: "Schedules");

            migrationBuilder.RenameIndex(
                name: "UQ_Class_Day_StartTime",
                table: "Schedules",
                newName: "IX_Schedules_ClassId_Day_StartTime");

            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "TeacherSubjectClasses",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "Schedules",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubjectId",
                table: "Schedules",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjectClasses_ClassId",
                table: "TeacherSubjectClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TeacherId_Day_StartTime",
                table: "Schedules",
                columns: new[] { "TeacherId", "Day", "StartTime" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CHK_METHOD_NOT_EMPTY",
                table: "Payments",
                sql: "LEN(LTRIM(RTRIM([Method]))) > 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Subjects_SubjectId",
                table: "Schedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teachers_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjectClasses_Classes_ClassId",
                table: "TeacherSubjectClasses",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
