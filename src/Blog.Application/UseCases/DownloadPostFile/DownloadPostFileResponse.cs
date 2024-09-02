namespace Blog.Application.UseCases.DownloadPostFile
{
    public class DownloadPostFileResponse
    {
        public byte[] File { get; set; }
        
        public string ContentType { get; set; }
        
        public string FileName { get; set; }
    }
}