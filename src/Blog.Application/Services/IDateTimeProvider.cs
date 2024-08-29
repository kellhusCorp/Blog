namespace Blog.Application.Services
{
    public interface IDateTimeProvider
    {
        public DateTimeOffset UtcNow { get; }
    }
}