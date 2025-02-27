using acadgest.Dto.Mark;
using acadgest.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace acadgest.Controllers
{
    [Authorize(Roles = "Admin,Coordinator,Classdirector")]
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
        public async Task<IActionResult> Index([FromRoute] Guid id, [FromQuery] int trim)
        {
            var model = await _markRepo.GetMiniAsync(id, trim);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("mini/{id}")]
        public IActionResult AddMiniPauta([FromRoute] Guid id)
        {
            var model = new CreateMiniPautaDto
            {
                SubjectId = id,
            };
            return View(model);
        }

        [Route("update")]
        public async Task<IActionResult> Update([FromForm] UpdateMiniPautaDto dto)
        {
            // Atualiza os dados
            var atualizar = await _markRepo.UpdateAsync(dto);

            // Recupera a URL da página de origem
            string refererUrl = Request.Headers["Referer"].ToString();

            // Redireciona para a página de origem ou retorna um fallback
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }

            // Fallback: retorna uma mensagem de sucesso
            return Ok(new { message = "Dados atualizados com sucesso", data = dto });
        }

        // var atualizar = await _markRepo.UpdateAsync(dto);
        // return RedirectToAction("Index", "Admin");


        [Authorize(Roles = "Admin")]
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