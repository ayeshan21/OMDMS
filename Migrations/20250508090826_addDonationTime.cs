using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Medicine_Donation.Migrations
{
    /// <inheritdoc />
    public partial class addDonationTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DonationTime",
                table: "DonationRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationTime",
                table: "DonationRequests");
        }
    }
}
