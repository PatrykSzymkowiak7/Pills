using Microsoft.EntityFrameworkCore;
using Pills.Application.Common;
using Pills.Application.DTOs.PillTaken;
using Pills.Application.Interfaces;
using Pills.Domain.Models;

namespace Pills.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for PillsTaken and PillsTypes.
    /// Methods doesnt save changes in db automatically, manual SaveChanges or SaveChangesAsync is needed.
    /// </summary>
    public class PillRepository : IPillRepository
    {
        private readonly AppDbContext _dbContext;
        public PillRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPillTakenAsync(PillsTaken pillTaken)
        {
            _dbContext.PillsTaken.Add(pillTaken);
        }

        public void AddPillTypeAsync(PillsTypes pillType)
        {
            _dbContext.PillsTypes.Add(pillType);
        }

        public async Task<int> CountTakenAsync(int pillTypeId, DateTime date, string userId)
        {
            return await _dbContext.PillsTaken
                .Where(pt => pt.Id == pillTypeId && 
                    pt.Date == date && 
                    pt.UserId == userId)
                .CountAsync();
        }

        public async Task<IReadOnlyList<PillsTypes>> GetAllPillTypesIgnoreFiltersAsync()
        {
            return await _dbContext.PillsTypes
                .IgnoreQueryFilters()
                .ToListAsync();
        }

        public Task<PillsTypes?> GetPillTypeByIdAsync(int id)
        {
            return _dbContext.PillsTypes
                .FirstOrDefaultAsync(pt => pt.Id == id && pt.IsDeleted == false);
        }

        public async Task<PillsTypes?> GetPillTypeByIdIgnoreFiltersAsync(int id)
        {
            return await _dbContext.PillsTypes
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(pt => pt.Id == id);
        }

        public async Task<bool> PillTypeExistsByNameAsync(string name)
        {
            var pillType = await _dbContext.PillsTypes
                .FirstOrDefaultAsync(pt => pt.Name == name);

            return pillType != null ? true : false;
        }

        public async Task<bool> RestorePillType(int id)
        {
            var pillType = await _dbContext.PillsTypes
                .IgnoreQueryFilters()
                .Where(pt => pt.Id == id)
                .FirstOrDefaultAsync();

            if (pillType == null)
                return false;

            pillType.IsDeleted = false;
            pillType.DeletedAt = null;
            pillType.DeletedBy = null;

            var pillsTaken = await _dbContext.PillsTaken
                .Where(pt => pt.PillTypeId == pillType.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.IsDeleted, false)
                    .SetProperty(p => p.DeletedAt, (DateTime?) null)
                    .SetProperty(p => p.DeletedBy, (string?) null));

            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        // fix pillstaken
        public async Task SoftDeletePillTypeAsync(int id, string userId)
        {
            var pillType = await _dbContext.PillsTaken
                .IgnoreQueryFilters()
                .Where(pt => pt.PillTypeId == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.IsDeleted, true)
                    .SetProperty(p => p.DeletedBy, userId));

            _dbContext.Remove(pillType);
        }

        public async Task<IReadOnlyList<PillByDateDto>> GetPillsTakenByUserAndDateAsync(string userId, DateTime date)
        {
            return await _dbContext.PillsTypes
                .Select(pt => new PillByDateDto
                {
                    PillTypeId = pt.Id,
                    Name = pt.Name,
                    MaxAllowed = pt.MaxAllowed,
                    TakenCountToday = _dbContext.PillsTaken
                        .Count(p => 
                            p.PillTypeId == pt.Id &&
                            p.UserId == userId &&
                            p.Date.Date == date)
                })
                .ToListAsync();
        }

        public async Task<List<(DateTime Date, string PillName)>> GetHistoryDataAsync(string userId, int? pillTypeId)
        {
            var query = _dbContext.PillsTaken
                .Include(p => p.PillType)
                .Where(p => p.UserId == userId);

            if (pillTypeId.HasValue)
                query = query.Where(p => p.PillTypeId == pillTypeId);

            return await query.Select(p => new ValueTuple<DateTime, string>(
                p.Date.Date,
                p.PillType.Name
            )).ToListAsync();
        }
    }
}
