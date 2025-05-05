using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Medicine_Donation.Migrations
{
    /// <inheritdoc />
    public partial class changeIdname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MedicineId",
                table: "DonationRequests",
                newName: "DonationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DonationId",
                table: "DonationRequests",
                newName: "MedicineId");
        }
    }
}
