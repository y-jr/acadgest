using acadgest.Interface;
using acadgest.Models.Subjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace acadgest.Controllers
{
    [Authorize(Roles = "Admin,Coordinator,Classdirector")]
    [Route("[controller]")]
    public class SubjectController : Controller
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly ISubjectRepository _subjectRepo;

        public SubjectController(ILogger<SubjectController> logger, ISubjectRepository subjectRepo)
        {
            _logger = logger;
            _subjectRepo = subjectRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [Route("add")]
        public IActionResult Add(Guid id)
        {
            var model = new Subject
            {
                ClassId = id,
            };
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Subject model)
        {
            // return Ok(model);
            var newSubject = await _subjectRepo.CreateAsync(model);
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