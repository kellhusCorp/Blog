namespace Blog.Application.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Body { get; set; }
        
        public string Email { get; set; }
        
        public string HomePage { get; set; }
        
        public bool AdminPost { get; set; }
        
        public Guid PostId { get; set; }
        
        public DateTimeOffset CreatedOn { get; set; }
    }
}