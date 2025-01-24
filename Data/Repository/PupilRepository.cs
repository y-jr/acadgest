using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Pupil;
using acadgest.Interface;
using acadgest.Models.Pupils;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data.Repository
{
    public class PupilRepository : IPupilRepository
    {
        private readonly ApplicationDbContext _context;

        public PupilRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(AddPupilDto pupilDto)
        {
            if (string.IsNullOrWhiteSpace(pupilDto.Alunos))
                return false; // Retorna imediatamente se a string de alunos estiver vazia.

            var lines = pupilDto.Alunos.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Inicia uma transação para garantir consistência dos dados
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var line in lines)
                {
                    var parts = line.Split('\t', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                        continue; // Pula linhas que não têm exatamente dois elementos (nome e gênero).

                    string name = parts[0].Trim();
                    string gender = parts[1].Trim().ToUpper();

                    // Validações básicas de entrada
                    if (string.IsNullOrWhiteSpace(name) || !(gender == "M" || gender == "F"))
                        continue;

                    var pupil = new Pupil
                    {
                        ClassId = pupilDto.TurmaId,
                        Name = name,
                        Gender = gender
                    };

                    await _context.Pupils.AddAsync(pupil);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync(); // Confirma a transação

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
