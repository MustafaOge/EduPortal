using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAU.Core.Models
{
    public class Address
    {
            public int Id { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Neighborhood { get; set; }
            public string Street { get; set; }
            public string BuildingNumber { get; set; }
            public string DoorNumber { get; set; }
            public Plumbing PlumbingNumber { get; set; }

            // Eğer Plumbing sınıfınızın da bir tanımı varsa onu eklemeyi unutmayın.
            // public Plumbing Plumbing { get; set; }
        }
    }
