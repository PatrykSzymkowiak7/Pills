using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Pills.Domain.Models;
using Pills.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Pills.Application.Common;
using Pills.Application.Common.Cache;
using Pills.Application.Common.Options;
using Pills.Application.DTOs.PillTaken;
using Pills.Application.DTOs.PillTypes;

namespace Pills.Application.Services
{
    public class PillService : IPillService
    {
        private readonly ILogger<PillService> _logger;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly FeatureFlags _features;
        private readonly IPillRepository _pillRepository;

        public PillService(IPillRepository pillRepository, ILogger<PillService> logger, 
            IMapper mapper, IMemoryCache cache, IOptions<FeatureFlags> options)
        {
            _pillRepository = pillRepository;
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

            if (await _pillRepository.PillTypeExistsByNameAsync(dto.Name))
                return OperationResult<PillTypeDto>.Fail(OperationStatus.AlreadyExists);

            var pillType = _mapper.Map<PillsTypes>(dto);
            pillType.CreatedBy = userId;

            _pillRepository.AddPillTypeAsync(pillType);
            await _pillRepository.SaveChangesAsync();

            var pillTypeDto = _mapper.Map<PillTypeDto>(pillType);

            _cache.Remove(CacheKeys.PillTypes);

            return OperationResult<PillTypeDto>.Ok(pillTypeDto);
        }

        public async Task<OperationResult<bool>> DeletePillTypeAsync(int pillTypeId, string userId)
        {
            if (!_features.EnablePillTypeDelete)
                return OperationResult<bool>.Fail(OperationStatus.FeatureDisabled);

            var pillType = await _pillRepository.GetPillTypeByIdIgnoreFiltersAsync(pillTypeId);

            if (pillType == null || pillType.IsDeleted)
                return OperationResult<bool>.Fail(OperationStatus.NotFound);

            await _pillRepository.SoftDeletePillTypeAsync(pillTypeId, userId);
            await _pillRepository.SaveChangesAsync();

            _cache.Remove(CacheKeys.PillTypes);

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<PillTakenDto>> TakePillAsync(TakePillDto dto, string userId)
        {
            if (userId == null)
                return OperationResult<PillTakenDto>.Fail(OperationStatus.Unauthorized);

            var pillType = await _pillRepository.GetPillTypeByIdAsync(dto.PillTypeId);

            if (pillType == null)
                return OperationResult<PillTakenDto>.Fail(OperationStatus.NotFound);

            var takenCount = await _pillRepository.CountTakenAsync(dto.PillTypeId, dto.Date, userId);

            if (takenCount >= pillType.MaxAllowed)
                return OperationResult<PillTakenDto>.Fail(OperationStatus.MaxLimitReached);

            var pillTaken = _mapper.Map<PillsTaken>(dto);
            pillTaken.UserId = userId;

            _pillRepository.AddPillTakenAsync(pillTaken);

            var pillTakenDto = _mapper.Map<PillTakenDto>(pillTaken);

            await _pillRepository.SaveChangesAsync();

            return OperationResult<PillTakenDto>.Ok(pillTakenDto);
        }

        public async Task<OperationResult<PillTypeDto>> EditPillAsync(EditPillTypeDto dto, string userId)
        {
            var pillType = await _pillRepository.GetPillTypeByIdAsync(dto.Id);

            if (pillType == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            pillType.Name = dto.Name;
            pillType.MaxAllowed = dto.MaxAllowed;
            pillType.EditedBy = userId;
            pillType.EditedAt = DateTime.Now;

            _pillRepository.AddPillTypeAsync(pillType);

            var pillDto = _mapper.Map<PillTypeDto>(pillType);

            _cache.Remove(CacheKeys.PillTypes);

            return OperationResult<PillTypeDto>.Ok(pillDto);
        }

        public async Task<OperationResult<PillTypeDto>> RestorePillTypeAsync(int pillTypeId, string userId)
        {
            var pillType = await _pillRepository.GetPillTypeByIdIgnoreFiltersAsync(pillTypeId);

            if (pillType == null)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            if (pillType.IsDeleted == false)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.InvalidData);

            var restored = await _pillRepository.RestorePillType(pillTypeId);

            if (restored == false)
                return OperationResult<PillTypeDto>.Fail(OperationStatus.NotFound);

            var dto = _mapper.Map<PillTypeDto>(pillType);

            _cache.Remove(CacheKeys.PillTypes);

            await _pillRepository.SaveChangesAsync();

            return OperationResult<PillTypeDto>.Ok(dto);
        }

        public async Task<IReadOnlyList<PillTypeHubDto>> GetAllPillTypesForHubAsync()
        {
            if(_cache.TryGetValue(CacheKeys.PillTypes, out List<PillTypeHubDto> cached))
            {
                return cached;
            }

            var pillTypes = await _pillRepository.GetAllPillTypesIgnoreFiltersAsync();

            var data = pillTypes.Select(p => new PillTypeHubDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    MaxAllowed = p.MaxAllowed,
                    IsDeleted = p.IsDeleted,
                    // fix me
                    TakenCount = 10
                }).ToList();

            _cache.Set(CacheKeys.PillTypes,
                data,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                });

            return data;
        }

        public async Task<IReadOnlyList<PillByDateDto>> GetPillsTakenByUserAndDateAsync(string UserId, DateTime date)
        {
            return await _pillRepository.GetPillsTakenByUserAndDateAsync(UserId, date);
        }

        public async Task<HistoryResultDto> GetHistoryAsync(string userId, int page, int pageSize, int? pillTypeId)
        {
            var data = await _pillRepository.GetHistoryDataAsync(userId, pillTypeId);

            var grouped = data
                .GroupBy(x => x.Date)
                .OrderByDescending(g => g.Key)
                .ToList();

            var totalDays = grouped.Count();

            var paged = grouped
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var days = paged.Select(g => new HistoryDayDto
            {
                Date = g.Key,
                PillsTaken = g.Select(x => x.PillName).ToList()
            }).ToList();

            return new HistoryResultDto
            {
                Days = days,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalDays / (double)pageSize),
                SelectedPillTypeId = pillTypeId
            };
        }

        public async Task<IReadOnlyList<PillTypeDto>> GetPillTypesAsync()
        {
            return await _pillRepository.GetPillTypesAsync();
        }
    }
}
