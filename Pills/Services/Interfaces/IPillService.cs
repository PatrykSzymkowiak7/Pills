using Pills.Common;
using Pills.Models;
using Pills.Models.DTOs.PillTaken;
using Pills.Models.DTOs.PillTypes;

namespace Pills.Services.Interfaces
{
    public interface IPillService
    {
        Task<OperationResult<PillTakenDto>> TakePillAsync(TakePillDto dto, string userId);
        Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId, string userId);
        Task<OperationResult<PillTypeDto>> CreatePillTypeAsync(CreatePillTypeDto dto, string userId);
        Task<OperationResult<PillTypeDto>> EditPillAsync(EditPillTypeDto dto, string userId);
        Task<OperationResult<PillTypeDto>> RestorePillTypeAsync(int pillTypeId, string userId);
    }
}
