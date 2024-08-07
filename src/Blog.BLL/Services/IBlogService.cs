namespace MyBlogOnCore.BLL.Services;

public interface IBlogService<T>
{
    Task AddOrUpdate(T entity, IEnumerable<string> tags);

    Task IncrementVisitsNumber(Guid id);
}