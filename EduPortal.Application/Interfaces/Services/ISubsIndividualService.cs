using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface ISubsIndividualService
    {
        Task<bool> CreateIndividualAsync(CreateIndividualDto individualCreate);


        //Response<List<SubsIndividual>> GetAll();
        //Response<SubsIndividual?> GetById(int id);
        //Response<SubscriberResponseResponseDTO> Save(SubscriberCreateDTO subscriber);
        //Response<string> Update(SubsIndividual subscriber);
        //Response<string> DeleteById(int id);

    }
}


