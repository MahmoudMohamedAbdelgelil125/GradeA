using GradeALMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GradeALMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base Identity configuration first
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser entity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
                entity.Property(u => u.RegistrationDate).HasDefaultValueSql("GETDATE()");

                // One-to-one relationship with Student
                entity.HasOne(u => u.Student)
                      .WithOne(s => s.ApplicationUser)
                      .HasForeignKey<Student>(s => s.ApplicationUserId)
                      .OnDelete(DeleteBehavior.SetNull);

                // One-to-one relationship with Instructor
                entity.HasOne(u => u.Instructor)
                      .WithOne(i => i.ApplicationUser)
                      .HasForeignKey<Instructor>(i => i.ApplicationUserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(s => s.Email).IsUnique();
            });

            // Configure Instructor entity
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
                entity.Property(i => i.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(i => i.Email).IsUnique();
            });

            // Configure Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Description).HasMaxLength(1000);
                
                entity.HasOne(c => c.Instructor)
                      .WithMany(i => i.Courses)
                      .HasForeignKey(c => c.InstructorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Grade entity
            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.GradeLetter).HasMaxLength(2);
                
                entity.HasOne(g => g.Student)
                      .WithMany(s => s.Grades)
                      .HasForeignKey(g => g.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(g => g.Course)
                      .WithMany(c => c.Grades)
                      .HasForeignKey(g => g.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ensure unique grade per student per course
                entity.HasIndex(g => new { g.StudentId, g.CourseId }).IsUnique();
            });

            // Configure StudentCourse many-to-many relationship
            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });
                
                entity.HasOne(sc => sc.Student)
                      .WithMany(s => s.StudentCourses)
                      .HasForeignKey(sc => sc.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(sc => sc.Course)
                      .WithMany(c => c.StudentCourses)
                      .HasForeignKey(sc => sc.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
