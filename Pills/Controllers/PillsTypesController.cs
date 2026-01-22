using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pills.Models.ViewModels.PillTypes;
using Pills.Services.Interfaces;
using Pills.Common;
using Microsoft.AspNetCore.Authorization;
using Pills.Identity;
using Pills.Controllers.Filters;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Runtime.CompilerServices;
using AutoMapper;
using Pills.Models.DTOs.PillTypes;

namespace Pills.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class PillsTypesController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IPillService _pillService;
        private readonly ILogger<PillsTypesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PillsTypesController(AppDbContext dbContext, IPillService pillService, ILogger<PillsTypesController> logger, 
            UserManager<ApplicationUser> userManager, IMapper mapper, IUserService userService)
        {
            _dbContext = dbContext;
            _pillService = pillService;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AdminAuditFilter))]
        public async Task<IActionResult> Create(CreatePillTypeViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            try
            {
                var dto = _mapper.Map<CreatePillTypeDto>(model);
                var userId = _userService.UserId;

                var result = await _pillService.CreatePillTypeAsync(dto, userId);

                switch (result.Status)
                {
                    case OperationStatus.Success:
                        TempData[TempDataKeys.Success] = "The operation was successful";
                        break;

                    case OperationStatus.AlreadyExists:
                        TempData[TempDataKeys.Error] = "The pill with the same name already exists";
                        break;

                    case OperationStatus.Error:
                        TempData[TempDataKeys.Error] = "An error occured";
                        break;

                    case OperationStatus.InvalidData:
                        TempData[TempDataKeys.Error] = "The data entered is incorrect";
                        break;

                    default:
                        throw new InvalidOperationException($"An unhandled exception occured: {result}");
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
            var pillTypeHubDto = await _pillService.GetAllPillTypesForHubAsync();

            var model = _mapper.Map<List<PillTypeHubViewModel>>(pillTypeHubDto);

            return View(model);
        }

        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var pillType = await _dbContext.PillsTypes
                .Where(pt => pt.Id == id)
                .Select(pt => new DeletePillTypeViewModel
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        Count = _dbContext.PillsTaken
                            .Where(pta => pta.PillTypeId == pt.Id)
                            .Count()
                    }).SingleOrDefaultAsync();

            if (pillType == null)
                return NotFound();

            return View(pillType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AdminAuditFilter))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userService.UserId;
            var result = await _pillService.DeletePillTypeAsync(id, userId);

            switch(result.Status)
            {
                case OperationStatus.Success:
                    TempData[TempDataKeys.Success] = "The operation was successful";
                    break;

                case OperationStatus.NotFound:
                    TempData[TempDataKeys.Error] = "Pill not found";
                    break;

                case OperationStatus.Error:
                    TempData[TempDataKeys.Error] = "An error during deletion occured";
                    break;

                default:
                    throw new InvalidOperationException($"An unhandled exception occured: {result}");
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
                MaxAllowed = pillType.MaxAllowed,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AdminAuditFilter))]
        public async Task<IActionResult> Edit(EditPillTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var userId = _userService.UserId;
                var dto = _mapper.Map<EditPillTypeDto>(model);

                var result = await _pillService.EditPillAsync(dto, userId);

                switch (result.Status)
                {
                    case OperationStatus.NotFound:
                        TempData[TempDataKeys.Error] = "Pill type not found";
                        break;

                    case OperationStatus.Success:
                        TempData[TempDataKeys.Success] = "Pill type updated";
                        break;

                    default:
                        throw new InvalidOperationException($"An unhandled exception occured: {result.Status}");
                }
            }
            catch(Exception)
            {
                return StatusCode(500);
            }

            return RedirectToAction(nameof(PillTypeHub));
        }

        public async Task<IActionResult> ConfirmRestore(int id)
        {
            var model = await _dbContext.PillsTypes
                .IgnoreQueryFilters()
                .Where(p => p.Id == id)
                .Select(pt => new RestorePillTypeViewModel
                {
                    Id = pt.Id,
                    Name = pt.Name,
                    MaxAllowed = pt.MaxAllowed,
                    Count = _dbContext.PillsTaken
                        .IgnoreQueryFilters()
                        .Where(pta => pta.PillTypeId == pt.Id)
                        .Count()
                }).SingleOrDefaultAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(AdminAuditFilter))]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var userId = _userService.UserId;

            var result = await _pillService.RestorePillTypeAsync(id, userId);

            switch(result.Status)
            {
                case OperationStatus.Success:
                    TempData[TempDataKeys.Success] = "The operation was successful";
                    break;

                case OperationStatus.NotFound:
                    TempData[TempDataKeys.Error] = "Pill type not found";
                    break;

                default:
                    throw new InvalidOperationException($"An unhandled exception occured: {result.Status}");
            }

            return RedirectToAction(nameof(PillTypeHub));
        }
    }
}
