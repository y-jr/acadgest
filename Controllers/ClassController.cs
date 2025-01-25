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
        public async Task<IActionResult> Index([FromRoute] Guid id)
        {
            var model = await _classRepo.ClassDetailsAsync(id);

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [Route("new/{id}")]
        public async Task<IActionResult> NewClass([FromRoute] Guid id)
        {
            var model = new ClassModelView();
            model.NewClass.CoordenationId = id;
            var users = await _accountRepo.GeAllAsync();
            if (users != null)
            {
                var usersDto = users.Select(u => u.ToUserDto()).ToList();
                model.Users = usersDto;
            }
            var courses = await _courseRepo.GetAllAsync();
            model.Courses = courses;
            return View(model);
        }
        [Authorize(Roles = "Admin")]
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
        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}