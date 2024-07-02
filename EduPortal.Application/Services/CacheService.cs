using AutoMapper;
using EduPortal.Application.DTO_s.Subscriber;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Redis.Sentinel.Services;
using StackExchange.Redis;

namespace EduPortal.Application.Services
{
    public class CacheService(IMapper mapper,
        IDistributedCache distributedCache,
        ISubsIndividualRepository subsIndividualRepository,
        ISubsCorporateRepository subsCorporateRepository,
        IInvoiceRepository invoiceRepository,
        ISubscriberRepository subscriberRepository) : ICacheService
    {
        public async Task CacheSubscribersAsync()
        {
            var redis = await RedisService.RedisMasterDatabase(); // Redis master veritabanına bağlan

            var subsIndividuals = await subsIndividualRepository.GetAllAsync(x => true);
            var subsCorporates = await subsCorporateRepository.GetAllAsync(x => true);
            var subscribers = await subscriberRepository.GetAllAsync(x => true);

            var selectedSubsIndividuals = subsIndividuals.Take(700);
            var selectedSubsCorporates = subsCorporates.Take(700);
            var subscriberList = subscribers.Take(2500);

            var subscribersJson = JsonConvert.SerializeObject(subscriberList);

            await redis.StringSetAsync("all_subscribers", subscribersJson, expiry: TimeSpan.FromMinutes(43)); // Redis'e veriyi kaydet

            foreach (var subscriber in selectedSubsIndividuals)
            {
                var serializedSubscriber = JsonConvert.SerializeObject(subscriber);
                string cacheKey = subscriber.CounterNumber;
                await redis.StringSetAsync($"subscribersIdenetity:{subscriber.IdentityNumber}", cacheKey, when: When.Always, flags: CommandFlags.None, expiry: TimeSpan.FromMinutes(44)); // Redis'e veriyi kaydet
            }

            foreach (var subscriber in selectedSubsCorporates)
            {
                var serializedSubscriber = JsonConvert.SerializeObject(subscriber);
                string cacheKey = subscriber.CounterNumber;
                await redis.StringSetAsync($"subscribersIdenetity:{subscriber.TaxIdNumber}", cacheKey, when: When.Always, flags: CommandFlags.None, expiry: TimeSpan.FromMinutes(44)); // Redis'e veriyi kaydet
            }

        }

        public async Task<SubsIndividualDto> FindIndividualDtosAsync(string identityOrCounterNumber)
        {
            var redis = await RedisService.RedisMasterDatabase();
            var subscribersJson = await redis.StringGetAsync("all_subscribers");

            if (string.IsNullOrEmpty(subscribersJson))
            {
                return null;
            }

            var subscribers = JsonConvert.DeserializeObject<List<SubsIndividualDto>>(subscribersJson);


            if (identityOrCounterNumber.Length > 7)
            {
                var counterNumber = await FindCounterNumber(identityOrCounterNumber);
                var subscriber2 = subscribers.FirstOrDefault(s => s.CounterNumber == counterNumber);
                return subscriber2;
            }
            else
            {
                var subscriber = subscribers.FirstOrDefault(s => s.CounterNumber == identityOrCounterNumber);
                return subscriber;
            }
        }

        public async Task<string> FindCounterNumber(string identityOrCounterNumber)
        {
            var redis = await RedisService.RedisMasterDatabase();
            var cachedData = await redis.StringGetAsync($"subscribersIdenetity:{identityOrCounterNumber}");
            return cachedData;
        }

        public async Task AddOrUpdateCacheAsync<T>(string cacheKey, T data, TimeSpan expirationTime)
        {
            var redis = await RedisService.RedisMasterDatabase();
            var serializedData = JsonConvert.SerializeObject(data);
            await redis.StringSetAsync(cacheKey, serializedData, expiry: expirationTime);
        }

        public async Task UpdateCacheAsync<T>(string cacheKey, T newData)
        {
            var redis = await RedisService.RedisMasterDatabase();
            var serializedData = JsonConvert.SerializeObject(newData);
            await redis.StringSetAsync(cacheKey, serializedData);
        }

        public async Task InvalidateCacheAsync(string cacheKey)
        {
            var redis = await RedisService.RedisMasterDatabase();
            await redis.KeyDeleteAsync(cacheKey);
        }
    }
}
