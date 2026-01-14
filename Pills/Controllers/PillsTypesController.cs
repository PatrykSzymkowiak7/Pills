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

        public IActionResult Delete()
        {
            var model = _dbContext.PillsTypes
                .AsNoTracking()
                .Select(pt => 
                new DeletePillTypeViewModel
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    Count = _dbContext.PillsTaken.Where(pta => pta.PillType.Id == pt.Id).Count()
                }).ToList();

            return View(model);
        }

        public IActionResult ConfirmDelete(int pillTypeId)
        {
            var pillType = _dbContext.PillsTypes.Where(pt => pt.Id == pillTypeId).Select(pt => new DeletePillTypeViewModel
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
        public IActionResult DeleteConfirmed(int pillTypeId)
        {
            var result = _pillService.DeletePillType(pillTypeId);

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

            return RedirectToAction(nameof(Delete));
        }
    }
}
