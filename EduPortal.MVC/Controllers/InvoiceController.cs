using EduPortal.Domain.Entities;
using EduPortal.MVC.Models.ViewModel.YourProject.ViewModels;
using EduPortal.Persistence.context;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Linq;

namespace EduPortal.MVC.Controllers
{
    public class InvoiceController
        (AppDbContext appDbContext,
        IToastNotification toast

        )
        : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateFakeData()
        {
            for (int i = 40; i < 45; i++)
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
                Invoice invoice = new()
                {
                    IsPaid = BooleanData.GetBoolean(),
                    PaymentDate = DateTimeData.GetDatetime(new(2023, 8, 10), DateTime.Now),
                    ReadingDate = DateTimeData.GetDatetime(new(2023, 2, 10), new(2023, 7, 10)),
                    DueDate = DateTimeData.GetDatetime(new(2023, 9, 10), new(2023, 12, 10)),
                    Date = DateTime.Now,
                    SubscriberId = NumberData.GetNumber(7, 15),
                    SubscriberType = "Bireysel",
                    MeterReading = meterReading,
                    Amount = (meterReading.TotalDifference) * 1.45m
                };

                appDbContext.Invoices.Add(invoice);
            }

            appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public IActionResult PayInvoice(int id)
        //{
        //    try
        //    {
        //        var invoice = appDbContext.Invoices.FirstOrDefault(i => i.Id == id);

        //        if (invoice != null)
        //        {
        //            invoice.IsPaid = true;
        //            invoice.PaymentDate = DateTime.Now;

        //            appDbContext.SaveChanges(); // Değişiklikleri kaydet
        //        }

        //        return RedirectToAction("Success"); // Başarılı ödeme sayfasına yönlendir
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Ödeme işlemi sırasında bir hata oluştu: " + ex.Message);
        //    }
        //}


        [HttpPost]
        public IActionResult PayInvoice(int id)
        {
            try
            {
                var invoice = appDbContext.Invoices.FirstOrDefault(i => i.Id == id);

                if (invoice != null)

                {
                    if (invoice.IsPaid == true)
                    {
                        toast.AddErrorToastMessage("Fatura Zaten Ödenmiş");

                        return View($"Index");
                    }

                    invoice.IsPaid = true;

                    invoice.PaymentDate = DateTime.Now;

                    appDbContext.Update(invoice);


                    appDbContext.SaveChanges(); // Değişiklikleri kaydet
                }

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                return BadRequest("Ödeme işlemi sırasında bir hata oluştu: " + ex.Message);
            }
        }

        public IActionResult Success()
        {
            // Başarılı ödeme sayfasına yönlendir
            return RedirectToAction("Index");
        }



        public IActionResult Payment()
        {
            return View();
        }



        public IActionResult PaymentIndividual()
        {

            return View();
        }

        [HttpPost]
        public IActionResult PaymentIndividual(string IdentityNumber)
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

        public IActionResult PaymentCorporate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PaymentCorporate(string taxIdNumber)
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



        public IActionResult Invoices(int id)
        {
            var subsInvoices = appDbContext.Invoices.Where(i => i.SubscriberId == id).ToList();
            return View(subsInvoices);
        }

        public IActionResult Detail(int id)
        {
            var invoice = appDbContext.Invoices.FirstOrDefault(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            var subscriber = GetSubscriber(invoice);

            var viewModel = new InvoiceDetailViewModel
            {
                Invoice = invoice,
                Subscriber = subscriber,
                MeterReadings = appDbContext.MeterReadings.Where(m => m.InvoiceId == id).ToList()
            };

            return View(viewModel);
        }

        public Subscriber GetSubscriber(Invoice invoice)
        {
            if (invoice.Subscriber != null)
            {
                return invoice.Subscriber;
            }
            else if (invoice.SubscriberId != null)
            {
                return appDbContext.Subscribers.FirstOrDefault(s => s.Id == invoice.SubscriberId);
            }
            else
            {
                return null;
            }
        }
    }
}

