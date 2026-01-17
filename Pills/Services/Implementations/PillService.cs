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

        public async Task<OperationResult<PillsTypes>> CreatePillTypeAsync(string name, int maxAllowed)
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
                MaxAllowed = maxAllowed
            };

            _dbContext.PillsTypes.Add(pillType);
            await _dbContext.SaveChangesAsync();

            return OperationResult<PillsTypes>.Ok(pillType);
        }

        public async Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId)
        {
            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(pt => pt.Id == pillTypeId);

            if (pillType == null)
                return OperationResult<bool>.Fail(OperationStatus.NotFound);

            var pillsTaken = await _dbContext.PillsTaken.Where(pt => pt.PillType.Id == pillTypeId).ToListAsync();

            _dbContext.PillsTaken.RemoveRange(pillsTaken);
            _dbContext.PillsTypes.Remove(pillType);
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
                pt.Date == date);

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

        public async Task<OperationResult<PillsTypes>> EditPillAsync(int id, string name, int maxAllowed)
        {
            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(p => p.Id == id);

            if (pillType == null)
                return OperationResult<PillsTypes>.Fail(OperationStatus.NotFound);

            pillType.Name = name;
            pillType.MaxAllowed = maxAllowed;

            await _dbContext.SaveChangesAsync();

            return OperationResult<PillsTypes>.Ok(pillType);
        }
    }
}
