using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Domain.Entities;
using EduPortal.Service.Services;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace EduPortal.Tests.UnitTests
{
    public class SubscriberServiceTests
    {
        private readonly Mock<ISubsCorporateRepository> _mockSubsCorporateRepository;
        private readonly Mock<ISubsIndividualRepository> _mockSubsIndivualRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ISubscriberRepository> _mockSubscriberRepository;
        private readonly Mock<ISubscriberTerminateService> _mockSubscriberTerminateService;
        private readonly Mock<IAddressService> _mockAddressService;

        public SubscriberServiceTests()
        {
            _mockSubsCorporateRepository = new Mock<ISubsCorporateRepository>();
            _mockSubsIndivualRepository = new Mock<ISubsIndividualRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSubscriberRepository = new Mock<ISubscriberRepository>();
            _mockSubscriberTerminateService = new Mock<ISubscriberTerminateService>();
            _mockAddressService = new Mock<IAddressService>();

            SetUp();
        }

        private void SetUp()
        {
            _mockMapper.Setup(m => m.Map<SubsCorporate>(It.IsAny<CreateCorporateDto>())).Returns(new SubsCorporate());
            _mockMapper.Setup(m => m.Map<SubsCorporateDto>(It.IsAny<SubsCorporate>())).Returns(new SubsCorporateDto());
            _mockMapper.Setup(m => m.Map<SubsIndividual>(It.IsAny<CreateIndividualDto>())).Returns(new SubsIndividual());
            _mockMapper.Setup(m => m.Map<SubsIndividualDto>(It.IsAny<SubsIndividual>())).Returns(new SubsIndividualDto());
        }

        private SubsCorporateService CreateCorporateService()
        {
            return new SubsCorporateService(
                _mockUnitOfWork.Object,
                _mockSubsCorporateRepository.Object,
                _mockSubscriberRepository.Object,
                _mockMapper.Object
            );
        }

        private SubsIndividualService CreateIndividualService()
        {
            return new SubsIndividualService(
                _mockUnitOfWork.Object,
                _mockSubsIndivualRepository.Object,
                _mockSubscriberRepository.Object,
                _mockSubscriberTerminateService.Object,
                _mockAddressService.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Should_Return_Success_Response_When_Creating_Corporate_With_Valid_Input()
        {
            // Arrange
            var service = CreateCorporateService();

            _mockSubsCorporateRepository.Setup(r => r.AddAsync(It.IsAny<SubsCorporate>())).Verifiable();
            _mockUnitOfWork.Setup(u => u.CommitAsync()).Verifiable();

            // Act
            var response = await service.CreateCorporateAsync(new CreateCorporateDto());

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotNull(response.Data);

            _mockSubsCorporateRepository.Verify();
            _mockUnitOfWork.Verify();
        }

        [Fact]
        public async Task Should_Return_Subscriber_Dto_When_Finding_Corporate_With_Existing_TaxIdNumber()
        {
            // Arrange
            var taxIdNumber = "1234567890";
            var subsCorporateEntity = new SubsCorporate();
            _mockSubsCorporateRepository.Setup(r => r.FindCorporateAsync(taxIdNumber)).ReturnsAsync(subsCorporateEntity);

            var service = CreateCorporateService();

            // Act
            var result = await service.FindCorporateAsync(taxIdNumber);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SubsCorporateDto>(result);
        }

        [Fact]
        public async Task Should_Return_Success_Response_When_Terminating_Subscriber_With_Existing_TaxIdNumber_And_No_Unpaid_Invoices()
        {
            // Arrange
            var taxIdNumber = "existingTaxIdNumber";
            var subsCorporateEntity = new SubsCorporate { IsActive = true, Id = 1 };

            _mockSubsCorporateRepository.Setup(r => r.FindCorporateAsync(taxIdNumber)).ReturnsAsync(subsCorporateEntity);
            _mockSubscriberRepository.Setup(r => r.HasUnpaidInvoices(subsCorporateEntity.Id)).ReturnsAsync(false);
            _mockUnitOfWork.Setup(u => u.CommitAsync()).Verifiable();

            var service = CreateCorporateService();

            // Act
            var result = await service.TerminateSubsCorporateAsync(taxIdNumber);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Data);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            _mockUnitOfWork.Verify();
        }

        [Fact]
        public async Task CreateIndividualAsync_ShouldReturnSuccessResponse_WhenCounterNumberIsPresent()
        {
            // Arrange
            var individualCreateDto = new CreateIndividualDto { CounterNumber = "12345" };
            var individualEntity = new SubsIndividual { CounterNumber = "12345" };
            var individualDto = new SubsIndividualDto { CounterNumber = "12345" };

            _mockMapper.Setup(m => m.Map<SubsIndividual>(individualCreateDto)).Returns(individualEntity);
            _mockMapper.Setup(m => m.Map<SubsIndividualDto>(individualEntity)).Returns(individualDto);
            _mockSubsIndivualRepository.Setup(r => r.AddAsync(individualEntity)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            var service = CreateIndividualService();

            // Act
            var response = await service.CreateIndividualAsync(individualCreateDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(individualDto, response.Data);
        }

        [Fact]
        public async Task CreateIndividualAsync_ShouldReturnBadRequest_WhenCounterNumberIsMissing()
        {
            // Arrange
            var individualCreateDto = new CreateIndividualDto { CounterNumber = null, InternalDoorNumber = null };
            var service = CreateIndividualService();

            // Act
            var response = await service.CreateIndividualAsync(individualCreateDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Sayaç numarası bulunamadı veya sağlanmadı.", response.Errors.First());
        }

        [Theory]
        [InlineData(null)]
        public async Task FindIndividualDtoAsync_IndivudalSubscriberNotFound_returnNull(string IdentityNumber)
        {
            SubsIndividual subsIndividual = null;
            var service = CreateIndividualService();


            _mockSubsIndivualRepository.Setup(repo => repo.FindIndividualAsync(IdentityNumber)).ReturnsAsync(subsIndividual);

            var result = service.FindIndividualDtoAsync(IdentityNumber);

            Assert.Null(result.Result);
        }

        [Theory]
        [InlineData("1")]
        public async Task FindIndividualDtoAsync_FindIndivualSubsrberFound_returnIndivudalDto(string IdentityNumber)
        {
            SubsIndividual subsIndividual = new SubsIndividual();
            var service = CreateIndividualService();


            _mockSubsIndivualRepository.Setup(repo => repo.FindIndividualAsync(IdentityNumber)).ReturnsAsync(subsIndividual);

            var result = service.FindIndividualDtoAsync(IdentityNumber);

            Assert.NotNull(result.Result);
            Assert.IsAssignableFrom<SubsIndividualDto>(result.Result);
        }

        [Fact]
        public async Task TerminateSubsIndividualAsync_Should_Return_NotFound_When_Subscriber_Not_Found()
        {
            // Arrange
            var service = new SubsIndividualService(
                _mockUnitOfWork.Object,
                _mockSubsIndivualRepository.Object,
                _mockSubscriberRepository.Object,
                _mockSubscriberTerminateService.Object,
                null, null
            );

            _mockSubsIndivualRepository.Setup(r => r.FindIndividualAsync(It.IsAny<string>())).ReturnsAsync((SubsIndividual)null);

            // Act
            var response = await service.TerminateSubsIndividualAsync("nonexistent_identity_number");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Abone bulunamadı.", response.Errors[0]);
        }

        [Fact]
        public async Task TerminateSubsIndividualAsync_Should_Return_Forbidden_When_Unpaid_Invoices_Exist()
        {
            // Arrange
            var service = new SubsIndividualService(
                _mockUnitOfWork.Object,
                _mockSubsIndivualRepository.Object,
                _mockSubscriberRepository.Object,
                _mockSubscriberTerminateService.Object,
                null, null
            );

            _mockSubsIndivualRepository.Setup(r => r.FindIndividualAsync(It.IsAny<string>())).ReturnsAsync(new SubsIndividual());
            _mockSubscriberRepository.Setup(r => r.HasUnpaidInvoices(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var response = await service.TerminateSubsIndividualAsync("existing_identity_number");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal("Ödenmemiş faturası bulunduğu için abonelik sonlandırılamadı.", response.Errors[0]);
        }

        [Fact]
        public async Task TerminateSubsIndividualAsync_Should_Return_Success_When_No_Unpaid_Invoices_And_Subscriber_Found()
        {
            // Arrange
            var service = new SubsIndividualService(
                _mockUnitOfWork.Object,
                _mockSubsIndivualRepository.Object,
                _mockSubscriberRepository.Object,
                _mockSubscriberTerminateService.Object,
                null, null
            );

            _mockSubsIndivualRepository.Setup(r => r.FindIndividualAsync(It.IsAny<string>())).ReturnsAsync(new SubsIndividual());
            _mockSubscriberRepository.Setup(r => r.HasUnpaidInvoices(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var response = await service.TerminateSubsIndividualAsync("existing_identity_number");

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Data);
        }
    }
}
