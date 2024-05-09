using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Services;
using EduPortal.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace EduPortal.Application.Messaging
{



    public class RabbitMQConsumerService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMQConsumerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://qdsaoytb:sCZAAMzrkUXxve6ZYzSJ7RPEbBqnB2zo@fish.rmq.cloudamqp.com/qdsaoytb")
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            //1.adım
            _channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct, autoDelete: true);

            //2.adım
            _queueName = _channel.QueueDeclare().QueueName;

            //3.adım
            _channel.QueueBind(
                queue: _queueName,
                exchange: "direct-exchange-example",
                routingKey: "Subscription-Termination"
                );
        }

        public async Task StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("Consumer başladı. Mesajları dinlemek için bekleniyor...");
            consumer.Received += async (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine(message);

                Console.WriteLine($"Gönderilen mesaj: {message}");
                

                // Scoped servisleri kullanmak için yeni bir scope oluştur
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedService = scope.ServiceProvider.GetRequiredService<SubscriberNotificationService>();

                    if(message.Length<6)
                    {
                        await scopedService.SendEmailNotification(MessageType.SubscriptionTermination, message);

                    }
                    else
                    {
                        await scopedService.SendEmailNotification(MessageType.InvoiceReminder, message);

                    }
                }
            };

            await Task.Delay(Timeout.Infinite);
        }
    }



}








//public class RabbitMQConsumer : BackgroundService
//{
//    private readonly IGenericRepository<OutboxMessage, int> _inboxRepository;
//    private readonly IConnection _connection;
//    private readonly string _queueName = "example-queue";

//    public RabbitMQConsumer(IGenericRepository<OutboxMessage, int> inboxRepository)
//    {
//        _inboxRepository = inboxRepository;

//        var factory = new ConnectionFactory
//        {
//            Uri = new Uri("amqps://bxsewtck:fvGdJLFJ5YM-re_2gk1DmgRhbyrN9eOl@moose.rmq.cloudamqp.com/bxsewtck")
//        };

//        _connection = factory.CreateConnection();
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        using (var channel = _connection.CreateModel())
//        {
//            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

//            var consumer = new EventingBasicConsumer(channel);
//            consumer.Received += async (sender, eventArgs) =>
//            {
//                var body = eventArgs.Body.ToArray();
//                var message = Encoding.UTF8.GetString(body);

//                // Mesajı işleme metodu çağır
//                await ProcessMessageAsync(message);

//                // Mesajı işaretleyerek RabbitMQ'dan kaldır
//                channel.BasicAck(eventArgs.DeliveryTag, false);
//            };

//            channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

//            while (!stoppingToken.IsCancellationRequested)
//            {
//                await Task.Delay(1000, stoppingToken);
//            }
//        }
//    }

//    private async Task ProcessMessageAsync(string message)
//    {
//        // Gelen mesajı işleme yöntemi
//        Console.WriteLine("Received message: {0}", message);

//        // Örnek olarak, gelen mesajı veritabanına kaydetme
//        var inboxMessage = new OutboxMessage { Payload = message };
//        await _inboxRepository.AddAsync(inboxMessage);
//    }

//    public override async Task StopAsync(CancellationToken cancellationToken)
//    {
//        // Servis sonlandığında bağlantıyı kapat
//        _connection.Close();
//        await base.StopAsync(cancellationToken);
//    }
//}




//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EduPortal.Application.Messaging
//{
//    using EduPortal.Application.Interfaces.Repositories;
//    using EduPortal.Domain.Entities;
//    using Microsoft.Extensions.Hosting;
//    using RabbitMQ.Client;
//    using RabbitMQ.Client.Events;
//    using System;
//    using System.Text;
//    using System.Threading;
//    using System.Threading.Tasks;
//    public class RabbitMQConsumer : BackgroundService
//    {
//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {


//            ConnectionFactory factory = new();
//            factory.Uri = new("amqps://bxsewtck:fvGdJLFJ5YM-re_2gk1DmgRhbyrN9eOl@moose.rmq.cloudamqp.com/bxsewtck");

//            using IConnection connection = factory.CreateConnection();
//            using IModel channel = connection.CreateModel();

//            //1. Adım
//            channel.ExchangeDeclare(exchange: "direct-exchange", type: ExchangeType.Direct);

//            //2. Adım
//            string queueName = channel.QueueDeclare().QueueName;
//            IEnumerable<string> messageList;
//            //3. Adım
//            channel.QueueBind(
//                queue: queueName,
//            exchange: "direct-exchange",
//            routingKey: "unpaid-invoices");

//            EventingBasicConsumer consumer = new(channel);
//            channel.BasicConsume(
//                queue: queueName,
//            autoAck: true,
//            consumer: consumer);
//            consumer.Received += (sender, e) =>
//            {
//                string message = Encoding.UTF8.GetString(e.Body.Span);
//                Console.WriteLine(message);
//            };

//        }



//    }


//}