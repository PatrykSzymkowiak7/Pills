using Pills.Application.Common;
using Pills.Application.DTOs.PillTaken;
using Pills.Application.DTOs.PillTypes;

namespace Pills.Application.Interfaces
{
    public interface IPillService
    {
        Task<OperationResult<PillTakenDto>> TakePillAsync(TakePillDto dto, string userId);
        Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId, string userId);
        Task<OperationResult<PillTypeDto>> CreatePillTypeAsync(CreatePillTypeDto dto, string userId);
        Task<OperationResult<PillTypeDto>> EditPillAsync(EditPillTypeDto dto, string userId);
        Task<OperationResult<PillTypeDto>> RestorePillTypeAsync(int pillTypeId, string userId);
        Task<IReadOnlyList<PillTypeHubDto>> GetAllPillTypesForHubAsync();
        Task<IReadOnlyList<PillByDateDto>> GetPillsTakenByUserAndDateAsync(string UserId,DateTime date);
        Task<HistoryResultDto> GetHistoryAsync(string userId, int page, int pageSize, int? pillTypeId);
    }
}
