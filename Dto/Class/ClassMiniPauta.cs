using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Pupil;

namespace acadgest.Dto.Class
{
    public class ClassMiniPauta
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Trimester { get; set; }
        public List<MiniPautaForView> MiniPautas { get; set; } = new();
    }
}