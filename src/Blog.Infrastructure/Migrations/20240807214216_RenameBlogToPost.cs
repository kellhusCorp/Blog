using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameBlogToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("Blogs", "public", "Posts", "public");
            migrationBuilder.RenameTable("Files", "public", "PostFiles", "public");
            migrationBuilder.RenameTable("Tags", "public", "PostTags", "public");
            migrationBuilder.RenameTable("TagAssignments", "public", "PostTagAssignments", "public");
            migrationBuilder.RenameTable("Comments", "public", "PostComments", "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("Posts", "public", "Blogs", "public");
            migrationBuilder.RenameTable("PostFiles", "public", "Files", "public");
            migrationBuilder.RenameTable("PostTags", "public", "Tags", "public");
            migrationBuilder.RenameTable("PostTagAssignments", "public", "TagAssignments", "public");
            migrationBuilder.RenameTable("PostComments", "public", "Comments", "public");
        }
    }
}
