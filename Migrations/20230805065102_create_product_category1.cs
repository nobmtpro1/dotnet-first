using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class create_product_category1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_ProductCategory_ParentId",
                table: "ProductCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_ProductCategory_ParentId",
                table: "ProductCategory",
                column: "ParentId",
                principalTable: "ProductCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_ProductCategory_ParentId",
                table: "ProductCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_ProductCategory_ParentId",
                table: "ProductCategory",
                column: "ParentId",
                principalTable: "ProductCategory",
                principalColumn: "Id");
        }
    }
}
