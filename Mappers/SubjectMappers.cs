using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Dto.Subject;
using acadgest.Models.Subjects;

namespace acadgest.Mappers
{
    public static class SubjectMappers
    {
        public static SubjectDto ToSubjectDto(this Subject model)
        {
            return new SubjectDto
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}