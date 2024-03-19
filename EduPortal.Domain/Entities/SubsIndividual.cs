using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities

{
    public class SubsIndividual : Subscriber
    {
        //[Key]

        //public int SubscriberId { get; set; }
        //public Subscriber Subscriber { get; set; }
        public string NameSurname { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime BirthDate { get; set; }


    }
}
