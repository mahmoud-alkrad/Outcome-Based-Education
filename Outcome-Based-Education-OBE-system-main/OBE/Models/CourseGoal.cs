using System;
using System.Collections.Generic;

namespace OBE.Models;

public partial class CourseGoal
{
    public int Id { get; set; }

    public int? CourseId { get; set; }

    public string? Code { get; set; }

    public string? DescriptionAr { get; set; }

    public string? DescriptionEn { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<ProgramGoalCourseGoal> ProgramGoalCourseGoals { get; set; } = new List<ProgramGoalCourseGoal>();
}
