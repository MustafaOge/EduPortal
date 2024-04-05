using CreditWiseHub.Repository.UnitOfWorks;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Models.ViewModel.YourProject.ViewModels;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Services
{
    public class InvoiceService(
        IInvoiceRepository invoiceRepository,
        IToastNotification toast,
        IUnitOfWork unitOfWork,
        ISubsCorporateRepository subsCorporateRepository,
        ISubsIndividualRepository subsIndividualRepository,
        ISubscriberRepository subscriberRepository,
        IGenericRepository<InvoiceComplaint, int> invoiceComplaint,
        IGenericRepository<MeterReading, int> meterReading


) : IInvoiceService
    {


        public async Task<Response<int>> PayInvoice(int id)
        {
            try
            {
                var invoice = await invoiceRepository.GetByIdAsync(id);

                if (invoice == null)
                {
                    return Response<int>.Fail("Fatura bulunamadı", HttpStatusCode.NotFound);
                }

                if (invoice.IsPaid)
                {
                    toast.AddErrorToastMessage("Fatura Zaten Ödenmiş");
                    return Response<int>.Fail("Fatura zaten ödenmiş", HttpStatusCode.BadRequest);
                }

                invoice.IsPaid = true;
                invoice.PaymentDate = DateTime.Now;

                invoiceRepository.Update(invoice);
                await unitOfWork.CommitAsync();

                toast.AddSuccessToastMessage("Fatura Başarı İle Ödendi");

                return Response<int>.Success(invoice.Id, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<int>.Fail("Ödeme işlemi sırasında bir hata oluştu: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<SubsCorporate>>> PaymentCorporate(string taxIdNumber)
        {
            try
            {
                // Kurumsal abone ödeme işlemi
                var corporateSubscribers = await subsCorporateRepository.FindCorporateAsync(taxIdNumber);

                if (corporateSubscribers == null || !corporateSubscribers.Any())
                {
                    return Response<List<SubsCorporate>>.Fail("Kurumsal abone bulunamadı.", HttpStatusCode.NotFound);
                }

                return Response<List<SubsCorporate>>.Success(corporateSubscribers, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<SubsCorporate>>.Fail("Kurumsal abone ödeme işlemi sırasında bir hata oluştu: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<SubsIndividual>>> PaymentIndividual(string identityNumber)
        {
            try
            {
                // Bireysel abone ödeme işlemi
                var individualSubscribers = await subsIndividualRepository.FindIndividualAsync(identityNumber);

                if (individualSubscribers == null || !individualSubscribers.Any())
                {
                    return Response<List<SubsIndividual>>.Fail("Abone bulunamadı.", HttpStatusCode.NotFound);
                }

                return Response<List<SubsIndividual>>.Success(individualSubscribers, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<SubsIndividual>>.Fail("Bireysel abone ödeme işlemi sırasında bir hata oluştu: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<Invoice>>> GetInvoiceDetail(int id)
        {
            var invoices = await invoiceRepository.Where(i => i.SubscriberId == id).ToListAsync();

            return Response<List<Invoice>>.Success(invoices, HttpStatusCode.OK);
        }






        public async Task<Response<InvoiceDetailView>> DetailPay(int id)
        {
            var invoice = await invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                return Response<InvoiceDetailView>.Fail("Invoice not found", HttpStatusCode.NotFound);
            }

            var subscriber = await GetSubscriberAsync(invoice);

            var viewModel = new InvoiceDetailView
            {
                Invoice = invoice,
                Subscriber = subscriber,
                MeterReadings = meterReading.Where(m => m.InvoiceId == id).ToList()

            };

            return Response<InvoiceDetailView>.Success(viewModel, HttpStatusCode.OK);
        }



        private async Task<Subscriber> GetSubscriberAsync(Invoice invoice)
        {
            if (invoice.Subscriber != null)
            {
                return invoice.Subscriber;
            }
            else if (invoice.SubscriberId != null)
            {
                return (await subscriberRepository.GetByIdAsync(invoice.SubscriberId.Value));

            }
            else
            {
                return null;
            }
        }

        public Task<Response<List<Invoice>>> GetInvoices(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<Response<InvoiceComplaint>> CreateComplaint(InvoiceComplaint model)
        {
            try
            {

                var existingInvoice = await invoiceRepository.GetByIdAsync(model.InvoiceId);

                // Eğer fatura bulunamadıysa, hata döndür
                if (existingInvoice == null)
                {
                    toast.AddErrorToastMessage("Hatalı Fatura ID, Fatura Bulunamadı!");
                    return Response<InvoiceComplaint>.Fail("Hatalı Fatura ID, Fatura Bulunamadı!", HttpStatusCode.NotFound);
                }

                // Yeni fatura itirazı oluştur
                var complaint = new InvoiceComplaint
                {
                    InvoiceId = model.InvoiceId,
                    Title = model.Title,
                    Reason = model.Reason,
                    CreatedAt = DateTime.Now
                };

                // Fatura itirazını veritabanına ekle
                await invoiceComplaint.AddAsync(complaint);
                await unitOfWork.CommitAsync();
                toast.AddSuccessToastMessage("İtiraz Talebi Oluşturuldu");


                // Başarı yanıtı dön
                return Response<InvoiceComplaint>.Success(complaint, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                toast.AddErrorToastMessage("İtiraz Talebi Oluşturulmadı");

                // Hata durumunda uygun bir hata mesajı dön
                return Response<InvoiceComplaint>.Fail("Fatura itirazı oluşturulurken bir hata oluştu: " + ex.Message, HttpStatusCode.InternalServerError);

            }
        }


    }
}
