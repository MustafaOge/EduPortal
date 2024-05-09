using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using EduPortal.Application.Services;
using EduPortal.Application.Interfaces.Services;
using System.Net;
using EduPortal.Application.Interfaces.UnitOfWorks;



namespace EduPortal.Application.Services
{
    public class RabbitMQPublisherService
    {
        private readonly IQueueService _queueService;
        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
        private readonly IUnitOfWork _unitOfWork;


        public RabbitMQPublisherService(IUnitOfWork unitOfWork, IQueueService queueService, IGenericRepository<OutboxMessage, int> outboxMessageRepository)
        {
            _queueService = queueService;
            _outboxMessageRepository = outboxMessageRepository;
            _unitOfWork = unitOfWork;

        }
        int a = 0;

        public async Task StartPublishing()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://qdsaoytb:sCZAAMzrkUXxve6ZYzSJ7RPEbBqnB2zo@fish.rmq.cloudamqp.com/qdsaoytb")
            };

            var mailList = await _outboxMessageRepository.GetAllAsync(x => !x.IsProcessed);

            if (mailList == null || !mailList.Any())
            {
                Console.WriteLine("Gönderilecek mesaj bulunamadı.");
                return;
            }

            var tasks = new List<Task>();

            foreach (var item in mailList)
            {
                var existingOutboxMessage = await _outboxMessageRepository.GetByIdAsync(item.Id); // Veritabanından mevcut OutboxMessage'ı al

                string message = $"{existingOutboxMessage.Payload}";
                byte[] byteMessage = Encoding.UTF8.GetBytes(message);

                existingOutboxMessage.IsProcessed = true; // İşaretleme işlemi
                _outboxMessageRepository.Update(existingOutboxMessage); // Güncelleme işlemi

                try
                {
                    // Mesajı yayınla ve işlemi bir Task'e ekleyerek listeye ekle
                    var task = PublishMessage(factory, existingOutboxMessage.Type, byteMessage);
                    tasks.Add(task);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata: Mesaj gönderilemedi. Hata: {ex.Message}");
                    // Hata durumunda gerekli işlemleri yapabilirsiniz.

                    // Mesaj gönderilmediği için IsProcessed değerini false olarak ayarla
                    existingOutboxMessage.IsProcessed = false;
                    _outboxMessageRepository.Update(existingOutboxMessage);
                }

                await Task.Delay(1000); // 1 saniye bekle
            }

            // Tüm mesajların işlenmesini bekleyin
            await Task.WhenAll(tasks);

            // Değişiklikleri kaydet
            await _unitOfWork.CommitAsync();
        }


        private async Task PublishMessage(ConnectionFactory factory, string routingKey, byte[] messageBody)
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct, autoDelete: true);

            channel.BasicPublish(
                exchange: "direct-exchange-example",
                routingKey: routingKey,
                body: messageBody);

            Console.WriteLine("Mesaj gönderildi.");
        }
    }
}










//public class RabbitMQPublisher : BackgroundService
//{
//    private readonly ISingletonRepository<OutboxMessage, int> _scopedRepository;
//    private static IConnection _connection; // RabbitMQ bağlantı alanı
//    private readonly TimeSpan _messageSendInterval;

//    public RabbitMQPublisher( ISingletonRepository<OutboxMessage, int> scopedRepository)
//    {
//        _scopedRepository = scopedRepository;
//        _messageSendInterval = TimeSpan.FromSeconds(5);

//        // Bağlantıyı oluştur
//        var factory = new ConnectionFactory
//        {
//            Uri = new Uri("amqps://bxsewtck:fvGdJLFJ5YM-re_2gk1DmgRhbyrN9eOl@moose.rmq.cloudamqp.com/bxsewtck")
//        };
//        _connection = factory.CreateConnection();
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            try
//            {
//                IQueryable<OutboxMessage> messages;
//                messages = _scopedRepository.Where(msg => !msg.IsProcessed).OrderBy(msg => msg.Id).AsQueryable();

//                // Mesajları RabbitMQ'ya gönder
//                await SendMessageToRabbitMQ(messages);

//                // Gönderilen mesajları işaretle ve Outbox tablosundan kaldır
//                foreach (var message in messages)
//                {
//                    message.IsProcessed = true;
//                    _scopedRepository.Remove(message);
//                }

//                // Belirli bir süre bekleyin ve ardından işlemi tekrarlayın
//                await Task.Delay(_messageSendInterval, stoppingToken);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Hata oluştu: {ex.Message}");
//            }
//        }
//    }

//    private async Task SendMessageToRabbitMQ(IQueryable<OutboxMessage> messages)
//    {
//        //using (var channel = _connection.CreateModel())
//        //{
//        //    // Kuyruğu ve exchange'i oluştur
//        //    channel.QueueDeclare(queue: "example-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
//        //    channel.ExchangeDeclare(exchange: "direct-exchange", type: ExchangeType.Direct);

//        //    foreach (var message in messages)
//        //    {
//        //        var body = Encoding.UTF8.GetBytes(message.Payload);
//        //        var properties = channel.CreateBasicProperties();
//        //        properties.Persistent = true;

//        //        // Mesajı gönder
//        //        channel.BasicPublish(exchange: "direct-exchange", routingKey: "unpaid-invoices", body: body, basicProperties: properties);
//        //        Console.WriteLine(" [x] Sent {0}", message.Payload);
//        //    }
//        //}
//    }

//    public override async Task StopAsync(CancellationToken cancellationToken)
//    {
//        _connection.Close(); // Servis sonlandığında bağlantıyı kapat
//        await base.StopAsync(cancellationToken);
//    }
//}}
