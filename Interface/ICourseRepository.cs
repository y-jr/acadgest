using acadgest.Dto.Course;

namespace acadgest.Interface
{
    public interface ICourseRepository
    {
        public Task<List<CourseDto>> GetAllAsync();
        public Task<List<CourseDto>> GetByCoordenationAsync(Guid cordenationId);
    }
}