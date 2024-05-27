using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface ISubscriberTerminateService
    {
        Task TerminateSubscriptionAndAddToOutbox(int subscriberId);
    }
}
