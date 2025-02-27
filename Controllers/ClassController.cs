using System.Threading.Tasks;
using acadgest.Dto.Class;
using acadgest.Interface;
using acadgest.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace acadgest.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ClassController : Controller
    {
        private readonly IClassRepository _classRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly ICourseRepository _courseRepo;
        public ClassController(IClassRepository classRepo, IAccountRepository accountRepo, ICourseRepository courseRepo)
        {
            _classRepo = classRepo;
            _accountRepo = accountRepo;
            _courseRepo = courseRepo;
        }

        [Route("{id}")]
        [Authorize(Roles = "Admin,Coordinator,Classdirector")]
        public async Task<IActionResult> Index([FromRoute] Guid id, [FromQuery] int trim)
        {
            var model = await _classRepo.ClassDetailsAsync(id, trim);

            // return Ok(model);
            return View(model);
        }
        [Authorize(Roles = "Classdirector")]
        public async Task<IActionResult> ClassDirector()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var model = await _classRepo.GetByClassDirector(Guid.Parse(userId));
            return View(model);
        }
        [Authorize(Roles = "Admin,Coordinator")]
        [Route("new/{id}")]
        public async Task<IActionResult> NewClass([FromRoute] Guid id)
        {
            var model = new ClassModelView();
            model.NewClass.CoordenationId = id;
            var users = await _accountRepo.GetAllAsync();

            if (users != null)
                model.Users = users;

            var courses = await _courseRepo.GetAllAsync();
            model.Courses = courses;
            return View(model);
        }
        [Authorize(Roles = "Admin,Coordinator")]
        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateClassDto classDto)
        {
            var model = classDto.ToClassFromCreateDto();
            var newClass = await _classRepo.CreateAsync(model);

            if (newClass != null)
                return RedirectToAction("Index", "Admin");
            return BadRequest("Erro ao adicionar Turma");
        }


        [Authorize(Roles = "Admin,Coordinator")]
        [Route("editclassdirector/{id}")]
        public async Task<IActionResult> EditClassDirector(Guid id)
        {
            var turma = await _classRepo.GetByIdAsync(id);
            if (turma == null) return NotFound("Turma não existe");

            ViewData["class"] = turma.Name;
            ViewData["turmaId"] = turma.Id;

            var users = await _accountRepo.GetAllAsync();
            return View(users);
        }

        [Route("setcclassdirector/{id}")]
        [HttpPost]
        public async Task<IActionResult> SetClassDirector(Guid id, [FromForm] Guid classDirectorId)
        {
            var result = await _classRepo.SetDirectorAsync(id, classDirectorId);
            if (result == null) return NotFound();
            return RedirectToAction("Index", "Home");
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}