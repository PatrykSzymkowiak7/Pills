using Pills.Common;
using Pills.Models;

namespace Pills.Services.Interfaces
{
    public interface IPillService
    {
        Task<OperationResult<PillsTaken>> TakePillAsync(int pillTypeId, DateTime date, string userId);
        Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId);
        Task<OperationResult<PillsTypes>> CreatePillTypeAsync(string name, int maxAllowed);
        Task<OperationResult<PillsTypes>> EditPillAsync(int id, string name, int maxAllowed);
    }
}
