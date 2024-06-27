using EduPortal.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.HangfireJobs.Managers.ReccurringJobs
{
    public class OutageNotificationJob(IOutageNotificationService outageNotificationService)
    {
       public void  StartOutageNotification()
        {
            outageNotificationService.SendOutageMessageAsync();
        }
    }
}
