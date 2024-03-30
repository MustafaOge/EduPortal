using EduPortal.Domain.Entities;

namespace EduPortal.MVC.Models.ViewModel
{
    namespace YourProject.ViewModels
    {
        public class InvoiceDetailViewModel
        {
            public int InvoiceId { get; set; }


            public Invoice Invoice { get; set; } // Fatura detayları
                                                 // detayları
            public List<MeterReading> MeterReadings { get; set; } // Faturaya ait metre okuma bilgileri

            public EduPortal.Domain.Entities.Subscriber Subscriber { get; set; } // Fatura

        }
    }
}
