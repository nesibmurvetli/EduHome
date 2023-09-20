
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
        public IActionResult Index()

        {
            List<Slider> sliders =_db.Sliders.ToList();
            List<Blog> blogs =_db.Blogs.ToList();
            List<Course> courses =_db.Courses.ToList();
            About about = _db.Abouts.FirstOrDefault();

            HomeVM homeVM = new HomeVM
            {
                Sliders = sliders,
                Blogs = blogs,
                Courses=courses,
                About = about,
          
            };
            return View(homeVM);
        }

    }

}