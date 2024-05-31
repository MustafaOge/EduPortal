using AutoMapper;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Models.ViewModel.YourProject.ViewModels;
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
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ISubsCorporateRepository subsCorporateRepository,
        ISubsIndividualRepository subsIndividualRepository,
        ISubscriberRepository subscriberRepository,
        IGenericRepository<InvoiceComplaint, int> invoiceComplaint,
        IGenericRepository<MeterReading, int> meterReading) : IInvoiceService
    {
        public async Task<Response<int>> PayInvoice(int id)
        {
            try
            {
                Invoice invoice = await invoiceRepository.GetByIdAsync(id);

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

        public async Task<Response<SubsCorporate>> PaymentCorporate(string taxIdNumber)
        {
            try
            {
                // Kurumsal abone ödeme işlemi
                SubsCorporate corporateSubscribers = await subsCorporateRepository.FindCorporateAsync(taxIdNumber);

                if (corporateSubscribers == null || corporateSubscribers == null)
                {
                    return Response<SubsCorporate>.Fail("Kurumsal abone bulunamadı.", HttpStatusCode.NotFound);
                }

                return Response<SubsCorporate>.Success(corporateSubscribers, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<SubsCorporate>.Fail("Kurumsal abone ödeme işlemi sırasında bir hata oluştu: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<SubsIndividual>> PaymentIndividual(string identityNumber)
        {
            try
            {
                // Bireysel abone ödeme işlemi
                SubsIndividual individualSubscriber = await subsIndividualRepository.FindIndividualAsync(identityNumber);

                if (individualSubscriber == null)
                {
                    return Response<SubsIndividual>.Fail("Abone bulunamadı.", HttpStatusCode.NotFound);
                }

                return Response<SubsIndividual>.Success(individualSubscriber, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<SubsIndividual>.Fail("Bireysel abone ödeme işlemi sırasında bir hata oluştu: " + ex.Message, HttpStatusCode.InternalServerError);
            }
        }


        public async Task<Response<List<Invoice>>> GetInvoiceDetail(int id)
        {
            List<Invoice> invoices = await invoiceRepository.Where(i => i.SubscriberId == id).ToListAsync();

            return Response<List<Invoice>>.Success(invoices, HttpStatusCode.OK);
        }

        public async Task<Response<InvoiceDetailView>> DetailPay(int id)
        {
            Invoice invoice = await invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                return Response<InvoiceDetailView>.Fail("Invoice not found", HttpStatusCode.NotFound);
            }

            Subscriber subscriber = await GetSubscriberAsync(invoice);

            InvoiceDetailView viewModel = new InvoiceDetailView
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
                return (await subscriberRepository.GetByIdAsync(invoice.SubscriberId));

            }
            else
            {
                return null;
            }
        }

        public async Task<Response<InvoiceComplaint>> CreateComplaint(InvoiceComplaint model)
        {
            try
            {
                Invoice existingInvoice = await invoiceRepository.GetByIdAsync(model.InvoiceId);

                if (existingInvoice == null)
                {
                    toast.AddErrorToastMessage("Hatalı Fatura ID, Fatura Bulunamadı!");
                    return Response<InvoiceComplaint>.Fail("Hatalı Fatura ID, Fatura Bulunamadı!", HttpStatusCode.NotFound);
                }

                // Yeni fatura itirazı oluştur
                InvoiceComplaint complaint = new InvoiceComplaint
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

                return Response<InvoiceComplaint>.Fail("Fatura itirazı oluşturulurken bir hata oluştu: " + ex.Message, HttpStatusCode.InternalServerError);

            }
        }      
    }
}
