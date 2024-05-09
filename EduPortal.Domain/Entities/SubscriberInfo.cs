using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class SubscriberInfo
    {
        public SubsIndividual Individual { get; set; }
        public SubsCorporate Corprorate { get; set; }
    }
}
