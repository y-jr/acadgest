using acadgest.Dto.Mark;
using acadgest.Dto.Subject;

namespace acadgest.Dto.Class
{
    public class ClassDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string? Course { get; set; } = string.Empty;
        public string? ClassDirector { get; set; } = string.Empty;
        public ICollection<SubjectDto> Subjects { get; set; } = new List<SubjectDto>();
        public List<MarksForCoordenationViewDto> Pautas { get; set; } = new();
    }
}