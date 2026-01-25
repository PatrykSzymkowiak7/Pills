using Pills.Models;
using Pills.Services.Interfaces;
using Pills.Common;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Pills.Models.DTOs.PillTypes;
using Pills.Models.DTOs.PillTaken;
using Microsoft.Extensions.Caching.Memory;
using Pills.Common.Cache;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Pills.Options;

namespace Pills.Services.Implementations
{
    public class PillService : IPillService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PillService> _logger;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly FeatureFlags _features;

        public PillService(AppDbContext dbContext, ILogger<PillService> logger, 
            IMapper mapper, IMemoryCache cache, IOptions<FeatureFlags> options)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
            _features = options.Value;
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

            _cache.Remove(CacheKeys.PillTypes);

            return OperationResult<PillTypeDto>.Ok(pillTypeDto);
        }

        public async Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId, string userId)
        {
            if (!_features.EnablePillTypeDelete)
                return OperationResult<bool>.Fail(OperationStatus.FeatureDisabled);

            var pillType = await _dbContext.PillsTypes
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(pt => pt.Id == pillTypeId);

            if (pillType == null || pillType.IsDeleted)
                return OperationResult<bool>.Fail(OperationStatus.NotFound);

            await _dbContext.PillsTaken
                .IgnoreQueryFilters()
                .Where(pt => pt.PillTypeId == pillTypeId)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.IsDeleted, true)
                .SetProperty(p => p.DeletedBy, userId));

            _dbContext.Remove(pillType);
            await _dbContext.SaveChangesAsync();

            _cache.Remove(CacheKeys.PillTypes);

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<PillTakenDto>> TakePillAsync(TakePillDto dto, string userId)
        {
            if (userId == null)
                return OperationResult<PillTakenDto>.Fail(OperationStatus.Unauthorized);

            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(pt => pt.Id == dto.PillTypeId);

            if (pillType == null)
                return OperationResult<PillTakenDto>.Fail(OperationStatus.NotFound);

            var takenCount = await _dbContext.PillsTaken.CountAsync(pt =>
                pt.PillType.Id == dto.PillTypeId &&
                pt.Date.Date == dto.Date.Date &&
                pt.UserId == userId);

            if (takenCount >= pillType.MaxAllowed)
                return OperationResult<PillTakenDto>.Fail(OperationStatus.MaxLimitReached);

            var pillTaken = _mapper.Map<PillsTaken>(dto);
            pillTaken.UserId = userId;

            _dbContext.PillsTaken.Add(pillTaken);

            await _dbContext.SaveChangesAsync();

            var pillTakenDto = _mapper.Map<PillTakenDto>(pillTaken);

            return OperationResult<PillTakenDto>.Ok(pillTakenDto);
        }

        public async Task<OperationResult<PillTypeDto>> EditPillAsync(EditPillTypeDto dto, string userId)
        {
            var pillType = await _dbContext.PillsTypes.SingleOrDefaultAsync(p => p.Id == dto.Id);

            if (pillType == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            pillType.Name = dto.Name;
            pillType.MaxAllowed = dto.MaxAllowed;
            pillType.EditedBy = userId;
            pillType.EditedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            var pillDto = _mapper.Map<PillTypeDto>(pillType);

            _cache.Remove(CacheKeys.PillTypes);

            return OperationResult<PillTypeDto>.Ok(pillDto);
        }

        public async Task<OperationResult<PillTypeDto>> RestorePillTypeAsync(int pillTypeId, string userId)
        {
            _dbContext.IgnoreSoftDelete = true; 

            var pillType = await _dbContext.PillsTypes
                .SingleOrDefaultAsync(p => p.Id == pillTypeId);

            if (pillType == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            if (pillType.IsDeleted == false)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.InvalidData);

            pillType.IsDeleted = false;

            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<PillTypeDto>(pillType);

            _cache.Remove(CacheKeys.PillTypes);

            return OperationResult<PillTypeDto>.Ok(dto);
        }

        public async Task<IReadOnlyList<PillTypeHubDto>> GetAllPillTypesForHubAsync()
        {
            if(_cache.TryGetValue(CacheKeys.PillTypes, out List<PillTypeHubDto> cached))
            {
                return cached;
            }

            _dbContext.IgnoreSoftDelete = true;

            var data = await _dbContext.PillsTypes
                .Select(p => new PillTypeHubDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    MaxAllowed = p.MaxAllowed,
                    IsDeleted = p.IsDeleted,
                    TakenCount = _dbContext.PillsTaken
                        .Count(pta => pta.PillTypeId == p.Id)
                }).ToListAsync();

            _cache.Set(CacheKeys.PillTypes,
                data,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                });

            return data;
        }
    }
}
