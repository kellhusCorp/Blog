using Blog.Domain;

namespace Blog.BLL.Commands;

public class AddCommentCommand
{
    public AddCommentCommand(Comment comment)
    {
        Comment = comment;
    }

    public Comment Comment { get; }
    
    public string? Referer { get; set; }
}