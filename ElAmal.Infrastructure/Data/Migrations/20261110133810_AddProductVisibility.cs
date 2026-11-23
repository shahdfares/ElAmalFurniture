using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElAmal.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVisibile",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVisibile",
                table: "Products");
        }
    }
}
