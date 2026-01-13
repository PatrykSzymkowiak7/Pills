using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Models;
using Pills.Models.ViewModels.PillTypes;
using Pills.Services.Interfaces;

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

                switch (result)
                {
                    case OperationResult.Success:
                        TempData["Success"] = "Operacja przebiegła pomyślnie";
                        break;

                    case OperationResult.AlreadyExists:
                        TempData["Error"] = "Tabletka o takiej nazwie już istnieje";
                        break;

                    case OperationResult.Error:
                        TempData["Error"] = "Wystąpił błąd podczas dodawania typu tabletki";
                        break;

                    case OperationResult.InvalidData:
                        TempData["Error"] = "Wprowadzono niepoprawne dane";
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
                    Name = pt.Name
                }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var result = _pillService.DeletePillType(id);

            switch(result)
            {
                case OperationResult.Success:
                    TempData["Success"] = "Operacja przebiegła pomyślnie";
                    break;

                case OperationResult.NotFound:
                    TempData["Error"] = "Nie znaleziono takiej tabletki";
                    break;

                case OperationResult.Error:
                    TempData["Error"] = "Wystąpił błąd podczas usuwania tabletki";
                    break;

                default:
                    throw new InvalidOperationException($"Wystąpił nieobsłużony wyjątek: {result}");
            }

            return RedirectToAction(nameof(Delete));
        }
    }
}
