using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Models;
using EduPortal.Persistence.context;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using static System.Reflection.Metadata.BlobBuilder;

namespace EduPortal.Controllers
{
    public class SubscriberController(
        IToastNotification toast,
        AppDbContext appDbContext,
        ISubsIndividualService subsIndividualService,
        ISubsCorporateService subsCorporateService
        ) : Controller
    {



        //[HttpPost]
        //public IActionResult SubscriberCreate(Subscriber subscriber)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        dbContext.Subscribers.Add(subscriber);
        //        dbContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(subscriber);
        //}
        //[Route("Subscriber")]

        [HttpPost]
        public IActionResult Create(Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                //_db.Subscribers.Add(subscriber);
                appDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult SubscriptionRemove()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Corporate()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Corporate(CreateCorporateDto individual)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await subsCorporateService.CreateIndividualAsync(individual);
                toast.AddSuccessToastMessage("İşlem Başarılı");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                toast.AddErrorToastMessage("Abone Eklenemedi: " + ex.Message);
                return View();
            }
        }


        //[HttpPost]
        //public IActionResult Corporate(SubsCorporate corporate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        toast.AddSuccessToastMessage("Abone Eklendi", new ToastrOptions { Title = "Başarılı!" });
        //        appDbContext.Corprorates.Add(corporate);
        //        appDbContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        toast.AddErrorToastMessage("Abone Eklenemedi", new ToastrOptions { Title = "Başarısız!" });
        //    }
        //    return View();
        //}

      
        [HttpPost]

        public async Task<IActionResult> Individual(CreateIndividualDto individual)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                await subsIndividualService.CreateIndividualAsync(individual);
                toast.AddSuccessToastMessage("İşlem Başarılı");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                toast.AddErrorToastMessage("Abone Eklenemedi: " + ex.Message);
                return View();
            }
        }


        [HttpGet]
        public IActionResult Individual()
        {
            return View();
        }


        //ToDo
        [HttpGet]
        public IActionResult FindSubscriber()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FindSubscriber(string IdentityNumber)
        {
            List<SubsIndividual> abone = appDbContext.Individuals.Where(a => a.IdentityNumber == IdentityNumber).ToList();

            if (abone == null)
            {
                TempData["Message"] = "Abone bulunamadı.";
                return View(abone);
            }

            return View(abone);
        }



        public IActionResult CreateFakeData()
        {
            for (int i = 11; i < 15; i++)
            {
                SubsIndividual subsIndividual = new SubsIndividual
                {

                    PhoneNumber = PhoneNumberData.GetPhoneNumber(),
                    NameSurname = NameData.GetFullName(),
                    BirthDate = DateTimeData.GetDatetime(),
                    CounterNumber = NumberData.GetNumber(1000, 100000),
                    IdentityNumber = NumberData.GetNumber(1000, 100000).ToString(),
                    Email = NetworkData.GetEmail(),
                    SubscriberType = "Bireysel"


                };
                appDbContext.Individuals.Add(subsIndividual);
            }
            appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpGet]

        public  IActionResult TerminateSubscription()
        {

            return View();
        }

    }
}
