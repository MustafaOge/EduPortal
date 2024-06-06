using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Controllers;
using NToastNotify;
using Newtonsoft.Json.Linq;

namespace EduPortal.Tests.UnitTests;

public class SubscriberControllerTests
{
    private readonly Mock<IToastNotification> _mockToast;
    private readonly Mock<IFakeDataService> _mockFakeDataService;
    private readonly Mock<ISubsIndividualService> _mockSubsIndividualService;
    private readonly Mock<ISubsCorporateService> _mockSubsCorporateService;
    private readonly Mock<IAddressService> _mockAddressService;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly Mock<IMailService> _mockMailService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SubscriberController _controller;

    private List<CreateCorporateDto> createCorporateDtos;
    private List<CreateIndividualDto> createIndividualDtos;

    public SubscriberControllerTests()
    {
        _mockToast = new Mock<IToastNotification>();
        _mockFakeDataService = new Mock<IFakeDataService>();
        _mockSubsIndividualService = new Mock<ISubsIndividualService>();
        _mockSubsCorporateService = new Mock<ISubsCorporateService>();
        _mockAddressService = new Mock<IAddressService>();
        _mockCacheService = new Mock<ICacheService>();
        _mockMailService = new Mock<IMailService>();
        _mockMapper = new Mock<IMapper>();

        _controller = new SubscriberController(
            _mockToast.Object,
            _mockMapper.Object,
            _mockFakeDataService.Object,
            _mockSubsIndividualService.Object,
            _mockSubsCorporateService.Object,
            _mockAddressService.Object,
            _mockCacheService.Object,
            _mockMailService.Object
        );

        createCorporateDtos = new List<CreateCorporateDto>()
            {
                new CreateCorporateDto { Id = 1, CorporateName = "Corporate1", TaxIdNumber = "1234567890" },
                new CreateCorporateDto { Id = 2, CorporateName = "Corporate2", TaxIdNumber = "0987654321" }
            };

        createIndividualDtos = new List<CreateIndividualDto>()
            {
                new CreateIndividualDto { Id = 1, NameSurname = "Individual1", IdentityNumber = "12345678901", BirthDate = new DateTime(1990, 1, 1), InternalDoorNumber = "A101" },
                new CreateIndividualDto { Id = 2, NameSurname = "Individual2", IdentityNumber = "09876543210", BirthDate = new DateTime(1985, 5, 10), InternalDoorNumber = "B202" }
            };
    }

