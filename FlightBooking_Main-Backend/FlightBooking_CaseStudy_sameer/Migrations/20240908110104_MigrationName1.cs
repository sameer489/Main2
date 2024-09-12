using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking_CaseStudy_sameer.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "checkins",
                columns: table => new
                {
                    CheckinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CheckInStatus = table.Column<bool>(type: "bit", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkins", x => x.CheckinId);
                });

            migrationBuilder.CreateTable(
                name: "Bookingcheckin",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CheckinId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookingcheckin", x => new { x.BookingId, x.CheckinId });
                    table.ForeignKey(
                        name: "FK_Bookingcheckin_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookingcheckin_checkins_CheckinId",
                        column: x => x.CheckinId,
                        principalTable: "checkins",
                        principalColumn: "CheckinId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookingcheckin_CheckinId",
                table: "Bookingcheckin",
                column: "CheckinId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookingcheckin");

            migrationBuilder.DropTable(
                name: "checkins");
        }
    }
}
