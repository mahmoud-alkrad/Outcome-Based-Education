using System;
using System.Collections.Generic;

namespace OBE.Models;

public partial class CollegeGoalProgramGoal
{
    public int Id { get; set; }

    public int? CollegeGoalId { get; set; }

    public int? ProgramGoalId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual CollegeGoal? CollegeGoal { get; set; }

    public virtual ProgramGoal? ProgramGoal { get; set; }
}
