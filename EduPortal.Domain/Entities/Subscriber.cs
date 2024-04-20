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
    public class Subscriber: BaseEntity<int>
    {
        [BindProperty]
        public string SubscriberType { get; set; }
        public string PhoneNumber { get; set; }
        public string CounterNumber { get; set; }
        public string Email { get; set; }
        public List<Invoice>? Invoices { get; set; }
        public bool IsActive { get; set; }

    }
}
// Authentşcatşon cookies