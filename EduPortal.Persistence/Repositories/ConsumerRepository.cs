
using EduPortal.Application.Interfaces;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Repository.Repositories
{
    public class ConsumerRepository(AppDbContext context) : IConsumerRepository
    {
        //public Consumer GetById(int id)
        //{
        //    //var consumer = context.Consumers.First(x => x.Id == id);
        //    return consumer;
        //}

        public void Create(Consumer consumer)
        {
            context.Entry(consumer).State = EntityState.Added;
            context.SaveChanges();
        }

        //public IReadOnlyList<Consumer> GetAll()
        //{
        //    //    return _context.Attach();
        //    return context.Consumers.AsNoTracking().ToList().AsReadOnly();
        //}

        public void DeleteById(Consumer id)
        {
            throw new NotImplementedException();
        }

        public void Update(Consumer consumer)
        {
            throw new NotImplementedException();
        }

        public void Save(Consumer consumer)
        {
            var state = context.Entry(consumer).State;
            context.Entry(consumer).State = EntityState.Added;
            // context.Products.Add(product); // change tracker ?
            var state2 = context.Entry(consumer).State;
            context.SaveChanges();
        }
    }
}
