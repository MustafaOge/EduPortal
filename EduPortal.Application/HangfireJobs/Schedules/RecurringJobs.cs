using EduPortal.Application.HangfireJobs.Managers.ReccurringJobs;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.HangfireJobs.Schedules
{


    public static class RecurringJobs
    {
        [AutomaticRetry(Attempts = 0)]
        [Obsolete]
        public static void CheckLastPayment()
        {
            //RecurringJob.AddOrUpdate<PaymentProcessor>(j => j.RunPaymentReminderJob(), "*/2 * * * *");



        }
        public static void StartMessageService( )
        {
            RecurringJob.AddOrUpdate<MessageService>(j => j.StartMessaging(), "*/2 * * * *");

            


        }


    }

}
