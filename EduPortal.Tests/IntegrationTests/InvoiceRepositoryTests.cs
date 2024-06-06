using EduPortal.Domain.Entities;
using EduPortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EduPortal.Tests.IntegrationTests
{
    public class InvoiceRepositoryTests : IntegrationTestBase
    {
        private readonly InvoiceRepository _repository;

        public InvoiceRepositoryTests()
            : base()
        {
            _repository = new InvoiceRepository(_context, new GenericRepository<SubsCorporate, int>(_context));
        }

        protected override async Task SeedDatabase()
        {
            // Veritabanını temizle
            _context.Invoices.RemoveRange(_context.Invoices);
            await _context.SaveChangesAsync();

            // Yeni verileri ekle
            var invoices = new[]
            {
                new Invoice
                {
                    Date = DateTime.Now,
                    Subscriber = new Subscriber
                    {
                        SubscriberType = "Residential",
                        PhoneNumber = "555-1234",
                        CounterNumber = "12345",
                        Email = "subscriber1@example.com",
                        IsActive = true
                    },
                    SubscriberId = 1,
                    Amount = 100m,
                    SubscriberType = "Residential",
                    ReadingDate = DateTime.Now.AddMonths(-1),
                    DueDate = DateTime.Now.AddDays(10),
                    PaymentDate = null,
                    IsPaid = false,
                    MeterReading = new MeterReading(DateTime.Now) {
                        /* gerekli alanları doldurun */ }
                },
                new Invoice
                {
                    Date = DateTime.Now,
                    Subscriber = new Subscriber
                    {
                        SubscriberType = "Business",
                        PhoneNumber = "555-5678",
                        CounterNumber = "54321",
                        Email = "subscriber2@example.com",
                        IsActive = true
                    },
                    SubscriberId = 2,
                    Amount = 200m,
                    SubscriberType = "Business",
                    ReadingDate = DateTime.Now.AddMonths(-1),
                    DueDate = DateTime.Now.AddDays(10),
                    PaymentDate = DateTime.Now,
                    IsPaid = true,
                    MeterReading = new MeterReading(DateTime.Now) { /* gerekli alanları doldurun */ }
                }
            };

            await _context.Invoices.AddRangeAsync(invoices);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task AnyInvoiceAsync_ShouldReturnTrue_WhenInvoicesExist()
        {
            // Act
            var result = await _repository.AnyInvoiceAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AnyInvoiceAsync_ShouldReturnFalse_WhenNoInvoicesExist()
        {
            // Arrange
            var invoices = _context.Invoices.ToList();

            // Detach all entities to avoid tracking conflicts
            foreach (var invoice in invoices)
            {
                _context.Entry(invoice).State = EntityState.Detached;
            }

            // Clear the ChangeTracker to ensure no entities are tracked
            _context.ChangeTracker.Clear();

            // Remove all invoices
            _context.Invoices.RemoveRange(invoices);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.AnyInvoiceAsync();

            // Assert
            Assert.False(result);
        }
    }
}
