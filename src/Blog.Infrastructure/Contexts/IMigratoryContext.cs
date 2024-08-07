using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Blog.Infrastructure.Contexts;

public interface IMigratoryContext
{
    DatabaseFacade Database { get; }
}