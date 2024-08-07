namespace Blog.BLL.Exceptions;

public class AdminUserValidatorException : Exception
{
    public IDictionary<string, string> Errors { get; }

    public AdminUserValidatorException(
        string message,
        IDictionary<string, string> errors) : base(message)
    {
        Errors = errors;
    }
}