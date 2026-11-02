using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBE.Models;

namespace OBE.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _db;
        public CourseController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult GetAllCourse()
        {
            var courses = _db.Courses.Include(x=>x.Program).ToList();
            ViewBag.Programs = _db.Programs.ToList();
            return View(courses);
        }
        [HttpPost]
        public IActionResult CreateCourse(string Code,string NameEn,string NameAr,int programId,decimal Mark)
        { 
        if(string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(NameAr) || string.IsNullOrEmpty(NameEn))
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
            var course = new OBE.Models.Course
            {
                Code = Code,
               NameEn = NameEn,
               NameAr = NameAr,
               ProgramId= programId,
               Mark= Mark,
                CreatedAt = DateTime.UtcNow,
               UpdatedAt= DateTime.UtcNow
            };
            _db.Courses.Add(course);
            _db.SaveChanges();
            return Ok();
        
        
        }
        [HttpPost]
        public IActionResult EditCourse(int id, string Code, string NameEn, string NameAr,decimal Mark ,int programId)
        {
            var course = _db.Courses.Find(id);
            if (course == null)
            {
                return NotFound("the Id is not exists");
            }
            course.Code = Code;
            course.NameEn = NameEn;
            course.NameAr = NameAr;
            course.ProgramId = programId;
            course.Mark = Mark;
            course.UpdatedAt = DateTime.Now;
            _db.Courses.Update(course);
            _db.SaveChanges();
            return Ok(course);
        }
        [HttpPost]
        public IActionResult DeleteCourse(int Id)
        {
            var course = _db.Courses.Find(Id);
            if (course == null)
            {
                return NotFound();
            }
            _db.Courses.Remove(course);
            _db.SaveChanges();
            return Ok();
        }
    }
}
