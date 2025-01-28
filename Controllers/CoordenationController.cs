using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using acadgest.Dto.Class;
using acadgest.Dto.Coordenation;
using acadgest.Interface;
using acadgest.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace acadgest.Controllers
{

    [Authorize(Roles = "Admin,Coordinator")]
    [Route("coordenacao")]
    public class CoordenationController : Controller
    {
        private readonly ICoordenationRepository _coordenationRepo;
        private readonly IClassRepository _classRepo;

        public CoordenationController(ICoordenationRepository coordenationRepo, IClassRepository classRepo)
        {
            _coordenationRepo = coordenationRepo;
            _classRepo = classRepo;
        }

        [Route("{id}")]
        public async Task<IActionResult> Index([FromRoute] Guid id)
        {
            ViewData["coord"] = id;
            var model = await _classRepo.GetByCordAsync(id);
            return View(model);
        }

        [Route("all")]
        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var coordinations = await _coordenationRepo.GetAllAsync();
            return Ok(coordinations);
        }

        [Authorize(Roles = "Admin")]
        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Id inválido");

            var coordenation = await _coordenationRepo.GetByIdAsync(id);
            ViewData["id"] = id;
            if (coordenation == null) return NotFound("Coordenação não encontrada");
            var coordenationDto = coordenation.ToUpdateDto();

            return View(coordenationDto);
        }

        [Authorize(Roles = "Admin")]
        [Route("update/{id}")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCoordenationDto dto)
        {
            var coordenation = await _coordenationRepo.UpdateAsync(id, dto);
            if (coordenation == null) return NotFound("Coordenação não encontrada");
            return RedirectToAction("Index", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCoordenationDto coordenationDto)
        {
            var coordenationModel = coordenationDto.ToCoordenationFromCreateDto();
            var coordenation = await _coordenationRepo.CreateAsync(coordenationModel);
            if (coordenation == null) return BadRequest();
            return RedirectToAction("Index", "Admin");
        }



        [Authorize(Roles = "Admin")]
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _coordenationRepo.DeleteAsync(id);

            if (result.DeleteSucceded) return RedirectToAction("Index", "Admin");
            else return NotFound(result.Error);
        }


        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}