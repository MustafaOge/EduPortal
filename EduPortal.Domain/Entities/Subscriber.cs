using Microsoft.AspNetCore.Mvc;

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
