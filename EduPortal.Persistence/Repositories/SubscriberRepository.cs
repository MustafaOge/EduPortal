using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{
    public class SubscriberRepository : GenericRepository<Subscriber, int>, ISubscriberRepository
    {
        public SubscriberRepository(AppDbContext context) : base(context)
        {
        }
    }
}
