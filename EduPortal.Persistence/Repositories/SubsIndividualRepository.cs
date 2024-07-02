
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;

namespace EduPortal.Persistence.Repositories
{
    public class SubsIndividualRepository : GenericRepository<SubsIndividual, int>, ISubsIndividualRepository
    {
        private readonly IGenericRepository<SubsIndividual, int> _genericRepository;
        public readonly AppDbContext _context;

        public SubsIndividualRepository(AppDbContext context, IGenericRepository<SubsIndividual, int> genericRepository) : base(context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        public async Task<SubsIndividual> FindIndividualAsync(string counterOrIdentityNumber)
        {
            var response = await _dbSet
                .Where(c => (counterOrIdentityNumber.Length > 10 ? c.IdentityNumber : c.CounterNumber) == counterOrIdentityNumber && c.IsActive)
                .FirstOrDefaultAsync();
            return response;
        }

    }
}

