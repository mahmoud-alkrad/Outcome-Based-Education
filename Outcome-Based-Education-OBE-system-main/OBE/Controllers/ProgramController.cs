using Microsoft.AspNetCore.Mvc;
using OBE.Models;

namespace OBE.Controllers
{
    public class ProgramController : Controller
    {
        private readonly AppDbContext _db;
        public ProgramController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetProgram()
        {
            var programs = _db.Programs.ToList();
            return View(programs);
        }
        [HttpPost]
        public IActionResult CreateProgram(string code,string NameEn,string NameAr, string isActive)
        {
            if(string.IsNullOrEmpty(code) || string.IsNullOrEmpty(NameEn) || string.IsNullOrEmpty(NameAr))
            {
                return BadRequest("Invalid input");
            }
           var program = new OBE.Models.Program
            {
                Code = code,
                NameEn = NameEn,
                NameAr = NameAr,
               IsActive = (isActive == "true"),
               CreatedAt = DateTime.UtcNow
            };
            _db.Programs.Add(program);
            _db.SaveChanges();
            return Ok(program);
        }
        [HttpPost]
        public IActionResult EditProgram(int id, string code, string NameEn, string NameAr, bool isActive)
        {
            
            var existingProgram = _db.Programs.Find(id);

            if (existingProgram == null)
            {
                return NotFound("Program not found");
            }

            
            existingProgram.Code = code;
            existingProgram.NameEn = NameEn;
            existingProgram.NameAr = NameAr;
            existingProgram.IsActive = isActive;
            existingProgram.UpdatedAt = DateTime.UtcNow; 

            _db.Programs.Update(existingProgram);
            _db.SaveChanges();

            return Ok(existingProgram);
        }
        [HttpPost]
        public IActionResult DeleteProgram(int id)
        {
            var program = _db.Programs.Find(id);

            if (program != null)
            {
                _db.Programs.Remove(program); 
                _db.SaveChanges();          
                return Ok(new { message = "Deleted" });
            }

            return NotFound(); 
        }
    }
}
