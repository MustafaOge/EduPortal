using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface ISubsCorporateRepository : IGenericRepository<SubsCorporate, int> 
    {

        //Task<List<SubsCorporate>> FindCorporate(string taxIdNumber);

        Task<List<SubsCorporate>> FindCorporateAsync(string TaxIdNumber);


        //Task CreateIndividualSubscription(SubsIndividual subsIndividual);

        //IReadOnlyList<SubsIndividual> GetAll();
        //SubsIndividual? GetById(int id);
        //void Create(SubsIndividual individual);
        //void Update(SubsIndividual individual);
        //void Delete(SubsIndividual individualToDelete);
        //SubsIndividual? GetByName(string name);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    //public Task AddAsync(Subscriber entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task AddRangeAsync(IEnumerable<Subscriber> items)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> AnyAsync(Expression<Func<Subscriber, bool>> expression)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<IEnumerable<Subscriber>> GetAllAsync(Expression<Func<Subscriber, bool>> expression)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<Subscriber> GetByIdAsync(int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Remove(Subscriber entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void RemoveRange(IEnumerable<Subscriber> items)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Update(Subscriber entity)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IQueryable<Subscriber> Where(Expression<Func<Subscriber, bool>> expression)
    //    {
    //        throw new NotImplementedException();
    //    }
}
