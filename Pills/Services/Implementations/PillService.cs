using Pills.Models;
using Pills.Services.Interfaces;

namespace Pills.Services.Implementations
{
    public class PillService : IPillService
    {
        private readonly AppDbContext _dbContext;

        public PillService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public OperationResult CreatePillType(string name, int maxAllowed)
        {
            if(_dbContext.PillsTypes.Any(p => p.Name == name))
                return OperationResult.AlreadyExists;

            if (maxAllowed < 1)
                return OperationResult.InvalidData;

            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.InvalidData;

            var pillType = new PillsTypes
            {
                Name = name.Trim(),
                MaxAllowed = maxAllowed
            };

            _dbContext.PillsTypes.Add(pillType);
            _dbContext.SaveChanges();

            return OperationResult.Success;
        }

        public OperationResult DeletePillType(int pillTypeId)
        {
            var pillType = _dbContext.PillsTypes.SingleOrDefault(pt => pt.Id == pillTypeId);

            if (pillType == null)
                return OperationResult.NotFound;

            var pillsTaken = _dbContext.PillsTaken.Where(pt => pt.PillType.Id == pillTypeId).ToList();

            _dbContext.PillsTaken.RemoveRange(pillsTaken);
            _dbContext.PillsTypes.Remove(pillType);
            _dbContext.SaveChanges();

            return OperationResult.Success;
        }

        public OperationResult TakePill(int pillTypeId, DateTime date)
        {
            var pillType = _dbContext.PillsTypes.SingleOrDefault(pt => pt.Id == pillTypeId);

            if (pillType == null)
                return OperationResult.NotFound;

            var takenCount = _dbContext.PillsTaken.Count(pt =>
                pt.PillType.Id == pillTypeId &&
                pt.Date == pt.Date);

            if (takenCount >= pillType.MaxAllowed)
                return OperationResult.MaxLimitReached;

            _dbContext.PillsTaken.Add(new PillsTaken
            {
                Date = date,
                PillType = pillType
            });

            _dbContext.SaveChanges();

            return OperationResult.Success;
        }
    }
}
