using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Interface;
using acadgest.Views.Document;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace acadgest.Controllers
{
    [Route("[controller]")]
    public class DocumentController : Controller
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IMarkRepository _markRepo;

        public DocumentController(ILogger<DocumentController> logger, IMarkRepository markRepo)
        {
            _logger = logger;
            _markRepo = markRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("boletim/{id}")]
        public async Task<IActionResult> Boletim([FromRoute] Guid id)
        {
            var boletim = await _markRepo.BoletimAsync(id);

            if (boletim == null) return NotFound("Aluno não encontrado");
            // return Ok(boletim);
            QuestPDF.Settings.License = LicenseType.Community;
            var newboletim = new Boletim
            {
                BoletimDto = boletim
            };
            var pdf = newboletim.GeneratePdf();
            return File(pdf, "application/pdf", $"Boletim - {boletim.PupilName}.pdf");
        }
    }
}