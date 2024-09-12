using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightBooking_CaseStudy_sameer.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CheckInStatus",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckInStatus",
                table: "Bookings");
        }
    }
}
