using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers
{
    public class CoursesController : Controller
    {
       
            private readonly AppDbContext _db;
            public CoursesController(AppDbContext db)
            {
                _db = db;
            }

            public IActionResult Index()
            {
                ViewBag.Procount = _db.Courses.Count();
                List<Course> courses = _db.Courses.OrderByDescending(x => x.Id).Take(6).ToList();
                return View(courses);
            }

            public IActionResult LoadMore(int skip)
            {
                if (_db.Courses.Count() <= skip)
                {
                    return Content("son");
                }

                List<Course> courses = _db.Courses.OrderByDescending(x => x.Id).Skip(skip).Take(6).ToList();
                return PartialView("_CoursesLoadMorePartial", courses);
            }

        }
    }