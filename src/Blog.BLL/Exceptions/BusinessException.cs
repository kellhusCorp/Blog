namespace MyBlogOnCore.BLL.Exceptions;

[Serializable]
public class BusinessException : Exception
{
    public BusinessException()
    {
    }

    public BusinessException(string? message) : base(message)
    {
    }
}