using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Interface;
using acadgest.Models.Subjects;

namespace acadgest.Data.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;
        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Subject> CreateAsync(Subject model)
        {
            var newSubject = new Subject
            {
                Name = model.Name,
                ClassId = model.ClassId,
                Grade = model.Grade
            };
            await _context.Subjects.AddAsync(newSubject);
            await _context.SaveChangesAsync();
            return model;
        }
    }
}