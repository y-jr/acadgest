using acadgest.Dto.AppUser;
using acadgest.Dto.Course;
using acadgest.Models.Courses;

namespace acadgest.Dto.Class
{
    public class ClassModelView
    {
        public CreateClassDto NewClass { get; set; } = new();
        public List<AppUserDto> Users { get; set; } = new();
        public List<CourseDto> Courses { get; set; } = new();
    }
}