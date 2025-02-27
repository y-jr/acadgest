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
        public Task<List<ClassDto>?> GetByClassDirector(Guid directorId);
        public Task<Class?> GetByIdAsync(Guid id);
        public Task<ClassDetailsDto?> ClassDetailsAsync(Guid id, int trimestre);
        public Task<List<ClassDto>?> GetByCordAsync(Guid cordId);
        // public Task<List<ClassDto>?> GetByDirAsync(Guid dirId);
        public Task<Class?> CreateAsync(Class classModel);
        public Task<Class?> UpdateAsync(Guid id, UpdateClassDto classDto);
        public Task<Class?> SetDirectorAsync(Guid id, Guid directorId);
        public Task<bool> ExistsAsync(Guid id);
    }
}