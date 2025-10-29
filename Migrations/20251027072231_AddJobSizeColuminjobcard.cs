using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobCardBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddJobSizeColuminjobcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobSize",
                table: "JobCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobSize",
                table: "JobCards");
        }
    }
}
