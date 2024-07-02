using EduPortal.Domain.Entities;

namespace EduPortal.MVC.Models.ViewModel
{
    public class InvoiceDetailView
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } 
        public List<MeterReading> MeterReadings { get; set; } 
        public Subscriber Subscriber { get; set; }
    }
}
