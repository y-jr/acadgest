using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Class;
using acadgest.Dto.Mark;
using acadgest.Dto.Pupil;

namespace acadgest.Interface
{
    public interface IMarkRepository
    {
        public Task<ClassMiniPauta?> GetMiniAsync(Guid subjectId, int trim);
        public Task<List<string>> AddAsync(CreateMiniPautaDto miniPautaDto);
        public Task<bool> UpdateAsync(UpdateMiniPautaDto miniPautaDto);
        public Task<BoletimDto?> BoletimAsync(Guid pupilId);
        public Task<ClassBoletinsDto?> BoletinsAsync(Guid classId);
    }
}