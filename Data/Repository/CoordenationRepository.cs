using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Class;
using acadgest.Dto.Coordenation;
using acadgest.Dto.Pupil;
using acadgest.Interface;
using acadgest.Mappers;
using acadgest.Models.Coordenations;
using acadgest.Models.Results;
using acadgest.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data.Repository
{
    public class CoordenationRepository : ICoordenationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public CoordenationRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<ClassDetailsDto>?> ClassDetails(Guid cordId)
        {
            var turmas = new List<ClassDetailsDto>();
            var classes = await _context.Classes
                .Where(c => c.CoordenationId == cordId)
                .Include(c => c.ClassDirector)
                .Include(c => c.Course)
                .Include(c => c.Subjects)
                .Include(c => c.pupils).ToListAsync();

            foreach (var turma in classes)
            {
                var newTurma = new ClassDetailsDto
                {
                    Id = turma.Id,
                    Grade = turma.Grade,
                    Course = turma.Course?.Name ?? "",
                    Name = turma.Name,
                    ClassDirector = turma.ClassDirector?.Name ?? ""
                };
                if (turma.Subjects == null) return null;
                newTurma.Subjects = turma.Subjects.Select(s => s.ToSubjectDto()).ToList();
                var pupils = await _context.Pupils.Where(p => p.ClassId == turma.Id).ToListAsync();
                foreach (var pupil in pupils)
                {
                    var pauta = new Dto.Mark.MarksForCoordenationViewDto
                    {
                        PupilId = pupil.Id,
                        PupilName = pupil.Name,
                        PupilGender = pupil?.Gender ?? "M"
                    };
                    foreach (var subject in turma.Subjects)
                    {
                        var mac = await _context.Marks
                                    .FirstOrDefaultAsync(m =>
                                    m.PupilId == pupil.Id &&
                                    m.SubjectId == subject.Id &&
                                    m.Trimester == 1 &&
                                    m.year == 2025 &&
                                    m.test == "mac");
                        float myMac = mac?.Value ?? 0;
                        var pp = await _context.Marks
                                    .FirstOrDefaultAsync(m =>
                                    m.PupilId == pupil.Id &&
                                    m.SubjectId == subject.Id &&
                                    m.Trimester == 1 &&
                                    m.year == 2025 &&
                                    m.test == "pp");
                        float myPp = pp?.Value ?? 0;
                        var pt = await _context.Marks
                                    .FirstOrDefaultAsync(m =>
                                    m.PupilId == pupil.Id &&
                                    m.SubjectId == subject.Id &&
                                    m.Trimester == 1 &&
                                    m.year == 2025 &&
                                    m.test == "pt");
                        float myPt = pt?.Value ?? 0;
                        var mt = ((myMac + myPp + myPt) / 3);
                        pauta.Marks.Add(mt);
                    }
                    newTurma.Pautas.Add(pauta);
                }
                turmas.Add(newTurma);
            }

            return turmas;
        }

        public async Task<Coordenation?> CreateAsync(Coordenation coordenationModel)
        {

            await _context.Coordenations.AddAsync(coordenationModel);
            await _context.SaveChangesAsync();
            return coordenationModel;
        }

        public async Task<DeleteResults> DeleteAsync(Guid id)
        {
            var result = new DeleteResults
            {
                DeleteSucceded = false
            };
            var coordenation = await _context.Coordenations.FirstOrDefaultAsync(c => c.Id == id);
            if (coordenation == null) result.Error = "Coordenação não encontrada";
            else
            {
                _context.Coordenations.Remove(coordenation);
                await _context.SaveChangesAsync();
                result.DeleteSucceded = true;
            }

            return result;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Coordenation>> GetAllAsync()
        {
            return await _context.Coordenations
                .Include(c => c.Coordinator)
                .ToListAsync();
        }

        public async Task<Coordenation?> GetByIdAsync(Guid id)
        {
            var coordenation = await _context.Coordenations
                .Include(c => c.Coordinator).FirstOrDefaultAsync(c => c.Id == id);
            return coordenation;
        }

        public async Task<SetCoordinatorResult> SetCoordinator(Guid coordenationId, Guid coordinatorId)
        {
            var result = new SetCoordinatorResult
            {
                Succeeded = false
            };
            var model = await _context.Coordenations.FindAsync(coordenationId);
            if (model == null) result.Error = "Coordenação não encontrada";
            else
            {
                // Verifica se já tem um coordenador
                var oldCoordinatorId = model.CoordinatorId;
                if (oldCoordinatorId != Guid.Empty)
                {
                    // Pega o antigo coordenador
                    var oldCoordinator = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == oldCoordinatorId);
                    if (oldCoordinator != null)
                    {
                        var roles = await _userManager.GetRolesAsync(oldCoordinator);
                        if (roles.Contains("Coordinator"))
                        {
                            await _userManager.RemoveFromRoleAsync(oldCoordinator, "Coordinator");
                        }
                    }
                }

                // Adicionar a role "Coordinator" ao novo
                var newCoordinator = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == coordinatorId);
                if (newCoordinator != null)
                {
                    await _userManager.AddToRoleAsync(newCoordinator, "Coordinator");
                    model.CoordinatorId = coordinatorId;
                    result.Succeeded = true;
                }
                else result.Error = "Usuário não encontrado";
            }

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Coordenation?> UpdateAsync(Guid id, UpdateCoordenationDto dto)
        {
            var coordenation = await _context.Coordenations.FindAsync(id);
            if (coordenation == null) return null;
            coordenation.Name = dto.Name;
            await _context.SaveChangesAsync();
            return coordenation;
        }
    }
}