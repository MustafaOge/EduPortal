
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
    public class SubsIndividualRepository : GenericRepository<SubsIndividual, int>, ISubsIndividualRepository
    {
        private readonly IGenericRepository<SubsIndividual, int> _genericRepository;

        public SubsIndividualRepository(AppDbContext context, IGenericRepository<SubsIndividual, int> genericRepository) : base(context)
        {
            _genericRepository = genericRepository;
        }


        public async Task CreateIndividualSubscription(SubsIndividual subsIndividual)
        {
            await _genericRepository.AddAsync(subsIndividual);
        }
    }
}