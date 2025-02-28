using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Models.User
{
    public class EditUserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IdNumber { get; set; } = string.Empty;
        public string UserName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
    }
}