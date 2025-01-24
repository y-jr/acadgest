using acadgest.Models.Classes;

namespace acadgest.Models.Subjects
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Grade { get; set; } = string.Empty;
        // Navigation Porps
        public Guid? ClassId { get; set; }
        public Class? Class { get; set; }
    }
}