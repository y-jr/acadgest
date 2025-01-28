using acadgest.Dto.Class;
using acadgest.Interface;
using acadgest.Mappers;
using acadgest.Models.Classes;
using acadgest.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ClassRepository> _logger;
        public ClassRepository(ApplicationDbContext context, UserManager<AppUser> userManager, ILogger<ClassRepository> logger)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<ClassDetailsDto?> ClassDetailsAsync(Guid id)
        {
            var turma = await _context.Classes
            .Include(c => c.ClassDirector)
                .Include(c => c.Course)
                .Include(c => c.Subjects)
                .Include(c => c.pupils).FirstOrDefaultAsync(t => t.Id == id);

            if (turma == null) return null;

            var classDetail = new ClassDetailsDto
            {
                Id = turma.Id,
                Grade = turma.Grade,
                Course = turma.Course?.Name ?? "",
                Name = turma.Name,
                ClassDirector = turma.ClassDirector?.Name ?? ""
            };
            if (turma.Subjects != null)
                classDetail.Subjects = turma.Subjects.Select(s => s.ToSubjectDto()).ToList();
            var pupils = await _context.Pupils.Where(p => p.ClassId == turma.Id).ToListAsync();
            var marks = await _context.Marks.Where(m => m.year == 2025).ToListAsync();

            foreach (var pupil in pupils)
            {
                var pauta = new Dto.Mark.MarksForCoordenationViewDto
                {
                    PupilId = pupil.Id,
                    PupilName = pupil.Name,
                    PupilGender = pupil?.Gender ?? "M"
                };
                foreach (var subject in classDetail.Subjects)
                {
                    var mac = marks.FirstOrDefault(m =>
                                    m.PupilId == pupil?.Id &&
                                    m.SubjectId == subject.Id &&
                                    m.Trimester == 1 &&
                                    m.year == 2025 &&
                                    m.test == "mac");
                    float myMac = mac?.Value ?? 0;
                    var pp = marks
                                .FirstOrDefault(m =>
                                m.PupilId == pupil?.Id &&
                                m.SubjectId == subject.Id &&
                                m.Trimester == 1 &&
                                m.year == 2025 &&
                                m.test == "pp");
                    float myPp = pp?.Value ?? 0;
                    var pt = marks
                                .FirstOrDefault(m =>
                                m.PupilId == pupil?.Id &&
                                m.SubjectId == subject.Id &&
                                m.Trimester == 1 &&
                                m.year == 2025 &&
                                m.test == "pt");
                    float myPt = pt?.Value ?? 0;
                    var mt = ((myMac + myPp + myPt) / 3);
                    pauta.Marks.Add(mt);
                }
                classDetail.Pautas.Add(pauta);
            }

            return classDetail;
        }

        public async Task<Class?> CreateAsync(Class classModel)
        {
            await _context.Classes.AddAsync(classModel);
            await _context.SaveChangesAsync();
            return classModel;
        }


        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Class>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<ClassDto>?> GetByCordAsync(Guid cordId)
        {
            var classes = await _context.Classes
                        .Where(c => c.CoordenationId == cordId)
                        .Include(c => c.ClassDirector)
                        .ToListAsync();

            if (classes == null) return null;
            var classesDto = classes.Select(c => c.ToClassDto()).ToList();
            return classesDto;
        }

        public async Task<Class?> GetByIdAsync(Guid id)
        {
            return await _context.Classes.FindAsync(id);
        }

        public Task<Guid> GetIdByCordAsync(Guid cordId)
        {
            throw new NotImplementedException();
        }

        public async Task<Class?> SetDirectorAsync(Guid id, Guid directorId)
        {
            var turma = await _context.Classes.FindAsync(id);
            if (turma == null) return null;
            else
            {
                // Verificar se a turma já tem um diretor
                var oldDirectorId = turma.ClassDirectorId;
                if (oldDirectorId != null)
                {
                    turma.ClassDirectorId = directorId;
                    await _context.SaveChangesAsync();
                    // Pega o antigo coordenador
                    var oldDirector = await _context.Users.FindAsync(oldDirectorId);
                    if (oldDirector != null)
                    {
                        // verifica se ainda tem turmas que ele coordena
                        var classes = await _context.Classes.Where(c => c.ClassDirectorId == oldDirectorId).ToListAsync();
                        if (classes.Count == 0)
                        {
                            // remove a role de coordenador
                            var roles = await _userManager.GetRolesAsync(oldDirector);
                            if (roles.Contains("Classdirector"))
                            {
                                await _userManager.RemoveFromRoleAsync(oldDirector, "Classdirector");
                            }
                        }
                    }
                }

            }
            return turma;
        }

        public Task<Class?> UpdateAsync(Guid id, UpdateClassDto classDto)
        {
            throw new NotImplementedException();
        }
    }
}