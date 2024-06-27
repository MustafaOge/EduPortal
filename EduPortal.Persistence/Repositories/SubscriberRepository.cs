using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{
    public class SubscriberRepository : GenericRepository<Subscriber, int>, ISubscriberRepository
    {
        private readonly AppDbContext _context;
        public SubscriberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasUnpaidInvoices(int subscriberId)
        {
            return await Task.FromResult(_context.Invoices.Any(i => i.SubscriberId == subscriberId && !i.IsPaid));
        }
        public async Task<SubscriberInfo> GetSubscriberInfoAsync(int counterNumber)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.CounterNumber == counterNumber.ToString());

            if (subscriber == null)
            {
                return null;
            }

            var subscriberInfo = new SubscriberInfo();

            if (subscriber.SubscriberType == "bireysel")
            {
                subscriberInfo.Individual = await _context.Individuals.FirstOrDefaultAsync(x => x.CounterNumber == counterNumber.ToString());
            }
            else
            {
                subscriberInfo.Corprorate = await _context.Corprorates.FirstOrDefaultAsync(x => x.CounterNumber == counterNumber.ToString());
            }

            return subscriberInfo;
        }

        public async Task<string> FindSubscriberAsync(string counterOrIdentityOrTaxIdNumber)
        {
            // Durum 1: Sayaç numarası ise
            if (counterOrIdentityOrTaxIdNumber.Length == 7)
            {
                // Sayaç numarasına göre müşteri tipini bul
                var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.CounterNumber == counterOrIdentityOrTaxIdNumber);
            

                // Bulunan müşteri tipine göre sorgu yap
                if (subscriber.SubscriberType  == "Bireysel")
                {
                    var individual = await _context.Individuals
                        .FirstOrDefaultAsync(s => s.CounterNumber == counterOrIdentityOrTaxIdNumber);

                    if (individual != null)
                    {
                        return $"Sayın {individual.NameSurname},\n\n sayaç numaralı: {individual.CounterNumber},\n\n" +
                               $"Aboneliğiniz elektrik dağıtım hizmeti için sonlandırılmıştır. " +
                               $"Artık EduPortal elektrik dağıtım uygulamasından faydalanamayacaksınız.\n\n" +
                               $"Eğer bu bir hata ise lütfen bizimle iletişime geçiniz.\n\n" +
                               $"Saygılarımızla,\nEduPortal Ekibi";
                    }
                }
                else if (subscriber.SubscriberType == "Kurumsal")
                {
                    var corporate = await _context.Corprorates
                        .FirstOrDefaultAsync(s => s.CounterNumber == counterOrIdentityOrTaxIdNumber);

                    if (corporate != null)
                    {
                        return $"Sayın {corporate.CorporateName},\n\n sayaç numaralı: {corporate.CounterNumber},\n\n" +
                               $"Aboneliğiniz elektrik dağıtım hizmeti için sonlandırılmıştır. " +
                               $"Artık EduPortal elektrik dağıtım uygulamasından faydalanamayacaksınız.\n\n" +
                               $"Eğer bu bir hata ise lütfen bizimle iletişime geçiniz.\n\n" +
                               $"Saygılarımızla,\nEduPortal Ekibi";
                    }
                }
            }
            // Durum 2: TCKimlik numarası ise
            else if (counterOrIdentityOrTaxIdNumber.Length == 11)
            {
                var individual = await _context.Individuals
                    .FirstOrDefaultAsync(s => s.IdentityNumber == counterOrIdentityOrTaxIdNumber);

                if (individual != null)
                {
                    return $"Sayın {individual.NameSurname},\n\n sayaç numaralı: {individual.CounterNumber},\n\n" +
                           $"Aboneliğiniz elektrik dağıtım hizmeti için sonlandırılmıştır. " +
                           $"Artık EduPortal elektrik dağıtım uygulamasından faydalanamayacaksınız.\n\n" +
                           $"Eğer bu bir hata ise lütfen bizimle iletişime geçiniz.\n\n" +
                           $"Saygılarımızla,\nEduPortal Ekibi";
                }
            }
            // Durum 3: Tax ID ise
            else if (counterOrIdentityOrTaxIdNumber.Length == 10)
            {
                var corporate = await _context.Corprorates
                    .FirstOrDefaultAsync(s => s.TaxIdNumber == counterOrIdentityOrTaxIdNumber);

                if (corporate != null)
                {
                    return $"Sayın {corporate.CorporateName},\n\n sayaç numaralı: {corporate.CounterNumber},\n\n" +
                           $"Aboneliğiniz elektrik dağıtım hizmeti için sonlandırılmıştır. " +
                           $"Artık EduPortal elektrik dağıtım uygulamasından faydalanamayacaksınız.\n\n" +
                           $"Eğer bu bir hata ise lütfen bizimle iletişime geçiniz.\n\n" +
                           $"Saygılarımızla,\nEduPortal Ekibi";
                }
            }

            return null; // Uygun durum bulunamadı
        }

        public async Task<string> CreateInvoiceReminderMessage(int invoiceId, int subscriberId)
        {
            // Abone bilgilerini al
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s=>s.Id == subscriberId);
            var invoice = await _context.Invoices.FirstOrDefaultAsync(s => s.Id == invoiceId);

            if (subscriber == null)
            {
                Console.WriteLine("Geçersiz abone ID'si.");
                return null;
            }

            // Abonenin tipine göre işlem yap
            string subscriberType = subscriber.SubscriberType;
            if (subscriberType == "Bireysel")
            {
                var individual = await _context.Individuals.FirstOrDefaultAsync(s => s.CounterNumber == subscriber.CounterNumber);
                if (individual != null)
                {
                    return $"Sayın {individual.NameSurname},\n\n" +
                           $"Tesisat ID: {individual.CounterNumber} ile ilgili {invoice.Amount} tutarında, {invoice.DueDate} son ödeme tarihli ödenmemiş bir faturanız bulunmaktadır.\n" +
                           $"Lütfen en kısa sürede faturayı ödeyiniz.\n\n" +
                           $"Saygılarımızla,\nEduPortal Ekibi";
                }
            }
            else if (subscriberType == "Kurumsal")
            {
                var corporate = await _context.Corprorates.FirstOrDefaultAsync(s => s.CounterNumber == subscriber.CounterNumber);
                if (corporate != null)
                {
                    return $"Sayın {corporate.CorporateName},\n\n" +
                           $"Tesisat ID: {corporate.CounterNumber} ile ilgili {invoice.Amount} tutarında,{invoice.DueDate} son ödeme tarihli ödenmemiş bir faturanız bulunmaktadır.\n" +
                           $"Lütfen en kısa sürede faturayı ödeyiniz.\n\n" +
                           $"Saygılarımızla,\nEduPortal Ekibi";
                }
            }

            return null; // Uygun durum bulunamadı
        }



        public async Task<List<Subscriber>> GetSubscribersByCounterNumberAsync(string counterNumber)
        {
            return await _context.Subscribers
                .Where(s => s.CounterNumber == counterNumber)
                .ToListAsync();
        }



    }
}
