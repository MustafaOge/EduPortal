using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Abstractions;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using MassTransit;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{


    public class AddressRepository  :GenericRepository<Ad_IcKapi, int>, IAddressRepository
    {
        protected readonly AppDbContext _context;

        public async Task<Ad_IcKapi> GetAsync(int neighborhoodId)
        {
            return await _context.Ad_IcKapi.FirstOrDefaultAsync(a => a.mahalleKimlikNo == neighborhoodId);
        }

        public AddressRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetMahalleKimlikNoByNameAsync(string mahalleAdi)
        {
            var mahalle = await _context.Ad_Mahalle
                                        .FirstOrDefaultAsync(m => m.adi == mahalleAdi);
            return mahalle?.mahalleKimlikNo ?? 0;  // 0 veya uygun bir varsayılan değeri döndürün
        }


        public async Task<Ad_Sayac> GetCounterNumber(string doorNumber)
        {
            var counter = await _context.Ad_Sayac.FirstOrDefaultAsync(x => x.icKapiKimlikNo == long.Parse(doorNumber));
            return counter;
        }

        public async Task CreateCounterNumberAsync()
        {
            var icKapiSayisi = await _context.Ad_IcKapi.ToListAsync();

            foreach (var icKapi in icKapiSayisi)
            {
                var existingSayac = await _context.Ad_Sayac.FirstOrDefaultAsync(s => s.counterNumber == icKapi.counterNumber);

                // Eğer varlık zaten mevcutsa güncelleme yap
                if (existingSayac != null)
                {
                    existingSayac.counterNumber = GenerateMeterNumber();
                    existingSayac.active = true; // Varsayılan olarak aktif olarak işaretleyelim
                }
                else
                {

                    // Yeni bir varlık ekleyin
                    Ad_Sayac adSayac = new Ad_Sayac();
                    adSayac.counterNumber = GenerateMeterNumber();
                    adSayac.icKapiKimlikNo = icKapi.icKapiKimlikNo;
                    adSayac.active = true; // Varsayılan olarak aktif olarak işaretleyelim
                    _context.Ad_Sayac.Add(adSayac);
                }
            }

            await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }


        public int GenerateMeterNumber()
        {
            Random rnd = new Random();
            int counterNumber;
            do
            {
                counterNumber = rnd.Next(1000000, 9999999);
            } while (_context.Ad_Sayac.Any(x => x.counterNumber == counterNumber));

            return counterNumber;
        }


        public async Task<IEnumerable<object>> GetAddressData()
        {
            var ilceler = await _context.Ad_Ilce.ToListAsync();
            var mahalleler = await _context.Ad_Mahalle.ToListAsync();
            var sokaklar = await _context.Ad_Sokak.ToListAsync();
            var disKapilar = await _context.Ad_DisKapi.ToListAsync();
            var icKapilar = await _context.Ad_IcKapi.ToListAsync();

            var data = new
            {
                Ilceler = ilceler,
                Mahalleler = mahalleler,
                Sokaklar = sokaklar,
                DisKapilar = disKapilar,
                IcKapilar = icKapilar
            };

            return new List<object> { data };
        }

        public  List<Subscriber> GetSubscriberList(List<long> mahalleKimlikNumarasi)
        {
            List<Ad_IcKapi> Ad_IcKapiList = null;
            List<Subscriber> subscribers = null;


            foreach (var item in mahalleKimlikNumarasi)
            {
                Ad_IcKapiList = _context.Ad_IcKapi.Where(x => x.mahalleKimlikNo == item).ToList();

            }
            foreach (var item in Ad_IcKapiList)
            {
                subscribers = _context.Subscribers.Where(x => x.CounterNumber == item.counterNumber.ToString()).ToList();
            }
            return subscribers;
        }

     

        //public List<Ad_IcKapi> GetAllAsync()
        //{
        //    _context.Ad_IcKapi.Where(x => x.counterNumber);
        //}
    }
}
