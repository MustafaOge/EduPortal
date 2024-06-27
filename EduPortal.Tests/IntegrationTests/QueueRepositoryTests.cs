using EduPortal.Domain.Entities;
using EduPortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EduPortal.Tests.IntegrationTests
{
    public class QueueRepositoryTests : IntegrationTestBase
    {
        private readonly QueueRepository _repository;

        public QueueRepositoryTests() : base()
        {
            _repository = new QueueRepository(_context);
        }

        protected override async Task SeedDatabase()
        {
            if (!await _context.Invoices.AnyAsync())
            {
                // Veritabanına örnek aboneleri ekleyelim
                var subscribers = new[]
                {
                    new Subscriber { SubscriberType = "Residential", PhoneNumber = "1234567890", CounterNumber = "C123", Email = "sub1@example.com", IsActive = true },
                    new Subscriber { SubscriberType = "Commercial", PhoneNumber = "0987654321", CounterNumber = "C124", Email = "sub2@example.com", IsActive = true },
                    new Subscriber { SubscriberType = "Residential", PhoneNumber = "1122334455", CounterNumber = "C125", Email = "sub3@example.com", IsActive = true },
                    new Subscriber { SubscriberType = "Industrial", PhoneNumber = "2233445566", CounterNumber = "C126", Email = "sub4@example.com", IsActive = true }
                };

                _context.Subscribers.AddRange(subscribers);
                await _context.SaveChangesAsync();

                // Abone ID'lerini alalım ve faturaları ekleyelim
                var subscriber1Id = subscribers[0].Id;
                var subscriber2Id = subscribers[1].Id;

                var invoices = new[]
                {
                    new Invoice { SubscriberId = subscriber1Id, IsPaid = false, DueDate = DateTime.Today.AddDays(-50), Amount = 100, SubscriberType = "Residential", ReadingDate = DateTime.Today.AddDays(-60), Date = DateTime.Today.AddDays(-45) },
                    new Invoice { SubscriberId = subscriber2Id, IsPaid = false, DueDate = DateTime.Today.AddDays(-46), Amount = 150, SubscriberType = "Commercial", ReadingDate = DateTime.Today.AddDays(-70), Date = DateTime.Today.AddDays(-55) },
                    new Invoice { SubscriberId = subscriber1Id, IsPaid = true, DueDate = DateTime.Today.AddDays(-40), Amount = 200, SubscriberType = "Residential", ReadingDate = DateTime.Today.AddDays(-65), Date = DateTime.Today.AddDays(-50) },
                    new Invoice { SubscriberId = subscriber1Id, IsPaid = false, DueDate = DateTime.Today.AddDays(-20), Amount = 250, SubscriberType = "Industrial", ReadingDate = DateTime.Today.AddDays(-75), Date = DateTime.Today.AddDays(-35) },
                };

                _context.Invoices.AddRange(invoices);
                await _context.SaveChangesAsync();
            }
        }

        //[Fact]
        //public async Task GetUnpaidInvoices_ReturnsUnpaidInvoicesBeforeDueDate()
        //{
        //    // Act
        //    var result = await _repository.GetUnpaidInvoices();

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count());

        //    // Doğru faturaların dönüp dönmediğini doğrulayın
        //    Assert.Contains(result, r => r.Id == 1 && r.SubscriberId == 1);
        //    Assert.Contains(result, r => r.Id == 2 && r.SubscriberId == 2);
        //}
    }
}
