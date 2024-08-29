namespace Blog.Application.Dtos
{
    public class PostDto
    {
        public Guid Id { get; set; }
        
        public string Header { get; set; }
        
        public string PermanentLink { get; set; }
        
        public string ShortContent { get; set; }
        
        public string Body { get; set; }
        
        public bool IsVisible {get; set; }
        
        public DateTimeOffset PublishDate { get; set; }
        
        public DateTimeOffset UpdateDate { get; set; }
        
        public int VisitsNumber { get; set; }
        
        public string? AuthorId { get; set; }
        
        public AuthorDto Author { get; set; }
        
        public ICollection<TagAssignmentDto> TagAssignments { get; set; }
        
        public ICollection<CommentDto> Comments { get; set; }
        
        public ICollection<PostFileDto> Files { get; set; }
    }
}