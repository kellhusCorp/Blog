namespace Blog.BLL.Commands;

public class IncrementBlogFileCounterCommand
{
    public IncrementBlogFileCounterCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}