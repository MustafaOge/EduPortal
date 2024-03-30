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


        public IActionResult Terminate()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
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
            if (!ModelState.IsValid) return View("Create");
            try
            {
                List<SubsIndividual> abone = appDbContext.Individuals.Where(a => a.CounterNumber == individual.CounterNumber).ToList();
                if (abone.Count > 0)
                {
                    foreach (SubsIndividual sub in abone)
                    {
                        if (sub.IsActive == true)
                        {
                            toast.AddErrorToastMessage("Bu sayaç numarasına ait aktif bir abonelik zaten mevcut.");
                            return View("Create");
                        }
                    }
                }
                else
                {
                    await subsIndividualService.CreateIndividualAsync(individual);
                    toast.AddSuccessToastMessage("İşlem Başarılı");

                }

                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                toast.AddErrorToastMessage("Abone Eklenemedi: " + ex.Message);
                return View("Create");
            }
        }


        [HttpGet]
        public IActionResult Find()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Find(string IdentityNumber)
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
                    CounterNumber = NumberData.GetNumber(1000000, 9999999).ToString(),
                    IdentityNumber = NumberData.GetNumber(1000, 100000).ToString(),
                    Email = NetworkData.GetEmail(),
                    SubscriberType = "Kurumsal",
                    IsActive = true


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



        [HttpGet]

        public IActionResult TerminateCorporate()
        {

            return View();
        }

        [HttpPost]
        public IActionResult TerminateCorporate(string taxIdNumber)
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


        [HttpGet]

        public IActionResult TerminateIndividual()
        {

            return View();
        }

        [HttpPost]
        public IActionResult TerminateIndividual(string IdentityNumber)
        {
            // Girilen TC numarasına sahip aktif aboneleri bulma işlemi
            List<SubsIndividual> aboneler = appDbContext.Individuals.Where(a => a.IdentityNumber == IdentityNumber && a.IsActive).ToList();

            return View(aboneler);
        }

        [HttpPost]
        public IActionResult TerminateSubsIndividual(string IdentityNumber)
        {
            // Girilen TC numarasına sahip aboneleri bulma işlemi
            List<SubsIndividual> aboneler = appDbContext.Individuals
                .Where(a => a.IdentityNumber == IdentityNumber && a.IsActive)
                .ToList();


            if (aboneler.Count == 0)
            {
                TempData["Message"] = "Girilen TC numarasına sahip aktif abone bulunamadı.";
                return RedirectToAction("Index");
            }

            foreach (var abone in aboneler)
            {
                // Abonenin ödenmemiş faturalarını kontrol et
                bool hasUnpaidInvoices = appDbContext.Invoices
                    .Any(i => i.SubscriberId == abone.Id && !i.IsPaid);

                if (hasUnpaidInvoices)
                {
                    toast.AddErrorToastMessage("Ödenmemiş Faturası Bulunduğu İçin Aboneliği Sonlandıramazsınız.");
                    TempData["Message"] = "Bazı abonelerin ödenmemiş faturası bulunduğu için aboneliklerini sonlandıramazsınız.";
                    return View("TerminateIndividual", aboneler);


                }
                else
                {
                    // Abonenin ödenmemiş faturası yoksa
                    abone.IsActive = false; // Aboneliği pasif hale getir
                    appDbContext.Update(abone);
                    appDbContext.SaveChanges();
                    toast.AddSuccessToastMessage("Abonelik Başarıyla Sonlandırıldı.");
                }
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public IActionResult TerminateIndividual(string IdentityNumber)
        //{
        //    // Girilen TC numarasına sahip aboneleri bulma işlemi
        //    List<SubsIndividual> aboneler = appDbContext.Individuals.Where(a => a.IdentityNumber == IdentityNumber).ToList();

        //    if (aboneler.Count == 0)
        //    {
        //        TempData["Message"] = "Girilen TC numarasına sahip abone bulunamadı.";
        //        return RedirectToAction("Index");
        //    }

        //    foreach (var abone in aboneler)
        //    {
        //        // Abonenin ödenmemiş faturalarını kontrol et
        //        bool hasUnpaidInvoices = appDbContext.Invoices
        //            .Any(i => i.SubscriberId == abone.Id && !i.IsPaid);

        //        if (hasUnpaidInvoices)
        //        {
        //            toast.AddErrorToastMessage("Ödenmemiş Faturası Bulunduğu İçin Aboneliği Sonlandıramazsınız.");

        //            // Abonenin ödenmemiş faturası varsa
        //            TempData["Message"] = "Bazı abonelerin ödenmemiş faturası bulunduğu için aboneliklerini sonlandıramazsınız.";
        //            return View(abone);
        //        }
        //        else
        //        {
        //            // Abonenin ödenmemiş faturası yoksa
        //            abone.IsActive = false; // Aboneliği pasif hale getir


        //            appDbContext.Update(abone);


        //            appDbContext.SaveChanges(); //
        //            toast.AddSuccessToastMessage("Abonelik Başarıyla Sonlandırıldı.");
        //        }
        //    }

        //    // Tüm abonelerin listesini döndür
        //    return View(aboneler);
        //}




    }
}


