using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Medicine_Donation.Migrations
{
    /// <inheritdoc />
    public partial class addNgoNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "EmergencyRequests",
                newName: "NgoName");

            migrationBuilder.AddColumn<string>(
                name: "MedicineName",
                table: "EmergencyRequests",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicineName",
                table: "EmergencyRequests");

            migrationBuilder.RenameColumn(
                name: "NgoName",
                table: "EmergencyRequests",
                newName: "Name");
        }
    }
}
