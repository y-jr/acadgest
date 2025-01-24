using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Pupil
{
    public class AddPupilDto
    {
        public Guid TurmaId { get; set; }
        public string Alunos { get; set; } = string.Empty;
    }
}