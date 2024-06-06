using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Services;
using EduPortal.Domain.Entities;
using EduPortal.Domain.Enums;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EduPortal.Tests.UnitTests
{
    public class SubscriberNotificationServiceTests
    {
        [Fact]
        public async Task SendEmailNotification_ShouldSendSubscriptionTerminationNotification_WhenMessageTypeIsSubscriptionTermination()
        {
            // Arrange
            var mailServiceMock = new Mock<IMailService>();
            var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
            var notificationService = new SubscriberNotificationService(
                mailServiceMock.Object,
                subscriberRepositoryMock.Object,
                null,
                null);

            var subscriberId = "1";
            var subscriber = new Subscriber { Id = Convert.ToInt32(subscriberId), Email = "mustafaoge1221@gmail.com", CounterNumber = "12345" };
            var message = "Subscription termination message";

            subscriberRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(subscriber);
            subscriberRepositoryMock.Setup(repo => repo.FindSubscriberAsync(It.IsAny<string>())).ReturnsAsync(message);

            // Act
            await notificationService.SendEmailNotification(MessageType.SubscriptionTermination, subscriberId);

            // Assert
            mailServiceMock.Verify(mailService => mailService.SendEmailWithMailKitPackage(
                "Abonelik Sonlandırma Bildirimi", message, subscriber.Email), Times.Once);
        }

        [Fact]
        public async Task SendEmailNotification_ShouldSendInvoiceReminderNotification_WhenMessageTypeIsInvoiceReminder()
        {
            // Arrange
            var mailServiceMock = new Mock<IMailService>();
            var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
            var notificationService = new SubscriberNotificationService(
                mailServiceMock.Object,
                subscriberRepositoryMock.Object,
                null,
                null);

            var message = "1-2"; // Örnek mesaj formatı: invoiceId-subscriberId
            var subscriber = new Subscriber { Id = 2, Email = "mustafaoge1221@gmail.com" };
            var invoiceReminderMessage = "Invoice reminder message";

            subscriberRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(subscriber);
            subscriberRepositoryMock.Setup(repo => repo.CreateInvoiceReminderMessage(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(invoiceReminderMessage);

            // Act
            await notificationService.SendEmailNotification(MessageType.InvoiceReminder, message);

            // Assert
            mailServiceMock.Verify(mailService => mailService.SendEmailWithMailKitPackage(
                "Ödenmemiş Fatura Bildirimi", invoiceReminderMessage, subscriber.Email), Times.Once);
        }

        [Fact]
        public async Task SendEmailNotification_ShouldThrowArgumentOutOfRangeException_WhenMessageTypeIsInvalid()
        {
            // Arrange
            var mailServiceMock = new Mock<IMailService>();
            var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
            var notificationService = new SubscriberNotificationService(
                mailServiceMock.Object,
                subscriberRepositoryMock.Object,
                null,
                null);

            var message = "1";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                notificationService.SendEmailNotification((MessageType)999, message));
        }

        [Fact]
        public async Task SendSubscriptionTerminationNotification_ShouldThrowException_WhenSubscriberNotFound()
        {
            // Arrange
            var mailServiceMock = new Mock<IMailService>();
            var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
            var notificationService = new SubscriberNotificationService(
                mailServiceMock.Object,
                subscriberRepositoryMock.Object,
                null,
                null);

            var subscriberId = "1";

            subscriberRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Subscriber)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                notificationService.SendEmailNotification(MessageType.SubscriptionTermination, subscriberId));
        }

        [Fact]
        public async Task SendInvoiceReminderNotification_ShouldThrowException_WhenSubscriberNotFound()
        {
            // Arrange
            var mailServiceMock = new Mock<IMailService>();
            var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
            var notificationService = new SubscriberNotificationService(
                mailServiceMock.Object,
                subscriberRepositoryMock.Object,
                null,
                null);

            var message = "1-2"; // Örnek mesaj formatı: invoiceId-subscriberId

            subscriberRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Subscriber)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                notificationService.SendEmailNotification(MessageType.InvoiceReminder, message));
        }

        [Fact]
        public async Task SendInvoiceReminderNotification_ShouldThrowArgumentException_WhenMessageFormatIsInvalid()
        {
            // Arrange
            var mailServiceMock = new Mock<IMailService>();
            var subscriberRepositoryMock = new Mock<ISubscriberRepository>();
            var notificationService = new SubscriberNotificationService(
                mailServiceMock.Object,
                subscriberRepositoryMock.Object,
                null,
                null);

            var invalidMessage = "invalid-format";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                notificationService.SendEmailNotification(MessageType.InvoiceReminder, invalidMessage));
        }
    }
}
