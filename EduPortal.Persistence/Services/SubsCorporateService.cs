using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Application.DTO_s.Subscriber;
using System.Net;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Persistence.context;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using EduPortal.Persistence.Repositories;

namespace EduPortal.Service.Services
{
    public class SubsCorporateService(
        AppDbContext context,
        IUnitOfWork unitOfWork,
        ISubsCorporateRepository subsCorporateRepository,
        IMapper mapper
        ) : ISubsCorporateService
    {
        public async Task<Response<SubsCorporateDto>> CreateCorporateAsync(CreateCorporateDto corporateDto)
        {
            var corporatelEntity = mapper.Map<SubsCorporate>(corporateDto);

            await subsCorporateRepository.AddAsync(corporatelEntity);
            await unitOfWork.CommitAsync();

            var individualDto = mapper.Map<SubsCorporateDto>(corporatelEntity);

            return Response<SubsCorporateDto>.Success(individualDto, HttpStatusCode.Created);
        }



        public async Task<Response<List<SubsCorporateDto>>> FindCorporateAsync(string taxIdNumber)
        {
            var corporates = await subsCorporateRepository.FindCorporateAsync(taxIdNumber);
            var corporatesDto = corporates.Select(corporate => new SubsCorporateDto
            {
                // Gerekirse mapping işlemleri burada yapılabilir
            }).ToList();

            if (corporatesDto.Any())
            {
                return Response<List<SubsCorporateDto>>.Success(corporatesDto, HttpStatusCode.OK);
            }
            else
            {
                return Response<List<SubsCorporateDto>>.Fail("Abone bulunamadı.", HttpStatusCode.Found);
            }
        }

        //public async Task<Response<List<SubsCorporateDto>>> FindCorporateAsync(string TaxIdNumber)
        //{
        //    var corporates = await subsCorporateRepository.FindCorporateAsync(TaxIdNumber);

        //    if (corporates == null || !corporates.Any())
        //    {
        //        return Response<List<SubsCorporateDto>>.Fail("Abone bulunamadı.", HttpStatusCode.NotFound);
        //    }

        //    var corporateDtos = mapper.Map<List<SubsCorporateDto>>(corporates);
        //    return Response<List<SubsCorporateDto>>.Success(corporateDtos, HttpStatusCode.OK);
        //},








        //public async Task<Response<List<SubsCorporateDto>>> FindCorporate(string TaxIdNumber)
        //{
        //    var abone = subsCorporateRepository.FindCorporate(TaxIdNumber);
        //    var aboneDto = mapper.Map<List<SubsCorporateDto>>(abone);
        //    return Response<List<SubsCorporateDto>>.Success(aboneDto, HttpStatusCode.OK);
        //}





        //[HttpPost]

        //public IActionResult Individual(SubsIndividual individual)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        toast.AddSuccessToastMessage("İşlem Başarılı", new ToastrOptions { Title = "Başarılı!" });
        //        appDbContext.Individuals.Add(individual);
        //        appDbContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        toast.AddErrorToastMessage("Abone Eklenemedi", new ToastrOptions { Title = "Başarısız!" });
        //    }
        //    return View();
        //}


        //public Response<SubscriberResponseResponseDTO> Save(SubscriberCreateDTO request)
        //{
        //    var newSubscriber = new SubsIndividual
        //    {
        //        Email = request.Email

        //    };

        //    subscriberReposityory.Create(newSubscriber);

        //    var SubscriberDto = new SubscriberResponseResponseDTO
        //    {
        //        //SubscriberContractNumber = newSubscriber.SubscriberContractNumber,           
        //        //Consumer = newSubscriber.Consumer,
        //        //ElectricityMeter = newSubscriber.ElectricityMeter,

        //    };

        //    return Response<SubscriberResponseResponseDTO>.Success(SubscriberDto, HttpStatusCode.Created);
        //}

        //public Response<SubsIndividual> Get(int id)
        //{
        //    var subscriber = subscriberReposityory.GetById(id);

        //    if (subscriber is null) return Response<SubsIndividual?>.Fail("Product not found", HttpStatusCode.NotFound);

        //    return Response<SubsIndividual?>.Success(subscriber, HttpStatusCode.OK);
        //}

        //public Response<List<SubsIndividual>> GetAll()
        //{
        //    var subscribers = subscriberReposityory.GetAll();


        //    var subscribersListDto = subscribers.Select(x => new SubsIndividual
        //    {
        //        //ElectricityMeter = x.ElectricityMeter,
        //        //SubscriberContractNumber = x.SubscriberContractNumber
        //    }).ToList();


        //    return Response<List<SubsIndividual>>.Success(subscribersListDto, HttpStatusCode.OK);
        //}

        //Response<List<SubsIndividual>> ISubsIndividualService.GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public Response<SubsIndividual?> GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}




        //public Response<string> DeleteById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Response<string> Update(SubsIndividual subscriber)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
