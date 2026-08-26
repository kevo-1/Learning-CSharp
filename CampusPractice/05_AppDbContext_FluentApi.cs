// ============================================================================
// DbContext for the Fluent-API model set.
// Everything the Data Annotations version spread across attributes lives here
// instead, in one place - useful for seeing the whole relationship map at once.
// ============================================================================

using CampusPractice.FluentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusPractice.FluentApi
{
    public class CampusContext : DbContext
    {
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<StudentAddress> StudentAddresses => Set<StudentAddress>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost\\TESTINGSQLSERVER;Database=CampusPracticeDb;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<StudentAddress>().ToTable("StudentAddress");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");

            modelBuilder.Entity<Department>(e =>
            {
                e.HasKey(d => d.DepartmentId);
                e.Property(d => d.Name).IsRequired().HasMaxLength(100);
                e.Property(d => d.Building).HasMaxLength(50);
                e.Property(d => d.Budget).HasPrecision(12, 2);
            });

            modelBuilder.Entity<Instructor>(e =>
            {
                e.HasKey(i => i.InstructorId);
                e.Property(i => i.FirstName).IsRequired().HasMaxLength(50);
                e.Property(i => i.LastName).IsRequired().HasMaxLength(50);

                // One-to-many: Department -> Instructors
                e.HasOne(i => i.Department)
                    .WithMany(d => d.Instructors)
                    .HasForeignKey(i => i.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict); // don't let a department delete wipe out instructors
            });

            modelBuilder.Entity<Student>(e =>
            {
                e.HasKey(s => s.StudentId);
                e.Property(s => s.FirstName).IsRequired().HasMaxLength(50);
                e.Property(s => s.LastName).IsRequired().HasMaxLength(50);
                e.Property(s => s.Email).IsRequired().HasMaxLength(100);
                e.HasIndex(s => s.Email).IsUnique();
            });

            modelBuilder.Entity<StudentAddress>(e =>
            {
                e.HasKey(a => a.StudentAddressId);
                e.Property(a => a.Street).IsRequired().HasMaxLength(100);
                e.Property(a => a.City).IsRequired().HasMaxLength(50);
                e.Property(a => a.ZipCode).IsRequired().HasMaxLength(10);

                // One-to-one: Student <-> StudentAddress.
                // The unique index on the FK is what makes it 1:1 instead of 1:many.
                e.HasOne(a => a.Student)
                    .WithOne(s => s.Address)
                    .HasForeignKey<StudentAddress>(a => a.StudentId)
                    .OnDelete(DeleteBehavior.Cascade); // delete student -> delete their address
            });

            modelBuilder.Entity<Course>(e =>
            {
                e.HasKey(c => c.CourseId);
                e.Property(c => c.Title).IsRequired().HasMaxLength(100);

                e.HasOne(c => c.Department)
                    .WithMany(d => d.Courses)
                    .HasForeignKey(c => c.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Optional one-to-many: Instructor -> Courses (nullable FK)
                e.HasOne(c => c.Instructor)
                    .WithMany(i => i.Courses)
                    .HasForeignKey(c => c.InstructorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Enrollment>(e =>
            {
                e.HasKey(en => en.EnrollmentId);
                e.Property(en => en.Grade).HasPrecision(4, 2);

                // Many-to-many WITH PAYLOAD, modeled as two one-to-many relationships
                // pointing at the join entity Enrollment.
                e.HasOne(en => en.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(en => en.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(en => en.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(en => en.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // A student can't enroll in the same course twice
                e.HasIndex(en => new { en.StudentId, en.CourseId }).IsUnique();
            });
        }
    }
}

/* ----------------------------------------------------------------------------
   Try building BOTH DbContexts against the same empty database and diff the
   generated migrations - it's a great way to see exactly what Fluent API is
   doing "for free" via convention vs. what you had to state explicitly.
---------------------------------------------------------------------------- */
