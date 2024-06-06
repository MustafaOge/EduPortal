using EduPortal.Domain.Entities;
using EduPortal.Persistence;
using EduPortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EduPortal.Tests.IntegrationTests
{
    public class AddressRepositoryTests : IntegrationTestBase
    {
        private readonly AddressRepository _repository;

        public AddressRepositoryTests()
            : base()
        {
            _repository = new AddressRepository(_context);
        }

        protected override async Task SeedDatabase()
        {
            // Veritabanına örnek veriler ekleyelim
            if (!await _context.Ad_Ilce.AnyAsync())
            {
                var newIlce1 = new Ad_Ilce { ilceKimlikNo = 1, adi = "Ilce 1" };
                var newIlce2 = new Ad_Ilce { ilceKimlikNo = 2, adi = "Ilce 2" };
                await _context.Ad_Ilce.AddRangeAsync(newIlce1, newIlce2);
                await _context.SaveChangesAsync();
            }

            var existingIlce1 = await _context.Ad_Ilce.FirstOrDefaultAsync(x => x.ilceKimlikNo == 1);
            var existingIlce2 = await _context.Ad_Ilce.FirstOrDefaultAsync(x => x.ilceKimlikNo == 2);

            // En yüksek mahalleKimlikNo değerini al
            int maxMahalleKimlikNo = await _context.Ad_Mahalle.MaxAsync(m => (int?)m.mahalleKimlikNo) ?? 0;

            var mahalle1 = new Ad_Mahalle { mahalleKimlikNo = maxMahalleKimlikNo + 1, adi = "Mahalle 1", ilceKimlikNo = existingIlce1.ilceKimlikNo };
            var mahalle2 = new Ad_Mahalle { mahalleKimlikNo = maxMahalleKimlikNo + 2, adi = "Mahalle 2", ilceKimlikNo = existingIlce2.ilceKimlikNo };

            // Mahallelerin eklenmesi
            if (!await _context.Ad_Mahalle.AnyAsync(m => m.adi == "Mahalle 1"))
                await _context.Ad_Mahalle.AddAsync(mahalle1);

            if (!await _context.Ad_Mahalle.AnyAsync(m => m.adi == "Mahalle 2"))
                await _context.Ad_Mahalle.AddAsync(mahalle2);

            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetCounterNumber_ReturnsCorrectCounter()
        {
            // Arrange
            var doorNumber = "123";
            var counterToAdd = new Ad_Sayac { icKapiKimlikNo = 123, /* diğer gerekli özellikler */ };
            _context.Ad_Sayac.Add(counterToAdd);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetCounterNumber(doorNumber);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(counterToAdd.icKapiKimlikNo, result.icKapiKimlikNo);
            // Diğer özellikleri de doğrulayabilirsiniz.
        }




    }
}
