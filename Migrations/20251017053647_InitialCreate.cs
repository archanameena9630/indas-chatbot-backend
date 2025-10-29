using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobCardBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaperDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCards", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobCards");
        }
    }
}
