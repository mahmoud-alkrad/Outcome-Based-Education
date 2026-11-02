using System;
using System.Collections.Generic;

namespace OBE.Models;

public partial class Program
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<ProgramGoal> ProgramGoals { get; set; } = new List<ProgramGoal>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
