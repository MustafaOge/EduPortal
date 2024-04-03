using System.ComponentModel.DataAnnotations;

namespace EduPortal.MVC.Models.Invoice
{
    public class InvoiceComplaintCreateModel
    {
        [Required(ErrorMessage = "Fatura ID gereklidir.")]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "İtiraz başlığı gereklidir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İtiraz nedeni gereklidir.")]
        public string Reason { get; set; }
    }
}
