namespace Blog.Domain
{
    public class Page : BaseEntity
    {
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public string ShortTitle { get; set; }
        
        public string Name { get; set; }
    }
}