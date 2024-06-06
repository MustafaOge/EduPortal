using EduPortal.Domain.Entities;
using EduPortal.Persistence.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace EduPortal.Tests.IntegrationTests
{
    public class SubsCorporateRepositoryTests : IntegrationTestBase
    {
        private readonly SubsCorporateRepository _repository;

        public SubsCorporateRepositoryTests()
            : base()
        {
            _repository = new SubsCorporateRepository(_context, new GenericRepository<SubsCorporate, int>(_context));
        }

        protected override async Task SeedDatabase()
        {
            var corporates = new[]
            {
                new SubsCorporate
                {
                    CounterNumber = "12345",
                    TaxIdNumber = "1234567891",
                    IsActive = true,
                    CorporateName = "John Doe",
                    PhoneNumber = "555-1234",
                    SubscriberType = "Residential",
                    Email = "john.doe@example.com"
                },
                new SubsCorporate
                {
                    CounterNumber = "54321",
                    TaxIdNumber = "0123456789",
                    IsActive = false,
                    CorporateName = "Jane Doe",
                    PhoneNumber = "555-5678",
                    SubscriberType = "Business",
                    Email = "jane.doe@example.com"
                },
                new SubsCorporate
                {
                    CounterNumber = "67890",
                    TaxIdNumber = "123456789",
                    IsActive = true,
                    CorporateName = "Alice Smith",
                    PhoneNumber = "555-8765",
                    SubscriberType = "Commercial",
                    Email = "alice.smith@example.com"
                }
            };

            await _context.Corprorates.AddRangeAsync(corporates);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task FindCorporateAsync_ShouldReturnCorporate_WhenCounterNumberIsMatched()
        {
            // Arrange
            var counterNumber = "12345";

            // Act
            var result = await _repository.FindCorporateAsync(counterNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(counterNumber, result.CounterNumber);
        }

        [Fact]
        public async Task FindCorporateAsync_ShouldReturnCorporate_WhenTaxIdNumberIsMatched()
        {
            // Arrange
            var taxIdNumber = "1234567891";

            // Act
            var result = await _repository.FindCorporateAsync(taxIdNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taxIdNumber, result.TaxIdNumber);
        }

        [Fact]
        public async Task FindCorporateAsync_ShouldReturnNull_WhenCorporateIsNotActive()
        {
            // Arrange
            var counterNumber = "54321";

            // Act
            var result = await _repository.FindCorporateAsync(counterNumber);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task FindCorporateAsync_ShouldReturnNull_WhenNoMatchingCorporate()
        {
            // Arrange
            var counterNumber = "00000";

            // Act
            var result = await _repository.FindCorporateAsync(counterNumber);

            // Assert
            Assert.Null(result);
        }
    }
}
