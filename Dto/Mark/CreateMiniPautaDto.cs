using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Mark
{
    public class CreateMiniPautaDto
    {
        public Guid SubjectId { get; set; }
        public string? MiniPauta { get; set; }
    }
}