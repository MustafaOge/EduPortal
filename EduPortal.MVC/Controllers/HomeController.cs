using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Services;
using EduPortal.Domain.Entities;
using EduPortal.Models.Entities;
using EduPortal.Models.ViewModels.AppUsers;
using EduPortal.MVC.Models;
using EduPortal.Persistence.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NToastNotify;
using System.Diagnostics;
using System.Text;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EduPortal.MVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        private readonly IToastNotification _toast;
        private readonly IDistributedCache _distributedCache;
        private readonly AppDbContext _appDbContext;
        private readonly ICacheService _cacheService;
        //private LanguageService _localization;
        private readonly LanguageService _localization;


        public HomeController(LanguageService localization, ICacheService cacheservice, IDistributedCache distributedCache,ILogger<HomeController> logger,  AppDbContext appDbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IToastNotification toast )
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _toast = toast;
            _distributedCache = distributedCache;
            _appDbContext = appDbContext;
            _cacheService = cacheservice;
            _localization = localization;
        }

        public IActionResult Index()
        {
            //ViewBag.Welcome = _localization.Getkey("Welcome").Value;
            //var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return View();
        }


                                

        [HttpGet("set")]
        public async Task<IActionResult> Set()
        {
            _cacheService.CacheSubscribersAsync();
            //var subscribers = await _appDbContext.Invoices.ToListAsync();

            //foreach (var subscriber in subscribers)
            //{
            //    // Serialize and store each subscriber in Redis cache
            //    var serializedSubscriber = JsonConvert.SerializeObject(subscriber);
            //    await _distributedCache.SetStringAsync("subscriber:" + subscriber.Id, serializedSubscriber);
            //}

            return View("Index");
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            // Try to get subscribers from Redis cache
            var serializedSubscribers = await _distributedCache.GetStringAsync("subscribers");


            if (serializedSubscribers != null)
            {
                var subscribers = JsonConvert.DeserializeObject<List<Invoice>>(serializedSubscribers);
                return Ok(subscribers);
            }
            else
            {
                // If not found in cache, fetch from database
                var subscribers = await _appDbContext.Invoices.ToListAsync();

                // Serialize and store subscribers in Redis cache
                //var serializedSubscribers = JsonConvert.SerializeObject(subscribers);
                //await _distributedCache.SetStringAsync("subscribers", serializedSubscribers);

                return Ok(subscribers);
            }
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
