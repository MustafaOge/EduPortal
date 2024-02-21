using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAU.Core.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string EICCode { get; set; }
        public decimal Amount { get; set; }
        public Plumbing Plumbing { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool PaymentStatus { get; set; }

        // Eğer Consumer sınıfınızın da bir tanımı varsa onu eklemeyi unutmayın.
        // public Consumer Consumer { get; set; }
    }
    
    }
