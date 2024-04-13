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


namespace EduPortal.Controllers
{
    public class SubscriberController(
        IToastNotification toast,
        IFakeDataService fakeDataService,
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
        public async Task<IActionResult> CreateCorporate(CreateCorporateDto corporate)
        {
            if (!ModelState.IsValid) return View("Create");
            try
            {
                await subsCorporateService.CreateCorporateAsync(corporate);
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

        public IActionResult CreateFakeData()
        {
            fakeDataService.CreateFakeSubsIndividualData();
            return RedirectToAction("Index");
        }

        public IActionResult FindIndividual()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindIndividual(string IdentityNumber)
        {
            List<SubsIndividualDto> entities = await subsIndividualService.FindIndividualDtosAsync(IdentityNumber);

            return View(entities);
        }

        [HttpGet]

        public IActionResult FindCorporate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindCorporate(string taxIdNumber)
        {
            List<SubsCorporateDto> entities = await subsCorporateService.FindCorporateAsync(taxIdNumber);

            return View(entities);
        }

        [HttpGet]

        public IActionResult TerminateCorporate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TerminateCorporate(string taxIdNumber)
        {
            List<SubsCorporateDto> corporate = await subsCorporateService.FindCorporateAsync(taxIdNumber);
            return View(corporate);
        }


        [HttpGet]

        public IActionResult TerminateIndividual()
        {

            return View();
        }
        //To-Do
        [HttpPost]
        public async Task<IActionResult> TerminateIndividual(string identityNumber)
        {
            List<SubsIndividualDto> indivudal = await subsIndividualService.FindIndividualDtosAsync(identityNumber);
            return View(indivudal);
        }


        [HttpPost]
        public async Task<IActionResult> TerminateSubsIndividual(string IdentityNumber)
        {
            var response = await subsIndividualService.TerminateSubsIndividualAsync(IdentityNumber);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                TempData["Message"] = "Abonelik sonlandırma işlemi başarısız oldu: " + response.Errors?[0];
                toast.AddSuccessToastMessage("Ödenmemiş faturası olduğu için abonelik sonladırılamadı");

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

    }
}


