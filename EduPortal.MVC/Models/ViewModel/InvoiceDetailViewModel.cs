
using EduPortal.Domain.Entities;

namespace EduPortal.MVC.Models.ViewModel
{
    namespace YourProject.ViewModels
    {
        public class InvoiceDetailViewModel
        {
            public int InvoiceId { get; set; }

            public EduPortal.Domain.Entities.Invoice Invoice {get ; set; }


            // detayları
            public List<MeterReading> MeterReadings { get; set; } // Faturaya ait metre okuma bilgileri

            public EduPortal.Domain.Entities.Subscriber Subscriber { get; set; } // Fatura

        }
    }
}
