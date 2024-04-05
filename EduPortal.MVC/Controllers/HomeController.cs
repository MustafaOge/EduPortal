using EduPortal.Models.Entities;
using EduPortal.Models.ViewModels.AppUsers;
using EduPortal.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EduPortal.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        private readonly IToastNotification _toast;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IToastNotification toast )
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _toast = toast;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);


                if (result.Succeeded && model.UserName == "Admin")
                {

                    await _userManager.AddToRoleAsync(appUser, "Admin");

                    TempData["Message"] = "Kullanýcý olusturuldu :)";
                    return RedirectToAction("Index");

                }
                else
                {
                    //await _userManager.AddToRoleAsync(appUser, "User");

                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(model);
        }



        public IActionResult SignIn(string returnUrl)
        {
            UserSignInRequestModel usModel = new()
            {
                ReturnUrl = returnUrl
            };
            return View(usModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("", "Kullanýcý adý boþ olamaz.");
                return View(model);
            }

            AppUser appUser = await _userManager.FindByNameAsync(model.UserName);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Kullanýcý bulunamadý.");
                TempData["ToastrMessage"] = "error,Kullanýcý bulunamadý.";
                return View(model);
            }

            IdentitySignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return !string.IsNullOrEmpty(model.ReturnUrl) ? Redirect(model.ReturnUrl) : RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Kullanýcý adý veya þifre hatalý.");
            return View(model);
        }

    }
}
