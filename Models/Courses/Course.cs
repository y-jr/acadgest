using acadgest.Models.Classes;
using acadgest.Models.Coordenations;

namespace acadgest.Models.Courses
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        // Navigations props
        public Guid? CoordenationId { get; set; }
        public Coordenation? Coordenation { get; set; } = new Coordenation();
        public ICollection<Class>? Classes { get; set; }
    }
}