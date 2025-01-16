using acadgest.Models.AbstractModel;
using acadgest.Models.Classes;
using acadgest.Models.Coordenations;
using acadgest.Models.Courses;
using acadgest.Models.Pupils;
using acadgest.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace acadgest.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Coordenation>()
                    .HasMany(c => c.Courses)
                    .WithOne(c => c.Coordenation)
                    .HasForeignKey(c => c.CoordenationId)
                    .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Coordenation>()
                    .HasMany(c => c.Classes)
                    .WithOne(c => c.Coordenation)
                    .HasForeignKey(c => c.CoordenationId)
                    .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Course>()
                    .HasMany(c => c.Classes)
                    .WithOne(c => c.Course)
                    .HasForeignKey(c => c.CourseId)
                    .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Class>()
                    .HasMany(c => c.pupils)
                    .WithOne(p => p.Class)
                    .HasForeignKey(p => p.ClassId)
                    .OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<Coordenation> Coordenations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Pupil> Pupils { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<ClassDirector> ClassDirectors { get; set; }
    }
}