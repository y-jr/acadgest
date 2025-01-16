using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Models.Classes;
using acadgest.Models.Coordenations;
using Microsoft.AspNetCore.Identity;

namespace acadgest.Models.User
{
    public class AppUser : IdentityUser<Guid>
    {
        public ICollection<Coordenation>? Coordenations { get; set; }
        public ICollection<Class>? Classes { get; set; }
    }
}