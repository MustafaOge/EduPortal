
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;

namespace EduPortal.Persistence.Repositories
{
    public class SubsCorporateRepository : GenericRepository<SubsCorporate, int>, ISubsCorporateRepository
    {
        private readonly IGenericRepository<SubsCorporate, int> _genericRepository;
        public DbContext _context;
        public SubsCorporateRepository(AppDbContext context, IGenericRepository<SubsCorporate, int> genericRepository) : base(context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        public async Task<SubsCorporate> FindCorporateAsync(string number)
        {
            
            var response = await _dbSet
                .Where(c => (number.Length > 9 ? c.TaxIdNumber : c.CounterNumber) == number && c.IsActive)
                       .FirstOrDefaultAsync();
            return response;
        }
    }
}