using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Models.Subjects;

namespace acadgest.Interface
{
    public interface ISubjectRepository
    {
        public Task<Subject> CreateAsync(Subject model);
    }
}