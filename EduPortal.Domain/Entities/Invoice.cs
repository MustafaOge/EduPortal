using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Subscriber? Subscriber { get; set; }
        public int? SubscriberId { get; set; }

        public string EICCode { get; set; }
        public decimal Amount { get; set; }
        public int ConsumerId { get; set; }
        public string SubscriberType { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsPaid { get; set; } // Fatura ödendi mi?
        public decimal TotalConsumptionKwh { get; set; }
    }
}
