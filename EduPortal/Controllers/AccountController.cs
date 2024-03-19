//using EduPortal.Application.DTO_s.User;

//using Microsoft.AspNetCore.Mvc;
//using EduPortal.Models;
//using EduPortal.API.Models;
//using Microsoft.AspNetCore.Identity;
//using Entities.Dtos;

//namespace EduPortal.Controllers
//{
//    public class AccountController(
//        UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager) : Controller
//    {


//        [HttpPost]
//        public IActionResult Login([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
//        {
//            return View(new LoginModel()
//            {
//                ReturnUrl = ReturnUrl
//            });
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login([FromForm] LoginModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                IdentityUser user = await userManager.FindByNameAsync(model.Name);
//                if (user is not null)
//                {
//                    await signInManager.SignOutAsync();
//                    if ((await signInManager.PasswordSignInAsync(user, model.Password, false, false)).Succeeded)
//                    {
//                        return Redirect(model?.ReturnUrl ?? "/");
//                    }
//                }
//                ModelState.AddModelError("Error", "Invalid username or password.");
//            }
//            return View();
//        }

//        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
//        {
//            await signInManager.SignOutAsync();
//            return Redirect(ReturnUrl);
//        }

//        public IActionResult Register()
//        {
//            return View();
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Register([FromForm] RegisterDto model)
//        {
//            var user = new IdentityUser
//            {
//                UserName = model.UserName,
//                Email = model.Email,
//            };

//            var result = await userManager.CreateAsync(user, model.Password);

//            if (result.Succeeded)
//            {
//                var roleResult = await userManager
//                    .AddToRoleAsync(user, "User");

//                if (roleResult.Succeeded)
//                    return RedirectToAction("Login", new { ReturnUrl = "/" });
//            }
//            else
//            {
//                foreach (var err in result.Errors)
//                {
//                    ModelState.AddModelError("", err.Description);
//                }
//            }

//            return View();
//        }

//        //[HttpPost]
//        //public async Task<IActionResult> Register(RegisterViewModel model)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        var user = new AppUser
//        //        {
//        //            UserName = model.Email,
//        //            Email = model.Email,
//        //        };

//        //        var result = await userManager.CreateAsync(user, model.Password);

//        //        if (result.Succeeded)
//        //        {
//        //            await signInManager.SignInAsync(user, isPersistent: false);

//        //            return RedirectToAction("index", "Home");
//        //        }

//        //        foreach (var error in result.Errors)
//        //        {
//        //            ModelState.AddModelError("", error.Description);
//        //        }

//        //        ModelState.AddModelError(string.Empty, "Invalid");

//        //    }
//        //    return View();
//        //}



//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Register([FromForm] RegisterDto model)
//        //{
//        //    var user = new IdentityUser
//        //    {
//        //        UserName = model.UserName,
//        //        Email = model.Email,
//        //    };

//        //    var result = await _userManager.CreateAsync(user, model.Password);

//        //    if (result.Succeeded)
//        //    {
//        //        var roleResult = await _userManager
//        //            .AddToRoleAsync(user, "User");

//        //        if (roleResult.Succeeded)
//        //            return RedirectToAction("Login", new { ReturnUrl = "/" });
//        //    }
//        //    else
//        //    {
//        //        foreach (var err in result.Errors)
//        //        {
//        //            ModelState.AddModelError("", err.Description);
//        //        }
//        //    }

//        //    return View();
//        //}

//        public IActionResult AccessDenied([FromQuery(Name = "ReturnUrl")] string returUrl)
//        {
//            return View();
//        }
//    }
//}
