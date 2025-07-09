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
                        builder.Entity<AppUser>(entity =>
                        {
                                entity.Property(u => u.Name).HasMaxLength(100);

                                entity.HasMany(u => u.Coordenations)
                                      .WithOne(c => c.Coordinator)
                                      .HasForeignKey(c => c.CoordinatorId)
                                      .OnDelete(DeleteBehavior.Restrict);
                                entity.HasMany(u => u.Classes)
                                      .WithOne(c => c.ClassDirector)
                                      .HasForeignKey(c => c.ClassDirectorId)
                                      .OnDelete(DeleteBehavior.Restrict);
                        });

                        builder.Entity<Coordenation>(entity =>
                        {
                                entity.Property(c => c.Name).HasMaxLength(100);

                                // Cursos da coordenação
                                entity.HasMany(c => c.Courses)
                                      .WithOne(c => c.Coordenation)
                                      .HasForeignKey(c => c.CoordenationId)
                                      .OnDelete(DeleteBehavior.Restrict);

                                // Turmas da coordenação
                                entity.HasMany(c => c.Classes)
                                      .WithOne(c => c.Coordenation)
                                      .HasForeignKey(c => c.CoordenationId)
                                      .OnDelete(DeleteBehavior.Restrict);
                        });
                        // Turmas do curso
                        builder.Entity<Course>(entity =>
                        {
                                entity.Property(c => c.Name).HasMaxLength(100);

                                entity.HasMany(c => c.Classes)
                                      .WithOne(c => c.Course)
                                      .HasForeignKey(c => c.CourseId)
                                      .OnDelete(DeleteBehavior.Restrict);
                        });
                        builder.Entity<Class>(entity =>
                        {
                                entity.Property(c => c.Name).HasMaxLength(100);

                                // Alunos da turma
                                entity.HasMany(c => c.pupils)
                                      .WithOne(p => p.Class)
                                      .HasForeignKey(p => p.ClassId)
                                      .OnDelete(DeleteBehavior.Restrict);
                                // Cadeiras da turma
                                entity.HasMany(c => c.Subjects)
                                      .WithOne(s => s.Class)
                                      .HasForeignKey(s => s.ClassId)
                                      .OnDelete(DeleteBehavior.Restrict);
                        });

                        builder.Entity<Pupil>(entity =>
                        {
                                entity.Property(p => p.Name).HasMaxLength(100);

                                // Notas do aluno
                                entity.HasMany(p => p.Marks)
                                      .WithOne(m => m.Pupil)
                                      .HasForeignKey(m => m.PupilId)
                                      .OnDelete(DeleteBehavior.Cascade);
                        });

                        builder.Entity<Subject>(entity =>
                        {
                                entity.Property(s => s.Name).HasMaxLength(100);
                                entity.Property(s => s.Grade).HasMaxLength(20);
                        });
                        builder.Entity<Mark>(entity =>
                        {
                                entity.Property(m => m.test).HasMaxLength(100);
                                entity.Property(m => m.Value).HasColumnType("REAL");
                        });



                }

                public DbSet<Coordenation> Coordenations { get; set; }
                public DbSet<Course> Courses { get; set; }
                public DbSet<Class> Classes { get; set; }
                public DbSet<Pupil> Pupils { get; set; }
                public DbSet<Subject> Subjects { get; set; }
                public DbSet<Mark> Marks { get; set; }

        }
}