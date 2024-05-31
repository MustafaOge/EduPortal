using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Services;
using EduPortal.Domain.Entities;
using System.ComponentModel;

namespace EduPortal.Tests
{
    public class AddressServiceTests
    {
        [Fact]
        public async Task GetAddressValue_ShouldReturnAddressData()
        {
            // Arrange
            var mockRepository = new Mock<IAddressRepository>();
            var expectedAddressData = new List<object>() { /* mocked address data */ };
            mockRepository.Setup(repo => repo.GetAddressData()).ReturnsAsync(expectedAddressData);

            var addressService = new AddressService(mockRepository.Object);

            // Act
            var result = await addressService.GetAddressValue();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAddressData, result);
        }

        [Fact]
        public async Task GetCounter_ShouldReturnCounterNumber()
        {
            // Arrange
            var doorNumber = "123"; // doorNumber bir dize olarak tanımlanmalıdır
            var expectedCounterNumber = "456";

            var mockRepository = new Mock<IAddressRepository>();
            var counterObject = new Ad_Sayac { counterNumber = Convert.ToInt32(expectedCounterNumber) }; // assuming the structure of the returned object
            mockRepository.Setup(repo => repo.GetCounterNumber(doorNumber)).ReturnsAsync(counterObject);

            var addressService = new AddressService(mockRepository.Object);

            // Act
            var result = await addressService.GetCounter(doorNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCounterNumber, result);
        }


    }
}