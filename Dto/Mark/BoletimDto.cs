using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Mark
{
    public class BoletimDto
    {
        public string PupilName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string ClassDirectorName { get; set; } = string.Empty;
        public List<BoletimMarkDto> Marks { get; set; } = new();
    }
}