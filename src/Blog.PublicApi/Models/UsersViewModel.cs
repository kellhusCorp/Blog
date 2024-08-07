using MyBlogOnCore.Domain;
using MyBlogOnCore.Infrastructure.Paging;

namespace MyBlogOnCore.Models;

public class UsersViewModel
{
    public string? SearchTerm { get; set; }
    
    public PagedResult<User>? Users { get; set; }
}