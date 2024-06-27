  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.Shared.Messages
{
    public interface IMessage
    {
        public PowerOutageItem powerOutage { get; set; }
    }
}
