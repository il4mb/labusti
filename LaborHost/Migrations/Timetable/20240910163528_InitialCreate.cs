using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Host.Migrations.Timetable
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduleTime",
                columns: table => new
                {
                    ScheduleTimeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Hour = table.Column<int>(type: "INTEGER", nullable: false),
                    Minute = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTime", x => x.ScheduleTimeId);
                });

            migrationBuilder.CreateTable(
                name: "Timetables",
                columns: table => new
                {
                    TimetableId = table.Column<int>(type: "INTEGER", nullable: false),
                    Course = table.Column<string>(type: "TEXT", nullable: false),
                    LecturerName = table.Column<string>(type: "TEXT", nullable: false),
                    Semester = table.Column<string>(type: "TEXT", nullable: false),
                    StudyProgram = table.Column<string>(type: "TEXT", nullable: false),
                    Schedule_ScheduleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Schedule_DayWeek = table.Column<int>(type: "INTEGER", nullable: false),
                    Schedule_TimeStartScheduleTimeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Schedule_TimeEndScheduleTimeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.TimetableId);
                    table.ForeignKey(
                        name: "FK_Timetables_ScheduleTime_Schedule_TimeEndScheduleTimeId",
                        column: x => x.Schedule_TimeEndScheduleTimeId,
                        principalTable: "ScheduleTime",
                        principalColumn: "ScheduleTimeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Timetables_ScheduleTime_Schedule_TimeStartScheduleTimeId",
                        column: x => x.Schedule_TimeStartScheduleTimeId,
                        principalTable: "ScheduleTime",
                        principalColumn: "ScheduleTimeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_Schedule_TimeEndScheduleTimeId",
                table: "Timetables",
                column: "Schedule_TimeEndScheduleTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_Schedule_TimeStartScheduleTimeId",
                table: "Timetables",
                column: "Schedule_TimeStartScheduleTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timetables");

            migrationBuilder.DropTable(
                name: "ScheduleTime");
        }
    }
}
