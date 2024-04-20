using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using EduPortal.Domain.Entities;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EduPortal.Persistence.Interceptors
{
    internal class SaveChangesInterceptor
          : Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor
    {
        readonly IHttpContextAccessor _contextAccessor;
        readonly IDistributedCache _distributedCache;
        public SaveChangesInterceptor(IHttpContextAccessor contextAccessor   , IDistributedCache distributedCache)
        {
            _contextAccessor = contextAccessor;
            _distributedCache = distributedCache;

        }

        readonly Dictionary<EntityState, Action<DbContext, BaseEntity<int>, string>> _actions = new()
        {
            { EntityState.Added, AddEntity },
            { EntityState.Modified, ModifiedEntity }
        };

        public async Task UpdateCacheAsync<T>( T newData)
        {
            var serializedData = JsonConvert.SerializeObject(newData);
            await _distributedCache.SetStringAsync("all_subscribers", serializedData);
        }
        public async Task InvalidateCacheAsync(string cacheKey)
        {
            await _distributedCache.RemoveAsync(cacheKey);
        }

        public async Task AddOrUpdateCacheAsync<T>( T data, TimeSpan expirationTime)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            await _distributedCache.SetStringAsync("all_subscribers", serializedData, options: new DistributedCacheEntryOptions
            {
                SlidingExpiration = expirationTime
            });
        }


        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var dbContext = eventData.Context;

            if (dbContext is null) return base.SavingChanges(eventData, result);


            var context = _contextAccessor.HttpContext;
            var userId = context?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            userId ??= "10";


            var entities = dbContext.ChangeTracker.Entries<BaseEntity<int>>().ToList();
            foreach (var entry in entities)
            {
                var baseEntity = entry.Entity;

                _actions[entry.State](dbContext, baseEntity, userId);

                Task.Run(async () => await InvalidateCacheAsync(baseEntity.Id.ToString()));

                // Yeni eklenen ve güncellenen öğeleri Redis'e ekle veya güncelle
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    Task.Run(async () =>
                    {
                        await AddOrUpdateCacheAsync( baseEntity, TimeSpan.FromMinutes(60));
                    });
                }

                switch (entry.State)
                {
                    case EntityState.Added:

                        AddEntity(dbContext, baseEntity, userId);

                        break;
                    case EntityState.Modified:
                        ModifiedEntity(dbContext, baseEntity, userId);

                        break;
                }
            }

            return base.SavingChanges(eventData, result);
        }

        private static void ModifiedEntity(DbContext dbContext, BaseEntity<int> baseEntity, string userId)
        {
            dbContext.Entry(baseEntity).Property(x => x.Created).IsModified = false;
            dbContext.Entry(baseEntity).Property(x => x.CreatedByUser).IsModified = false;
            baseEntity.UpdatedByUser = Convert.ToInt32(userId);
            baseEntity.Updated = DateTime.Now;
        }

        private static void AddEntity(DbContext dbContext, BaseEntity<int> baseEntity, string userId)
        {
            dbContext.Entry(baseEntity).Property(x => x.Updated).IsModified = false;
            dbContext.Entry(baseEntity).Property(x => x.UpdatedByUser).IsModified = false;
            baseEntity.Created = DateTime.Now;
            baseEntity.CreatedByUser = Convert.ToInt32(userId);
        }
    }
}