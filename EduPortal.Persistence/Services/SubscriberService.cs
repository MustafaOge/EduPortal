using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Application.DTO_s.Subscriber;
using System.Net;

namespace EduPortal.Service.Services
{
    public class SubscriberService(ISubscriberRepository subscriberReposityory) : ISubscriberService
    {
        public Response<SubscriberResponseResponseDTO> Save(SubscriberCreateDTO request)
        {
            var newSubscriber = new Subscriber
            {
                SubscriberContractNumber = request.SubscriberContractNumber
            };

            subscriberReposityory.Create(newSubscriber);

            var SubscriberDto = new SubscriberResponseResponseDTO
            {
                SubscriberContractNumber = newSubscriber.SubscriberContractNumber,           
                //Consumer = newSubscriber.Consumer,
                //ElectricityMeter = newSubscriber.ElectricityMeter,

            };

            return Response<SubscriberResponseResponseDTO>.Success(SubscriberDto, HttpStatusCode.Created);
        }

        public Response<Subscriber> Get(int id)
        {
            var subscriber = subscriberReposityory.GetById(id);

            if (subscriber is null) return Response<Subscriber?>.Fail("Product not found", HttpStatusCode.NotFound);

            return Response<Subscriber?>.Success(subscriber, HttpStatusCode.OK);
        }

        public Response<List<Subscriber>> GetAll()
        {
            var subscribers = subscriberReposityory.GetAll();


            var subscribersListDto = subscribers.Select(x => new Subscriber
            {
                //ElectricityMeter = x.ElectricityMeter,
                SubscriberContractNumber = x.SubscriberContractNumber
            }).ToList();


            return Response<List<Subscriber>>.Success(subscribersListDto, HttpStatusCode.OK);
        }

        Response<List<Subscriber>> ISubscriberService.GetAll()
        {
            throw new NotImplementedException();
        }

        public Response<Subscriber?> GetById(int id)
        {
            throw new NotImplementedException();
        }


        public Response<string> Update(Subscriber subscriber)
        {
            throw new NotImplementedException();
        }

        public Response<string> DeleteById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
