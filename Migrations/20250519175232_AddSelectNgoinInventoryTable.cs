using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Medicine_Donation.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectNgoinInventoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SelectNgo",
                table: "MedicineInventories",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectNgo",
                table: "MedicineInventories");
        }
    }
}
