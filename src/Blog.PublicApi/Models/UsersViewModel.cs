using Blog.Domain;
using Blog.PublicApi.Infrastructure.Paging;

namespace Blog.PublicApi.Models;

public class UsersViewModel
{
    public string? SearchTerm { get; set; }
    
    public PagedResult<User>? Users { get; set; }
}