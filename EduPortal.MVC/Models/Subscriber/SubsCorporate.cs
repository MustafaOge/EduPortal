using System.ComponentModel.DataAnnotations;

namespace EduPortal.MVC.Models.Subscriber
{
    public class SubsCorporate
    {
        [Required(ErrorMessage = "Kurum Adı Alanı Zorunludur")]
        [MaxLength(200)]
        public string CorprorateName { get; set; }

       
        [Required(ErrorMessage = "Vergi Numarası Alanı Zorunludur")]
        public string TaxIdNumber { get; set; }
        public string SubscriberType { get; set; }

        [MinLength(11, ErrorMessage = "Gerçersiz Telefon Numarası"), MaxLength(11,ErrorMessage ="Gerçersiz Telefon Numarası")]
        [Required(ErrorMessage = "Telefon Numarası Alanı Zorunludur")]

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Sayaç Naumarası Alanı Zorunludur")]

        public int CounterNumber { get; set; }
        [Required(ErrorMessage = "Email Adresi Alanı Zorunludur")]

        public string Email { get; set; }
    }
}
