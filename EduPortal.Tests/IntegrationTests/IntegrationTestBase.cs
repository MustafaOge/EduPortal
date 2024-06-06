using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Tests.IntegrationTests
{

    using EduPortal.Persistence.context;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading.Tasks;

    public abstract class IntegrationTestBase : IDisposable
    {
        protected readonly AppDbContext _context;

        protected IntegrationTestBase()
        {
            // Setup HttpContextAccessor and DistributedCache
            var httpContextAccessor = new HttpContextAccessor();
            var cacheOptions = Options.Create(new MemoryDistributedCacheOptions());
            var cache = new MemoryDistributedCache(cacheOptions);

            // In-memory database configuration
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options, httpContextAccessor, cache);

            // Seed the database
            SeedDatabase().Wait();
        }

        protected abstract Task SeedDatabase();

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
