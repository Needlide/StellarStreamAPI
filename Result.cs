namespace StellarStreamAPI
{
    public class Result<T>
    {
        public T Value { get; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public Exception? Exception { get; }

        protected Result(T value, bool isSuccess, string? errorMessage, Exception? exception = null)
        {
            Value = value;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static Result<T> Success(T value) => new(value, true, null);

        public static Result<T> Fail(string message) => new(default!, false, message);

        public static Result<T> Fail(Exception ex) => new(default!, false, ex.Message, ex);
    }
}
