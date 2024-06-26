﻿using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;

namespace EduPortal.Persistence.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice, int>, IInvoiceRepository
    {
        private readonly IGenericRepository<SubsCorporate, int> _genericRepository;
        public AppDbContext _context;
        

        public InvoiceRepository(AppDbContext context, IGenericRepository<SubsCorporate, int> genericRepository) : base(context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }

        public async Task<bool> AnyInvoiceAsync()
        {
            return await _context.Set<Invoice>().AnyAsync();
        }
    }
}
