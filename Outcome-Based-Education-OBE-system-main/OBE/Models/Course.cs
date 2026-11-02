using System;
using System.Collections.Generic;

namespace OBE.Models;

public partial class Course
{
    public int Id { get; set; }

    public int? ProgramId { get; set; }

    public string? Code { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? Mark { get; set; }

    public virtual ICollection<CourseGoal> CourseGoals { get; set; } = new List<CourseGoal>();

    public virtual ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();

    public virtual Program? Program { get; set; }
}
