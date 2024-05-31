using AutoMapper;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Models.ViewModel;
using EduPortal.Persistence.context;
using EduPortal.Service.Services;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Net;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using EduPortal.MVC.Controllers;
using EduPortal.Application.Services;
using NToastNotify.Helpers;
namespace EduPortal.Controllers
{
    public class SubscriberController(
        IToastNotification toast,
        IMapper mapper,
        IFakeDataService fakeDataService,
        ISubsIndividualService subsIndividualService,
        ISubsCorporateService subsCorporateService,
        IAddressService addressService,
        ICacheService cacheService

        , IMailService mailService
        ) : BaseController
    {
        //await outboxMessageProcessor.ProcessOutboxMessagesAsync(cancellationToken);
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Terminate()
        {
            return View();
        }

        //[Authorize]
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
        public async Task<IActionResult> CreateCorporate(CreateCorporateDto corporate, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View("Create");
            try
            {
                await subsCorporateService.CreateCorporateAsync(corporate);
                toast.AddSuccessToastMessage("İşlem Başarılı");
                //outboxMessageProcessor.ProcessOutboxMessagesAsync(cancellationToken);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                toast.AddErrorToastMessage("Abone Eklenemedi: " + ex.Message);
                return View("Create");
            }
        }

        [HttpGet]
        public IActionResult CreateIndividual()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIndividual(CreateIndividualDto individual, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View("Create");
            try
            {
                await subsIndividualService.CreateIndividualAsync(individual);
                toast.AddSuccessToastMessage("İşlem Başarılı");


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
        [HttpGet]
        public async Task<IActionResult> CreateFakeData()
        {
            await fakeDataService.CreateCounterNumber();
            return RedirectToAction("Index");
        }

        public IActionResult FindIndividual()
        {

            return View();
        }

        [HttpPost]
        // [Cros]
        //  Cross Cutting Concerns
        public async Task<IActionResult> FindIndividual(string IdentityOrCounterNumber)


        {
            //List<SubsIndividualDto> entities = await subsIndividualService.FindIndividualDtosAsync(IdentityOrCounterNumber);

            //var entities = await cacheService.FindIndividualDtosAsync(IdentityOrCounterNumber);
            var entities = await cacheService.FindIndividualDtosAsync(IdentityOrCounterNumber);

            return View(entities);
        }

        //[HttpPost]
        //public async Task<IActionResult> FindIndividual(string IdentityOrCounterNumber)
        //{
        //    // Virgülle ayrılmış dizeyi böler ve her bir numarayı alır
        //    string[] numbers = IdentityOrCounterNumber.Split(',');

        //    // Boşlukları temizler ve her bir numarayı işler
        //    List<string> cleanedNumbers = numbers.Select(n => n.Trim()).ToList();

        //    List<SubsIndividualDto> allSubscribers = new List<SubsIndividualDto>();

        //    foreach (var number in cleanedNumbers)
        //    {
        //        // Her bir numara için ilk 10 aboneyi sorgular
        //        var subscribers = await appDbContext.Individuals
        //                                             .Where(x => x.CounterNumber == number)
        //                                             .Take(10)
        //                                             .ToListAsync();

        //        // Aboneleri DTO'ya dönüştürür
        //        var subsIndividualDtos = mapper.Map<List<SubsIndividualDto>>(subscribers);

        //        // Sonuçları genel listeye ekler
        //        allSubscribers.AddRange(subsIndividualDtos);
        //    }

        //    // Sonuçları görünüme aktarır
        //    return View(allSubscribers);
        //}


        //List<SubsIndividualDto> entities = await subsIndividualService.FindIndividualDtosAsync(IdentityOrCounterNumber);


        [HttpGet]

        public IActionResult FindCorporate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindCorporate(string TaxIdOrCounterNumber)
        {
           SubsCorporateDto entities = await subsCorporateService.FindCorporateAsync(TaxIdOrCounterNumber);

            return View(entities);
        }

        [HttpGet]

        public IActionResult TerminateCorporate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TerminateCorporate(string TaxIdOrCounterNumber)
        {
            SubsCorporateDto corporate = await subsCorporateService.FindCorporateAsync(TaxIdOrCounterNumber);
            return View(corporate);
        }


        [HttpGet]

        public IActionResult TerminateIndividual()
        {

            return View();
        }
        //To-Do
        [HttpPost]
        public async Task<IActionResult> TerminateIndividual(string identityOrCounterNumber)
        {
            SubsIndividualDto indivudal = await subsIndividualService.FindIndividualDtoAsync(identityOrCounterNumber);
            return View(indivudal);
        }


        [HttpPost]
        public async Task<IActionResult> TerminateSubsIndividual(string IdentityNumber)
        {
            var response = await subsIndividualService.TerminateSubsIndividualAsync(IdentityNumber);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                TempData["Message"] = "Abonelik sonlandırma işlemi başarısız oldu: " + response.Errors?[0];
                toast.AddErrorToastMessage("Ödenmemiş faturası olduğu için abonelik sonladırılamadı");

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Abonelik(ler) başarıyla sonlandırıldı.";
            toast.AddSuccessToastMessage("Abonelik başarıyla sonlandırıldı.");

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> TerminateSubsCorporate(string TaxIdNumber)
        {
            var response = await subsCorporateService.TerminateSubsCorporateAsync(TaxIdNumber);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                TempData["Message"] = "Abonelik sonlandırma işlemi başarısız oldu: " + response.Errors?[0];

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Abonelik(ler) başarıyla sonlandırıldı.";
            toast.AddSuccessToastMessage("Abonelik başarıyla sonlandırıldı.");

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> AddressRegister(int i)
        //{

        //    return RedirectToAction("Index");
        //}


        [HttpGet]
        public IActionResult AddressRegister()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressData()
        {
            var addressData = await addressService.GetAddressValue();
            return Json(addressData);
        }

        [HttpPost]
        public async Task<IActionResult> GetAddressData(int icKapiKimlikNo)
        {
            return Json(new { icKapiKimlikNo = icKapiKimlikNo });
        }




    }
}

