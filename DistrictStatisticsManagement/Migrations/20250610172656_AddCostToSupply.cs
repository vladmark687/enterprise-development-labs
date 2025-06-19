using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DistrictStatisticsManagement.Migrations
{
    public partial class AddCostToSupply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "Supplies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Supplies");
        }
    }
}
