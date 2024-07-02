using EduPortal.Domain.Enums;
using MassTransit;

namespace EduPortal.Application.Messaging
{
    public class MessagePublisherService
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessagePublisherService(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
        }

        public async Task SendMessageAsync(string id, MessageType messageType)
        {
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:payment2.main.queueu"));

            try
            {
                await sendEndpoint.Send(new PaymentStartingMessage()
                {
                    Id = id,
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


}
