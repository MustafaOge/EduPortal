using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;

//namespace EduPortal.Application.Services
//{
//    public interface IRabbitMQPublisherService
//    {
//        Task StartPublishing();
//    }
//    public class RabbitMQPublisherService : IRabbitMQPublisherService
//    {
//        private readonly IQueueService _queueService;
//        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IConfiguration _configuration;
//        private readonly ConnectionFactory _connectionFactory;

//        public RabbitMQPublisherService(IUnitOfWork unitOfWork, IQueueService queueService, IGenericRepository<OutboxMessage, int> outboxMessageRepository, IConfiguration configuration)
//        {
//            _queueService = queueService;
//            _outboxMessageRepository = outboxMessageRepository;
//            _unitOfWork = unitOfWork;
//            _configuration = configuration;
//            _connectionFactory = new ConnectionFactory
//            {
//                Uri = new Uri(_configuration["RabbitMQ:Uri"])
//            };
//        }

//        public async Task StartPublishing()
//        {
//            var mailList = await _outboxMessageRepository.GetAllAsync(x => !x.IsProcessed);
//            if (!mailList.Any())
//            {
//                Console.WriteLine("Gönderilecek mesaj bulunamadı.");
//                return;
//            }

//            var tasks = mailList.Select(item => ProcessMessage(item)).ToList();
//            await Task.WhenAll(tasks);
//            await _unitOfWork.CommitAsync();
//        }

//        private async Task ProcessMessage(OutboxMessage item)
//        {
//            var existingOutboxMessage = await _outboxMessageRepository.GetByIdAsync(item.Id);

//            string message = existingOutboxMessage.Payload;
//            byte[] byteMessage = Encoding.UTF8.GetBytes(message);

//            existingOutboxMessage.IsProcessed = true;
//            _outboxMessageRepository.Update(existingOutboxMessage);

//            try
//            {
//                await PublishMessage(existingOutboxMessage.Type, byteMessage);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Hata: Mesaj gönderilemedi. Hata: {ex.Message}");
//                existingOutboxMessage.IsProcessed = false;
//                _outboxMessageRepository.Update(existingOutboxMessage);
//            }

//            await Task.Delay(1000); // 1 saniye bekle
//        }

//        private async Task PublishMessage(string routingKey, byte[] messageBody)
//        {
//            using var connection = _connectionFactory.CreateConnection();
//            using var channel = connection.CreateModel();

//            channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct, autoDelete: true);
//            channel.BasicPublish(
//                exchange: "direct-exchange-example",
//                routingKey: routingKey,
//                body: messageBody);

//            Console.WriteLine("Mesaj gönderildi.");
//        }
//    }
////}
