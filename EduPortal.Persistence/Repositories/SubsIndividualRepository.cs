
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Abstractions;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using EduPortal.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

