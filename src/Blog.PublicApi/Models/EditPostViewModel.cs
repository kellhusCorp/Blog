using Blog.Domain;
using Blog.Domain.Entities;

namespace Blog.PublicApi.Models;

public class EditPostViewModel
{
    public EditPostViewModel()
    {
    }
    
    public EditPostViewModel(Post post)
    {
        Post = post;
    }

    public Post Post { get; set; }

    public List<string> SelectedTagNames { get; set; } = new List<string>();

    public List<Tag> AllTags { get; set; } = new List<Tag>();

    public List<User> Authors { get; set; } = new List<User>();
}