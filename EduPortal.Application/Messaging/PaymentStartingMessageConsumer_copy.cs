using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Messaging
{
    public class PaymentStartingMessageConsumer_copy(IPublishEndpoint publishEndpoint, IServiceProvider serviceProvider)
         : IConsumer<PaymentStartingMessage>
    {
        public Task Consume(ConsumeContext<PaymentStartingMessage> context)
        {
            using (var scope = serviceProvider.CreateScope())
            {


                // db.save changes
            }


            //redis

            // publishEndpoint.publish
            // publishEndpoint.publish


            Console.WriteLine($"payment :{context.Message.Id}");

            return Task.CompletedTask;
        }
    }
}
