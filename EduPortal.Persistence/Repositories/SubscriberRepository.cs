
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EAU.Repository.Repositories
{
    public class SubscriberRepository(AppDbContext context) : ISubscriberRepository
    {

        public IReadOnlyList<Subscriber> GetAll()
        {
            //    return _context.Attach();
            return context.Subscribers.AsNoTracking().ToList().AsReadOnly();

        }

        public Subscriber? GetById(int id)
        {
            var product = context.Subscribers.First(x => x.Id == id);

            //var state = context.Entry(product).State;
            //product.Name = "kalem 1";
            //context.Products.Update(product);
            //context.SaveChanges();

            //var state2 = context.Entry(product).State;
            return product;
        }

        public void Create(Subscriber product)
        {
            var state = context.Entry(product).State;
            context.Entry(product).State = EntityState.Added;

            // context.Products.Add(product); // change tracker ?
            var state2 = context.Entry(product).State;


            context.SaveChanges();
        }

        public void Update(Subscriber product)
        {
            // context.Entry(product).State = EntityState.Modified;
            context.Subscribers.Update(product);
            context.SaveChanges();
        }

        public void Delete(Subscriber productToDelete)
        {
            //context.Entry(productToDelete).State = EntityState.Deleted;
            context.Subscribers.Remove(productToDelete);

            context.SaveChanges();
        }

        public Subscriber? GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}