using System;
using System.Collections.Generic;

namespace CampusScaffolded.Entities;

public partial class StudentAddress
{
    public int StudentAddressId { get; set; }

    public int StudentId { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
