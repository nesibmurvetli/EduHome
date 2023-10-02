using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers
{
    public class ServicesController : Controller
    {
        #region Sql vəya databaseden datanı götürmək üçün
        private readonly AppDbContext _db;

        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        #endregion
        #region Index
        public IActionResult Index(int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = Math.Ceiling((decimal)_db.Services.Count() / 3);
            List<Service> services = _db.Services.OrderByDescending(x => x.Id).Skip((page - 1) * 3).Take(3).ToList(); /*bütün dataları göstərmək üçün*/
            return View(services);
        }
        #endregion
        #region create
        //Create Get METODU
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] //pOSTMETODU
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)  /*verilən şərtlər ödəmədikdə */
            {
                return View();

            }
            bool isExist = await _db.Services.AnyAsync(x => x.Title == service.Title);   //database de var və yox olduğun yoxlamaq üçün
            if (isExist)
            {
                ModelState.AddModelError("Title", "This Servis is already exist");
                return View();
            }
            await _db.Services.AddAsync(service); /*buradan database məlumat əlavə etmək*/
            await _db.SaveChangesAsync(); /*edilən dəyişikliyi database de yaddaşa vurmaq üçün*/
            string color = "color:red";
            string message = $"<a style={color:red}>Salam</a>";
            //await Helper.SendMailAsync("Basliq", message, "nesib.murvetli986@gmail.com");
            return RedirectToAction("Index");
        }
        #endregion
    }
}
