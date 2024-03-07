using EduPortal.Application.DTO_s.Consumer;
using EduPortal.Application.Interfaces;
using EduPortal.Domain.Entities;
using EduPortal.Models;
using EduPortal.Persistence.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Diagnostics;

namespace EduPortal.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            ViewData["Title"] = "Welcome";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }



    }
}


//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Register([FromForm] UserCreateDTO model)
//{
//    var user = new IdentityUser
//    {
//        UserName = model.Password,
//        Email = model.Email,
//    };

//    var result = await _userManager
//        .CreateAsync(user, model.Password);

//    if (result.Succeeded)
//    {
//        var roleResult = await _userManager
//            .AddToRoleAsync(user, "User");

//        if (roleResult.Succeeded)
//            return RedirectToAction("Login", new { ReturnUrl = "/" });
//    }
//    else
//    {
//        foreach (var err in result.Errors)
//        {
//            ModelState.AddModelError("", err.Description);
//        }
//    }

//    return View();
//}

