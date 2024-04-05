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
namespace EduPortal.Service.Services
{
    public class SubsIndividualService(
        IGenericRepository<SubsIndividual, int> subsInvidiualRepository,
        IUnitOfWork unitOfWork,
        ISubsIndividualRepository subsIndividualRepository,
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
