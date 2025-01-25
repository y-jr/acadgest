using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Pupil;
using acadgest.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace acadgest.Controllers
{
    [Authorize(Roles = "Admin,Coordinator,Classdirector")]
    [Route("[controller]")]
    public class PupilController : Controller
    {
        private readonly ILogger<PupilController> _logger;
        private readonly IPupilRepository _pupilRepo;

        public PupilController(ILogger<PupilController> logger, IPupilRepository pupilRepo)
        {
            _logger = logger;
            _pupilRepo = pupilRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Route("add/{id}")]
        public IActionResult Add(Guid id)
        {
            ViewData["TurmaId"] = id;
            var model = new AddPupilDto
            {
                TurmaId = id,
                Alunos = ""
            };
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AddPupilDto pupilDto)
        {
            var success = await _pupilRepo.AddAsync(pupilDto);

            if (success)
            {
                // Obtém o cabeçalho 'Referer' para redirecionar o usuário de volta à página de origem
                var refererUrl = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(refererUrl))
                {
                    return Redirect(refererUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home"); // Página padrão caso o referer não esteja presente
                }
            }

            // Caso haja erro, retorna à mesma página com uma mensagem de erro
            ModelState.AddModelError("", "Não foi possível adicionar os alunos.");
            return View(pupilDto); // Certifique-se de ter a View correspondente configurada
        }


        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}