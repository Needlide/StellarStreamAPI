namespace StellarStreamAPI
{
    public class Result<T>
    {
        public static T? Value { get; set; }
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public Exception? Exception { get; }

        protected Result(bool isSuccess, string? errorMessage, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static Result<T> Success() => new(true, null);

        public static Result<T> Fail(string message) => new(false, message);

        public static Result<T> Fail(Exception ex) => new(false, ex.Message, ex);
    }
}
