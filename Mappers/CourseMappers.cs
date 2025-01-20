using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Course;
using acadgest.Models.Courses;

namespace acadgest.Mappers
{
    public static class CourseMappers
    {
        public static CourseDto ToCourseDto(this Course model)
        {
            return new CourseDto
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}