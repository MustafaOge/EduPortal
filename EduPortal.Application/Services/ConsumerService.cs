using EduPortal.Application.DTO_s.Consumer;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Services
{

    public class ConsumerService(IConsumerRepository consumerRepository) : IConsumerService
    {


        //public Response<List<ConsumerDTO>> GetAll()
        //{
        //    var consumers = consumerRepository.GetAll();
        //    var consumersListDto = consumers.Select(x => new ConsumerDTO
        //    {
        //        FirstName = x.FirstName,
        //        LastName = x.LastName,
        //    }).ToList();

        //    return Response<List<ConsumerDTO>>.Success(consumersListDto, HttpStatusCode.OK);
        //}

        public Consumer GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Response<ConsumerCreateResponseDTO> Save(ConsumerCreateDTO request)
        {
            var newConsumer = new Consumer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdentityNumber = request.IdentityNumber,
                BirthDate = request.BirthDate,
            };

            consumerRepository.Create(newConsumer);

            var ConsumerDto = new ConsumerCreateResponseDTO
            {
                FirstName = newConsumer.FirstName,
                LastName = newConsumer.LastName,
                IdentityNumber = newConsumer.IdentityNumber,
                UserName = newConsumer.UserName
                
            };
            return Response<ConsumerCreateResponseDTO>.Success(ConsumerDto, HttpStatusCode.Created);
        }


        public Response<string> Update(ConsumerUpdateDTO consumer)
        {
            throw new NotImplementedException();
        }


        Response<Consumer> IConsumerService.GetById(int id)
        {
            throw new NotImplementedException();
        }


        public Response<string> DeleteById(Consumer id)
        {
            throw new NotImplementedException();
        }


    }
}

