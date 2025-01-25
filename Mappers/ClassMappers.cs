using acadgest.Dto.Class;
using acadgest.Models.Classes;

namespace acadgest.Mappers
{
    public static class ClassMappers
    {
        public static Class ToClassFromCreateDto(this CreateClassDto classDto)
        {
            return new Class
            {
                Name = classDto.Name,
                Grade = classDto.Grade,
                Classroom = classDto.Classroom,
                Obs = classDto.Obs,
                ClassDirectorId = classDto.ClassDirectorId,
                CoordenationId = classDto.CoordenationId,
                CourseId = classDto.CourseId,
            };
        }
        public static ClassDto ToClassDto(this Class model)
        {
            return new ClassDto
            {
                Id = model.Id,
                Name = model.Name,
                ClassDirectorName = model.ClassDirector?.Name ?? "Sem diretor de turma"
            };
        }
    }
}