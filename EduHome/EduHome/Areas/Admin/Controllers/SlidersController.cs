using EduHome.DAL;
using EduHome.Helpers;
using EduHome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]

    public class SlidersController : Controller
    {
        #region DAta baseden mel.cek
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SlidersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public IActionResult Index()
        {
            List<Slider> Sliders = _db.Sliders.ToList();  /* Bu iki metodu da istifade ede bilerik*/
            return View(Sliders);
            //return View(_db.Sliders.Where(x => !x.IsDeactive).ToList());
        }
        #endregion
        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Slider slider)
        {

            if (!ModelState.IsValid)
            {
                return View();

            }
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Pleace Choose a photo");
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Pleace Choose an image file");
                return View();

            }
            if (slider.Photo.IsOlderTwoMB())
            {
                ModelState.AddModelError("Photo", "İmage max 2mb");
                return View();

            }
            string folder = Path.Combine(_env.WebRootPath, "img", "slider"); ;
            slider.Image = await slider.Photo.SaveFileAsync(folder);
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
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
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbSlider);
        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Slider slider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.Photo != null) /*sekil yoxdursada deyisiklik et*/
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Pleace Choose an image file");
                    return View();
                }
                if (slider.Photo.IsOlderTwoMB())
                {
                    ModelState.AddModelError("Photo", "İmage max 2mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                string path = Path.Combine(folder, dbSlider.Image);  /*kohne şekili yenisi ile evez et*/
                if (System.IO.File.Exists(path))  /*databasede  de kohne sekil tapıldissa sil onu*/
                {
                    System.IO.File.Delete(path);
                }
                dbSlider.Image = await slider.Photo.SaveFileAsync(folder);
            }
            dbSlider.Title = slider.Title;
            dbSlider.SubTitle = slider.SubTitle;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete
        //Delet Get
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            return View(dbSlider);
        }
        //Delete post
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)  /*id si olmayanın yoxlanılmasınının qarçısını almaq üçün*/
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)  /*yaranmamış id lini yoxlamaq üçün */
            {
                return BadRequest();
            }
            dbSlider.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Activiti
        public async Task<IActionResult> Activity(int? id)//deletin post metodu
        {
            if (id == null)
            {
                return NotFound();

            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {

                return NotFound();

            }
            if (dbSlider.IsDeactive)
            {
                dbSlider.IsDeactive = false;
            }
            else
            {
                dbSlider.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
