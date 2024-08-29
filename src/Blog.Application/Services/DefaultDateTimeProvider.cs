namespace Blog.Application.Services
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.Now;
    }
}