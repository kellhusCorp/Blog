namespace Blog.Domain
{
    public struct Result<T>
    {
        public T Value { get; set; }
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    
        public static Result<T> Success(T value)
        {
            return new Result<T> { Value = value, IsSuccessful = true };
        }
    
        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T> { ErrorMessage = errorMessage, IsSuccessful = false };
        }
    }
}