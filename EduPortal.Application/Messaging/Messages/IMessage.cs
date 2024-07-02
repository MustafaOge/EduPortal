namespace RabbitMQ.ESB.MassTransit.Shared.Messages
{
    public interface IMessage
    {
        public PowerOutageItem powerOutage { get; set; }
    }
}
