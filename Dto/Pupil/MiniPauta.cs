using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Models.Pupils;

namespace acadgest.Dto.Pupil
{
    public class MiniPauta
    {
        public Guid SubjectId { get; set; }
        public float Mac { get; set; }
        public float Pp { get; set; }
        public float Pt { get; set; }
        public float Mt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}