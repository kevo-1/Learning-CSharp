using System;
using System.Collections.Generic;

namespace CampusScaffolded.Entities;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public int Credits { get; set; }

    public int DepartmentId { get; set; }

    public int? InstructorId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Instructor? Instructor { get; set; }
}
