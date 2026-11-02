using System;
using System.Collections.Generic;

namespace OBE.Models;

public partial class ProgramGoalCourseGoal
{
    public int Id { get; set; }

    public int? ProgramGoalId { get; set; }

    public int? CourseGoalId { get; set; }

    public string? CorrelationLevel { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CourseGoal? CourseGoal { get; set; }

    public virtual ProgramGoal? ProgramGoal { get; set; }
}
