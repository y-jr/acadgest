using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Mark
{
    public class MarksForCoordenationViewDto
    {

        public Guid PupilId { get; set; }
        public string PupilName { get; set; } = string.Empty;
        public string PupilGender { get; set; } = string.Empty;
        public List<float> Marks { get; set; } = new();
    }
}