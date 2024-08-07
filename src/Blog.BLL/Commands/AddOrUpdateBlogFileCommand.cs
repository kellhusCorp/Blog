namespace MyBlogOnCore.BLL.Commands;

public class AddOrUpdateBlogFileCommand
{
    public AddOrUpdateBlogFileCommand(string fileName, byte[] data, Guid blogId)
    {
        FileName = fileName;
        Data = data;
        BlogId = blogId;
    }

    public string FileName { get; set; }
    
    public byte[] Data { get; set; }
    
    public Guid BlogId { get; set; }
}