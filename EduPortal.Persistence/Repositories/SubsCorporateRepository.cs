
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Abstractions;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<SubsCorporate>> FindCorporateAsync(string TaxIdNumber)
        {
                        return await _dbSet.Where(c => c.TaxIdNumber == TaxIdNumber).ToListAsync();


        }


        public async Task CreateCorporateSubscription(SubsCorporate subsCorporate)
        {
            await _genericRepository.AddAsync(subsCorporate);
        }

        //public async Task<List<SubsCorporate>> FindCorporate(string taxIdNumber)
        //{
        //    return await _context.Corprorates.Where(c => c.TaxIdNumber == taxIdNumber).ToListAsync();
        //}

  
    }
}