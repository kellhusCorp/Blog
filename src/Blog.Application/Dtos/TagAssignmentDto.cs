namespace Blog.Application.Dtos
{
    public class TagAssignmentDto
    {
        public Guid PostId { get; set; }
        
        public Guid TagId { get; set; }
        
        public TagDto Tag { get; set; }
    }
}