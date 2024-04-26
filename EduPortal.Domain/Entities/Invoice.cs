using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Invoice : BaseEntity<int>
    {
        public DateTime Date { get; set; }

        public Subscriber? Subscriber { get; set; }
        public int? SubscriberId { get; set; }
        public decimal Amount { get; set; }
        public string SubscriberType { get; set; }
        public DateTime ReadingDate { get; set; }  
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsPaid { get; set; } // Fatura ödendi mi?

        public MeterReading MeterReading { get; set; }


    }
}
