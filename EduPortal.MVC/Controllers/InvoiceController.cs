using AutoMapper;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Models.Invoice;
using EduPortal.Persistence.context;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Linq;

namespace EduPortal.MVC.Controllers
{
    public class InvoiceController
        (AppDbContext appDbContext,
        IToastNotification toast,
        IFakeDataService fakeDataService,
        IInvoiceService invoiceService,
        IMapper mapper

        )
        : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region CreateFakeData
        public IActionResult CreateFakeData()
        {
            for (int i = 40; i < 57; i++)
            {
                decimal totalIndex = NumberData.GetNumber(1000, 5000);
                decimal dayFirstIndex = NumberData.GetNumber(0, 500); // Gündüzün ilk indeksi
                decimal dayLastIndex = NumberData.GetNumber(500, 2000); // Gündüzün son indeksi
                decimal peakFirstIndex = NumberData.GetNumber(0, 200); // Puantın ilk indeksi
                decimal peakLastIndex = NumberData.GetNumber(200, 800); // Puantın son indeksi
                decimal nightFirstIndex = NumberData.GetNumber(0, 300); // Gece nin ilk indeksi
                decimal nightLastIndex = NumberData.GetNumber(300, 1200); // Gece nin son indeksi

                var meterReading = new MeterReading(DateTimeData.GetDatetime(new DateTime(2023, 2, 10), new DateTime(2023, 7, 10)))
                {
                    ReadingDate = DateTimeData.GetDatetime(new DateTime(2023, 2, 10), new DateTime(2023, 7, 10)),

                    TotalIndex = totalIndex,
                    TotalFirstIndex = totalIndex - (dayLastIndex + peakLastIndex + nightLastIndex),
                    TotalLastIndex = totalIndex,

                    DayFirstIndex = dayFirstIndex,
                    DayLastIndex = dayLastIndex,

                    PeakFirstIndex = peakFirstIndex,
                    PeakLastIndex = peakLastIndex,

                    NightFirstIndex = nightFirstIndex,
                    NightLastIndex = nightLastIndex
                };

                // Fark değerlerini hesapla ve ayarla
                meterReading.DayDifference = meterReading.DayLastIndex - meterReading.DayFirstIndex;
                meterReading.PeakDifference = meterReading.PeakLastIndex - meterReading.PeakFirstIndex;
                meterReading.NightDifference = meterReading.NightLastIndex - meterReading.NightFirstIndex;
                meterReading.TotalDifference = meterReading.TotalLastIndex - meterReading.TotalFirstIndex;

                meterReading.ReadingDayDifference = (meterReading.LastIndexDate - meterReading.ReadingDate).Days;


                // Invoice oluştur
                EduPortal.Domain.Entities.Invoice invoice = new()
                {
                    IsPaid = BooleanData.GetBoolean(),
                    PaymentDate = DateTimeData.GetDatetime(new(2023, 8, 10), DateTime.Now),
                    ReadingDate = DateTimeData.GetDatetime(new(2023, 2, 10), new(2023, 7, 10)),
                    DueDate = DateTimeData.GetDatetime(new(2023, 9, 10), new(2023, 12, 10)),
                    Date = DateTime.Now,
                    SubscriberId = NumberData.GetNumber(1, 100),
                    SubscriberType = "Bireysel",
                    MeterReading = meterReading,
                    Amount = (meterReading.TotalDifference) * 1.45m
                };

                appDbContext.Invoices.Add(invoice);
            }

            appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion


        [HttpPost]
        public async Task<IActionResult> PayInvoice(int id)
        {
            await invoiceService.PayInvoice(id);
            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Find()
        {
            return View();
        }

        public IActionResult PaymentIndividual()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentIndividual(string identityNumber)
        {
            var response = await invoiceService.PaymentIndividual(identityNumber);
            return View(response.Data);
        }

        [HttpGet]
        public IActionResult PaymentCorporate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCorporate(string taxIdNumber)
        {
            var response = await invoiceService.PaymentCorporate(taxIdNumber);
            return View(response.Data);
        }

        public async Task<IActionResult> InvoiceList(int id)
        {
            var subsInvoices = await invoiceService.GetInvoiceDetail(id);
            return View(subsInvoices.Data);
        }


        public async Task<IActionResult> DetailPay(int id)
        {
            var invoiceDetail = await invoiceService.DetailPay(id);
            return View(invoiceDetail.Data);
        }

        [HttpGet]
        public IActionResult Complaint()
        {
            return View();
        }

        public async Task<IActionResult> Complaint(InvoiceComplaint model)
        {
            var complaintApplication = await invoiceService.CreateComplaint(model);
            if (complaintApplication.StatusCode != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(complaintApplication);
            }
        }
    }
}
