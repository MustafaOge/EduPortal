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
using System.Net;

namespace EduPortal.MVC.Controllers
{
    public class InvoiceController(IInvoiceService invoiceService, IFakeDataService fakeDataService)
        : BaseController
    {

        public IActionResult CreateFakeData()
        {
            fakeDataService.CreateFakeInvoiceData();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize]
        [HttpGet]
        public IActionResult Find()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PayInvoice(int id)
        {
            await invoiceService.PayInvoice(id);
            return RedirectToAction("Index", "Subscriber");
        }
  
        public IActionResult PaymentIndividual()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentIndividual(string IdentityOrCounterNumber)
        {
            var response = await invoiceService.PaymentIndividual(IdentityOrCounterNumber);
            return View(response.Data);
        }

        [HttpGet]
        public IActionResult PaymentCorporate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCorporate(string TaxIdOrCounterNumber)
        {
            var response = await invoiceService.PaymentCorporate(TaxIdOrCounterNumber);
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
            if (complaintApplication.StatusCode == HttpStatusCode.OK)
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
