namespace Blog.BLL.Dtos
{
    public record PageDto
    {
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public string ShortTitle { get; set; }
        
        public string Name { get; set; }
    }
}