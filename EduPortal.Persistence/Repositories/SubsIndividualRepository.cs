
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
    public class SubscriberRepository(AppDbContext context) : ISubsIndividualRepository
    {

        public IReadOnlyList<SubsIndividual> GetAll()
        {
            //    return _context.Attach();
            return context.Individuals.AsNoTracking().ToList().AsReadOnly();

        }

        public SubsIndividual? GetById(int id)
        {
            var product = context.Individuals.First(x => x.Id == id);

            //var state = context.Entry(product).State;
            //product.Name = "kalem 1";
            //context.Products.Update(product);
            //context.SaveChanges();

            //var state2 = context.Entry(product).State;
            return product;
        }

        public void Create(SubsIndividual product)
        {
            var state = context.Entry(product).State;
            context.Entry(product).State = EntityState.Added;

            // context.Products.Add(product); // change tracker ?
            var state2 = context.Entry(product).State;


            context.SaveChanges();
        }

        public void Update(SubsIndividual individuals)
        {
            // context.Entry(product).State = EntityState.Modified;
            context.Individuals.Update(individuals);
            context.SaveChanges();
        }

        public void Delete(SubsIndividual productToDelete)
        {
            //context.Entry(productToDelete).State = EntityState.Deleted;
            context.Individuals.Remove(productToDelete);

            context.SaveChanges();
        }

        public SubsIndividual? GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}