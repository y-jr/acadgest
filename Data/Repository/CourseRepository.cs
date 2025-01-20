using acadgest.Dto.Course;
using acadgest.Interface;
using acadgest.Mappers;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CourseDto>> GetAllAsync()
        {
            var courses = await _context.Courses.ToListAsync();
            var coursesDto = courses.Select(c => c.ToCourseDto()).ToList();
            return coursesDto;
        }

        public Task<List<CourseDto>> GetByCoordenationAsync(Guid cordenationId)
        {
            throw new NotImplementedException();
        }
    }
}