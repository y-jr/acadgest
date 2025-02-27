using acadgest.Dto.Class;
using acadgest.Dto.Mark;
using acadgest.Dto.Pupil;
using acadgest.Interface;
using acadgest.Models.Pupils;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data.Repository
{
    public class MarkRepository : IMarkRepository
    {
        private readonly ApplicationDbContext _context;
        public MarkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> AddAsync(CreateMiniPautaDto miniPautaDto)
        {
            var subjectId = miniPautaDto.SubjectId;
            var mensagens = new List<string>();
            if (string.IsNullOrWhiteSpace(miniPautaDto.MiniPauta))
                mensagens.Add("Alunos vazios");
            var Lines = miniPautaDto?.MiniPauta ?? "";
            var lines = Lines.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Inicia uma transação para garantir consistência dos dados
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var line in lines)
                {
                    var parts = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 4)
                    {
                        // mensagens.Add("Não coaduna");
                        continue; // Pula linhas que não têm exatamente quatro elementos
                    }
                    // else
                    // {
                    //     mensagens.Add("coaduna");
                    // }

                    string name = parts[0].Trim();
                    float mac = float.Parse(parts[1].Trim());
                    float pp = float.Parse(parts[2].Trim());
                    float pt = float.Parse(parts[3].Trim());

                    var subject = await _context.Subjects.FindAsync(subjectId);
                    if (subject != null)
                    {
                        var aluno = await _context.Pupils.FirstOrDefaultAsync(a => a.Name == name && a.ClassId == subject.ClassId);

                        if (aluno == null)
                        {
                            mensagens.Add($"As notas de {name} não foram carregadas");
                        }
                        else
                        {
                            if (miniPautaDto != null)
                            {
                                await UpsertMarkAsync(aluno.Id, miniPautaDto.SubjectId, mac, 1, 2025, "mac");
                                await UpsertMarkAsync(aluno.Id, miniPautaDto.SubjectId, pp, 1, 2025, "pp");
                                await UpsertMarkAsync(aluno.Id, miniPautaDto.SubjectId, pt, 1, 2025, "pt");
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Confirma a transação

                return mensagens;
            }
            catch
            {
                await transaction.RollbackAsync();
                mensagens.Add("Falha ao adicionar as notas dos alunos");
                return mensagens;
            }
        }

        private async Task UpsertMarkAsync(Guid pupilId, Guid subjectId, float value, int trimester, int year, string test)
        {
            // Verifica se já existe uma nota com as mesmas propriedades
            var existingMark = await _context.Marks
                .FirstOrDefaultAsync(m => m.PupilId == pupilId && m.SubjectId == subjectId &&
                                          m.Trimester == trimester && m.year == year && m.test == test);

            if (existingMark != null)
            {
                // Atualiza os valores da nota existente
                existingMark.Value = value;
                _context.Marks.Update(existingMark);
            }
            else
            {
                // Adiciona uma nova nota
                var newMark = new Mark
                {
                    PupilId = pupilId,
                    SubjectId = subjectId,
                    Value = value,
                    Trimester = trimester,
                    year = year,
                    test = test
                };
                await _context.Marks.AddAsync(newMark);
            }
        }


        public async Task<ClassMiniPauta?> GetMiniAsync(Guid subjectId, int trim)
        {
            var classMiniPauta = new ClassMiniPauta();
            classMiniPauta.Trimester = trim;

            var subject = await _context.Subjects.FindAsync(subjectId);

            classMiniPauta.Id = subjectId;
            classMiniPauta.Name = subject?.Name ?? "";
            if (subject == null) return null;
            var alunos = await _context.Pupils.Where(p => p.ClassId == subject.ClassId).ToListAsync();

            foreach (var aluno in alunos)
            {
                var MAC = await _context.Marks.FirstOrDefaultAsync(m => m.PupilId == aluno.Id && m.test == "mac" && m.SubjectId == subjectId && m.Trimester == trim);
                var PP = await _context.Marks.FirstOrDefaultAsync(m => m.PupilId == aluno.Id && m.test == "pp" && m.SubjectId == subjectId && m.Trimester == trim);
                var PT = await _context.Marks.FirstOrDefaultAsync(m => m.PupilId == aluno.Id && m.test == "pt" && m.SubjectId == subjectId && m.Trimester == trim);
                var miniPauta = new MiniPautaForView
                {
                    PupilId = aluno.Id,
                    PupilName = aluno.Name,
                    PupilGender = aluno?.Gender ?? "M",
                    Mac = MAC?.Value ?? 0,
                    Pp = PP?.Value ?? 0,
                    Pt = PT?.Value ?? 0,
                };
                miniPauta.Mt = ((miniPauta.Mac + miniPauta.Pp + miniPauta.Pt) / 3);
                miniPauta.Status = (miniPauta.Mt < 10) ? "Reprovado" : "Aprovado";

                classMiniPauta.MiniPautas.Add(miniPauta);
            }

            return classMiniPauta;
        }

        public async Task<BoletimDto?> BoletimAsync(Guid pupilId)
        {
            var boletim = new BoletimDto();

            var pupil = await _context.Pupils.FindAsync(pupilId);
            if (pupil == null) return null;
            boletim.PupilName = pupil.Name; // Nome do aluno

            var turma = await _context.Classes
                .Include(t => t.Subjects) // Inclui os Subjects relacionados
                .FirstOrDefaultAsync(t => t.Id == pupil.ClassId); // Filtra pelo ClassId
            if (turma == null) return null;

            var diretor = await _context.Users.FindAsync(turma.ClassDirectorId);
            if (diretor == null) return null;
            boletim.ClassDirectorName = diretor.Name; // Nome do diretor de turma

            var curso = await _context.Courses.FindAsync(turma.CourseId);
            if (curso == null) return null;

            boletim.ClassName = $"{turma.Name} - {curso.Name}"; // Nome da turma
            if (turma.Subjects == null) return null;
            foreach (var subject in turma.Subjects)
            {
                var mac = await _context.Marks.FirstOrDefaultAsync(m =>
                m.PupilId == pupil.Id &&
                m.year == 2025 &&
                m.Trimester == 1 &&
                m.SubjectId == subject.Id &&
                m.test == "mac"
                );
                float myMac = mac?.Value ?? 0;
                var pp = await _context.Marks.FirstOrDefaultAsync(m =>
                m.PupilId == pupil.Id &&
                m.year == 2025 &&
                m.Trimester == 1 &&
                m.SubjectId == subject.Id &&
                m.test == "pp"
                );
                float myPp = pp?.Value ?? 0;
                var pt = await _context.Marks.FirstOrDefaultAsync(m =>
                m.PupilId == pupil.Id &&
                m.year == 2025 &&
                m.Trimester == 1 &&
                m.SubjectId == subject.Id &&
                m.test == "pt"
                );
                float myPt = pt?.Value ?? 0;

                var newMark = new BoletimMarkDto
                {
                    Subject = subject.Name,
                    Mac = myMac,
                    Pp = myPp,
                    Pt = myPt,
                    Mt = ((myMac + myPp + myPt) / 3)
                };

                boletim.Marks.Add(newMark);
            }
            return boletim;
        }

        public async Task<ClassBoletinsDto?> BoletinsAsync(Guid classId)
        {
            var classBoletins = new ClassBoletinsDto();
            var turma = await _context.Classes
                .Include(t => t.ClassDirector)
                .Include(t => t.Course)
                .Include(t => t.Subjects) // Inclui os Subjects relacionados
                .FirstOrDefaultAsync(t => t.Id == classId); // Filtra pelo ClassId
            if (turma == null) return null;
            if (turma.Subjects == null) return null;

            classBoletins.ClassDirectorName = turma.ClassDirector?.Name ?? "";
            var marks = await _context.Marks.Where(m => m.year == 2025 && m.Trimester == 1).ToListAsync();

            var pupils = await _context.Pupils.Where(p => p.ClassId == classId).ToListAsync();
            if (pupils == null) return null;

            foreach (var pupil in pupils)
            {
                var newBoletim = new BoletimDto();
                newBoletim.PupilName = pupil.Name; // Nome do aluno
                newBoletim.ClassDirectorName = turma.ClassDirector?.Name ?? "Adelson Matari"; // Nome do diretor de turma
                newBoletim.ClassName = $"{turma.Name} - {turma.Course?.Name}"; // Nome da turma
                foreach (var subject in turma.Subjects)
                {
                    var mac = marks.FirstOrDefault(m =>
                    m.PupilId == pupil.Id &&
                    m.SubjectId == subject.Id &&
                    m.test == "mac"
                    );
                    float myMac = mac?.Value ?? 0;
                    var pp = marks.FirstOrDefault(m =>
                    m.PupilId == pupil.Id &&
                    m.SubjectId == subject.Id &&
                    m.test == "pp"
                    );
                    float myPp = pp?.Value ?? 0;
                    var pt = marks.FirstOrDefault(m =>
                    m.PupilId == pupil.Id &&
                    m.SubjectId == subject.Id &&
                    m.test == "pt"
                    );
                    float myPt = pt?.Value ?? 0;

                    var newMark = new BoletimMarkDto
                    {
                        Subject = subject.Name,
                        Mac = myMac,
                        Pp = myPp,
                        Pt = myPt,
                        Mt = ((myMac + myPp + myPt) / 3)
                    };

                    newBoletim.Marks.Add(newMark);
                }
                classBoletins.Boletins.Add(newBoletim);
            }


            return classBoletins;
        }

        public async Task<bool> UpdateAsync(UpdateMiniPautaDto miniPautaDto)
        {
            var mac = await _context.Marks.FirstOrDefaultAsync(m =>
                                m.SubjectId == miniPautaDto.Id &&
                                m.Trimester == miniPautaDto.Trim &&
                                m.year == 2025 &&
                                m.test == "mac" &&
                                m.PupilId == miniPautaDto.PupilId
                                );

            var pp = await _context.Marks.FirstOrDefaultAsync(m =>
                                m.SubjectId == miniPautaDto.Id &&
                                m.Trimester == miniPautaDto.Trim &&
                                m.year == 2025 &&
                                m.test == "pp" &&
                                m.PupilId == miniPautaDto.PupilId
                                );

            var pt = await _context.Marks.FirstOrDefaultAsync(m =>
                                m.SubjectId == miniPautaDto.Id &&
                                m.Trimester == miniPautaDto.Trim &&
                                m.year == 2025 &&
                                m.test == "pt" &&
                                m.PupilId == miniPautaDto.PupilId
                                );
            if (mac == null)
            {
                await _context.Marks.AddAsync(new Mark
                {
                    SubjectId = miniPautaDto.Id,
                    Trimester = miniPautaDto.Trim,
                    year = 2025,  // Certifique-se de que a propriedade tem a primeira letra maiúscula
                    test = "mac", // Certifique-se de que a propriedade existe na entidade
                    PupilId = miniPautaDto.PupilId,
                    Value = float.Parse(miniPautaDto.Mac.Replace(",", "."))
                });
            }
            else
            {
                mac.Value = float.Parse(miniPautaDto.Mac.Replace(",", "."));
            }
            if (pp == null)
            {
                await _context.Marks.AddAsync(new Mark
                {
                    SubjectId = miniPautaDto.Id,
                    Trimester = miniPautaDto.Trim,
                    year = 2025,  // Certifique-se de que a propriedade tem a primeira letra maiúscula
                    test = "pp", // Certifique-se de que a propriedade existe na entidade
                    PupilId = miniPautaDto.PupilId,
                    Value = float.Parse(miniPautaDto.Pp.Replace(",", "."))
                });
            }
            else
            {
                pp.Value = float.Parse(miniPautaDto.Pp.Replace(",", "."));
            }
            if (pt == null)
            {
                await _context.Marks.AddAsync(new Mark
                {
                    SubjectId = miniPautaDto.Id,
                    Trimester = miniPautaDto.Trim,
                    year = 2025,  // Certifique-se de que a propriedade tem a primeira letra maiúscula
                    test = "pt", // Certifique-se de que a propriedade existe na entidade
                    PupilId = miniPautaDto.PupilId,
                    Value = float.Parse(miniPautaDto.Pt.Replace(",", "."))
                });
            }
            else
            {
                pt.Value = float.Parse(miniPautaDto.Pt.Replace(",", "."));
            }
            await _context.SaveChangesAsync();
            return true;

        }
    }
}