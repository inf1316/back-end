using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QvaCar.Infraestructure.Chat.Migrations.Chat
{
    public partial class InitialChatMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Chat");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutAd_ModelVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboutAd_MainImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutAd_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnotherParticipant_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnotherParticipant_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnotherParticipant_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastMessage_CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastMessage_Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMessage_MessageType_Id = table.Column<int>(type: "int", nullable: true),
                    LastMessage_MessageType_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastMessage_Id = table.Column<long>(type: "bigint", nullable: true),
                    MyRole_Id = table.Column<int>(type: "int", nullable: false),
                    MyRole_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_Users_ChatUserId",
                        column: x => x.ChatUserId,
                        principalSchema: "Chat",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Chat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CorrelationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageType_Id = table.Column<int>(type: "int", nullable: false),
                    MessageType_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "Chat",
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ChatUserId",
                schema: "Chat",
                table: "Channels",
                column: "ChatUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChannelId",
                schema: "Chat",
                table: "Messages",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Chat");

            migrationBuilder.DropTable(
                name: "Channels",
                schema: "Chat");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Chat");
        }
    }
}
