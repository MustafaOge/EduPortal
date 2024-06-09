using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Services
{
    public class CreateFakeDataService(
        IUnitOfWork unitOfWork,
        IGenericRepository<Invoice, int> genericRepository,
        ISubsIndividualRepository individualRepository,
        ISubsCorporateRepository corporateRepository,
        IAddressRepository addressRepository,
        IInvoiceRepository invoiceRepository) : IFakeDataService
    {



        //TO-DO
        public async Task CreateFakeData()
        {
       
            if (await invoiceRepository.AnyInvoiceAsync())
            {
                CreateFakeSubsIndividualData(); // Create and save individual subscriber data
                CreateFakeInvoiceData(); // Create invoice data
            }
            await unitOfWork.CommitAsync(); // Save changes
        }


        public void CreateFakeSubsIndividualData()
        {
            // Bireysel abone verilerini oluştur
            for (int i = 0; i < 400; i++)
            {
                string firstNumber = NumberData.GetNumber(10000, 99999).ToString();
                string secondIdentityNumber = NumberData.GetNumber(100000, 999999).ToString();
                string secondTaxIdNumber = NumberData.GetNumber(10000, 99999).ToString();

                string fullIdentityNumber = firstNumber + secondIdentityNumber;
                string fullTaxIdNumber = firstNumber + secondTaxIdNumber;


                SubsIndividual subsIndividual = new SubsIndividual
                {
                    PhoneNumber = PhoneNumberData.GetPhoneNumber(),
                    NameSurname = NameData.GetFullName(),
                    BirthDate = DateTimeData.GetDatetime(),
                    CounterNumber = NumberData.GetNumber(1000000, 9999999).ToString(),
                    IdentityNumber = fullIdentityNumber,
                    Email = NetworkData.GetEmail(),
                    SubscriberType = "Bireysel",
                    IsActive = true
                };

                SubsCorporate subsCorporate = new SubsCorporate
                {
                    PhoneNumber = PhoneNumberData.GetPhoneNumber(),
                    CorporateName = NameData.GetCompanyName(),
                    CounterNumber = NumberData.GetNumber(1000000, 9999999).ToString(),
                    TaxIdNumber = fullTaxIdNumber,
                    Email = NetworkData.GetEmail(),
                    SubscriberType = "Kurumsal",
                    IsActive = true
                };
                individualRepository.AddAsync(subsIndividual);
                corporateRepository.AddAsync(subsCorporate);
            }
            unitOfWork.CommitAsync().GetAwaiter().GetResult(); // Bireysel abone verilerini kaydet
        }

        #region CreateFakeData
        public void CreateFakeInvoiceData()
        {
            for (int i = 0; i < 200; i++)
            {
                decimal totalIndex = NumberData.GetNumber(1000, 5000);
                decimal dayFirstIndex = NumberData.GetNumber(0, 500); // Gündüzün ilk indeksi
                decimal dayLastIndex = NumberData.GetNumber(500, 2000); // Gündüzün son indeksi
                decimal peakFirstIndex = NumberData.GetNumber(0, 200); // Puantın ilk indeksi
                decimal peakLastIndex = NumberData.GetNumber(200, 800); // Puantın son indeksi
                decimal nightFirstIndex = NumberData.GetNumber(0, 300); // Gece nin ilk indeksi
                decimal nightLastIndex = NumberData.GetNumber(300, 1200); // Gece nin son indeksi

                var meterReading = new MeterReading(DateTimeData.GetDatetime(new DateTime(2023, 2, 10), new DateTime(2023, 7, 10)))
                {
                    ReadingDate = DateTimeData.GetDatetime(new DateTime(2023, 2, 10), new DateTime(2023, 7, 10)),

                    TotalIndex = totalIndex,
                    TotalFirstIndex = totalIndex - (dayLastIndex + peakLastIndex + nightLastIndex),
                    TotalLastIndex = totalIndex,

                    DayFirstIndex = dayFirstIndex,
                    DayLastIndex = dayLastIndex,

                    PeakFirstIndex = peakFirstIndex,
                    PeakLastIndex = peakLastIndex,

                    NightFirstIndex = nightFirstIndex,
                    NightLastIndex = nightLastIndex
                };

                // Fark değerlerini hesapla ve ayarla
                meterReading.DayDifference = meterReading.DayLastIndex - meterReading.DayFirstIndex;
                meterReading.PeakDifference = meterReading.PeakLastIndex - meterReading.PeakFirstIndex;
                meterReading.NightDifference = meterReading.NightLastIndex - meterReading.NightFirstIndex;
                meterReading.TotalDifference = meterReading.TotalLastIndex - meterReading.TotalFirstIndex;
                meterReading.ReadingDayDifference = (meterReading.LastIndexDate - meterReading.ReadingDate).Days;


                // Invoice oluştur
                EduPortal.Domain.Entities.Invoice invoice = new()
                {
                    IsPaid = BooleanData.GetBoolean(),
                    PaymentDate = DateTimeData.GetDatetime(new(2023, 8, 10), DateTime.Now),
                    ReadingDate = DateTimeData.GetDatetime(new(2023, 2, 10), new(2023, 7, 10)),
                    DueDate = DateTimeData.GetDatetime(new(2024, 3, 10), new(2024, 4, 20)),
                    Date = DateTime.Now,
                    SubscriberId = NumberData.GetNumber(4490, 5280),
                    SubscriberType = "Bireysel",
                    MeterReading = meterReading,
                    Amount = (meterReading.TotalDifference) * 1.45m
                };

                invoiceRepository.AddAsync(invoice);
            }
            unitOfWork.CommitAsync().GetAwaiter().GetResult(); // Fatura verilerini kaydet
        }

        #endregion


        public async Task  CreateCounterNumber()
        {

          await  addressRepository.CreateCounterNumberAsync();
        }






    }
}

