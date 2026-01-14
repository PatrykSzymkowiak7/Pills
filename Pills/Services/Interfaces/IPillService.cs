using Pills.Common;

namespace Pills.Services.Interfaces
{
    public interface IPillService
    {
        OperationResult<bool> TakePill(int pillTypeId, DateTime date);
        OperationResult<bool> DeletePillType(int pillTypeId);
        OperationResult<int> CreatePillType(string name, int maxAllowed);
    }
}
