using Microsoft.EntityFrameworkCore.Migrations;

namespace QvaCar.Infraestructure.Identity.Data.Migrations.IdentityServer.Identity
{
    public partial class SubscriptionLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubscriptionLevelId",
                schema: "Core",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SubscriptionLevels",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionLevels", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Core",
                table: "SubscriptionLevels",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Free" });

            migrationBuilder.InsertData(
                schema: "Core",
                table: "SubscriptionLevels",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Paid" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionLevelId",
                schema: "Core",
                table: "Users",
                column: "SubscriptionLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SubscriptionLevels_SubscriptionLevelId",
                schema: "Core",
                table: "Users",
                column: "SubscriptionLevelId",
                principalSchema: "Core",
                principalTable: "SubscriptionLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SubscriptionLevels_SubscriptionLevelId",
                schema: "Core",
                table: "Users");

            migrationBuilder.DropTable(
                name: "SubscriptionLevels",
                schema: "Core");

            migrationBuilder.DropIndex(
                name: "IX_Users_SubscriptionLevelId",
                schema: "Core",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionLevelId",
                schema: "Core",
                table: "Users");
        }
    }
}
