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
                string counterNumber = await GetCounterNumberAsync(individualCreate);
                if (string.IsNullOrEmpty(counterNumber))
                {
                    return Response<SubsIndividualDto>.Fail("Sayaç numarası bulunamadı veya sağlanmadı.", HttpStatusCode.BadRequest);
                }
                var individualEntity = mapper.Map<SubsIndividual>(individualCreate);
                individualEntity.CounterNumber = counterNumber;


                await subsIndividualRepository.AddAsync(individualEntity);
                await unitOfWork.CommitAsync();

                var individualDto = mapper.Map<SubsIndividualDto>(individualEntity);
                return Response<SubsIndividualDto>.Success(individualDto, HttpStatusCode.Created);
            }
            catch (DbUpdateException ex)
            {
                return Response<SubsIndividualDto>.Fail($"Veritabanı güncelleme hatası oluştu.{ex}", HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return Response<SubsIndividualDto>.Fail($"Bireysel abonelik oluşturulurken bir hata oluştu.{ex}", HttpStatusCode.InternalServerError);
            }
        }

        private async Task<string> GetCounterNumberAsync(CreateIndividualDto individualCreate)
        {
            if (!string.IsNullOrEmpty(individualCreate.CounterNumber))
            {
                return individualCreate.CounterNumber;
            }

            if (!string.IsNullOrEmpty(individualCreate.InternalDoorNumber))
            {
                return await addressService.GetCounter(individualCreate.InternalDoorNumber);
            }

            return null;
        }


        public async Task<SubsIndividualDto> FindIndividualDtoAsync(string IdentityNumber)
        {
            SubsIndividual entity = await subsIndividualRepository.FindIndividualAsync(IdentityNumber);

            if (entity == null)
            {
                return null;
            }

            SubsIndividualDto dto = mapper.Map<SubsIndividualDto>(entity);

            return dto;
        }


        public async Task<Response<bool>> TerminateSubsIndividualAsync(string identityNumber)
        {
            var subscribers = await subsIndividualRepository.FindIndividualAsync(identityNumber);

            if (subscribers == null)
            {
                return Response<bool>.Fail("Abone bulunamadı.", HttpStatusCode.NotFound);
            }

            var updatedSubscribers = new List<SubsIndividual>(); // Güncellenen abonelerin listesi

            if (await subscriberRepository.HasUnpaidInvoices(subscribers.Id))
            {
                return Response<bool>.Fail("Ödenmemiş faturası bulunduğu için abonelik sonlandırılamadı.", HttpStatusCode.Forbidden);
            }

            subscribers.IsActive = false;
            updatedSubscribers.Add(subscribers);

            subscriberRepository.Update(subscribers);
            await subscriberTerminate.TerminateSubscriptionAndAddToOutbox(subscribers.Id);

            await unitOfWork.CommitAsync();

            return Response<bool>.Success(true, HttpStatusCode.OK);
        }







    }
}
