using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Medicine_Donation.Migrations
{
    /// <inheritdoc />
    public partial class addDonationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DonationRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "DonationRequests");
        }
    }
}
