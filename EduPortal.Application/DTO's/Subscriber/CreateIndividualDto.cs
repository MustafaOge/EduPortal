using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.DTO_s.Subscriber
{
    public class CreateIndividualDto : SubscriberCreateDTO
    {
        public string NameSurname { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
