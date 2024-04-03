using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice, int>, IInvoiceRepository
    {
        private readonly IGenericRepository<SubsCorporate, int> _genericRepository;
        public DbContext _context;

        public InvoiceRepository(AppDbContext context, IGenericRepository<SubsCorporate, int> genericRepository) : base(context)
        {
            _genericRepository = genericRepository;
            _context = context;
        }


    }
}
