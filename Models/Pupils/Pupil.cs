using acadgest.Models.Classes;

namespace acadgest.Models.Pupils
{
    public class Pupil
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        // Navigation props
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!;

    }
}