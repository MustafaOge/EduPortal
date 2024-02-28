using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces
{
    public interface ISubscriberService
    {
        Response<List<Subscriber>> GetAll();
        Response <Subscriber?> GetById(int id);
        Response<SubscriberResponseResponseDTO> Save(SubscriberCreateDTO subscriber);
        Response<string> Update(Subscriber subscriber); 
        Response<string> DeleteById(int id);

    }
}
