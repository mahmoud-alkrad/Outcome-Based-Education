using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBE.Models;

namespace OBE.Controllers
{
    public class ProgramGoalController : Controller
    {
        private readonly AppDbContext _db;
        public ProgramGoalController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetProgramGoals()
        {
            var programGoals = _db.ProgramGoals
                          .Include(x => x.Program) // عشان تجيب اسم البرنامج المرتبط
                          .ToList();
            ViewBag.Programs = _db.Programs.ToList();
            return View(programGoals);
        }
        [HttpPost]
        public IActionResult CreateProgramGoal(string code,string TitleEn,string TitleAr, string DescriptionAr, string DescriptionEn,int programId)
        {
            if(string.IsNullOrEmpty(code) || string.IsNullOrEmpty(TitleAr) || string.IsNullOrEmpty(TitleEn)|| string.IsNullOrEmpty(DescriptionEn) || string.IsNullOrEmpty(DescriptionAr))
            {
                return BadRequest("Invalid input");
            }
            ModelState.Remove("Program"); 
            if (!ModelState.IsValid)
            {
                // هذا الكود يجمع أسماء الحقول التي فيها مشاكل
                var errors = string.Join("\n", ModelState.Keys
                            .Where(k => ModelState[k].Errors.Count > 0)
                            .Select(k => $"{k}: {ModelState[k].Errors.First().ErrorMessage}"));

                return BadRequest("الأخطاء هي:\n" + errors);
            }
            var programGoal = new OBE.Models.ProgramGoal
            {
                Code = code,
               TitleEn = TitleEn,
               TitleAr = TitleAr,
              DescriptionAr = DescriptionAr,
                DescriptionEn = DescriptionEn,
                ProgramId= programId,
               CreatedAt = DateTime.UtcNow,
               UpdatedAt= DateTime.UtcNow
            };
            _db.ProgramGoals.Add(programGoal);
            _db.SaveChanges();
            return Ok();

        }
        [HttpPost]
        public IActionResult EditProgramGoal(int id, string code,string TitleEn,string TitleAr, string DescrptionAr, string DescrptionEn)
        {
            
            var existingProgramGoal = _db.ProgramGoals.Find(id);
            if (existingProgramGoal == null)
            {
                return NotFound("Program Goal not found");
            }
            
            existingProgramGoal.Code = code;
           existingProgramGoal.TitleEn = TitleEn;
           existingProgramGoal.TitleAr = TitleAr;
          existingProgramGoal.DescriptionAr = DescrptionAr;
            existingProgramGoal.DescriptionEn = DescrptionEn;
            existingProgramGoal.UpdatedAt = DateTime.UtcNow; 
            _db.ProgramGoals.Update(existingProgramGoal);
            _db.SaveChanges();
            return Ok(existingProgramGoal);
        }
        [HttpPost]
        public IActionResult DeleteProgramGoal(int id)
        {
            var existingProgramGoal = _db.ProgramGoals.Find(id);
            if (existingProgramGoal == null)
            {
                return NotFound("Program Goal not found");
            }
            _db.ProgramGoals.Remove(existingProgramGoal);
            _db.SaveChanges();
            return Ok("Program Goal deleted successfully");
        }

    }
}
