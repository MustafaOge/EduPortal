using EduPortal.Domain.Entities;
using EduPortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EduPortal.Tests.IntegrationTests
{
    public class SubscriberRepositoryTests : IntegrationTestBase
    {
        private readonly SubscriberRepository _repository;

        public SubscriberRepositoryTests()
            : base()
        {
            _repository = new SubscriberRepository(_context);
        }

        protected override async Task SeedDatabase()
        {
            // Aboneleri ekleyelim
            var subscribers = new[]
            {
                new Subscriber { CounterNumber = "12345", SubscriberType = "bireysel", PhoneNumber = "555-1234", Email = "john.doe@example.com", IsActive = true },
                new Subscriber { CounterNumber = "54321", SubscriberType = "kurumsal", PhoneNumber = "555-5678", Email = "jane.doe@example.com", IsActive = false }
                // Daha fazla abone ekleme işlemleri yapılabilir
            };
            await _context.Subscribers.AddRangeAsync(subscribers);
            await _context.SaveChangesAsync();

            // Faturaları ekleyelim
            var invoices = new[]
            {
                new Invoice { Date = DateTime.Now, SubscriberId = subscribers[0].Id, Amount = 100, SubscriberType = "bireysel", ReadingDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(30), IsPaid = false },
                new Invoice { Date = DateTime.Now, SubscriberId = subscribers[1].Id, Amount = 200, SubscriberType = "kurumsal", ReadingDate = DateTime.Now.AddDays(-30), DueDate = DateTime.Now.AddDays(30), IsPaid = true }
                // Daha fazla fatura ekleme işlemleri yapılabilir
            };
            await _context.Invoices.AddRangeAsync(invoices);
            await _context.SaveChangesAsync();

            // İlgili diğer tablolara da gerekiyorsa veri ekleyebilirsiniz
        }

        [Fact]
        public async Task GetSubscriberInfoAsync_ShouldReturnSubscriberInfo_WhenCounterNumberIsMatched()
        {
            // Arrange
            var counterNumber = 12345;

            // Act
            var result = await _repository.GetSubscriberInfoAsync(counterNumber);

            // Assert
            Assert.NotNull(result);
            // Gerekirse, dönen SubscriberInfo nesnesinin özelliklerini doğrulayabilirsiniz.
            // Örneğin: Assert.Equal(expectedValue, result.Property);
        }

        [Fact]
        public async Task HasUnpaidInvoices_ShouldReturnTrue_WhenUnpaidInvoicesExistForSubscriber()
        {
            // Arrange
            var subscriber = await _context.Subscribers.FirstAsync(s => s.CounterNumber == "12345");

            // Act
            var result = await _repository.HasUnpaidInvoices(subscriber.Id);

            // Assert
            Assert.True(result);
        }
    }
}
