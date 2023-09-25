
using EduHome.DAL;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = await _db.Sliders.ToListAsync(),
                About = await _db.Abouts.FirstOrDefaultAsync(),
                Courses = await _db.Courses.Take(3).ToListAsync(),
                Blogs = await _db.Blogs.Take(3).ToListAsync(),
               

            };
            return View(homeVM);
        }

    }
}
