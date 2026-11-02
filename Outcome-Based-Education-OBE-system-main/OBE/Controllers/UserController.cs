using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OBE.Models;

namespace OBE.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Register()
        {
         return View();
        }
        [HttpPost]
        public IActionResult Register(string email,string password,int programId,string name_ar,string name_en)
        {
            if (password.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters long.");
            }
            if (_db.Users.Any(u => u.Email == email))
            {
                return BadRequest("Email is already registered.");
            }
            if (string.IsNullOrEmpty(email)|| string.IsNullOrEmpty(password)|| string.IsNullOrEmpty(name_en)|| string.IsNullOrEmpty(name_ar))
            {
                return BadRequest("please enter All fileds");
            }
            var user = new User
            {
                Email = email,
                Password = password,
                ProgramId = programId,
                NameAr = name_ar,
                NameEn = name_en,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                Role = "Admin"
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok("User registered successfully.");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login (string email , string password)
        {
           
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "you dont have account");
                return View();
            }
            var user = _db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            HttpContext.Session.SetString("CurrentUserName",user.NameEn);
            HttpContext.Session.SetString("Role",user.Role);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email and password is not true");
                return View();
            }

            return RedirectToAction("GetProgram","Program");
        }
    }
}
