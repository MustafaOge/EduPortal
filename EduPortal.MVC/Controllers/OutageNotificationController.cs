using EduPortal.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EduPortal.MVC.Controllers
{
    public class OutageNotificationController : Controller
    {
        private readonly IOutageNotificationService _outageNotificationService;

        public OutageNotificationController(IOutageNotificationService outageNotificationService)
        {
            _outageNotificationService = outageNotificationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetDistricts()
        {
            var districts = _outageNotificationService.GetDistricts();
            return Json(districts);
        }

        [HttpGet]
        public IActionResult ListOutages(string selectedDate, string district)
        {
            if (!DateTime.TryParse(selectedDate, out DateTime date))
            {
                return BadRequest("Geçersiz tarih formatı.");
            }

            var outages = _outageNotificationService.GetOutagesByDateAndDistrict(date, district);
            return Json(outages);
        }
    }
}
