using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acadgest.Dto.AppUser
{
    public class AppUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IdNumber { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

    }
}