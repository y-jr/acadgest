using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Class
{
    public class CreateClassDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string? Classroom { get; set; } = string.Empty;
        public string? Obs { get; set; } = string.Empty;
        // Navigation props
        public Guid? ClassDirectorId { get; set; }
        public Guid? CoordenationId { get; set; }
        public Guid? CourseId { get; set; }
    }
}