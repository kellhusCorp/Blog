namespace PetBlog.Infrastructure.Types
{
    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }

        public OperationResult()
        {
            
        }
        
        public OperationResult(T value)
        {
            Value = value;
        }

        public OperationResult(T value, string failMessage) : this(value)
        {
            AddFailure(failMessage);
        }

        public static new OperationResult<T> Failed(T value, string failMessage) =>
            new OperationResult<T>(value, failMessage);
    }
}