using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Application.DTO_s.Subscriber;
using System.Net;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EduPortal.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace EduPortal.Service.Services
{
    public class SubsIndividualService(
        IGenericRepository<SubsIndividual, int> subsInvidiualRepository,
        IUnitOfWork unitOfWork,
        ISubsIndividualRepository subsIndividualRepository,
        ISubscriberRepository subscriberRepository,
        ISubscriberTerminateService subscriberTerminate,
        IAddressService addressService,
        IMapper mapper
        ) : ISubsIndividualService
    {
        public async Task<Response<SubsIndividualDto>> CreateIndividualAsync(CreateIndividualDto individualCreate)
        {
            try
            {
                string counterNumber = individualCreate.CounterNumber; // Gelen sayaç numarasını kontrol et

                // Eğer sayaç numarası yoksa, ancak iç kapı numarası varsa, sayaç numarasını al
                if (string.IsNullOrEmpty(counterNumber) && !string.IsNullOrEmpty(individualCreate.InternalDoorNumber))
                {
                    counterNumber = await addressService.GetCounter(individualCreate.InternalDoorNumber);
                }

                // Sayaç numarasını alıp hala boşsa, bir hata döndür
                if (string.IsNullOrEmpty(counterNumber))
                {
                    return Response<SubsIndividualDto>.Fail("Sayaç numarası bulunamadı veya sağlanmadı.", HttpStatusCode.BadRequest);
                }

                // Sayaç numarası varsa, kaydı oluştur
                SubsIndividual individualEntity = mapper.Map<SubsIndividual>(individualCreate);
                individualEntity.CounterNumber = counterNumber; // Sayaç numarasını atama
                await subsIndividualRepository.AddAsync(individualEntity);
                await unitOfWork.CommitAsync();

                SubsIndividualDto individualDto = mapper.Map<SubsIndividualDto>(individualEntity);

                return Response<SubsIndividualDto>.Success(individualDto, HttpStatusCode.Created);
            }
            catch (DbUpdateException ex)
            {
                // Veritabanı güncelleme hatası durumunda loglama
                return Response<SubsIndividualDto>.Fail($"Veritabanı güncelleme hatası oluştu.{ex}", HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                // Diğer tüm hata durumlarını ele almak için genel bir catch bloğu
                return Response<SubsIndividualDto>.Fail($"Bireysel abonelik oluşturulurken bir hata oluştu.{ex}", HttpStatusCode.InternalServerError);
            }
        }






        public async Task<List<SubsIndividualDto>> FindIndividualDtosAsync(string IdentityNumber)
        {
            List<SubsIndividual> entities = await subsIndividualRepository.FindIndividualAsync(IdentityNumber);

            List<SubsIndividualDto> dtos = entities.Select(entity => mapper.Map<SubsIndividualDto>(entity)).ToList();

            return dtos;
        }

        public async Task<Response<bool>> TerminateSubsIndividualAsync(string identityNumber)
        {
            var subscribers = await subsIndividualRepository.FindIndividualAsync(identityNumber);



            if (subscribers.Count == 0)
                return Response<bool>.Fail("Abone bulunamadı.", HttpStatusCode.NotFound);

            var updatedSubscribers = new List<SubsIndividual>(); // Güncellenen abonelerin listesi

            foreach (var abone in subscribers)
            {
                if (await subscriberRepository.HasUnpaidInvoices(abone.Id))

                    return Response<bool>.Fail("Ödenmemiş faturası bulunduğu için abonelik sonlandırılamadı.", HttpStatusCode.Forbidden);

                abone.IsActive = false;
                updatedSubscribers.Add(abone);
            }

            foreach (var updatedSubscriber in updatedSubscribers)
            {
                subscriberRepository.Update(updatedSubscriber);
                await subscriberTerminate.TerminateSubscriptionAndAddToOutbox(updatedSubscriber.Id);
            }

            await unitOfWork.CommitAsync();

            return Response<bool>.Success(true, HttpStatusCode.OK);
        }

    }
}
