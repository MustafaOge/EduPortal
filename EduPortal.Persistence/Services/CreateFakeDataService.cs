using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Services
{
    public class CreateFakeDataService : IFakeDataService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUnitOfWork _unitOfWork;

        public CreateFakeDataService(AppDbContext appDbContext, IUnitOfWork unitOfWork)
        {
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
        }

        public void CreateFakeData()
        {
            if (!_appDbContext.Individuals.Any() && !_appDbContext.Corprorates.Any() && !_appDbContext.Invoices.Any())
            {
                CreateFakeSubsIndividualData(); // Bireysel abone verilerini oluştur ve kaydet

                CreateFakeInvoiceData(); // Fatura verilerini oluştur

                _unitOfWork.CommitAsync().GetAwaiter().GetResult(); // Değişiklikleri kaydet
            }
        }

        public void CreateFakeSubsIndividualData()
        {
            // Bireysel abone verilerini oluştur
            for (int i = 0; i < 100; i++)
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

                _appDbContext.Individuals.Add(subsIndividual);
            }

            _unitOfWork.CommitAsync().GetAwaiter().GetResult(); // Bireysel abone verilerini kaydet
        }

        public void CreateFakeInvoiceData()
        {
            // Fatura verilerini oluştur
            for (int i = 1; i < 101; i++)
            {
                decimal totalIndex = NumberData.GetNumber(1000, 5000);
                // Diğer fatura verilerini oluştur...

                // Abone ID'sini rastgele seç
                int subscriberId = NumberData.GetNumber(1, 100);

                // Faturayı oluştur ve abone ID'sini ayarla
                Invoice invoice = new()
                {
                    IsPaid = BooleanData.GetBoolean(),
                    PaymentDate = DateTimeData.GetDatetime(new(2023, 8, 10), DateTime.Now),
                    ReadingDate = DateTimeData.GetDatetime(new(2023, 2, 10), new(2023, 7, 10)),
                    DueDate = DateTimeData.GetDatetime(new(2023, 9, 10), new(2023, 12, 10)),
                    Date = DateTime.Now,
                    SubscriberId = subscriberId,
                    SubscriberType = "Bireysel",
                    // Diğer fatura bilgilerini ayarla...
                };

                _appDbContext.Invoices.Add(invoice);
            }

            _unitOfWork.CommitAsync().GetAwaiter().GetResult(); // Fatura verilerini kaydet
        }
    }
}
