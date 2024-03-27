using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.DTO_s.Subscriber
{
    public class CreateSubscriberDto
    {
        public string SubscriberType { get; set; }
        // Diğer ortak özellikler

        // Bireysel abone oluşturulduğunda kullanılacak özellikler
        public CreateIndividualDto Individual { get; set; }

        // Kurumsal abone oluşturulduğunda kullanılacak özellikler
        public CreateCorporateDto Corporate { get; set; }
    }

}
