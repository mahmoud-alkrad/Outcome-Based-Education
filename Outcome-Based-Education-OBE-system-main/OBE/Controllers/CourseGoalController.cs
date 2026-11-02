using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OBE.Models;

namespace OBE.Controllers
{
    public class CourseGoalController : Controller
    {
        private readonly AppDbContext _db;
        public CourseGoalController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult GetCourseGoals()
        {
          var courseGoals = _db.CourseGoals.Include(x=>x.Course).ToList();
            ViewBag.Course = _db.Courses.ToList();
            return View(courseGoals);
        }
        [HttpPost]
        public IActionResult CreateCourseGoal( CourseGoal courseGoal)
        {
            if (courseGoal == null)
            {
                return BadRequest("Invalid course goal data.");
            }
            _db.CourseGoals.Add(courseGoal);
            _db.SaveChanges();
            return Ok(courseGoal);
        }
        [HttpPost]
        public IActionResult UpdateCourseGoal( CourseGoal courseGoal)
        {
            if (courseGoal == null || courseGoal.Id == 0)
            {
                return BadRequest("Invalid course goal data.");
            }
            var existingCourseGoal = _db.CourseGoals.Find(courseGoal.Id);
            if (existingCourseGoal == null)
            {
                return NotFound("Course goal not found.");
            }
            existingCourseGoal.Code = courseGoal.Code;
            existingCourseGoal.DescriptionAr = courseGoal.DescriptionAr;
            existingCourseGoal.DescriptionEn = courseGoal.DescriptionEn;
            existingCourseGoal.CourseId = courseGoal.CourseId;
            existingCourseGoal.UpdatedAt = DateTime.Now;
            _db.CourseGoals.Update(existingCourseGoal);
            _db.SaveChanges();
            return Ok(existingCourseGoal);
        }
        [HttpPost]
        public IActionResult DeleteCourseGoal( int id)
        {
            var courseGoal = _db.CourseGoals.Find(id);
            if (courseGoal == null)
            {
                return NotFound("Course goal not found.");
            }
            _db.CourseGoals.Remove(courseGoal);
            _db.SaveChanges();
            return Ok("Course goal deleted successfully.");

        }
        }
}
