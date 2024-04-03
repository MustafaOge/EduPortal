using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Services
{
    public class CreateFakeDataService(AppDbContext appDbContext, IUnitOfWork unitOfWork) : IFakeDataService
    {
        public void CreateFakeSubsInvoicesData()
        {
            throw new NotImplementedException();
        }
        public void CreateFakeSubsIndividualData()
        {
            for (int i = 0; i < 20000; i++)
            {
                SubsIndividual subsIndividual = new SubsIndividual
                {

                    PhoneNumber = PhoneNumberData.GetPhoneNumber(),
                    NameSurname = NameData.GetFullName(),
                    BirthDate = DateTimeData.GetDatetime(),
                    CounterNumber = NumberData.GetNumber(1000000, 9999999).ToString(),
                    IdentityNumber = NumberData.GetNumber(1000, 100000).ToString(),
                    Email = NetworkData.GetEmail(),
                    SubscriberType = "Bireysel",
                    IsActive = true
                };

                SubsCorporate subsCorporate = new SubsCorporate
                {

                    PhoneNumber = PhoneNumberData.GetPhoneNumber(),
                    CorporateName = NameData.GetFullName(),
                    CounterNumber = NumberData.GetNumber(1000000, 9999999).ToString(),
                    TaxIdNumber = NumberData.GetNumber(1000, 100000).ToString(),
                    Email = NetworkData.GetEmail(),
                    SubscriberType = "Bireysel",
                    IsActive = true,

                };
                appDbContext.Corprorates.Add(subsCorporate);
                appDbContext.Individuals.Add(subsIndividual);

            }
            unitOfWork.CommitAsync();

        }


        public void CreateFakeInvoiceData()
        {
            for (int i = 50; i < 10500; i++)
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
                Invoice invoice = new()
                {
                    IsPaid = BooleanData.GetBoolean(),
                    PaymentDate = DateTimeData.GetDatetime(new(2023, 8, 10), DateTime.Now),
                    ReadingDate = DateTimeData.GetDatetime(new(2023, 2, 10), new(2023, 7, 10)),
                    DueDate = DateTimeData.GetDatetime(new(2023, 9, 10), new(2023, 12, 10)),
                    Date = DateTime.Now,
                    SubscriberId = NumberData.GetNumber(50, 2460),
                    SubscriberType = "Kurumsal",
                    MeterReading = meterReading,
                    Amount = (meterReading.TotalDifference) * 1.45m
                };

                appDbContext.Invoices.Add(invoice);
            }
            unitOfWork.CommitAsync();
        }
    }
}

