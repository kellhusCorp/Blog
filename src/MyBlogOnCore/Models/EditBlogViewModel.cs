using MyBlogOnCore.Domain;

namespace MyBlogOnCore.Models;

public class EditBlogViewModel
{
    public EditBlogViewModel()
    {
    }
    
    public EditBlogViewModel(Blog blog)
    {
        Blog = blog;
    }

    public Blog Blog { get; set; }

    public List<string> SelectedTagNames { get; set; } = new List<string>();

    public List<Tag> AllTags { get; set; } = new List<Tag>();

    public List<User> Authors { get; set; } = new List<User>();
}