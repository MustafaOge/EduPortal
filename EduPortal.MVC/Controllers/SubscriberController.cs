using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Models.ViewModel;
using EduPortal.Persistence.context;
using EduPortal.Service.Services;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Net;
using System.Security.Policy;


namespace EduPortal.Controllers
{
    public class SubscriberController(
        IToastNotification toast,
        AppDbContext appDbContext,
        ISubsIndividualService subsIndividualService,
        ISubsCorporateService subsCorporateService
        ) : Controller
    {


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult TerminateSubscription()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateSubscriber()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateCorporate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCorporate(CreateCorporateDto corporate)
        {
            if (!ModelState.IsValid) return View("CreateSubscriber");
            try
            {
                await subsCorporateService.CreateCorporateAsync(corporate);
                toast.AddSuccessToastMessage("İşlem Başarılı");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                toast.AddErrorToastMessage("Abone Eklenemedi: " + ex.Message);
                return View("CreateSubscriber");
            }
        }


        [HttpGet]
        public IActionResult CreateIndividual()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIndividual(CreateIndividualDto individual)
        {
            if (!ModelState.IsValid) return View("CreateSubscriber");
            try
            {
                await subsIndividualService.CreateIndividualAsync(individual);
                toast.AddSuccessToastMessage("İşlem Başarılı");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                toast.AddErrorToastMessage("Abone Eklenemedi: " + ex.Message);
                return View("CreateSubscriber");
            }
        }


        [HttpGet]
        public IActionResult FindSubscriber()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindSubscriber(string IdentityNumber)
        {
            List<SubsIndividual> abone = appDbContext.Individuals.Where(a => a.IdentityNumber == IdentityNumber).ToList();
            if (abone.Count == 0)
            {
                TempData["Message"] = "Abone bulunamadı.";
                return View(abone);
            }
            return View(abone);
        }


        public IActionResult CreateFakeData()
        {
            for (int i = 0; i < 5; i++)
            {
                SubsIndividual subsIndividual = new SubsIndividual
                {

                    PhoneNumber = PhoneNumberData.GetPhoneNumber(),
                    NameSurname = NameData.GetFullName(),
                    BirthDate = DateTimeData.GetDatetime(),
                    CounterNumber = "değişecek sayi11",
                    IdentityNumber = NumberData.GetNumber(1000, 100000).ToString(),
                    Email = NetworkData.GetEmail(),
                    SubscriberType = "Kurumsal"


                };
                appDbContext.Individuals.Add(subsIndividual);
            }
            appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult FindIndividual()
        {

            return View();
        }

        [HttpPost]
        public IActionResult FindIndividual(string IdentityNumber)
        {
            List<SubsIndividual> abone = appDbContext.Individuals.Where(a => a.IdentityNumber == IdentityNumber).ToList();
            if (abone == null)
            {
                TempData["Message"] = "Abone bulunamadı.";
                return View(abone);
            }
            return View(abone);
        }


        [HttpGet]

        public IActionResult FindCorporate()
        {

            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> FindCorporate(string TaxIdNumber)
        //{
        //    var abone = await subsCorporateService.FindCorporateAsync(TaxIdNumber);

        //    if (abone == null)
        //    {
        //        TempData["Message"] = "Kurumsal abone bulunamadı.";
        //        return View(abone);
        //    }

        //    return View(abone);
        //}


        [HttpPost]
        public IActionResult FindCorporate(string taxIdNumber)
        {
            List<SubsCorporate> abone = appDbContext.Corprorates.Where(a => a.TaxIdNumber == taxIdNumber).ToList();

            //List<SubsCorporate> abone = appDbContext.Corprorates.Where(a => a.TaxIdNumber == TaxIdNumber).ToList();

            if (abone == null)
            {
                TempData["Message"] = "Kurumsal abone bulunamadı.";
                return View(abone);
            }

            return View(abone);
        }



    }
}


