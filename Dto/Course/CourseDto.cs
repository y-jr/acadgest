using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.Course
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}