using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using acadgest.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace acadgest.Controllers
{
    [Route("[controller]")]
    public class RedirectController : Controller
    {
        private readonly ICoordenationRepository _coordenationRepo;
        private readonly ILogger<RedirectController> _logger;

        public RedirectController(ILogger<RedirectController> logger, ICoordenationRepository coordenationRepo)
        {
            _logger = logger;
            _coordenationRepo = coordenationRepo;
        }

        [Route("coordenation")]
        public async Task<IActionResult> Coordenation()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return RedirectToAction("Logout", "Account");

            // Obter as roles do usuário
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            if (roles.Contains("Admin")) return RedirectToAction("Index", "Admin");

            if (!roles.Contains("Coordinator")) return Unauthorized("Não tens permissões de coordenador!");

            var cordId = await _coordenationRepo.GetIdByCordAsync(Guid.Parse(userId));

            if (cordId == null) return Unauthorized("Não tens nenhuma coordenação sobre os seus cuidados!");

            return Redirect($"/coordenacao/{cordId}");
        }

    }
}