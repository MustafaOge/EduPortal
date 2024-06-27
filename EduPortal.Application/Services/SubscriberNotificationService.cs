using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using EduPortal.Domain.Enums;
using EduPortal.Domain.Entities;

namespace EduPortal.Application.Services
{
    public class SubscriberNotificationService
    {
        private readonly IMailService _mailService;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISubsIndividualRepository _subsIndividualRepository;

        public SubscriberNotificationService(
            IMailService mailService,
            ISubscriberRepository subscriberRepository,
            IInvoiceRepository invoiceRepository,
            ISubsIndividualRepository subsIndividualRepository)
        {
            _mailService = mailService;
            _subscriberRepository = subscriberRepository;
            _invoiceRepository = invoiceRepository;
            _subsIndividualRepository = subsIndividualRepository;
        }

        public async Task SendEmailNotification(MessageType messageType, string message)
        {
            try
            {
                switch (messageType)
                {
                    case MessageType.SubscriptionTermination:
                        await SendSubscriptionTerminationNotification(message);
                        break;
                    case MessageType.InvoiceReminder:
                        await SendInvoiceReminderNotification(message);
                        break;
                    case MessageType.OutageNotification:
                       await SendOutageNotification(message);
                        Console.WriteLine("Gönderilecek email mesajı: " + message);
                        //await SendInvoiceReminderNotification(message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
                }
            }
            catch (Exception ex)
            {
                // Hata yönetimi için loglama veya yeniden fırlatma (rethrow) yapılabilir
                // Örneğin loglama:
                Console.WriteLine($"Hata oluştu: {ex.Message}");

                // İstisnayı yeniden fırlat (rethrow):
                throw;
            }
        }
        public async Task SendOutageNotification(string message)
        {
            if (message == null)
            {
                throw new Exception("message is empty");
            }
            else
            {
                await SendEmail("Planlı Elektrik Kesintisi", message);
                //Console.WriteLine($"Sending outage notification: {message}");
            }
        }

        private async Task SendSubscriptionTerminationNotification(string message)
        {
            var subscriber = await _subscriberRepository.GetByIdAsync(Convert.ToInt32(message));
            if (subscriber == null)
            {
                throw new Exception("Subscriber not found");
            }

            var subscriberTerminateMessage = await _subscriberRepository.FindSubscriberAsync(subscriber.CounterNumber);
            await SendEmail("Abonelik Sonlandırma Bildirimi", subscriberTerminateMessage);
        }

        private async Task SendInvoiceReminderNotification(string message)
        {
            string[] messageParts = message.Split('-');
            if (messageParts.Length != 2)
            {
                throw new ArgumentException("Geçersiz mesaj formatı.");
            }

            try
            {
                int invoiceId = Convert.ToInt32(messageParts[0]);
                int subscriberId = Convert.ToInt32(messageParts[1]);

                var invoiceReminderMessage = await _subscriberRepository.CreateInvoiceReminderMessage(invoiceId, subscriberId);
                var subscriber = await _subscriberRepository.GetByIdAsync(subscriberId);
                if (subscriber == null)
                {
                    throw new Exception("Subscriber not found");
                }

                await SendEmail("Ödenmemiş Fatura Bildirimi", invoiceReminderMessage);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Geçersiz mesaj formatı.");
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentException("Geçersiz mesaj formatı.");
            }
        }


        private async Task SendEmail(string subject, string body)
        {
            _mailService.SendEmailWithMailKitPackage(subject, body, "mustafaoge1221@gmail.com");
            await Task.CompletedTask;
        }
    }

}
