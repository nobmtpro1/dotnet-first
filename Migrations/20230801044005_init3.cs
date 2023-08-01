using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
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
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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
                principalColumn: "Id");
        }
    }
}
