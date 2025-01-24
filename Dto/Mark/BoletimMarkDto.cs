using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Mark
{
    public class BoletimMarkDto
    {
        public string Subject { get; set; } = string.Empty;
        public float Mac { get; set; }
        public float Pp { get; set; }
        public float Pt { get; set; }
        public float Mt { get; set; }
    }
}