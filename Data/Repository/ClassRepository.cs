using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Class;
using acadgest.Interface;
using acadgest.Models.Classes;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClassRepository> _logger;
        public ClassRepository(ApplicationDbContext context, ILogger<ClassRepository> logger)
        {
            _context = context;
            _logger = logger;
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

        public Task<Class?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Class?> SetDirectorAsync(Guid id, Guid directorId)
        {
            throw new NotImplementedException();
        }

        public Task<Class?> UpdateAsync(Guid id, UpdateClassDto classDto)
        {
            throw new NotImplementedException();
        }
    }
}