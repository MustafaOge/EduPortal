using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class SubsCorporate : Subscriber
    {
        public int Id { get; set; }

        public string CorporateName { get; set; }
        public string TaxIdNumber { get; set; }



    }
}
