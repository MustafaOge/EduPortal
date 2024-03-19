using EduPortal.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Domain.Entities
{
    public class Subscriber: BaseEntity
    {
        public int Id { get; set; }

        [BindProperty]
        public string SubscriberType { get; set; }

        [MaxLength(100)]

        public string PhoneNumber { get; set; }

        public int CounterNumber { get; set; }

        public string Email { get; set; }

        public List<Invoice>? Invoices { get; set; }




    }
}
// Authentşcatşon cookies