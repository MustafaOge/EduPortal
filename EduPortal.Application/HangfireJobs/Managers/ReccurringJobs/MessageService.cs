using EduPortal.Application.Messaging;
using EduPortal.Application.Services;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.HangfireJobs.Managers.ReccurringJobs
{
    public class MessageService(RabbitMQPublisherService rabbitMQPublisherService, RabbitMQConsumerService rabbitMQConsumerService)
    {

        public async Task StartMessaging()
        { 
      //await   Task.WhenAll(rabbitMQPublisherService.StartPublishing(), rabbitMQConsumerService.StartConsuming());

    }
    }
}
