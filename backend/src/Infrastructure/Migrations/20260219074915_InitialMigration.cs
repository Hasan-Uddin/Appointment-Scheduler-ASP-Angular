using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    google_id = table.Column<string>(type: "TEXT", nullable: false),
                    picture_url = table.Column<string>(type: "TEXT", nullable: false),
                    google_access_token = table.Column<string>(type: "TEXT", nullable: true),
                    google_refresh_token = table.Column<string>(type: "TEXT", nullable: true),
                    password_hash = table.Column<string>(type: "TEXT", nullable: true),
                    time_zone = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "availabilities",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    day_of_week = table.Column<int>(type: "INTEGER", nullable: false),
                    start_time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_availabilities", x => x.id);
                    table.ForeignKey(
                        name: "fk_availabilities_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "calendar_setting",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    time_zone = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    minimum_notice_minutes = table.Column<int>(type: "INTEGER", nullable: false),
                    maximum_days_in_advance = table.Column<int>(type: "INTEGER", nullable: false),
                    slot_interval_minutes = table.Column<int>(type: "INTEGER", nullable: false),
                    buffer_before_minutes = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    buffer_after_minutes = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    max_bookings_per_day = table.Column<int>(type: "INTEGER", nullable: true),
                    max_bookings_per_week = table.Column<int>(type: "INTEGER", nullable: true),
                    default_start_time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    default_end_time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    rolling_days_available = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 30),
                    custom_availability_start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    custom_availability_end = table.Column<DateTime>(type: "TEXT", nullable: true),
                    sync_to_google_calendar = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    check_google_calendar_conflicts = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    google_calendar_id = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    send_confirmation_email = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    send_reminder_email = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    reminder_minutes_before = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1440),
                    welcome_message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    require_guest_phone = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    allow_guest_notes = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    allow_overlap_booking = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    minimum_scheduling_gap = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    user_id1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_calendar_setting", x => x.id);
                    table.ForeignKey(
                        name: "fk_calendar_setting_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_calendar_setting_users_user_id1",
                        column: x => x.user_id1,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "event_types",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    slug = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    duration_minutes = table.Column<int>(type: "INTEGER", nullable: false),
                    buffer_minutes = table.Column<int>(type: "INTEGER", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false),
                    color = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_types", x => x.id);
                    table.ForeignKey(
                        name: "fk_event_types_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "todo_items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    due_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    labels = table.Column<string>(type: "TEXT", nullable: false),
                    is_completed = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    completed_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    priority = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_todo_items_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    event_type_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    guest_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    guest_email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    start_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    end_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    google_event_id = table.Column<string>(type: "TEXT", nullable: true),
                    notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    event_type_id1 = table.Column<Guid>(type: "TEXT", nullable: true),
                    user_id1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bookings", x => x.id);
                    table.ForeignKey(
                        name: "fk_bookings_event_types_event_type_id",
                        column: x => x.event_type_id,
                        principalSchema: "public",
                        principalTable: "event_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bookings_event_types_event_type_id1",
                        column: x => x.event_type_id1,
                        principalSchema: "public",
                        principalTable: "event_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_bookings_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bookings_users_user_id1",
                        column: x => x.user_id1,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_availabilities_user_id",
                schema: "public",
                table: "availabilities",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_event_type_id",
                schema: "public",
                table: "bookings",
                column: "event_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_event_type_id1",
                schema: "public",
                table: "bookings",
                column: "event_type_id1");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_guest_email",
                schema: "public",
                table: "bookings",
                column: "guest_email");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_start_time",
                schema: "public",
                table: "bookings",
                column: "start_time");

            migrationBuilder.CreateIndex(
                name: "ix_bookings_user_id_start_time",
                schema: "public",
                table: "bookings",
                columns: new[] { "user_id", "start_time" });

            migrationBuilder.CreateIndex(
                name: "ix_bookings_user_id1",
                schema: "public",
                table: "bookings",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_calendar_setting_user_id",
                schema: "public",
                table: "calendar_setting",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_calendar_setting_user_id1",
                schema: "public",
                table: "calendar_setting",
                column: "user_id1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_event_types_user_id",
                schema: "public",
                table: "event_types",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_todo_items_user_id",
                schema: "public",
                table: "todo_items",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                schema: "public",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "availabilities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "bookings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "calendar_setting",
                schema: "public");

            migrationBuilder.DropTable(
                name: "todo_items",
                schema: "public");

            migrationBuilder.DropTable(
                name: "event_types",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
