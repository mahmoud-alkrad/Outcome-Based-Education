using System;
using System.Collections.Generic;

namespace OBE.Models;

public partial class ProgramGoal
{
    public int Id { get; set; }

    public int? ProgramId { get; set; }

    public string? Code { get; set; }

    public string? TitleAr { get; set; }

    public string? TitleEn { get; set; }

    public string? DescriptionAr { get; set; }

    public string? DescriptionEn { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CollegeGoalProgramGoal> CollegeGoalProgramGoals { get; set; } = new List<CollegeGoalProgramGoal>();

    public virtual Program? Program { get; set; }

    public virtual ICollection<ProgramGoalCourseGoal> ProgramGoalCourseGoals { get; set; } = new List<ProgramGoalCourseGoal>();
}
