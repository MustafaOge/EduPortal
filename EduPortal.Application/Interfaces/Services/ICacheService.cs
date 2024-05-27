using EduPortal.Application.DTO_s.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task CacheSubscribersAsync();
        Task<SubsIndividualDto> FindIndividualDtosAsync(string identityOrCounterNumber); 
        Task<string> FindCounterNumber(string CounterNumber);
        Task InvalidateCacheAsync(string cacheKey);
        Task AddOrUpdateCacheAsync<T>(string cacheKey, T data, TimeSpan expirationTime);
        Task UpdateCacheAsync<T>(string cacheKey, T newData);
    }
}
