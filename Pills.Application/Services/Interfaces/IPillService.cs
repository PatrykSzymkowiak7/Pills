using Pills.Infrastructure.Common;
using Pills.Domain.Models.DTOs.PillTaken;
using Pills.Domain.Models.DTOs.PillTypes;

namespace Pills.Infrastructure.Services.Interfaces
{
    public interface IPillService
    {
        Task<OperationResult<PillTakenDto>> TakePillAsync(TakePillDto dto, string userId);
        Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId, string userId);
        Task<OperationResult<PillTypeDto>> CreatePillTypeAsync(CreatePillTypeDto dto, string userId);
        Task<OperationResult<PillTypeDto>> EditPillAsync(EditPillTypeDto dto, string userId);
        Task<OperationResult<PillTypeDto>> RestorePillTypeAsync(int pillTypeId, string userId);
        Task<IReadOnlyList<PillTypeHubDto>> GetAllPillTypesForHubAsync();
    }
}
