//using AutoMapper;
//using EduPortal.Application.DTO_s.Subscriber;
//using EduPortal.Application.Interfaces.Repositories;
//using EduPortal.Application.Interfaces.Services;
//using EduPortal.Application.Services;
//using EduPortal.Domain.Entities;
//using Microsoft.Extensions.Caching.Distributed;
//using Moq;
//using Redis.Sentinel.Services;
//using StackExchange.Redis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;
//using Xunit;
//using Newtonsoft.Json;

//namespace EduPortal.Tests
//{
//    public class CacheServiceTests
//    {
//        private Mock<IDatabase> redisMock;
//        private Mock<RedisService> redisServiceMock;
//        private Mock<ISubsIndividualRepository> subsIndividualRepositoryMock;
//        private Mock<ISubsCorporateRepository> subsCorporateRepositoryMock;
//        private Mock<ISubscriberRepository> subscriberRepositoryMock;
//        private Mock<IInvoiceRepository> invoiceRepositoryMock;
//        private Mock<IMapper> mapperMock;
//        private Mock<IDistributedCache> cacheMock;
//        private CacheService cacheService;

//        public CacheServiceTests()
//        {
//            redisMock = new Mock<IDatabase>();
//            redisServiceMock = new Mock<IRedisService>();
//            subsIndividualRepositoryMock = new Mock<ISubsIndividualRepository>();
//            subsCorporateRepositoryMock = new Mock<ISubsCorporateRepository>();
//            subscriberRepositoryMock = new Mock<ISubscriberRepository>();
//            invoiceRepositoryMock = new Mock<IInvoiceRepository>();
//            mapperMock = new Mock<IMapper>();
//            cacheMock = new Mock<IDistributedCache>();

//            // Mock RedisService
//            Mock<IRedisService> redisServiceMock = new Mock<IRedisService>();
//            redisServiceMock.Setup(service => service.GetRedisMasterDatabaseAsync()).ReturnsAsync(redisMock.Object);

//            cacheService = new CacheService(
//                mapperMock.Object,
//                cacheMock.Object,
//                subsIndividualRepositoryMock.Object,
//                subsCorporateRepositoryMock.Object,
//                invoiceRepositoryMock.Object,
//                subscriberRepositoryMock.Object
//            );
//        }

//        [Fact]
//        public async Task CacheSubscribersAsync_ShouldCacheDataCorrectly()
//        {
//            // Arrange
//            var subsIndividuals = new List<SubsIndividual> { /* populate with test data */ };
//            var subsCorporates = new List<SubsCorporate> { /* populate with test data */ };
//            var subscribers = new List<Subscriber> { /* populate with test data */ };

//            subsIndividualRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<SubsIndividual, bool>>>())).ReturnsAsync(subsIndividuals);
//            subsCorporateRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<SubsCorporate, bool>>>())).ReturnsAsync(subsCorporates);
//            subscriberRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Subscriber, bool>>>())).ReturnsAsync(subscribers);

//            // Act
//            await cacheService.CacheSubscribersAsync();

//            // Assert
//            redisMock.Verify(db => db.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.AtLeastOnce);
//        }

//        [Fact]
//        public async Task FindIndividualDtosAsync_ShouldReturnSubscriber_WhenFoundByCounterNumber()
//        {
//            // Arrange
//            var subscribers = new List<SubsIndividualDto>
//    {
//        new SubsIndividualDto { CounterNumber = "1234567" }
//    };
//            var subscribersJson = JsonConvert.SerializeObject(subscribers);

//            // Mock setup
//            redisMock.Setup(db => db.StringGetAsync("all_subscribers", It.IsAny<CommandFlags>())).ReturnsAsync(subscribersJson);

//            // Act
//            var result = await cacheService.FindIndividualDtosAsync("1234567");

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal("1234567", result.CounterNumber);
//        }


//        [Fact]
//        public async Task FindIndividualDtosAsync_ShouldReturnNull_WhenSubscribersJsonIsEmpty()
//        {
//            // Arrange
//            redisMock.Setup(db => db.StringGetAsync("all_subscribers", It.IsAny<CommandFlags>())).ReturnsAsync((string)null);

//            // Act
//            var result = await cacheService.FindIndividualDtosAsync("1234567");

//            // Assert
//            Assert.Null(result);
//        }
//    }
//}
