using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class PaymentDueInvoiceQueueMessage
    {
        public string CounterNumber { get; set; }
        public string Email { get; set; }
        Invoice Invoice { get; set; }

    }
}
