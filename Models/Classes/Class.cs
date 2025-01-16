using acadgest.Models.Coordenations;
using acadgest.Models.Courses;
using acadgest.Models.Pupils;
using acadgest.Models.User;

namespace acadgest.Models.Classes
{
    public class Class
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Classroom { get; set; } = string.Empty;
        // Navigation props

        public Guid? ClassDirectorId { get; set; }
        public AppUser? ClassDirector { get; set; }
        public Guid? CoordenationId { get; set; }
        public Coordenation? Coordenation { get; set; }
        public Guid? CourseId { get; set; }
        public Course? Course { get; set; }
        public ICollection<Pupil>? pupils { get; set; }


    }
}