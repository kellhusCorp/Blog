using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MyBlogOnCore.DataSource.Contexts;

public interface IMigratoryContext
{
    DatabaseFacade Database { get; }
}