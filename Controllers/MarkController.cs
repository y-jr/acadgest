using System.Threading.Tasks;
using acadgest.Dto.Mark;
using acadgest.Interface;
using Microsoft.AspNetCore.Mvc;

namespace acadgest.Controllers
{
    [Route("[controller]")]
    public class MarkController : Controller
    {
        private readonly ILogger<MarkController> _logger;
        private readonly IMarkRepository _markRepo;

        public MarkController(ILogger<MarkController> logger, IMarkRepository markRepo)
        {
            _logger = logger;
            _markRepo = markRepo;
        }


        [Route("{id}")]
        public async Task<IActionResult> Index([FromRoute] Guid id)
        {
            var model = await _markRepo.GetMiniAsync(id);
            return View(model);
        }

        [Route("mini/{id}")]
        public IActionResult AddMiniPauta([FromRoute] Guid id)
        {
            var model = new CreateMiniPautaDto
            {
                SubjectId = id,
            };
            return View(model);
        }
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] CreateMiniPautaDto dto)
        {
            var mensagem = await _markRepo.AddAsync(dto);
            // return Ok(mensagem);
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