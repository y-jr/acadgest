using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Coordenation
{
    public class CoordenationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CoordinatorName { get; set; } = string.Empty;
    }
}