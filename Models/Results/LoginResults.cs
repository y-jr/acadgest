using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Models.User;

namespace acadgest.Models.Results
{
    public class LoginResults
    {
        public AppUser? User { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsNotAllowed { get; set; }
        public bool RequiresTwoFactor { get; set; }
    }
}