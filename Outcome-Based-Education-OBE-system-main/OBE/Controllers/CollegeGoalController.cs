using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBE.Models;

namespace OBE.Controllers
{
    public class CollegeGoalController : Controller
    {
        private readonly AppDbContext _db;
        public CollegeGoalController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult GetAllCollage()
        {
            var collegeGoals = _db.CollegeGoals.Include(x =>x.CollegeGoalProgramGoals).ThenInclude(b=>b.ProgramGoal).ThenInclude(c=>c.Program).ToList();
            ViewBag.CollegeGoals = collegeGoals;
            ViewBag.Programs = _db.Programs.ToList();
            return View(collegeGoals);
        }
       
       
            [HttpPost]
            public IActionResult CreateCollage(CollegeGoal college, int programId)
            {
                if (college == null)
                {
                    return BadRequest("Invalid course goal data.");
                }

              
                if (programId <= 0)
                {
                    return BadRequest("Invalid Program ID.");
                }

                
                college.CreatedAt = DateTime.Now;

               
                college.CollegeGoalProgramGoals = new List<CollegeGoalProgramGoal>();

          
                college.CollegeGoalProgramGoals.Add(new CollegeGoalProgramGoal
                {
                    ProgramGoalId = programId,
                    CreatedAt = DateTime.Now
                   
                });

                try
                {
                  
                    _db.CollegeGoals.Add(college);
                    _db.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                   
                    return BadRequest($"Error saving data: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
        
        [HttpPost]
        public IActionResult UpdateCollage(CollegeGoal college)
        {
            if (college == null || college.Id == 0)
            {
                return BadRequest("Invalid course goal data.");
            }
            var existingCollegeGoal = _db.CollegeGoals.Find(college.Id);
                if (existingCollegeGoal != null)
                {
                    existingCollegeGoal.Code = college.Code;
                    existingCollegeGoal.TitleAr = college.TitleAr;
                    existingCollegeGoal.TitleEn = college.TitleEn;
                    existingCollegeGoal.DescriptionAr = college.DescriptionAr;
                    existingCollegeGoal.DescriptionEn = college.DescriptionEn;
                    existingCollegeGoal.UpdatedAt = DateTime.Now;
                    _db.SaveChanges();
                }
             
            
            return Ok();
        }
        [HttpPost]
        public IActionResult DeleteCollage(int id)
        {
            var collegeGoal = _db.CollegeGoals.Find(id);
            if (collegeGoal == null)
            {
              return NotFound("College goal is not exists.");
            }
            _db.CollegeGoals.Remove(collegeGoal);
            _db.SaveChanges();
            return Ok();
        }

    }
}
