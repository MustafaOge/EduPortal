using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.ESB.MassTransit.Shared.Messages
{
    public class ExampleMessage : IMessage
    {

        public PowerOutageItem powerOutage { get; set; }

        //public string Text { get; set; }
    }


    public class PowerOutageItem 
    {
            public string? Province { get; set; }
            public string? District { get; set; }
            public DateTime Date { get; set; }
            public string? DistributionCompanyName { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string? Reason { get; set; }
            public string? EffectedNeighbourhoods { get; set; }
            public int EffectedSubscribers { get; set; }
            public int HourlyLoadAvg { get; set; }
            public int Id { get; set; }
    }
    

}
