using Microsoft.EntityFrameworkCore.Migrations;

namespace TwitterAuth_Backend.Migrations
{
    public partial class TwitterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwitterUserInfo",
                columns: table => new
                {
                    UserKey = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: true),
                    TokenSecret = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterUserInfo", x => x.UserKey);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitterUserInfo");
        }
    }
}
