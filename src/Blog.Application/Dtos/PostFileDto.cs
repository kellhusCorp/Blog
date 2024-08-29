namespace Blog.Application.Dtos
{
    public class PostFileDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public int Counter { get; set; }
        
        public Guid PostId { get; set; }
    }
}