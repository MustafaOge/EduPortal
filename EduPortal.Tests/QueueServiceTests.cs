using AutoMapper;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Services;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace EduPortal.Tests;

public class QueueServiceTests
{
    [Fact]
    public async Task GetUpcomingPaymentInvoices_ShouldReturnSuccessResponse_WhenInvoicesAreFound()
    {
        // Arrange
        var outboxMessageRepositoryMock = new Mock<IGenericRepository<OutboxMessage, int>>();
        var queueRepositoryMock = new Mock<IQueueRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();

        var queueService = new QueueService(outboxMessageRepositoryMock.Object,
                                            queueRepositoryMock.Object,
                                            unitOfWorkMock.Object,
                                            subscriberRepositoryMock.Object,
                                            invoiceRepositoryMock.Object);

        var invoices = new List<(int Id, int SubscriberId)> { (1, 1), (2, 2) };
        var subscribers = new List<Subscriber> { new Subscriber { Id = 1 }, new Subscriber { Id = 2 } };

        queueRepositoryMock.Setup(repo => repo.GetUnpaidInvoices()).ReturnsAsync(invoices);
        subscriberRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Subscriber, bool>>>()))
                                 .ReturnsAsync(subscribers);

        // Act
        var response = await queueService.GetUpcomingPaymentInvoices();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.NotEmpty(response.Data);
    }

    [Fact]
    public async Task GetUpcomingPaymentInvoices_ShouldReturnFailResponse_WhenNoInvoicesAreFound()
    {
        // Arrange
        var outboxMessageRepositoryMock = new Mock<IGenericRepository<OutboxMessage, int>>();
        var queueRepositoryMock = new Mock<IQueueRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();

        var queueService = new QueueService(outboxMessageRepositoryMock.Object,
                                            queueRepositoryMock.Object,
                                            unitOfWorkMock.Object,
                                            subscriberRepositoryMock.Object,
                                            invoiceRepositoryMock.Object);

        var invoices = new List<(int Id, int SubscriberId)> { };
        var subscribers = new List<Subscriber> { };

        queueRepositoryMock.Setup(repo => repo.GetUnpaidInvoices()).ReturnsAsync(invoices);
        subscriberRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Subscriber, bool>>>()))
                                 .ReturnsAsync(subscribers);

        // Act
        var response = await queueService.GetUpcomingPaymentInvoices();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Null(response.Data);
        Assert.False(string.IsNullOrEmpty(response.Errors.FirstOrDefault()));
    }

    [Fact]
    public async Task MessageProccesed_ShouldUpdateOutboxMessageAndCommitUnitOfWork()
    {
        // Arrange
        var outboxMessageRepositoryMock = new Mock<IGenericRepository<OutboxMessage, int>>();
        var queueRepositoryMock = new Mock<IQueueRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
        var invoiceRepositoryMock = new Mock<IInvoiceRepository>();

        var queueService = new QueueService(outboxMessageRepositoryMock.Object,
                                            queueRepositoryMock.Object,
                                            unitOfWorkMock.Object,
                                            subscriberRepositoryMock.Object,
                                            invoiceRepositoryMock.Object);

        var message = new OutboxMessage();

        // Act
        await queueService.MessageProccesed(message);

        // Assert
        outboxMessageRepositoryMock.Verify(repo => repo.Update(message), Times.Once);
        unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }
}
