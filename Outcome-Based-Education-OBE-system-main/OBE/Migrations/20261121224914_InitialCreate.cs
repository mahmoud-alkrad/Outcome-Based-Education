using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OBE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "college_goals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    title_ar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    title_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    description_ar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description_en = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__college___3213E83F492B08D5", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "programs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name_ar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    isActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__programs__3213E83F7C32F630", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    program_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name_ar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    mark = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__courses__3213E83FD13C0853", x => x.id);
                    table.ForeignKey(
                        name: "FK__courses__program__4F7CD00D",
                        column: x => x.program_id,
                        principalTable: "programs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "program_goals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    program_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    title_ar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    title_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    description_ar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description_en = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__program___3213E83F20BAA779", x => x.id);
                    table.ForeignKey(
                        name: "FK__program_g__progr__4AB81AF0",
                        column: x => x.program_id,
                        principalTable: "programs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    program_id = table.Column<int>(type: "int", nullable: true),
                    name_ar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    name_en = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Instructor"),
                    is_active = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83F3BABA6E7", x => x.id);
                    table.ForeignKey(
                        name: "FK__users__program_i__45F365D3",
                        column: x => x.program_id,
                        principalTable: "programs",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_goals",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    description_ar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description_en = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__course_g__3213E83FDB9C7584", x => x.id);
                    table.ForeignKey(
                        name: "FK__course_go__cours__5441852A",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "college_goal_program_goal",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    college_goal_id = table.Column<int>(type: "int", nullable: true),
                    program_goal_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__college___3213E83F94357F8C", x => x.id);
                    table.ForeignKey(
                        name: "FK__college_g__colle__5812160E",
                        column: x => x.college_goal_id,
                        principalTable: "college_goals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__college_g__progr__59063A47",
                        column: x => x.program_goal_id,
                        principalTable: "program_goals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_instructors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: true),
                    instructor_id = table.Column<int>(type: "int", nullable: true),
                    semester = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__course_i__3213E83FA3D86B8C", x => x.id);
                    table.ForeignKey(
                        name: "FK__course_in__cours__628FA481",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__course_in__instr__6383C8BA",
                        column: x => x.instructor_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "program_goal_course_goal",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    program_goal_id = table.Column<int>(type: "int", nullable: true),
                    course_goal_id = table.Column<int>(type: "int", nullable: true),
                    correlation_level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__program___3213E83F734E1A3D", x => x.id);
                    table.ForeignKey(
                        name: "FK__program_g__cours__5EBF139D",
                        column: x => x.course_goal_id,
                        principalTable: "course_goals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__program_g__progr__5DCAEF64",
                        column: x => x.program_goal_id,
                        principalTable: "program_goals",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_college_goal_program_goal_college_goal_id",
                table: "college_goal_program_goal",
                column: "college_goal_id");

            migrationBuilder.CreateIndex(
                name: "IX_college_goal_program_goal_program_goal_id",
                table: "college_goal_program_goal",
                column: "program_goal_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_goals_course_id",
                table: "course_goals",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_instructors_course_id",
                table: "course_instructors",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_instructors_instructor_id",
                table: "course_instructors",
                column: "instructor_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_program_id",
                table: "courses",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_goal_course_goal_course_goal_id",
                table: "program_goal_course_goal",
                column: "course_goal_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_goal_course_goal_program_goal_id",
                table: "program_goal_course_goal",
                column: "program_goal_id");

            migrationBuilder.CreateIndex(
                name: "IX_program_goals_program_id",
                table: "program_goals",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "UQ__programs__357D4CF9C9C8BE4E",
                table: "programs",
                column: "code",
                unique: true,
                filter: "[code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_users_program_id",
                table: "users",
                column: "program_id");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E61649D689533",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "college_goal_program_goal");

            migrationBuilder.DropTable(
                name: "course_instructors");

            migrationBuilder.DropTable(
                name: "program_goal_course_goal");

            migrationBuilder.DropTable(
                name: "college_goals");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "course_goals");

            migrationBuilder.DropTable(
                name: "program_goals");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "programs");
        }
    }
}
