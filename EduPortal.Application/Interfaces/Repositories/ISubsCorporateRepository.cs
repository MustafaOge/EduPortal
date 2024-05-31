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
        /// <summary>
        /// Finds corporations based on a specific tax identification number or counter number and lists the active ones.
        /// </summary>
        /// <param name="number">Tax identification number or counter number.</param>
        /// <returns>A list of corporations with a specific tax identification number or counter number and are active.</returns>
        Task<SubsCorporate> FindCorporateAsync(string TaxIdNumber);
    }
}
