using acadgest.Models.Classes;
using acadgest.Models.Courses;

namespace acadgest.Models.Coordenations
{
    public class Coordenation
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigatios Props
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Class>? Classes { get; set; }
    }
}