namespace Pills.Services.Interfaces
{
    public enum OperationResult
    {
        Success,
        MaxLimitReached,
        NotFound,
        Error,
        AlreadyExists,
        InvalidData
    }

    public interface IPillService
    {
        OperationResult TakePill(int pillTypeId, DateTime date);
        OperationResult DeletePillType(int pillTypeId);
        OperationResult CreatePillType(string name, int maxAllowed);
    }
}
