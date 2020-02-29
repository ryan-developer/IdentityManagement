using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityManagement.Persistence.Migrations.IdentityServer.AspNetIdentity
{
    public partial class AddedIdentitySource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityProvider",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityProvider",
                table: "AspNetUsers");
        }
    }
}
