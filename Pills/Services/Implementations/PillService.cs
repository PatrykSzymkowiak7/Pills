using Pills.Models;
using Pills.Services.Interfaces;
using Pills.Common;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Pills.Models.DTOs;

namespace Pills.Services.Implementations
{
    public class PillService : IPillService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PillService> _logger;
        private readonly IMapper _mapper;

        public PillService(AppDbContext dbContext, ILogger<PillService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<OperationResult<PillTypeDto>> CreatePillTypeAsync(CreatePillTypeDto dto, string userId)
        {
            if (dto.MaxAllowed < 1)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.InvalidData);

            if (string.IsNullOrWhiteSpace(dto.Name))
                return OperationResult<PillTypeDto>.Fail(OperationStatus.InvalidData);

            if (string.IsNullOrEmpty(userId))
                return OperationResult<PillTypeDto>.Fail(OperationStatus.Unauthorized);

            if (await _dbContext.PillsTypes.AnyAsync(p => p.Name == dto.Name))
                return OperationResult<PillTypeDto>.Fail(OperationStatus.AlreadyExists);

            var pillType = _mapper.Map<PillsTypes>(dto);
            pillType.CreatedBy = userId;

            _dbContext.PillsTypes.Add(pillType);
            await _dbContext.SaveChangesAsync();

            var pillTypeDto = _mapper.Map<PillTypeDto>(pillType);

            return OperationResult<PillTypeDto>.Ok(pillTypeDto);
        }

        public async Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId, string userId)
        {
            var pillType = await _dbContext.PillsTypes
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(pt => pt.Id == pillTypeId);

            if (pillType == null || pillType.IsDeleted)
                return OperationResult<bool>.Fail(OperationStatus.NotFound);

            pillType.IsDeleted = true;

            await _dbContext.PillsTaken
                .IgnoreQueryFilters()
                .Where(pt => pt.PillTypeId == pillTypeId)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.IsDeleted, true)
                .SetProperty(p => p.DeletedBy, userId));

            await _dbContext.SaveChangesAsync();

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<PillTypeDto>> TakePillAsync(int pillTypeId, DateTime date, string userId)
        {
            if (userId == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.InvalidUser);

            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(pt => pt.Id == pillTypeId);

            if (pillType == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            var takenCount = await _dbContext.PillsTaken.CountAsync(pt =>
                pt.PillType.Id == pillTypeId &&
                pt.Date == date &&
                pt.UserId == userId);

            if (takenCount >= pillType.MaxAllowed)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.MaxLimitReached);

            var pillTaken = new PillsTaken
            {
                Date = date,
                PillType = pillType,
                UserId = userId
            };

            _dbContext.PillsTaken.Add(pillTaken);

            await _dbContext.SaveChangesAsync();

            return OperationResult<PillTypeDto>.Ok(pillTaken);
        }

        public async Task<OperationResult<PillTypeDto>> EditPillAsync(int id, string name, int maxAllowed, string userId)
        {
            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(p => p.Id == id);

            if (pillType == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            pillType.Name = name;
            pillType.MaxAllowed = maxAllowed;
            pillType.EditedBy = userId;
            pillType.EditedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return OperationResult<PillTypeDto>.Ok(pillType);
        }

        public async Task<OperationResult<PillTypeDto>> RestorePillTypeAsync(int pillTypeId, string userId)
        {
            var pillType = await _dbContext.PillsTypes
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(p => p.Id == pillTypeId);

            if (pillType == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            if (pillType.IsDeleted == false)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.InvalidData);

            pillType.IsDeleted = false;

            await _dbContext.SaveChangesAsync();

            return OperationResult<PillTypeDto>.Ok(pillType);
        }
    }
}
