using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Coordenation;
using acadgest.Interface;
using acadgest.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace acadgest.Controllers
{
    [Route("coordenacao")]
    public class CoordenationController : Controller
    {
        private readonly ICoordenationRepository _coordenationRepo;

        public CoordenationController(ICoordenationRepository coordenationRepo)
        {
            _coordenationRepo = coordenationRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("all")]
        [HttpPost]
        public async Task<IActionResult> GetAll()
        {
            var coordinations = await _coordenationRepo.GetAllAsync();
            return Ok(coordinations);
        }

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

        [Route("update/{id}")]
        [HttpPost]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCoordenationDto dto)
        {
            var coordenation = await _coordenationRepo.UpdateAsync(id, dto);
            if (coordenation == null) return NotFound("Coordenação não encontrada");
            return RedirectToAction("Index", "Admin");
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCoordenationDto coordenationDto)
        {
            var coordenationModel = coordenationDto.ToCoordenationFromCreateDto();
            var coordenation = await _coordenationRepo.CreateAsync(coordenationModel);
            if (coordenation == null) return BadRequest();
            return RedirectToAction("Index", "Admin");
        }



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