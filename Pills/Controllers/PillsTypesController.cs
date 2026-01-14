using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Models;
using Pills.Models.ViewModels.PillTypes;
using Pills.Services.Interfaces;
using Pills.Common;

namespace Pills.Controllers
{
    public class PillsTypesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IPillService _pillService;

        public PillsTypesController(AppDbContext dbContext, IPillService pillService)
        {
            _dbContext = dbContext;
            _pillService = pillService;
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePillTypeViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            try
            {
                var result = _pillService.CreatePillType(model.Name, model.MaxAllowed);

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
        public IActionResult PillTypeHub()
        {
            var model = _dbContext.PillsTypes
                .AsNoTracking()
                .Select(pt => 
                new PillTypeHubViewModel
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    Count = _dbContext.PillsTaken.Where(pta => pta.PillType.Id == pt.Id).Count(),
                    MaxAllowed = pt.MaxAllowed
                }).ToList();

            return View(model);
        }

        public IActionResult ConfirmDelete(int id)
        {
            var pillType = _dbContext.PillsTypes.Where(pt => pt.Id == id).Select(pt => new DeletePillTypeViewModel
            {
                Id = pt.Id,
                Name = pt.Name,
                Count = _dbContext.PillsTaken.Where(pta => pta.PillType.Id == pt.Id).Count()
            }).SingleOrDefault();

            if (pillType == null)
                return NotFound();

            return View(pillType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _pillService.DeletePillType(id);

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
        public IActionResult Edit(EditPillTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var pillType = _dbContext.PillsTypes.Find(model.Id);
            if (pillType == null)
                return NotFound();

            pillType.Name = model.Name;
            pillType.MaxAllowed = model.MaxAllowed;

            _dbContext.SaveChanges();

            TempData[TempDataKeys.Success] = "Tabletka zaktualizowana";

            return RedirectToAction(nameof(PillTypeHub));
        }
    }
}
