using EduPortal.Application.DTO_s.Consumer;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces
{
    public interface IConsumerService
    {
        //Response<List<ConsumerDTO>> GetAll();
        Response<Consumer> GetById(int id);
        Response<ConsumerCreateResponseDTO> Save(ConsumerCreateDTO consumer);
        Response<string> Update(ConsumerUpdateDTO consumer);
        Response <string> DeleteById(Consumer id);
z
    }
}
