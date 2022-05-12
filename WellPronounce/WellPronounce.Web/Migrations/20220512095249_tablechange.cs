using Microsoft.EntityFrameworkCore.Migrations;

namespace WellPronounce.Web.Migrations
{
    public partial class tablechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InputText",
                table: "SpeechDetails",
                newName: "PreferedName");

            migrationBuilder.AddColumn<string>(
                name: "LegalFirstName",
                table: "SpeechDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LegalLastName",
                table: "SpeechDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phonetics",
                table: "SpeechDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LegalFirstName",
                table: "SpeechDetails");

            migrationBuilder.DropColumn(
                name: "LegalLastName",
                table: "SpeechDetails");

            migrationBuilder.DropColumn(
                name: "Phonetics",
                table: "SpeechDetails");

            migrationBuilder.RenameColumn(
                name: "PreferedName",
                table: "SpeechDetails",
                newName: "InputText");
        }
    }
}
