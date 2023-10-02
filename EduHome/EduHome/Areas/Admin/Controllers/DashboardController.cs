using Microsoft.AspNetCore.Mvc;

namespace EduHome.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Admin")]  /*Admin paneli controller olaraq gorməsi üçün bu üsuldan istifadə edilir  /Dashboard  */
        //[Authorize(Roles = "Admin")]

        public IActionResult Index()
        {

            return View();
        }
    }
}
