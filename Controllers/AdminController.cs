using acadgest.Interface;
using acadgest.Mappers;
using acadgest.Models.Coordenations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace acadgest.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICoordenationRepository _coordenationRepo;
        private readonly IAccountRepository _accountRepo;

        public AdminController(ICoordenationRepository coordenationRepo, IAccountRepository accountRepo)
        {
            _coordenationRepo = coordenationRepo;
            _accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index()
        {
            var coordenations = await _coordenationRepo.GetAllAsync();
            var coordinationsDto = coordenations.Select(c => c.ToCoordinationDto());
            return View(coordinationsDto);
        }

        [Route("novacoordenacao")]
        public IActionResult NewCoordenation()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Coordinator")]
        [Route("editcoordinator/{id}")]
        public async Task<IActionResult> EditCoordinator(Guid id)
        {
            var coordenation = await _coordenationRepo.GetByIdAsync(id);
            if (coordenation == null) return NotFound("Coordenação não existe não existe");

            ViewData["coordenation"] = coordenation.Name;
            ViewData["coordenationId"] = coordenation.Id;

            var users = await _accountRepo.GetAllAsync();
            return View(users);
        }


        [Route("setcoordinator/{id}")]
        [HttpPost]
        public async Task<IActionResult> SetCoordinator(Guid id, [FromForm] Guid coordinatorId)
        {
            var result = await _coordenationRepo.SetCoordinator(id, coordinatorId);
            if (!result.Succeeded) return NotFound(result.Error);
            return RedirectToAction("Index", "Admin");
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
