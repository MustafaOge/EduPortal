using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Messaging;
using EduPortal.Domain.Entities;
using EduPortal.Domain.Enums;

namespace EduPortal.Application.Services
{

    public class OutageNotificationService : IOutageNotificationService
    {
        private readonly IOutageNotificationRepository _outageNotificationRepository;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MessagePublisherService _publisherServiceMassTransit;
        private readonly IAddressRepository _addressRepository;


        public OutageNotificationService(
                IOutageNotificationRepository outageNotificationRepository,
                ISubscriberRepository subscriberRepository,
                IUnitOfWork unitOfWork,
           IAddressRepository addressRepository,

        MessagePublisherService publisherServiceMassTransit)
        {
            _addressRepository = addressRepository;
            _outageNotificationRepository = outageNotificationRepository;
            _subscriberRepository = subscriberRepository;
            _unitOfWork = unitOfWork;
            _publisherServiceMassTransit = publisherServiceMassTransit;
        }

        public List<string> GetDistricts()
        {
            var districts = _outageNotificationRepository.GetAll()
                .Select(o => o.District)
                .Distinct()
                .ToList();

            return districts;
        }

        public List<string> GetDistinctDates()
        {
            var dates = _outageNotificationRepository.GetDistinctDates();
            return dates.ToList();
        }

        public List<OutageNotification> GetOutagesByDateAndDistrict(DateTime selectedDate, string district)
        {
            var outages = _outageNotificationRepository.GetAll()
                .Where(o => o.Date.Date == selectedDate.Date && o.District == district)
                .ToList();

            return outages;
        }

        public Task SendOutageMessageAsync2()
        {
            // Implementasyonu gerektiği şekilde buraya ekleyin
            return Task.CompletedTask;
        }




        public async Task SendOutageMessageAsync()
        {
            // 1. İşlenmemiş kesinti bildirimlerini al
            var unprocessedOutages = _outageNotificationRepository
                .Where(x => x.IsProcessed == false)
                .ToList();

            // 2. Her bir bildirim için mahalle kimlik numaralarını ayıkla
            var mahalleKimlikNoList = unprocessedOutages
                .SelectMany(o => o.MahalleKimlikNumaralari)
                .Distinct()
                .ToList();

            // 3. Bu mahalle kimlik numaralarına göre Ad_IcKapi tablosundan iç kapı kayıtlarını bul
            var addresses = _addressRepository
                .Where(a => mahalleKimlikNoList.Contains(a.mahalleKimlikNo))
                .ToList();

            // 4. İç kapı kayıtlarından sayaç numaralarını al
            var counterNumbers = addresses
                .Select(a => a.counterNumber.ToString())
                .Distinct()
                .ToList();

            // 5. Sayaç numaralarına göre Subscribers tablosundan aboneleri bul
            var subscribers = _subscriberRepository
                .Where(s => counterNumbers.Contains(s.CounterNumber))
                .ToList();

            // Eğer subscribers listesi boşsa, tasks işlemlerine girme
            if (!subscribers.Any())
            {
                // Eğer subscribers listesi boşsa, devam etme
                return;
            }

            // 6. Her bir aboneye kesinti mesajını gönder
            foreach (var outage in unprocessedOutages)
            {
                var affectedSubscribers = subscribers
                    .Where(s => counterNumbers.Contains(s.CounterNumber))
                    .ToList();

                int counter = 0; // Abonelerin sayacı

                var subscriberTasks = affectedSubscribers.Select(async subscriber =>
                {
                    counter++; // Her abone için sayacı artır

                    string message = $"{subscriber.CounterNumber} Sayaç Numaralı abonemiz mahallenizde Elektrik kesintisi olacaktır:\n\n" +
                                     $"Başlangıç zamanı: {outage.StartTime}\n" +
                                     $"Bitiş zamanı: {outage.EndTime}\n\n" +
                                     $"Detaylı bilgi için destek ekibimizle iletişime geçebilirsiniz.";

                    await _publisherServiceMassTransit.SendMessageAsync(message, MessageType.OutageNotification);

                    await Task.Delay(TimeSpan.FromSeconds(10));

                });

                await Task.WhenAll(subscriberTasks);

                // 7. Kesinti bildirimini işlenmiş olarak işaretle
                outage.IsProcessed = true;
                await _outageNotificationRepository.UpdateAsync(outage);

                Console.WriteLine($"Toplam {counter} aboneye mesaj gönderildi."); // Her outage için gönderilen abone sayısını yazdır
            }

            //8.Değişiklikleri veritabanına kaydet
            await _unitOfWork.CommitAsync();
        }
    }
}
