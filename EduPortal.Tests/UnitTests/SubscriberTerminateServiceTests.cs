using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Messaging;
using EduPortal.Application.Services;
using EduPortal.Domain.Entities;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Tests.UnitTests
{

    public class SubscriberTerminateServiceTests
    {
        //[Fact]
        //public async Task TerminateSubscriptionAndAddToOutbox_ShouldAddOutboxMessageAndPublishToRabbitMQ()
        //{
        //    // Arrange
        //    var outboxMessageRepositoryMock = new Mock<IGenericRepository<OutboxMessage, int>>();
        //    var rabbitMQPublisherMock = new Mock<IRabbitMQPublisherService>();
        //    var rabbitMqConsumerMock = new Mock<RabbitMQConsumerService>();

        //    var subscriberTerminateService = new SubscriberTerminateService(
        //        outboxMessageRepositoryMock.Object,
        //        rabbitMQPublisherMock.Object,
        //        rabbitMqConsumerMock.Object);

        //    var subscriberId = 1;

        //    // Act
        //    await subscriberTerminateService.TerminateSubscriptionAndAddToOutbox(subscriberId);

        //    // Assert
        //    outboxMessageRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<OutboxMessage>()), Times.Once);
        //    rabbitMQPublisherMock.Verify(publisher => publisher.StartPublishing(), Times.Once);
        //    rabbitMqConsumerMock.Verify(consumer => consumer.StartConsuming(), Times.Once);
        //}
    }
}
