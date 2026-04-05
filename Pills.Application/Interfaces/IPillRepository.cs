using Pills.Application.Common;
using Pills.Application.DTOs.PillTaken;
using Pills.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pills.Application.Interfaces
{
    public interface IPillRepository
    {
        Task<bool> PillTypeExistsByNameAsync(string name);
        void AddPillTakenAsync(PillsTaken pillTaken);
        void AddPillTypeAsync(PillsTypes pillType);
        Task SaveChangesAsync();
        void SaveChanges();
        Task<PillsTypes?> GetPillTypeByIdAsync(int id);
        Task<PillsTypes?> GetPillTypeByIdIgnoreFiltersAsync(int id);
        Task<int> CountTakenAsync(int pillTypeId, DateTime date, string userId);
        Task SoftDeletePillTypeAsync(int id, string userId);
        Task<IReadOnlyList<PillsTypes>> GetAllPillTypesIgnoreFiltersAsync();
        Task<bool> RestorePillType(int id);
        Task<IReadOnlyList<PillByDateDto>> GetPillsTakenByUserAndDateAsync(string userId, DateTime date);
        public Task<List<(DateTime Date, string PillName)>> GetHistoryDataAsync(string userId, int? pillTypeId);
    }
}
