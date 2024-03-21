using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.DTO_s.Subscriber
{
    public class CreateCorporateDto : SubscriberCreateDTO
    {
        public string CorprorateName { get; set; }
        public string TaxIdNumber { get; set; }

    }
}
