using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloggie.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeadingBlo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageTitleBlo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentBlo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescriptionBlo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FeatureImageUrlBlo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlHandleBlo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDateBlo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorBlo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisibleBlo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayNameTag = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTag",
                columns: table => new
                {
                    BlogPostTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsBloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTag", x => new { x.BlogPostTagId, x.TagsBloId });
                    table.ForeignKey(
                        name: "FK_BlogPostTag_BlogPosts_BlogPostTagId",
                        column: x => x.BlogPostTagId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostTag_Tags_TagsBloId",
                        column: x => x.TagsBloId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTag_TagsBloId",
                table: "BlogPostTag",
                column: "TagsBloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostTag");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
