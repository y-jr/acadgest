using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Pupil
{
    public class MiniPautaForView
    {
        public Guid PupilId { get; set; }
        public string PupilName { get; set; } = string.Empty;
        public string PupilGender { get; set; } = string.Empty;
        public float Mac { get; set; }
        public float Pp { get; set; }
        public float Pt { get; set; }
        public float Mt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}