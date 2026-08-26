// ============================================================================
// APPROACH 2: CODE-FIRST WITH FLUENT API
// ----------------------------------------------------------------------------
// Same domain, but the POCOs are deliberately "clean" - zero attributes.
// Every rule (keys, required, max length, relationships, indexes, delete
// behavior) is declared centrally in OnModelCreating.
//
// Why teams often prefer this: it keeps your domain model persistence-
// ignorant (no EF/System.ComponentModel references leaking into your classes),
// and it can express things attributes flatly cannot (composite keys,
// shadow properties, value conversions, query filters, table splitting...).
// ============================================================================

namespace CampusPractice.FluentApi.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = null!;
        public string? Building { get; set; }
        public decimal Budget { get; set; }

        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }

    public class Instructor
    {
        public int InstructorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime HireDate { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }

        public StudentAddress? Address { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

    public class StudentAddress
    {
        public int StudentAddressId { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public int Credits { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public int? InstructorId { get; set; }
        public Instructor? Instructor { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public decimal? Grade { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
