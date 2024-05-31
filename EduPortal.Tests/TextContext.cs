using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Domain.Entities;
using Moq;
using Redis.Sentinel.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Tests;
public class TestContext
{
    public Mock<IDatabase> RedisDatabaseMock { get; }
    public Mock<RedisService> RedisServiceMock { get; }
    public Mock<ISubsIndividualRepository> SubsIndividualRepositoryMock { get; }
    public Mock<ISubsCorporateRepository> SubsCorporateRepositoryMock { get; }
    public Mock<ISubscriberRepository> SubscriberRepositoryMock { get; }
    public List<SubsIndividual> SubsIndividuals { get; }
    public List<SubsCorporate> SubsCorporates { get; }
    public List<Subscriber> Subscribers { get; }

    public TestContext()
    {
        RedisDatabaseMock = new Mock<IDatabase>();
        RedisServiceMock = new Mock<RedisService>();

        SubsIndividuals = Enumerable.Range(1, 1000).Select(i => new SubsIndividual { IdentityNumber = i.ToString(), CounterNumber = $"C{i}" }).ToList();
        SubsCorporates = Enumerable.Range(1, 1000).Select(i => new SubsCorporate { TaxIdNumber = i.ToString(), CounterNumber = $"C{i}" }).ToList();
        Subscribers = Enumerable.Range(1, 3000).Select(i => new Subscriber { CounterNumber = $"C{i}" }).ToList();

        SubsIndividualRepositoryMock = new Mock<ISubsIndividualRepository>();
        SubsIndividualRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<SubsIndividual, bool>>>())).ReturnsAsync(SubsIndividuals);

        SubsCorporateRepositoryMock = new Mock<ISubsCorporateRepository>();
        SubsCorporateRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<SubsCorporate, bool>>>())).ReturnsAsync(SubsCorporates);

        SubscriberRepositoryMock = new Mock<ISubscriberRepository>();
        SubscriberRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Subscriber, bool>>>())).ReturnsAsync(Subscribers);
    }
}




