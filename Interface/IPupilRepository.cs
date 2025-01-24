using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Pupil;

namespace acadgest.Interface
{
    public interface IPupilRepository
    {
        public Task<bool> AddAsync(AddPupilDto pupilDto);
    }
}