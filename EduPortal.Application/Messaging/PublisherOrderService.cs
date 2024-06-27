using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Domain.Enums;
using MassTransit;
using MassTransit.Transports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EduPortal.Application.Messaging
{
    public class MessagePublisherService
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessagePublisherService(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
        }

        public async Task SendMessageAsync<T>(T message, MessageType messageType)
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment2.main.queue"));

            try
            {
                await sendEndpoint.Send(new PaymentStartingMessage()
                {
                    Id = message.ToString(),
                    ProccesType = messageType
                }, pipe =>
                {
                    pipe.Durable = true;
                    pipe.SetAwaitAck(true);
                    pipe.CorrelationId = Guid.NewGuid();
                }, tokenSource.Token);

                Console.WriteLine("Message sent successfully.");
            }
            catch (OperationCanceledException ex)
            {
                // Timeout veya iptal hatası durumunda yapılacak işlemler
                Console.WriteLine($"Message sending canceled or timed out: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Diğer hatalar için yapılacak işlemler
                Console.WriteLine($"An error occurred while sending the message: {ex.Message}");
            }
        }
    }

    //public class MessagePublisherService
    //{
    //    private readonly ISendEndpointProvider _sendEndpointProvider;

    //    public MessagePublisherService(ISendEndpointProvider sendEndpointProvider)
    //    {
    //        _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
    //    }

    //    public async Task SendMessageAsync<T>(T message, MessageType messageType)
    //    {
    //        var tokenSource = new CancellationTokenSource(5000);
    //        var sendEndpoint =
    //            await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment2.main.queueu"));

    //        await sendEndpoint.Send(new PaymentStartingMessage() { Id = message.ToString(), ProccesType = messageType }, pipe =>
    //        {
    //            pipe.Durable = true;
    //            pipe.SetAwaitAck(true);
    //            pipe.CorrelationId = Guid.NewGuid();
    //        }, tokenSource.Token);
    //    }

    //}

    //public class PublisherServiceMassTransit
    //{
    //    private readonly IPublishEndpoint _publishEndpoint;
    //    private readonly ISendEndpointProvider _sendEndpointProvider;
    //    private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;




    //    public PublisherServiceMassTransit(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider, IGenericRepository<OutboxMessage, int> outboxMessageRepository)
    //    {
    //        _publishEndpoint = publishEndpoint;
    //        _sendEndpointProvider = sendEndpointProvider;
    //        _outboxMessageRepository = outboxMessageRepository;
    //    }



    //    public async Task SendMessageAsync<T>(T message, string queueName)
    //    {
    //        var tokenSource = new CancellationTokenSource(5000);
    //        var sendEndpoint =
    //            await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment2.order.created.event"));




    //        await sendEndpoint.Send(new PaymentStartingMessage() { Id = message.ToString() }, pipe =>
    //        {
    //            pipe.Durable = true;
    //            pipe.SetAwaitAck(true);
    //            pipe.CorrelationId = Guid.NewGuid();
    //        }, tokenSource.Token);


    //    }
    //}
}
