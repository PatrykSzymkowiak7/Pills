using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Models.ViewModels.PillTypes;
using Pills.Services.Interfaces;
using Pills.Common;
using Microsoft.AspNetCore.Authorization;
using Pills.Identity;

namespace Pills.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class PillsTypesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IPillService _pillService;
        private readonly ILogger<PillsTypesController> _logger;

        public PillsTypesController(AppDbContext dbContext, IPillService pillService, ILogger<PillsTypesController> logger)
        {
            _dbContext = dbContext;
            _pillService = pillService;
            _logger = logger;
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePillTypeViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _pillService.CreatePillTypeAsync(model.Name, model.MaxAllowed);

                switch (result.Status)
                {
                    case OperationStatus.Success:
                        TempData[TempDataKeys.Success] = "Operacja przebiegła pomyślnie";
                        break;

                    case OperationStatus.AlreadyExists:
                        TempData[TempDataKeys.Error] = "Tabletka o takiej nazwie już istnieje";
                        break;

                    case OperationStatus.Error:
                        TempData[TempDataKeys.Error] = "Wystąpił błąd podczas dodawania typu tabletki";
                        break;

                    case OperationStatus.InvalidData:
                        TempData[TempDataKeys.Error] = "Wprowadzono niepoprawne dane";
                        break;

                    default:
                        throw new InvalidOperationException($"Wystąpił nieobsłużony wyjątek: {result}");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }

            return View();
        }

        // GET
        public async Task<IActionResult> PillTypeHub()
        {
            var model = await _dbContext.PillsTypes
                .AsNoTracking()
                .Select(pt => 
                new PillTypeHubViewModel
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    Count = _dbContext.PillsTaken.Where(pta => pta.PillType.Id == pt.Id).Count(),
                    MaxAllowed = pt.MaxAllowed
                }).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var pillType = await _dbContext.PillsTypes.Where(pt => pt.Id == id).Select(pt => new DeletePillTypeViewModel
            {
                Id = pt.Id,
                Name = pt.Name,
                Count = _dbContext.PillsTaken.Where(pta => pta.PillType.Id == pt.Id).Count()
            }).SingleOrDefaultAsync();

            if (pillType == null)
                return NotFound();

            return View(pillType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _pillService.DeletePillTypeAsync(id);

            switch(result.Status)
            {
                case OperationStatus.Success:
                    TempData[TempDataKeys.Success] = "Operacja przebiegła pomyślnie";
                    break;

                case OperationStatus.NotFound:
                    TempData[TempDataKeys.Error] = "Nie znaleziono takiej tabletki";
                    break;

                case OperationStatus.Error:
                    TempData[TempDataKeys.Error] = "Wystąpił błąd podczas usuwania tabletki";
                    break;

                default:
                    throw new InvalidOperationException($"Wystąpił nieobsłużony wyjątek: {result}");
            }

            return RedirectToAction(nameof(PillTypeHub));
        }

        // GET
        public IActionResult Edit(int id)
        {
            var pillType = _dbContext.PillsTypes.Find(id);
            if (pillType == null)
                return NotFound();

            var model = new EditPillTypeViewModel
            {
                Id = pillType.Id,
                Name = pillType.Name,
                MaxAllowed = pillType.MaxAllowed
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditPillTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _pillService.EditPillAsync(model.Id, model.Name, model.MaxAllowed);

            switch(result.Status)
            {
                case OperationStatus.NotFound:
                    TempData[TempDataKeys.Error] = "Nie znaleziono takiej tabletki";
                    break;

                case OperationStatus.Success:
                    TempData[TempDataKeys.Success] = "Operacja przebiegła pomyślnie";
                    break;

                default:
                    throw new InvalidOperationException($"Wystąpił nieobsłużony wyjątek: {result.Status}");
            }

            TempData[TempDataKeys.Success] = "Tabletka zaktualizowana";

            return RedirectToAction(nameof(PillTypeHub));
        }
    }
}
