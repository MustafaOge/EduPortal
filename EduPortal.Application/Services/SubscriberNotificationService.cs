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


                // subscriberRepository ve invoiceRepository gibi diğer gerekli scoped servisleri burada da alabilirsiniz
                try
                {
                    switch (messageType)
                    {

                        case MessageType.SubscriptionTermination:
                            var subscriberTerminate = subscriberRepository.GetByIdAsync(Convert.ToInt32(message));
                            string a =  subscriberTerminate.Result.CounterNumber;
                          //var subsIndividualTerminate =  subsIndividual.Where(x=>x.CounterNumber == a).AnyAsync();

                            string subscriptionTerminationMessage = $"Sayın {subscriberTerminate.Result.PhoneNumber},\n\n" +
                                $"Aboneliğiniz elektrik dağıtım hizmeti için sonlandırılmıştır. Artık EduPortal elektrik dağıtım uygulamasından faydalanamayacaksınız.\n" +
                                $"Eğer bu bir hata ise lütfen bizimle iletişime geçiniz.\n\n" +
                                $"Saygılarımızla,\nEduPortal Ekibi";

                            await SendEmail(mailService, "Abonelik Sonlandırma Bildirimi", subscriptionTerminationMessage, message);
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
                            //var invoice = invoiceRepository.GetByIdAsync(invoiceId);
                            //var subscriber = subscriberRepository.GetByIdAsync(subscriberId);


                            // E-posta mesajını oluştur
                            string reminderMessage = $"Sayın {invoiceId},\n\n" +
                                $"Tesisat ID: {invoiceId} ile ilgili {invoiceId} tutarında ödenmemiş bir faturanız bulunmaktadır.\n" +
                                $"Lütfen en kısa sürede faturayı ödeyiniz.\n\n" +
                                $"Saygılarımızla,\nEduPortal Ekibi";
                            await SendEmail(mailService, "Ödenmemiş Fatura Bildirimi", reminderMessage, message);

                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda burada işlem yapabilirsiniz
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
