using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAU.Core.Models
{
    public class Plumbing
    {
        public int Id { get; set; }
        public int Number { get; set; }

        // Eğer Invoice sınıfınızın da bir tanımı varsa onu eklemeyi unutmayın.
        public List<Invoice> Invoices { get; set; }
    }
}

