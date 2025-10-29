using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobCardBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToJobCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "JobCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobCards_UserId",
                table: "JobCards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCards_Users_UserId",
                table: "JobCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCards_Users_UserId",
                table: "JobCards");

            migrationBuilder.DropIndex(
                name: "IX_JobCards_UserId",
                table: "JobCards");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobCards");
        }
    }
}
