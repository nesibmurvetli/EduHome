using EduHome.Helpers;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static EduHome.Helpers.Helper;

namespace EduHome.Controllers
{
    public class AccountController : Controller
    {
        #region userlerin istifadsi ucun
        private readonly UserManager<AppUser> _userManager; /* userlerin idarəsi üçün ist olunur*/
        private readonly RoleManager<IdentityRole> _roleManager;/* burda konkret obyekt yaratmışıq*/
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
                                   RoleManager<IdentityRole> roleManager,
                                     SignInManager<AppUser> signInManager)
        {
            _userManager = userManager; /* userləriidarəedən kod*/
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        #endregion
        #region register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Index", "Home");
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        { 
            if (!ModelState.IsValid) /*xanalar tam doldurulmayanda verilen xetanı eks eletdirir*/
            {
                return View();
            }
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password); /*qeydiyyatdan kecmek ucun*/
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(appUser, true);  /*Burdaki true yaddaaşda qalsın mi funksiyası üçündü*/
            await _userManager.AddToRoleAsync(appUser, Helper.Roles.Member.ToString());
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Index", "Home");
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser appUser = await _userManager.FindByNameAsync(loginVM.Username);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong!");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut) /*giriş koduna qoyulan limiti aşdıqda çıxan error kodu*/
            {
                ModelState.AddModelError("", "Lockout is error");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is wrong!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Logout
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
                //return NotFound();
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        //#region CreateRole
        //public async Task CreateRole()
        //{
        //    if (!(await _roleManager.RoleExistsAsync(Roles.SuperAdmin.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.SuperAdmin.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Roles.Admin.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Roles.Member.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member.ToString() });
        //    }
        //}
        //#endregion
    }
}
