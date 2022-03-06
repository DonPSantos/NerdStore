using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NerdStore.Catalogo.Data.Migrations
{
    public partial class updateimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "Produtos");

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagem",
                table: "Produtos",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
