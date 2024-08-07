namespace MyBlogOnCore.Repository;

public interface IRepository<T>
    where T: class, new()
{
    public Task<IEnumerable<T>> GetAll();

    public Task<T> CreateOrUpdate(T entity);
}