using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using MassTransit;
using MassTransit.Transports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EduPortal.Application.Messaging
{
    public class PublisherServiceMassTransit
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;




        public PublisherServiceMassTransit(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider, IGenericRepository<OutboxMessage, int> outboxMessageRepository)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
            _outboxMessageRepository = outboxMessageRepository;
        }

        public async Task SendMessageAsync<T>(T message, string queueName)
        {
            var tokenSource = new CancellationTokenSource(5000);
            var sendEndpoint =
                await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment2.order.created.event"));




            await sendEndpoint.Send(new PaymentStartingMessage() { Id = message.ToString() }, pipe =>
            {
                pipe.Durable = true;
                pipe.SetAwaitAck(true);
                pipe.CorrelationId = Guid.NewGuid();
            }, tokenSource.Token);


        }
    }
}
