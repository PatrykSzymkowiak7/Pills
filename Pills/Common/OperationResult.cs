namespace Pills.Common
{
    public class OperationResult<T>
    {
        public OperationStatus Status { get; }
        public bool Success => Status == OperationStatus.Success;
        public T? Data { get; }

        private OperationResult(OperationStatus status, T? data = default)
        {
            Status = status;
            Data = data;
        }

        public static OperationResult<T> Ok(T data)
            => new(OperationStatus.Success, data);

        public static OperationResult<T> Fail(OperationStatus status)
            => new(status);
    }
}
