using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Coordenation;
using acadgest.Models.Coordenations;

namespace acadgest.Mappers
{
    public static class CooredenationMapper
    {
        public static Coordenation ToCoordenationFromCreateDto(this CreateCoordenationDto coordenationDto)
        {
            return new Coordenation
            {
                Name = coordenationDto.Name
            };
        }
        public static CoordenationDto ToCoordinationDto(this Coordenation model)
        {
            var coordenador = model.Coordinator;
            return new CoordenationDto
            {
                Id = model.Id,
                Name = model.Name,
                CoordinatorName = model.Coordinator?.Name ?? "Sem Coordenador"

            };
        }
        public static UpdateCoordenationDto ToUpdateDto(this Coordenation model)
        {
            return new UpdateCoordenationDto
            {
                Name = model.Name,
            };
        }
    }
}