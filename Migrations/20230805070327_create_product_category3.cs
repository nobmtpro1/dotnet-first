using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class create_product_category3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleCategory_ArticleCategory_ParentId",
                table: "ArticleCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleCategory_ArticleCategory_ParentId",
                table: "ArticleCategory",
                column: "ParentId",
                principalTable: "ArticleCategory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleCategory_ArticleCategory_ParentId",
                table: "ArticleCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleCategory_ArticleCategory_ParentId",
                table: "ArticleCategory",
                column: "ParentId",
                principalTable: "ArticleCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
