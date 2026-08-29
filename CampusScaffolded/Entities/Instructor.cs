using System;
using System.Collections.Generic;

namespace CampusScaffolded.Entities;

public partial class Instructor
{
    public int InstructorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public int DepartmentId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Department Department { get; set; } = null!;
}
