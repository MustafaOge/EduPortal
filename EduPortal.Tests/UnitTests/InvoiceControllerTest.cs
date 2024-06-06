using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Controllers;
using EduPortal.MVC.Models.ViewModel.YourProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Tests.UnitTests
{
    public class InvoiceControllerTest
    {
        private readonly Mock<IInvoiceService> _mockInvoiceService;
        private readonly InvoiceController _controller;

        public InvoiceControllerTest()
        {
            _mockInvoiceService = new Mock<IInvoiceService>();
            _controller = new InvoiceController(_mockInvoiceService.Object);
        }

        [Fact]
        public void Find_ExecuteAction_ReturnView()
        {
            var result = _controller.Find();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Paying_an_invoice_by_id_should_redirect_to_subscriber_index(int invoiceId)
        {
            // Arrange
            var response = Response<int>.Success(invoiceId, HttpStatusCode.OK);
            _mockInvoiceService.Setup(service => service.PayInvoice(invoiceId))
                               .ReturnsAsync(response);

            // Act
            var result = await _controller.PayInvoice(invoiceId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Subscriber", redirectResult.ControllerName);
        }

        [Fact]
        public void PaymentIndividual_ExecuteAction_ReturnView()
        {
            var result = _controller.PaymentIndividual();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData("1234567")]
        public async Task Kimlik_SayacNo_ile_faturası_ödenecek_bireysel_abone_bilgileri_getir(string counterOrIdentityNumber)
        {
            // Arrange
            SubsIndividual subsIndividual = new SubsIndividual();
            subsIndividual.SubscriberType = "Bireysel";
            subsIndividual.IdentityNumber = "11223344556";
            subsIndividual.CounterNumber = "1234567";
            subsIndividual.PhoneNumber = "05555432121";
            subsIndividual.NameSurname = "Bob Marley";
            subsIndividual.Email = "bob@gmail.com";
            subsIndividual.IsActive = true;
            subsIndividual.BirthDate = new DateTime(1990, 5, 15);

            var response = Response<SubsIndividual>.Success(subsIndividual, HttpStatusCode.OK);
            _mockInvoiceService.Setup(service => service.PaymentIndividual(counterOrIdentityNumber)).ReturnsAsync(response);

            // Act
            var result = await _controller.PaymentIndividual(counterOrIdentityNumber);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SubsIndividual>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Equal(subsIndividual.CounterNumber, model.CounterNumber);
        }

        [Fact]
        public void PaymentCorporate_ExecuteAction_ReturnView()
        {
            var result = _controller.PaymentCorporate();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData("1234567")]
        public async Task Vergi_SayacNo_ile_faturası_ödenecek_kurumsal_abone_bilgileri_getir(string counterOrTaxIdNumber)
        {
            // Arrange
            SubsCorporate subsCorporate = new SubsCorporate();
            subsCorporate.SubscriberType = "Bireysel";
            subsCorporate.TaxIdNumber = "1122334456";
            subsCorporate.CounterNumber = "1234567";
            subsCorporate.PhoneNumber = "05555432121";
            subsCorporate.CorporateName = "Bob Rent a Car";
            subsCorporate.Email = "bob@gmail.com";
            subsCorporate.IsActive = true;


            var response = Response<SubsCorporate>.Success(subsCorporate, HttpStatusCode.OK);
            _mockInvoiceService.Setup(service => service.PaymentCorporate(counterOrTaxIdNumber)).ReturnsAsync(response);

            // Act
            var result = await _controller.PaymentCorporate(counterOrTaxIdNumber);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SubsCorporate>(viewResult.Model);
            Assert.NotNull(model); 
            Assert.Equal(subsCorporate.CounterNumber, model.CounterNumber);
        }


        [Theory]
        [InlineData(1)]
        public async Task InvoiceList_ShouldReturnInvoicesForGivenId(int subscriberId)
        {
            // Arrange
            var expectedInvoices = new List<Invoice>
        {
            new Invoice { Id = 1, Amount = 100, SubscriberId = subscriberId },
            new Invoice { Id = 2, Amount = 200, SubscriberId = subscriberId }
        };
            var response = Response<List<Invoice>>.Success(expectedInvoices, HttpStatusCode.OK);

            _mockInvoiceService.Setup(service => service.GetInvoiceDetail(subscriberId))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.InvoiceList(subscriberId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Invoice>>(viewResult.Model);
            Assert.Equal(expectedInvoices.Count, model.Count);
            Assert.Equal(expectedInvoices.First().Amount, model.First().Amount);
        }

        [Theory]
        [InlineData(1)]
        public async Task DetailPay_ShouldReturnInvoiceDetailForGivenId(int incoiceId)
        {
            // Arrange
            var invoice = new Invoice { Id = incoiceId, Amount = 100, SubscriberId = 1 };
            var subscriber = new Subscriber { Id = 1, CounterNumber = "1234567" };
            var meterReadings = new List<MeterReading>
        {
            new MeterReading(new DateTime(2023, 1, 1)) { Id = 1, InvoiceId = incoiceId, TotalIndex = 150 },
            new MeterReading(new DateTime(2023, 2, 1)) { Id = 2, InvoiceId = incoiceId, TotalIndex = 200 }
        };


            var invoiceDetailView = new InvoiceDetailView
            {
                Invoice = invoice,
                Subscriber = subscriber,
                MeterReadings = meterReadings
            };
            var response = Response<InvoiceDetailView>.Success(invoiceDetailView, HttpStatusCode.OK);

            _mockInvoiceService.Setup(service => service.DetailPay(incoiceId)).ReturnsAsync(response);

            // Act
            var result = await _controller.DetailPay(incoiceId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<InvoiceDetailView>(viewResult.Model);
            Assert.Equal(invoice.Id, model.Invoice.Id);
            Assert.Equal(subscriber.CounterNumber, model.Subscriber.CounterNumber);
            Assert.Equal(meterReadings.Count, model.MeterReadings.Count);
        }

        [Fact]
        public void Complaint_ExecuteAction_ReturnView()
        {
            var result = _controller.Complaint();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Complaint_ShouldRedirectToIndex_WhenComplaintIsSuccessful()
        {
            // Arrange
            var invoiceComplaint = new InvoiceComplaint
            {
                InvoiceId = 1,
                Title = "Incorrect Amount",
                Reason = "The amount billed is incorrect",
                CreatedAt = DateTime.Now
            };

            var response = Response<InvoiceComplaint>.Success(invoiceComplaint, HttpStatusCode.OK);
            _mockInvoiceService.Setup(service => service.CreateComplaint(invoiceComplaint)).ReturnsAsync(response);

            // Act
            var result = await _controller.Complaint(invoiceComplaint);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Complaint_ShouldReturnView_WhenComplaintFails()
        {
            // Arrange
            var invoiceComplaint = new InvoiceComplaint
            {
                InvoiceId = 1,
                Title = "Incorrect Amount",
                Reason = "The amount billed is incorrect",
                CreatedAt = DateTime.Now
            };

            var response = Response<InvoiceComplaint>.Fail("Error", HttpStatusCode.BadRequest);
            _mockInvoiceService.Setup(service => service.CreateComplaint(invoiceComplaint)).ReturnsAsync(response);

            // Act
            var result = await _controller.Complaint(invoiceComplaint);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Response<InvoiceComplaint>>(viewResult.Model);
            Assert.Equal(response, model);
        }

    }


}
