﻿using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Messaging;
using EduPortal.Domain.Entities;
using EduPortal.Domain.Enums;
using Serilog;

namespace EduPortal.Application.Services
{
    public class SubscriberTerminateService : ISubscriberTerminateService
    {
        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
        private readonly MessagePublisherService _publisherServiceMassTransit;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriberTerminateService(IUnitOfWork unitOfWork, MessagePublisherService publisherServiceMassTransit, IGenericRepository<OutboxMessage, int> outboxMessageRepository)
        {
            _outboxMessageRepository = outboxMessageRepository;
            _publisherServiceMassTransit = publisherServiceMassTransit;
            _unitOfWork = unitOfWork;

        }

        public async Task TerminateSubscriptionAndAddToOutbox(int subscriberId)
        {
            // Sonlandırılan abonelik bilgisini outbox tablosuna ekle
            OutboxMessage outboxMessage = new OutboxMessage
            {
                Type = "Subscription-Termination",
                Payload = subscriberId.ToString(),
                CreatedByUser = 10,
                IsProcessed = false
            };
            Log.Information("Yeni bir abone silindi silenn abone id :" + outboxMessage.Payload);
            await _outboxMessageRepository.AddAsync(outboxMessage);
            await SendMessage();
        }
        public async Task SendMessage()
        {
            var outboxMessages = await _outboxMessageRepository.GetAllAsync(x => x.IsProcessed == false);

            foreach (var item in outboxMessages)
            {
                await _publisherServiceMassTransit.SendMessageAsync(item.Payload, MessageType.SubscriptionTermination);
            }

            // Gönderme işlemi tamamlandıktan sonra işaretlenmiş mesajları işlemek için başka bir metod çağırılır.
            await ProcessProcessedMessages(outboxMessages);
        }

        private async Task ProcessProcessedMessages(IEnumerable<OutboxMessage> outboxMessages)
        {
            foreach (var item in outboxMessages)
            {
                var existingMessage = await _outboxMessageRepository.GetByIdAsync(item.Id);
                if (existingMessage != null)
                {
                    existingMessage.IsProcessed = true;
                    _outboxMessageRepository.Update(existingMessage);
                }
            }

            // İşlem yapıldı olarak işaretlenmiş öğeleri veritabanına kaydetmek için commit yapılabilir
            await _unitOfWork.CommitAsync();
        }
    }
}
