using AutoMapper;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.Services;
using Moq;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Tests
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoiceRepository> _mockInvoiceRepository;
        private readonly Mock<IToastNotification> _mockToastNotification;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ISubsCorporateRepository> _mockSubsCorporateRepository;
        private readonly Mock<ISubsIndividualRepository> _mockSubsIndividualRepository;
        private readonly Mock<ISubscriberRepository> _mockSubscriberRepository;
        private readonly Mock<IGenericRepository<InvoiceComplaint, int>> _mockInvoiceComplaintRepository;
        private readonly Mock<IGenericRepository<MeterReading, int>> _mockMeterReadingRepository;
        private readonly IInvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            _mockInvoiceRepository = new Mock<IInvoiceRepository>();
            _mockToastNotification = new Mock<IToastNotification>();
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSubsCorporateRepository = new Mock<ISubsCorporateRepository>();
            _mockSubsIndividualRepository = new Mock<ISubsIndividualRepository>();
            _mockSubscriberRepository = new Mock<ISubscriberRepository>();
            _mockInvoiceComplaintRepository = new Mock<IGenericRepository<InvoiceComplaint, int>>();
            _mockMeterReadingRepository = new Mock<IGenericRepository<MeterReading, int>>();

            _invoiceService = new InvoiceService(
                _mockInvoiceRepository.Object,
                _mockToastNotification.Object,
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockSubsCorporateRepository.Object,
                _mockSubsIndividualRepository.Object,
                _mockSubscriberRepository.Object,
                _mockInvoiceComplaintRepository.Object,
                _mockMeterReadingRepository.Object
            );
        }
        [Fact]
        public async Task PayInvoice_ShouldReturnSuccess_WhenInvoiceIsPaidSuccessfully()
        {
            // Arrange
            var invoice = new Invoice { Id = 1, IsPaid = false };
            _mockInvoiceRepository.Setup(repo => repo.GetByIdAsync(invoice.Id)).ReturnsAsync(invoice);

            // Act
            var result = await _invoiceService.PayInvoice(invoice.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(invoice.Id, result.Data);
            _mockInvoiceRepository.Verify(repo => repo.Update(invoice), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            _mockToastNotification.Verify(toast => toast.AddSuccessToastMessage("Fatura Başarı İle Ödendi", null), Times.Once);
        }


        [Fact]
        public async Task PaymentCorporate_ShouldReturnSuccess_WhenCorporateSubscriberIsFound()
        {
            // Arrange
            var corporateSubscriber = new SubsCorporate { TaxIdNumber = "1234567890" };
            _mockSubsCorporateRepository.Setup(repo => repo.FindCorporateAsync(corporateSubscriber.TaxIdNumber)).ReturnsAsync(corporateSubscriber);

            // Act
            var result = await _invoiceService.PaymentCorporate(corporateSubscriber.TaxIdNumber);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(corporateSubscriber, result.Data);
        }

        [Fact]
        public async Task PaymentIndividual_ShouldReturnSuccess_WhenIndividualSubscriberIsFound()
        {
            // Arrange
            var individualSubscriber = new SubsIndividual { IdentityNumber = "11223344556" };
            _mockSubsIndividualRepository.Setup(repo => repo.FindIndividualAsync(individualSubscriber.IdentityNumber)).ReturnsAsync(individualSubscriber);

            // Act
            var result = await _invoiceService.PaymentIndividual(individualSubscriber.IdentityNumber);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(individualSubscriber, result.Data);
        }

        [Fact]
        public async Task CreateComplaint_ShouldReturnSuccess_WhenComplaintIsCreatedSuccessfully()
        {
            // Arrange
            var invoiceComplaint = new InvoiceComplaint { InvoiceId = 1, Title = "Title", Reason = "Reason", CreatedAt = DateTime.Now };
            var invoice = new Invoice { Id = 1 };

            _mockInvoiceRepository.Setup(repo => repo.GetByIdAsync(invoiceComplaint.InvoiceId)).ReturnsAsync(invoice);
            _mockInvoiceComplaintRepository.Setup(repo => repo.AddAsync(It.IsAny<InvoiceComplaint>())).Returns(Task.CompletedTask);

            // Act
            var result = await _invoiceService.CreateComplaint(invoiceComplaint);

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            _mockInvoiceComplaintRepository.Verify(repo => repo.AddAsync(It.IsAny<InvoiceComplaint>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
            _mockToastNotification.Verify(toast => toast.AddSuccessToastMessage("İtiraz Talebi Oluşturuldu", null   ), Times.Once);
        }

    }

}
