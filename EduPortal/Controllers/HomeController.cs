using EduPortal.Application.DTO_s.Consumer;
using EduPortal.Application.Interfaces;
using EduPortal.Domain.Entities;
using EduPortal.Models;
using EduPortal.Persistence.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using StoreApp.Models;
using System.Diagnostics;

namespace EduPortal.Controllers
{
    public class HomeController(ILogger<HomeController> loggeri,AppDbContext context) : Controller
    {

        [HttpGet]
        public  IActionResult Index()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login (LoginModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityUser user = await context.FindByNameAsync(model.Name);
                if (user is not null)
                {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
                    {
                        return Redirect(model?.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("Error", "Invalid username or password.");
            }
            return View();

        }

        // log out


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
