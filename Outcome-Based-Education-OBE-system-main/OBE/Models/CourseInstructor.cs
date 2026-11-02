using System;
using System.Collections.Generic;

namespace OBE.Models;

public partial class CourseInstructor
{
    public int Id { get; set; }

    public int? CourseId { get; set; }

    public int? InstructorId { get; set; }

    public string? Semester { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Course? Course { get; set; }

    public virtual User? Instructor { get; set; }
}
