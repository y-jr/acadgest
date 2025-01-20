using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Class;
using acadgest.Models.Classes;

namespace acadgest.Interface
{
    public interface IClassRepository
    {
        public Task<List<Class>> GetAllAsync();
        public Task<Class?> GetByIdAsync(Guid id);
        public Task<Class?> CreateAsync(Class classModel);
        public Task<Class?> UpdateAsync(Guid id, UpdateClassDto classDto);
        public Task<Class?> SetDirectorAsync(Guid id, Guid directorId);
        public Task<bool> ExistsAsync(Guid id);
    }
}