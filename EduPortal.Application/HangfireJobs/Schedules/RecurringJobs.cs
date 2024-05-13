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
        [Obsolete]
        public static void StartMessageService()
        {
            RecurringJob.AddOrUpdate<InvoiceReminderJob>(j => j.RunPaymentReminderJob(), "*/2 * * * *");
        }
    }
}
