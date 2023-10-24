using EduHome.DAL;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]

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
        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbService);
        }
        #endregion
        #region Update
        //get metodu
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbService);
        }
        //set metodu
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Service service)  /*service teze yaradılacaq dbservice bazada olan köhnə*/
        {
            if (id == null)
            {
                return View();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Services.AnyAsync(x => x.Title == service.Title && x.Id != id);//mövcudolan

            if (isExist)
            {
                ModelState.AddModelError("Title", "This Servis already exists");
                return View();
            }
            dbService.Title = service.Title;
            dbService.SubTitle = service.SubTitle;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
        #region Delet
        //Delet Get
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
        //    {
        //        return NotFound();
        //    }
        //    Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
        //    if (dbService == null)  /*yaranmamış id lini yoxlamaq üçün */
        //    {
        //        return BadRequest();
        //    }
        //    return View(dbService);
        //}
        ////Delete post
        //[HttpPost]
        //[ActionName("Delete")]
        //public async Task<IActionResult> DeletePost(int? id)
        //{
        //    if (id == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
        //    {
        //        return NotFound();
        //    }
        //    Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
        //    if (dbService == null)  /*yaranmamış id lini yoxlamaq üçün */
        //    {
        //        return BadRequest();
        //    }
        //    dbService.IsDeactive = true;
        //    await _db.SaveChangesAsync();
        //    return View(dbService);
        //}
        #region Aktiv
        public async Task<IActionResult> Activity(int? id)    /*//deletin post metodu*/
        {
            if (id == null)
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return BadRequest();
            }
            if (dbService.IsDeactive)
            {
                dbService.IsDeactive = false;
            }
            else
            {
                dbService.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #endregion
    }
}
