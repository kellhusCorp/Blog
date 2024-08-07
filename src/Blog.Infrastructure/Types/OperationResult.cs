namespace PetBlog.Infrastructure.Types
{
    public class OperationResult
    {
        protected bool _success = true;
        private readonly List<string> _failures = new();

        public IReadOnlyList<string> Failures => _failures;

        public bool Succeeded => _success;
        
        public OperationResult()
        {
            
        }

        public OperationResult(string message)
        {
            AddFailure(message);
            _success = false;
        }

        public void AddFailure(string message)
        {
            _failures.Add(message);
            _success = false;
        }

        public static OperationResult Failed(string message) => new(message);
    }
}