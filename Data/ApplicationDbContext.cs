using acadgest.Models.Classes;
using acadgest.Models.Coordenations;
using acadgest.Models.Courses;
using acadgest.Models.Pupils;
using acadgest.Models.Subjects;
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

                        // Coordenador
                        builder.Entity<AppUser>()
                                .HasMany(u => u.Coordenations)
                                .WithOne(c => c.Coordinator)
                                .HasForeignKey(c => c.CoordinatorId)
                                .OnDelete(DeleteBehavior.SetNull);

                        // Diretor de turma
                        builder.Entity<AppUser>()
                                .HasMany(u => u.Classes)
                                .WithOne(c => c.ClassDirector)
                                .HasForeignKey(c => c.ClassDirectorId)
                                .OnDelete(DeleteBehavior.SetNull);

                        // Cursos da coordenação
                        builder.Entity<Coordenation>()
                                .HasMany(c => c.Courses)
                                .WithOne(c => c.Coordenation)
                                .HasForeignKey(c => c.CoordenationId)
                                .OnDelete(DeleteBehavior.SetNull);

                        // Turmas da coordenação
                        builder.Entity<Coordenation>()
                                .HasMany(c => c.Classes)
                                .WithOne(c => c.Coordenation)
                                .HasForeignKey(c => c.CoordenationId)
                                .OnDelete(DeleteBehavior.SetNull);

                        // Turmas do curso
                        builder.Entity<Course>()
                                .HasMany(c => c.Classes)
                                .WithOne(c => c.Course)
                                .HasForeignKey(c => c.CourseId)
                                .OnDelete(DeleteBehavior.SetNull);

                        // Alunos da turma
                        builder.Entity<Class>()
                                .HasMany(c => c.pupils)
                                .WithOne(p => p.Class)
                                .HasForeignKey(p => p.ClassId)
                                .OnDelete(DeleteBehavior.SetNull);
                        // Cadeiras da turma
                        builder.Entity<Class>()
                                .HasMany(c => c.Subjects)
                                .WithOne(s => s.Class)
                                .HasForeignKey(s => s.ClassId)
                                .OnDelete(DeleteBehavior.SetNull);
                        // Notas do aluno
                        builder.Entity<Pupil>()
                                .HasMany(p => p.Marks)
                                .WithOne(m => m.Pupil)
                                .HasForeignKey(m => m.PupilId)
                                .OnDelete(DeleteBehavior.Cascade);

                }

                public DbSet<Coordenation> Coordenations { get; set; }
                public DbSet<Course> Courses { get; set; }
                public DbSet<Class> Classes { get; set; }
                public DbSet<Pupil> Pupils { get; set; }
                public DbSet<Subject> Subjects { get; set; }
                public DbSet<Mark> Marks { get; set; }

        }
}