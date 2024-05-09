using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using EduPortal.Application.Messaging;

namespace EduPortal.Application.Services
{
    // RabbitMQConsumerService'ı başlatmak ve durdurmak için bir servis sınıfı
    public class RabbitMQService : IHostedService
    {
        private readonly RabbitMQConsumerService _rabbitMQConsumer;

        public RabbitMQService(RabbitMQConsumerService rabbitMQConsumer)
        {
            _rabbitMQConsumer = rabbitMQConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_rabbitMQConsumer.StartConsuming();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // RabbitMQConsumerService'i durdurma kodu buraya eklenebilir
            return Task.CompletedTask;
        }
    }

    // Uygulama başlatıldığında ve durdurulduğunda RabbitMQService'ı başlatmak ve durdurmak için bir genişletme metodu
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseRabbitMQService(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddHostedService<RabbitMQService>();
            });
        }
    }

}
