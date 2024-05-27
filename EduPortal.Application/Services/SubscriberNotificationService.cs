using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using EduPortal.Domain.Enums;
using EduPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduPortal.Application.Services
{
    public class SubscriberNotificationService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SubscriberNotificationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task SendEmailNotification(MessageType messageType, string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                var subscriberRepository = scope.ServiceProvider.GetRequiredService<ISubscriberRepository>();
                var invoiceRepository = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
                var subsIndividual = scope.ServiceProvider.GetRequiredService<ISubsIndividualRepository>();


                try
                {
                    switch (messageType)
                    {

                        case MessageType.SubscriptionTermination:
                            
                            var subscriber = await subscriberRepository.GetByIdAsync(Convert.ToInt32(message));

                            var subscriberTerminateMessage = await subscriberRepository.FindSubscriberAsync(subscriber.CounterNumber);

                                await SendEmail(mailService, "Abonelik Sonlandırma Bildirimi", subscriberTerminateMessage, message);
                                                     
                                break;
                           

                        case MessageType.InvoiceReminder:
                            string[] messageParts = message.Split('-');
                            if (messageParts.Length != 2)
                            {
                                Console.WriteLine("Geçersiz mesaj formatı.");
                                return;
                            }
                            int invoiceId = Convert.ToInt32(messageParts[0]);
                            int subscriberId = Convert.ToInt32(messageParts[1]);
                            var InvoiceReminderMessage = await subscriberRepository.CreateInvoiceReminderMessage(invoiceId, subscriberId);



                            await SendEmail(mailService, "Ödenmemiş Fatura Bildirimi", InvoiceReminderMessage, message);

                            break;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                }
            }
        }

        private async Task SendEmail(IMailService mailService, string subject, string body, string recipient)
        {
            mailService.SendEmailWithMailKitPackage(subject, body, "mustafaoge1221@gmail.com");
            await Task.CompletedTask; 
        }
    }
}
