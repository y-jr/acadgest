using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Pupil
{
    public class Pauta
    {
        public Guid PupilId { get; set; }
        public string PupilName { get; set; } = string.Empty;
        public string PupilGender { get; set; } = string.Empty;
        public List<MiniPauta> IT { get; set; } = new();
        public List<MiniPauta> IIT { get; set; } = new();
        public List<MiniPauta> IIIT { get; set; } = new();
    }
}