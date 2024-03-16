using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Application.DTO_s.Subscriber;
using System.Net;

namespace EduPortal.Service.Services
{
    public class SubscriberService(ISubsIndividualRepository subscriberReposityory) : ISubsIndividualService
    {
        public Response<SubscriberResponseResponseDTO> Save(SubscriberCreateDTO request)
        {
            var newSubscriber = new SubsIndividual
            {
                Email = request.Email
             
            };

            subscriberReposityory.Create(newSubscriber);

            var SubscriberDto = new SubscriberResponseResponseDTO
            {
                //SubscriberContractNumber = newSubscriber.SubscriberContractNumber,           
                //Consumer = newSubscriber.Consumer,
                //ElectricityMeter = newSubscriber.ElectricityMeter,

            };

            return Response<SubscriberResponseResponseDTO>.Success(SubscriberDto, HttpStatusCode.Created);
        }

        public Response<SubsIndividual> Get(int id)
        {
            var subscriber = subscriberReposityory.GetById(id);

            if (subscriber is null) return Response<SubsIndividual?>.Fail("Product not found", HttpStatusCode.NotFound);

            return Response<SubsIndividual?>.Success(subscriber, HttpStatusCode.OK);
        }

        public Response<List<SubsIndividual>> GetAll()
        {
            var subscribers = subscriberReposityory.GetAll();


            var subscribersListDto = subscribers.Select(x => new SubsIndividual
            {
                //ElectricityMeter = x.ElectricityMeter,
                //SubscriberContractNumber = x.SubscriberContractNumber
            }).ToList();


            return Response<List<SubsIndividual>>.Success(subscribersListDto, HttpStatusCode.OK);
        }

        Response<List<SubsIndividual>> ISubsIndividualService.GetAll()
        {
            throw new NotImplementedException();
        }

        public Response<SubsIndividual?> GetById(int id)
        {
            throw new NotImplementedException();
        }




        public Response<string> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Response<string> Update(SubsIndividual subscriber)
        {
            throw new NotImplementedException();
        }
    }
}
