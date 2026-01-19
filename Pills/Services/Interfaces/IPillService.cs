using Pills.Common;
using Pills.Models;
using Pills.Models.DTOs;

namespace Pills.Services.Interfaces
{
    public interface IPillService
    {
        Task<OperationResult<PillTypeDto>> TakePillAsync(int pillTypeId, DateTime date, string userId);
        Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId, string userId);
        Task<OperationResult<PillTypeDto>> CreatePillTypeAsync(CreatePillTypeDto dto, string userId);
        Task<OperationResult<PillTypeDto>> EditPillAsync(int id, string name, int maxAllowed, string userId);
        Task<OperationResult<PillTypeDto>> RestorePillTypeAsync(int pillTypeId, string userId);
    }
}