    [Fact]
    public void Index_View_Is_Returned()
    {
        var result = _controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Create_View_Is_Returned()
    {
        var result = _controller.Create();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Create_Corporate_View_Is_Returned()
    {
        var result = _controller.CreateCorporate();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Corporate_Invalid_ModelState_Returns_Create_View()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");

        var result = await _controller.CreateCorporate(new CreateCorporateDto(), default) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("Create", result?.ViewName);
    }

    [Fact]
    public async Task Create_Corporate_Valid_ModelState_Redirects_To_Index()
    {
        CreateCorporateDto createCorporateDto = null;

        _mockSubsCorporateService.Setup(service => service.CreateCorporateAsync(It.IsAny<CreateCorporateDto>())).Callback<CreateCorporateDto>(
            x => createCorporateDto = x);

        var result = await _controller.CreateCorporate(createCorporateDtos.First(), default);

        _mockSubsCorporateService.Verify(s => s.CreateCorporateAsync(It.IsAny<CreateCorporateDto>()), Times.Once);
        _mockToast.Verify(t => t.AddSuccessToastMessage("İşlem Başarılı", null), Times.Once); // Mesajı kontrol etme

        Assert.NotNull(result);
        Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        Assert.Equal(createCorporateDtos.First().TaxIdNumber, createCorporateDto.TaxIdNumber);
    }

    [Fact]
    public async Task Create_Corporate_Exception_Shows_Error_Message_And_Returns_Create_View()
    {
        var corporateDto = new CreateCorporateDto();
        var exceptionMessage = "Error occurred";

        _mockSubsCorporateService.Setup(s => s.CreateCorporateAsync(It.IsAny<CreateCorporateDto>()))
                                 .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.CreateCorporate(corporateDto, default) as ViewResult;

        _mockToast.Verify(t => t.AddErrorToastMessage($"Abone Eklenemedi: {exceptionMessage}", null), Times.Once);

        Assert.NotNull(result);
        Assert.Equal("Create", result?.ViewName);
    }

    [Fact]
    public void Create_Individual_View_Is_Returned()
    {
        var result = _controller.CreateIndividual();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Individual_Invalid_ModelState_Returns_Create_View()
    {
        _controller.ModelState.AddModelError("Error", "Invalid model state");

        var result = await _controller.CreateIndividual(new CreateIndividualDto(), default) as ViewResult;

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Create_Individual_Valid_ModelState_Redirects_To_Index()
    {
        var individualDto = new CreateIndividualDto();

        var result = await _controller.CreateIndividual(individualDto, default) as RedirectToActionResult;

        _mockSubsIndividualService.Verify(s => s.CreateIndividualAsync(individualDto), Times.Once);
        _mockToast.Verify(t => t.AddSuccessToastMessage(It.IsAny<string>(), null), Times.Once);

        Assert.NotNull(result);
        Assert.Equal("Index", result?.ActionName);
    }



    [Fact]
    public async Task Create_Individual_Exception_Shows_Error_Message_And_Returns_Create_View()
    {
        var individualDto = new CreateIndividualDto();
        var exceptionMessage = "Error occurred";

        _mockSubsIndividualService.Setup(s => s.CreateIndividualAsync(It.IsAny<CreateIndividualDto>()))
                                  .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _controller.CreateIndividual(individualDto, default) as ViewResult;

        _mockToast.Verify(t => t.AddErrorToastMessage($"Abone Eklenemedi: {exceptionMessage}", null), Times.Once);

        Assert.NotNull(result);
        Assert.Equal("Create", result?.ViewName);
    }

    [Fact]
    public void Find_View_Is_Returned()
    {
        var result = _controller.Find();

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Find_Individual_View_Is_Returned()
    {
        var result = _controller.FindIndividual();

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Find_Individual_By_IdentityOrCounterNumber_Returns_View_With_Entities()
    {
        var identityOrCounterNumber = "12345";
        var entities = new SubsIndividualDto();

        _mockCacheService.Setup(c => c.FindIndividualDtosAsync(identityOrCounterNumber))
                            .ReturnsAsync(entities);

        var result = await _controller.FindIndividual(identityOrCounterNumber) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal(entities, result?.Model);
    }


    [Fact]
    public void Find_Corporate_View_Is_Returned()
    {
        var result = _controller.FindCorporate() as ViewResult;

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Find_Corporate_By_TaxIdOrCounterNumber_Returns_View_With_Entity()
    {
        var taxIdOrCounterNumber = "67890";
        var entity = new SubsCorporateDto();
        _mockSubsCorporateService.Setup(c => c.FindCorporateAsync(taxIdOrCounterNumber)).ReturnsAsync(entity);

        var result = await _controller.FindCorporate(taxIdOrCounterNumber) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal(entity, result?.Model);
    }

    [Fact]
    public void Terminate_View_Is_Returned()
    {
        var result = _controller.Terminate();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Terminate_Corporate_View_Is_Returned()
    {
        var result = _controller.TerminateCorporate() as ViewResult;

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }


    [Fact]
    public async Task Terminate_Corporate_By_TaxIdOrCounterNumber_Returns_View_With_Entity()
    {
        var taxIdOrCounterNumber = "67890";
        var entity = new SubsCorporateDto();
        _mockSubsCorporateService.Setup(c => c.FindCorporateAsync(taxIdOrCounterNumber)).ReturnsAsync(entity);

        var result = await _controller.TerminateCorporate(taxIdOrCounterNumber) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal(entity, result?.Model);
    }


    [Fact]
    public void Terminate_Individual_View_Is_Returned()
    {
        var result = _controller.TerminateIndividual();

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Terminate_Individual_By_IdentityOrCounterNumber_Returns_View_With_Entity()
    {
        var identityOrCounterNumber = "12345";
        var entity = new SubsIndividualDto();
        _mockSubsIndividualService.Setup(c => c.FindIndividualDtoAsync(identityOrCounterNumber)).ReturnsAsync(entity);

        var result = await _controller.TerminateIndividual(identityOrCounterNumber) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal(entity, result?.Model);
    }

    [Fact]
    public async Task GetAddressData_Returns_Json_Result()
    {
        // Arrange
        var expectedData = new List<object> { new { Key = "value" } };
        _mockAddressService.Setup(service => service.GetAddressValue()).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetAddressData();

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        Assert.NotNull(jsonResult.Value);
        Assert.Equal(expectedData, jsonResult.Value);
    }


    [Fact]
    public async Task GetAddressData_Post_Returns_Json_Result_With_Input()
    {
        // Arrange

        int testIcKapiKimlikNo = 12345;

        // Act
        var result = await _controller.GetAddressData(testIcKapiKimlikNo);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var jsonData = JObject.FromObject(jsonResult.Value);

        Assert.NotNull(jsonData);
        Assert.Equal(testIcKapiKimlikNo, (int)jsonData["icKapiKimlikNo"]);
    }


    [Fact]
    public async Task AddressRegister_ExecuteView_ReturnView()
    {
        var result = _controller.AddressRegister();

        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }


    //[Fact]
    //public async Task Create_Fake_Data_Redirects_To_Index()
    //{
    //    var result = await _controller.CreateFakeData() as RedirectToActionResult;

    //    _mockFakeDataService.Verify(f => f.CreateCounterNumber(), Times.Once);

    //    Assert.NotNull(result);
    //    Assert.Equal("Index", result?.ActionName);
    //}



}
