using Pills.Models;
using Pills.Services.Interfaces;
using Pills.Common;

namespace Pills.Services.Implementations
{
    public class PillService : IPillService
    {
        private readonly AppDbContext _dbContext;

        public PillService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OperationResult<int> CreatePillType(string name, int maxAllowed)
        {
            if (_dbContext.PillsTypes.Any(p => p.Name == name))
                return OperationResult<int>.Fail(OperationStatus.AlreadyExists);

            if (maxAllowed < 1)
                return OperationResult<int>.Fail(OperationStatus.InvalidData);

            if (string.IsNullOrWhiteSpace(name))
                return OperationResult<int>.Fail(OperationStatus.InvalidData);

            var pillType = new PillsTypes
            {
                Name = name.Trim(),
                MaxAllowed = maxAllowed
            };

            _dbContext.PillsTypes.Add(pillType);
            _dbContext.SaveChanges();

            return OperationResult<int>.Ok(pillType.Id);
        }

        public OperationResult<bool> DeletePillType(int pillTypeId)
        {
            var pillType = _dbContext.PillsTypes.SingleOrDefault(pt => pt.Id == pillTypeId);

            if (pillType == null)
                return OperationResult<bool>.Fail(OperationStatus.NotFound);

            var pillsTaken = _dbContext.PillsTaken.Where(pt => pt.PillType.Id == pillTypeId).ToList();

            _dbContext.PillsTaken.RemoveRange(pillsTaken);
            _dbContext.PillsTypes.Remove(pillType);
            _dbContext.SaveChanges();

            return OperationResult<bool>.Ok(true);
        }

        public OperationResult<bool> TakePill(int pillTypeId, DateTime date)
        {
            var pillType = _dbContext.PillsTypes.SingleOrDefault(pt => pt.Id == pillTypeId);

            if (pillType == null)
                return OperationResult<bool>.Fail(OperationStatus.NotFound);

            var takenCount = _dbContext.PillsTaken.Count(pt =>
                pt.PillType.Id == pillTypeId &&
                pt.Date == date);

            if (takenCount >= pillType.MaxAllowed)
                return OperationResult<bool>.Fail(OperationStatus.MaxLimitReached);

            _dbContext.PillsTaken.Add(new PillsTaken
            {
                Date = date,
                PillType = pillType
            });

            _dbContext.SaveChanges();

            return OperationResult<bool>.Ok(true);
        }
    }
}
