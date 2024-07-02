using AutoMapper;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
                //throw new Exception();
                SubsCorporateDto subsCorporateDto = mapper.Map<SubsCorporateDto>(corporatelEntity);

                return Response<SubsCorporateDto>.Success(subsCorporateDto, HttpStatusCode.Created);
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


        public async Task<SubsCorporateDto> FindCorporateAsync(string taxIdNumber)
        {
            SubsCorporate entity = await subsCorporateRepository.FindCorporateAsync(taxIdNumber);
            if (entity == null)
            {
                return null;
            }

            SubsCorporateDto dto = mapper.Map<SubsCorporateDto>(entity);
            return dto;
        }


        public async Task<Response<bool>> TerminateSubsCorporateAsync(string taxIdNumber)
        {
            SubsCorporate abone = await subsCorporateRepository.FindCorporateAsync(taxIdNumber);

            if (abone == null)
            {
                return Response<bool>.Fail("Abone bulunamadı.", HttpStatusCode.NotFound);
            }

            bool hasUnpaidInvoices = await subscriberRepository.HasUnpaidInvoices(abone.Id);

            if (hasUnpaidInvoices)
            {
                return Response<bool>.Fail("Ödenmemiş faturası bulunduğu için abonelik sonlandırılamadı.", HttpStatusCode.Forbidden);
            }

            abone.IsActive = false;
            subscriberRepository.Update(abone);
            await unitOfWork.CommitAsync();

            return Response<bool>.Success(true, HttpStatusCode.OK);
        }

    }
}
