using EduPortal.Domain.Entities;
using EduPortal.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;


    namespace EduPortal.Tests.IntegrationTests
    {
        public class SubsIndividualRepositoryTests : IntegrationTestBase
        {
            private readonly SubsIndividualRepository _repository;

            public SubsIndividualRepositoryTests()
                : base()
            {
                _repository = new SubsIndividualRepository(_context, new GenericRepository<SubsIndividual, int>(_context));
            }

            protected override async Task SeedDatabase()
            {
                var individuals = new[]
                {
                new SubsIndividual
                {
                    CounterNumber = "12345",
                    IdentityNumber = "12345678912",
                    IsActive = true,
                    NameSurname = "John Doe",
                    PhoneNumber = "555-1234",
                    SubscriberType = "Residential",
                    Email = "john.doe@example.com"
                },
                new SubsIndividual
                {
                    CounterNumber = "54321",
                    IdentityNumber = "0123456789",
                    IsActive = false,
                    NameSurname = "Jane Doe",
                    PhoneNumber = "555-5678",
                    SubscriberType = "Business",
                    Email = "jane.doe@example.com"
                },
                new SubsIndividual
                {
                    CounterNumber = "67890",
                    IdentityNumber = "1234567890",
                    IsActive = true,
                    NameSurname = "Alice Smith",
                    PhoneNumber = "555-8765",
                    SubscriberType = "Commercial",
                    Email = "alice.smith@example.com"
                }
            };

                await _context.Individuals.AddRangeAsync(individuals);
                await _context.SaveChangesAsync();
            }

            [Fact]
            public async Task FindIndividualAsync_ShouldReturnIndividual_WhenCounterNumberIsMatched()
            {
                // Arrange
                var counterNumber = "12345";

                // Act
                var result = await _repository.FindIndividualAsync(counterNumber);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(counterNumber, result.CounterNumber);
            }

            [Fact]
            public async Task FindIndividualAsync_ShouldReturnIndividual_WhenIdentityNumberIsMatched()
            {
                // Arrange
                var identityNumber = "12345678912";

                // Act
                var result = await _repository.FindIndividualAsync(identityNumber);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(identityNumber, result.IdentityNumber);
            }

            [Fact]
            public async Task FindIndividualAsync_ShouldReturnNull_WhenIndividualIsNotActive()
            {
                // Arrange
                var counterNumber = "54321";

                // Act
                var result = await _repository.FindIndividualAsync(counterNumber);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task FindIndividualAsync_ShouldReturnNull_WhenNoMatchingIndividual()
            {
                // Arrange
                var counterNumber = "00000";

                // Act
                var result = await _repository.FindIndividualAsync(counterNumber);

                // Assert
                Assert.Null(result);
            }
        }
    }

