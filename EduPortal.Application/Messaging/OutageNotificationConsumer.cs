using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Messaging;
using EduPortal.Domain.Entities;
using MassTransit;
using Newtonsoft.Json;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

namespace EduPortal.Application.Messaging
{
    public class OutageNotificationConsumer : IConsumer<IMessage>
    {
 
        private readonly IAddressRepository _adMahalleRepository;
        private readonly IGenericRepository<OutageNotification, int> _outageNotificationRepository;

        public OutageNotificationConsumer(IGenericRepository<OutageNotification, int> outageNotificationRepository, IAddressRepository adMahalleRepository)
        {
            _outageNotificationRepository = outageNotificationRepository;
            
            _adMahalleRepository = adMahalleRepository;
        }

        public async Task Consume(ConsumeContext<IMessage> context)
        {
            var message = context.Message;

            // Gelen mesaj içeriğini yazdırma
            Console.WriteLine($"Gelen mesaj içeriği: {JsonConvert.SerializeObject(message)}");

            // PowerOutageItem'ı OutageNotification'a dönüştürme
            if (message.powerOutage is PowerOutageItem powerOutage)
            {
                var outageNotification = new OutageNotification
                {
                    Province = powerOutage.Province,
                    District = powerOutage.District,
                    Date = powerOutage.Date,
                    DistributionCompanyName = powerOutage.DistributionCompanyName,
                    StartTime = powerOutage.StartTime,
                    EndTime = powerOutage.EndTime,
                    Reason = powerOutage.Reason,
                    EffectedNeighbourhoods = powerOutage.EffectedNeighbourhoods,
                    EffectedSubscribers = powerOutage.EffectedSubscribers,
                    HourlyLoadAvg = powerOutage.HourlyLoadAvg,
                };

                if (!string.IsNullOrEmpty(outageNotification.EffectedNeighbourhoods))
                {
                    var mahalleAdlari = outageNotification.EffectedNeighbourhoods.Split(new[] { " | " }, StringSplitOptions.None);
                    foreach (var mahalleAdi in mahalleAdlari)
                    {
                        var mahalleKimlikNo = await _adMahalleRepository.GetMahalleKimlikNoByNameAsync(mahalleAdi);
                        if (mahalleKimlikNo  != null) 
                        {
                            outageNotification.MahalleKimlikNumaralari.Add(mahalleKimlikNo);
                        }
                    }
                }

                await _outageNotificationRepository.AddAsync(outageNotification);
                Console.WriteLine("Mesaj başarılı bir şekilde kaydedildi.");
            }
            else
            {
                Console.WriteLine("Mesaj deserializasyonu başarısız.");
            }
        }


        //public async Task Consume(ConsumeContext<IMessage> context)
        //{
        //    var message = context.Message;

        //    // Gelen mesaj içeriğini yazdırma
        //    Console.WriteLine($"Gelen mesaj içeriği: {JsonConvert.SerializeObject(message)}");

        //    // PowerOutageItem'ı OutageNotification'a dönüştürme
        //    if (message.powerOutage is PowerOutageItem powerOutage)
        //    {
        //        var outageNotification = new OutageNotification
        //        {
        //            Province = powerOutage.Province,
        //            District = powerOutage.District,
        //            Date = powerOutage.Date,
        //            DistributionCompanyName = powerOutage.DistributionCompanyName,
        //            StartTime = powerOutage.StartTime,
        //            EndTime = powerOutage.EndTime,
        //            Reason = powerOutage.Reason,
        //            EffectedNeighbourhoods = powerOutage.EffectedNeighbourhoods,
        //            EffectedSubscribers = powerOutage.EffectedSubscribers,
        //            HourlyLoadAvg = powerOutage.HourlyLoadAvg,
        //            // mahalleKimlikNo bu aşamada atanmamış olabilir, aşağıda gerekli işlem yapılabilir
        //        };

        //        var mahalleAdi = outageNotification.EffectedNeighbourhoods?.Split(" | ").FirstOrDefault();
        //        if (!string.IsNullOrEmpty(mahalleAdi))
        //        {
        //            outageNotification.MahalleKimlikNumaralari = await _adMahalleRepository.GetMahalleKimlikNoByNameAsync(mahalleAdi);
        //        }

        //        await _outageNotificationRepository.AddAsync(outageNotification);
        //        Console.WriteLine("Mesaj başarılı bir şekilde kaydedildi.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Mesaj deserializasyonu başarısız.");
        //    }
        //}



        //public async Task Consume(ConsumeContext<IMessage> context)
        //{
        //    var message = context.Message;
        //    OutageNotification msg = context.Message.powerOutage;


        //    // Gelen mesaj içeriğini yazdırma
        //    Console.WriteLine($"Gelen mesaj içeriği: {JsonConvert.SerializeObject(message)}");

        //    // Mesajı OutageNotification modeline dönüştürme
        //    //var outageNotifications = JsonConvert.DeserializeObject<List<OutageNotification>>(JsonConvert.SerializeObject(message));

        //   List<OutageNotification> ListA = new List<OutageNotification>();
        //    ListA.Add(msg);

        //    if (ListA != null && ListA.Count > 0)
        //    {
        //        foreach (var outageNotification in ListA)
        //        {
        //            var mahalleAdi = outageNotification.EffectedNeighbourhoods.Split(" | ").FirstOrDefault();  // İlk mahalleyi al
        //            if (!string.IsNullOrEmpty(mahalleAdi))
        //            {
        //                outageNotification.mahalleKimlikNo = await _adMahalleRepository.GetMahalleKimlikNoByNameAsync(mahalleAdi);
        //            }
        //            await _outageNotificationRepository.AddAsync(outageNotification);
        //        }
        //        Console.WriteLine("Mesajlar başarılı bir şekilde kaydedildi.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Mesaj deserializasyonu başarısız.");
        //    }
    }
}




//using EduPortal.Domain.Entities;
//using MassTransit;
//using Newtonsoft.Json;
//using RabbitMQ.ESB.MassTransit.Shared.Messages;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EduPortal.Application.Messaging
//{
//    public class OutageNotificationConsumer : IConsumer<IMessage>
//    {

//        public Task Consume(ConsumeContext<IMessage> context)
//        {
//            var message = context.Message;

//            var outageNotifications = JsonConvert.DeserializeObject<List<OutageNotification>>(JsonConvert.SerializeObject(message));

//            // Örneğin, powerOutage özelliğinin tüm içeriğini yazdıralım
//            Console.WriteLine($"Gelen mesaj içeriği: {Newtonsoft.Json.JsonConvert.SerializeObject(message)}");

//            return Task.CompletedTask;
//        }
//    }
//}
