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
        /// <summary>
        /// Retrieves a list of individual subscribers based on the provided number.
        /// </summary>
        /// <param name="number">The number to search for individual subscribers.</param>
        /// <returns>A list of individual subscribers associated with the provided number.</returns>
        Task<List<SubsIndividual>> FindIndividualAsync(string counterOrIdentityNumber);
    }
}
