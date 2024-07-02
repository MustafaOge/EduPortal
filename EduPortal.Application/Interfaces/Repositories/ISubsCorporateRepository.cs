using EduPortal.Domain.Entities;

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
