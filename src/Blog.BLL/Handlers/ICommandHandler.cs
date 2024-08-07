namespace Blog.BLL.Handlers;

public interface ICommandHandler<T>
{
    Task ExecuteAsync(T command);
}