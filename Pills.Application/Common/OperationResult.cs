using Pills.Application.Common;

namespace Pills.Application.Common
{
    public class OperationResult
    {
        public OperationStatus Status { get; }
        public bool Success => Status == OperationStatus.Success;

        protected OperationResult(OperationStatus status)
        {
            Status = status;
        }

        public static OperationResult Ok()
        {
            return new OperationResult(OperationStatus.Success);
        }

        public static OperationResult Fail(OperationStatus status)
        {
            return new OperationResult(status);
        }
    }


    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; }

        private OperationResult(OperationStatus status, T? data = default)
            : base(status)
        {
            Data = data;
        }

        public static OperationResult<T> Ok(T data)
        {
            return new OperationResult<T>(OperationStatus.Success, data);
        }

        public static new OperationResult<T> Fail(OperationStatus status)
        {
            return new OperationResult<T>(status);
        }

    }
}
