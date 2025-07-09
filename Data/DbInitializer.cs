using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acadgest.Models.Courses;
using acadgest.Models.Coordenations;
using acadgest.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<AppUser> userManager
        )
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
                return;

            // 1. Primeiro criar as coordenações
            var coordenations = new Coordenation[]
            {
                new Coordenation { Name = "INFORGEST" },
                new Coordenation { Name = "CONGEST" },
                new Coordenation { Name = "C. CIVIL" },
                new Coordenation { Name = "PUNIV" },
            };

            foreach (var coord in coordenations)
            {
                await context.Coordenations.AddAsync(coord);
            }
            await context.SaveChangesAsync();
            // 2. Agora criar os cursos associando às coordenações
            var coordenationInf = await context.Coordenations.FirstAsync(c =>
                c.Name == "INFORGEST"
            );
            var coordenationCong = await context.Coordenations.FirstAsync(c => c.Name == "CONGEST");
            var coordenationCiv = await context.Coordenations.FirstAsync(c => c.Name == "C. CIVIL");
            var coordenationPuniv = await context.Coordenations.FirstAsync(c => c.Name == "PUNIV");

            var courses = new Course[]
            {
                new Course { Name = "INFORGEST", Coordenation = coordenationInf },
                new Course { Name = "CONGEST", Coordenation = coordenationCong },
                new Course { Name = "C. CIVIL", Coordenation = coordenationCiv },
                new Course { Name = "C.F.B", Coordenation = coordenationPuniv },
                new Course { Name = "C.E.J", Coordenation = coordenationPuniv },
            };

            foreach (var c in courses)
            {
                await context.Courses.AddAsync(c);
            }
            await context.SaveChangesAsync();

            var admin = new AppUser
            {
                Name = "Adilson Yango Muieba",
                UserName = "yango",
                IdNumber = "009905238LA047",
                Email = "muiebaadilson@gmail.com",
            };

            var result = await userManager.CreateAsync(admin, "Laravel!234");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            await context.SaveChangesAsync();
        }
    }
}
