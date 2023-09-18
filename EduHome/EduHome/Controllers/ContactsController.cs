using Microsoft.AspNetCore.Mvc;

namespace EduHome.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
