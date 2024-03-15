using EduPortal.Application.DTO_s.Consumer;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IConsumerRepository
    {
        //IReadOnlyList<Consumer> GetAll();
        //Consumer? GetById(int id);
        void Create(Consumer consumer);
        void Update(Consumer consumer);
        void DeleteById(Consumer consumerToDelete);

    }
}
