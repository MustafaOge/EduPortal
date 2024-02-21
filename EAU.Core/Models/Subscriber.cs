using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAU.Core.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public int PlumbingNumber { get; set; }

        // Eğer Plumbing sınıfınızın da bir tanımı varsa onu eklemeyi unutmayın.
        public Plumbing Plumbing { get; set; }
    }
}
