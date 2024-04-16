using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface ISubsIndividualRepository : IGenericRepository<SubsIndividual, int> 
    {

        Task<List<SubsIndividual>> FindIndividualAsync(string number);

        //Task CreateIndividualSubscription(SubsIndividual subsIndividual);

        //IReadOnlyList<SubsIndividual> GetAll();
        //SubsIndividual? GetById(int id);
        //void Create(SubsIndividual individual);
        //void Update(SubsIndividual individual);
        //void Delete(SubsIndividual individualToDelete);
        //SubsIndividual? GetByName(string name);
    }




}
