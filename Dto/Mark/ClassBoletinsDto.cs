using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Mark
{
    public class ClassBoletinsDto
    {
        public string ClassDirectorName { get; set; } = string.Empty;
        public List<BoletimDto> Boletins { get; set; } = new();
    }
}