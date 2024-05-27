using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EduPortal.Application.Services;
using EduPortal.Domain.Enums;

namespace EduPortal.Application.Messaging
{
    public class RabbitMQConsumerService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _connectionFactory;

        public RabbitMQConsumerService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;

            _connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_configuration["RabbitMQ:Uri"])
            };


            var connection = _connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            _queueName = _channel.QueueDeclare().QueueName;

            SetupQueueAndExchange(_queueName);
        }

        private void SetupQueueAndExchange(string queueName)
        {
            // Adım 1: Exchange tanımla
            _channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct, autoDelete: true);

            // Adım 2: Queue oluştur

            // Adım 3: Queue'yu Exchange'e bağla
            _channel.QueueBind(
                queue: queueName,
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
            consumer.Received += async (sender, e) => await ProcessMessage(e);


            await Task.Delay(Timeout.Infinite);
        }

        private async Task ProcessMessage(BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body.Span);
            Console.WriteLine($"Gönderilen mesaj: {message}");
            //_channel.BasicAck(e.DeliveryTag, true);

            // Scoped servisleri kullanmak için yeni bir scope oluştur
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<SubscriberNotificationService>();
                MessageType messageType = message.Length < 6 ? MessageType.SubscriptionTermination : MessageType.InvoiceReminder;

                await scopedService.SendEmailNotification(messageType, message);
            }
        }
    }
}
