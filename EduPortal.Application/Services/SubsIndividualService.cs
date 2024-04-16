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
            var individualEntity = mapper.Map<SubsIndividual>(individualCreate);

            await subsIndividualRepository.AddAsync(individualEntity);
            await unitOfWork.CommitAsync();

            var individualDto = mapper.Map<SubsIndividualDto>(individualEntity);

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
            List<SubsIndividual> aboneler = await subsIndividualRepository.FindIndividualAsync(identityNumber);

            if (aboneler.Count == 0)
            {
                return Response<bool>.Fail("Abone bulunamadı.",HttpStatusCode.NotFound);
            }

            foreach (var abone in aboneler)
            {
                bool hasUnpaidInvoices = await subscriberRepository.HasUnpaidInvoices(abone.Id);

                if (hasUnpaidInvoices)
                {
                    return Response<bool>.Fail("Ödenmemiş faturası bulunduğu için abonelik sonlandırılamadı.",HttpStatusCode.Forbidden);
                }
                else
                {
                    abone.IsActive = false;
                    subscriberRepository.Update(abone);
                }
            }

            await unitOfWork.CommitAsync();

            return Response<bool>.Success(true,HttpStatusCode.OK);
        }
    }
}
