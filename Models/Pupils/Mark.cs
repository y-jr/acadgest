using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Models.Subjects;

namespace acadgest.Models.Pupils
{
    public class Mark
    {
        public Guid Id { get; set; }
        public float Value { get; set; }
        public int Trimester { get; set; }
        public int year { get; set; }
        public string test { get; set; } = string.Empty;

        // Navigations props
        public Guid? PupilId { get; set; }
        public Pupil? Pupil { get; set; }
        public Guid? SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}