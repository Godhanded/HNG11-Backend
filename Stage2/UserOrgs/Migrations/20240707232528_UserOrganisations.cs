using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserOrgs.Migrations
{
    /// <inheritdoc />
    public partial class UserOrganisations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    orgId = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.orgId);
                });

            migrationBuilder.CreateTable(
                name: "UserOrganisations",
                columns: table => new
                {
                    userId = table.Column<string>(type: "text", nullable: false),
                    organisationId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganisations", x => new { x.userId, x.organisationId });
                    table.ForeignKey(
                        name: "FK_UserOrganisations_Organisations_organisationId",
                        column: x => x.organisationId,
                        principalTable: "Organisations",
                        principalColumn: "orgId");
                    table.ForeignKey(
                        name: "FK_UserOrganisations_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganisations_organisationId",
                table: "UserOrganisations",
                column: "organisationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOrganisations");

            migrationBuilder.DropTable(
                name: "Organisations");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
