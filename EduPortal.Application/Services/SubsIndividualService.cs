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

namespace EduPortal.Service.Services
{
    public class SubsIndividualService(
        IGenericRepository<SubsIndividual, int> subsInvidiualRepository,
        IUnitOfWork unitOfWork,
        ISubsIndividualRepository subsIndividualRepository,
        ISubscriberRepository subscriberRepository,
        IMapper mapper
        ) : ISubsIndividualService
    {
        public async Task<Response<SubsIndividualDto>> CreateIndividualAsync(CreateIndividualDto individualCreate)
        {
            SubsIndividual individualEntity = mapper.Map<SubsIndividual>(individualCreate);

            await subsIndividualRepository.AddAsync(individualEntity);
            await unitOfWork.CommitAsync();

            SubsIndividualDto individualDto = mapper.Map<SubsIndividualDto>(individualEntity);

            return Response<SubsIndividualDto>.Success(individualDto, HttpStatusCode.Created);
        }



        public async Task<List<SubsIndividualDto>> FindIndividualDtosAsync(string IdentityNumber)
        {
            List<SubsIndividual> entities = await subsIndividualRepository.FindIndividualAsync(IdentityNumber);

            List<SubsIndividualDto> dtos = entities.Select(entity => mapper.Map<SubsIndividualDto>(entity)).ToList();

            return dtos;
        }

        public async Task<Response<bool>> TerminateSubsIndividualAsync(string identityNumber)
        {
            List<SubsIndividual> subscribers = await subsIndividualRepository.FindIndividualAsync(identityNumber);

            if (subscribers.Count == 0)
            {
                return Response<bool>.Fail("Abone bulunamadı.",HttpStatusCode.NotFound);
            }

            foreach (var abone in subscribers)
            {

                if (await subscriberRepository.HasUnpaidInvoices(abone.Id))
                {
                    return Response<bool>.Fail("Ödenmemiş faturası bulunduğu için abonelik sonlandırılamadı.",HttpStatusCode.Forbidden);
                          
                    abone.IsActive = false;
                    subscriberRepository.Update(abone);
                }
            }

            await unitOfWork.CommitAsync();

            return Response<bool>.Success(true,HttpStatusCode.OK);
        }
    }
}
