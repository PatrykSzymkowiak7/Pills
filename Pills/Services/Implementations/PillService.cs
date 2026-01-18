using Pills.Models;
using Pills.Services.Interfaces;
using Pills.Common;
using Microsoft.EntityFrameworkCore;

namespace Pills.Services.Implementations
{
    public class PillService : IPillService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PillService> _logger;

        public PillService(AppDbContext dbContext, ILogger<PillService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<OperationResult<PillsTypes>> CreatePillTypeAsync(string name, int maxAllowed, string userId)
        {
            if (await _dbContext.PillsTypes.AnyAsync(p => p.Name == name))
                return OperationResult<PillsTypes>.Fail(OperationStatus.AlreadyExists);

            if (maxAllowed < 1)
                return OperationResult<PillsTypes>.Fail(OperationStatus.InvalidData);

            if (string.IsNullOrWhiteSpace(name))
                return OperationResult<PillsTypes>.Fail(OperationStatus.InvalidData);

            var pillType = new PillsTypes
            {
                Name = name.Trim(),
                MaxAllowed = maxAllowed,
                CreatedBy = userId
            };

            _dbContext.PillsTypes.Add(pillType);
            await _dbContext.SaveChangesAsync();

            return OperationResult<PillsTypes>.Ok(pillType);
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

        public async Task<OperationResult<PillsTaken>> TakePillAsync(int pillTypeId, DateTime date, string userId)
        {
            if (userId == null)
                return OperationResult<PillsTaken>.Fail(OperationStatus.InvalidUser);

            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(pt => pt.Id == pillTypeId);

            if (pillType == null)
                return OperationResult<PillsTaken>.Fail(OperationStatus.NotFound);

            var takenCount = await _dbContext.PillsTaken.CountAsync(pt =>
                pt.PillType.Id == pillTypeId &&
                pt.Date == date &&
                pt.UserId == userId);

            if (takenCount >= pillType.MaxAllowed)
                return OperationResult<PillsTaken>.Fail(OperationStatus.MaxLimitReached);

            var pillTaken = new PillsTaken
            {
                Date = date,
                PillType = pillType,
                UserId = userId
            };

            _dbContext.PillsTaken.Add(pillTaken);

            await _dbContext.SaveChangesAsync();

            return OperationResult<PillsTaken>.Ok(pillTaken);
        }

        public async Task<OperationResult<PillsTypes>> EditPillAsync(int id, string name, int maxAllowed, string userId)
        {
            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(p => p.Id == id);

            if (pillType == null)
                return OperationResult<PillsTypes>.Fail(OperationStatus.NotFound);

            pillType.Name = name;
            pillType.MaxAllowed = maxAllowed;
            pillType.EditedBy = userId;
            pillType.EditedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return OperationResult<PillsTypes>.Ok(pillType);
        }
    }
}
