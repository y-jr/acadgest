using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Pupil
{
    public class MarkDto
    {
        public Guid SubjectId { get; set; }
        public float Value { get; set; }
        public int Trimester { get; set; }
        public int year { get; set; }
    }
}