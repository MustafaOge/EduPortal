using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Application.DTO_s.Subscriber;
using System.Net;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;

namespace EduPortal.Service.Services
{
    public class SubsCorporateService(
        IUnitOfWork unitOfWork,
        ISubsCorporateRepository subsCorporateRepository,
        ISubscriberRepository subscriberRepository,
        IMapper mapper
        ) : ISubsCorporateService
    {
        public async Task<Response<SubsCorporateDto>> CreateCorporateAsync(CreateCorporateDto corporateDto)
        {
            try
            {
                SubsCorporate corporatelEntity = mapper.Map<SubsCorporate>(corporateDto);

                await subsCorporateRepository.AddAsync(corporatelEntity);
                await unitOfWork.CommitAsync();
                throw new Exception();
                SubsCorporateDto individualDto = mapper.Map<SubsCorporateDto>(corporatelEntity);

                return Response<SubsCorporateDto>.Success(individualDto, HttpStatusCode.Created);
            }
            catch (DbUpdateException ex)
            {
                // Veritabanı güncelleme hatası durumunda loglama
                return Response<SubsCorporateDto>.Fail($"Veritabanı güncelleme hatası oluştu.{ex}", HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                // Diğer tüm hata durumlarını ele almak için genel bir catch bloğu
                return Response<SubsCorporateDto>.Fail($"Kurumsal abonelik oluşturulurken bir hata oluştu.{ex}", HttpStatusCode.InternalServerError);
            }

        }


        public async Task<List<SubsCorporateDto>> FindCorporateAsync(string taxIdNumber)
        {
            List<SubsCorporate> entities = await subsCorporateRepository.FindCorporateAsync(taxIdNumber);

            List<SubsCorporateDto> dtos = entities.Select(entity => mapper.Map<SubsCorporateDto>(entity)).ToList();

            return dtos;
        }

        public async Task<Response<bool>> TerminateSubsCorporateAsync(string TaxIdNumber)
        {
        
            List<SubsCorporate> aboneler = await subsCorporateRepository.FindCorporateAsync(TaxIdNumber);

            if (aboneler.Count == 0)
            {
                return Response<bool>.Fail("Abone bulunamadı.", HttpStatusCode.NotFound);
            }

            foreach (var abone in aboneler)
            {
                bool hasUnpaidInvoices = await subscriberRepository.HasUnpaidInvoices(abone.Id);

                if (hasUnpaidInvoices)
                {
                    return Response<bool>.Fail("Ödenmemiş faturası bulunduğu için abonelik sonlandırılamadı.", HttpStatusCode.Forbidden);
                }
                else
                {
                    abone.IsActive = false;
                    subscriberRepository.Update(abone);
                }
            }
            await unitOfWork.CommitAsync();
            return Response<bool>.Success(true, HttpStatusCode.OK);
        }
    }
}
